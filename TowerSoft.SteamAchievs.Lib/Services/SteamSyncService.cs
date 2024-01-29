using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TowerSoft.SteamAchievs.Lib.Comparers;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Utilities;
using TowerSoft.SteamTower.Models;
using TowerSoft.Utilities;
using TowerSoft.SteamTower;
using TowerSoft.Repository;

namespace TowerSoft.SteamAchievs.Lib.Services {
    public class SteamSyncService {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steamApi;
        private readonly ProtonDbService protonDbService;
        private readonly HowLongToBeatService howLongToBeatService;
        private readonly ILogger logger;
        private readonly AppSettings appSettings;

        public SteamSyncService(UnitOfWork uow, SteamApiClient steamApi,
            ProtonDbService protonDbService, HowLongToBeatService howLongToBeatService,
            IOptionsSnapshot<AppSettings> appSettings, ILogger<SteamSyncService> logger) {
            this.uow = uow;
            this.steamApi = steamApi;
            this.protonDbService = protonDbService;
            this.howLongToBeatService = howLongToBeatService;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public List<UserAppModel> LoadSteamData(List<OwnedApp> ownedApps) {
            List<UserAppModel> userAppModels = new();

            CompanyRepository companyRepo = uow.GetRepo<CompanyRepository>();
            Dictionary<string, Company> companies = companyRepo.GetAll().ToDictionary(x => x.Name.ToLower());
            Dictionary<long, Company> companiesByID = companies.Values.ToDictionary(x => x.ID);

            int count = 1;
            foreach (OwnedApp ownedApp in ownedApps.OrderBy(x => x.Name)) {
                ownedApp.Name = ownedApp.Name.SafeTrim();

                if (appSettings.IgnoredAppIDs.Contains(ownedApp.SteamAppID)) {
                    logger.LogInformation($"ID {ownedApp.SteamAppID} is ignored. Skipping {ownedApp.Name}");
                    continue;
                }

                // Making requests too quickly seems to cause a short soft ban from Steam's API
                SteamApp steamApp = steamApi.StoreClient.GetSteamApp(ownedApp.SteamAppID).Result;
                if (steamApp != null)
                    steamApp.Name = steamApp.Name.SafeTrim();

                List<GlobalAchievementStat> globalAchievements = steamApi.UserStatsClient.GetGlobalAchievementStats(ownedApp.SteamAppID).Result;


                List<string> userTags = null;
                if (steamApp != null) {
                    userTags = steamApi.StoreScraper.GetAppUserTags(ownedApp.SteamAppID).Result;
                }

                bool delisted = false;
                if (steamApp == null) {
                    logger.LogWarning($"  {ownedApp.Name} could not be found in the store");
                    steamApp = new() {
                        ID = ownedApp.SteamAppID,
                        Name = ownedApp.Name,
                    };
                    delisted = true;
                    if (globalAchievements.SafeAny()) {
                        steamApp.AchievmentCount = globalAchievements.Count;
                    }
                }

                if (!string.IsNullOrWhiteSpace(steamApp.Type) &&
                    steamApp.Type.Equals("mod", StringComparison.OrdinalIgnoreCase)) {
                    logger.LogWarning($"{count}/{ownedApps.Count}: {steamApp.Name} type is '{steamApp.Type}'. Skipping.");
                    count++;
                    continue;
                }

                List<HltbModel> hltbModels = howLongToBeatService.Search(ownedApp.Name).Result;
                HltbModel? matchedHltbModel = null;
                if (hltbModels.Count == 1) {
                    matchedHltbModel = hltbModels.First();
                }

                ProtonDbGame protonDbGame = protonDbService.GetGame(ownedApp.SteamAppID).Result;

                UserAppModel model = new() {
                    OwnedApp = ownedApp,
                    SteamApp = steamApp,
                    GlobalAchievementStats = globalAchievements,
                    GameStatsSchema = steamApi.UserStatsClient.GetGameStatsSchema(ownedApp.SteamAppID).Result,
                    UserAchievements = steamApi.UserStatsClient.GetUserAchievements(appSettings.DefaultSteamUserID, ownedApp.SteamAppID).Result,
                    DeckCompatibility = steamApi.StoreClient.GetDeckCompatibility(ownedApp.SteamAppID).Result,
                    ReviewSummary = steamApi.StoreClient.GetReviews(ownedApp.SteamAppID).Result,
                    HltbModel = matchedHltbModel,
                    ProtonDbGame = protonDbGame,
                    UserTags = userTags,
                    Delisted = delisted,
                    Developers = new(),
                    Publishers = new()
                };


                if (steamApp.Developers.SafeAny()) {
                    foreach (string dev in steamApp.Developers) {
                        if (string.IsNullOrWhiteSpace(dev)) continue;
                        string devTrimmed = dev.Trim();

                        Company company;
                        if (companies.ContainsKey(devTrimmed.ToLower())) {
                            company = companies[devTrimmed.ToLower()];
                        } else {
                            company = new Company { Name = devTrimmed };
                            companyRepo.Add(company);
                            companies.Add(devTrimmed.ToLower(), company);
                        }
                        model.Developers.Add(company.ID);
                    }
                }

                if (steamApp.Publishers.SafeAny()) {
                    foreach (string pub in steamApp.Publishers) {
                        if (string.IsNullOrWhiteSpace(pub)) continue;
                        string pubTrimmed = pub.Trim();

                        Company company;
                        if (companies.ContainsKey(pubTrimmed.ToLower())) {
                            company = companies[pubTrimmed.ToLower()];
                        } else {
                            company = new Company { Name = pubTrimmed };
                            companyRepo.Add(company);
                            companies.Add(pubTrimmed.ToLower(), company);
                        }
                        model.Publishers.Add(company.ID);
                    }
                }

                logger.LogInformation($"{count}/{ownedApps.Count}: {ownedApp.Name} - {steamApp.AchievmentCount} Achievements");
                userAppModels.Add(model);
                count++;
            }

            return userAppModels;
        }

        #region Syncs
        public void RunAllSyncs(List<UserAppModel> userAppModels) {
            SyncSteamGames(userAppModels);
            SyncSteamGameDescriptions(userAppModels);
            SyncSteamCategories(userAppModels);
            SyncSteamGameCategories(userAppModels);
            SyncSteamUserTags(userAppModels);
            SyncSteamGameUserTags(userAppModels);
            SyncAchievementSchemas(userAppModels);
            SyncSteamUserAchievements(userAppModels);
            SyncCompanies(userAppModels);
        }

        public void SyncSteamGames(List<UserAppModel> userAppModels) {
            SteamGameRepository repo = uow.GetRepo<SteamGameRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamGames(userAppModels).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamGame>());
        }

        private void SyncSteamGameDescriptions(List<UserAppModel> userAppModels) {
            SteamGameDescriptionsRepository repo = uow.GetRepo<SteamGameDescriptionsRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamGameDescriptions(userAppModels).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamGameDescriptions>());
        }

        private void SyncSteamCategories(List<UserAppModel> userAppModels) {
            SteamCategoryRepository repo = uow.GetRepo<SteamCategoryRepository>();
            SteamCategoryNameComparer comparer = new();
            var dbEntities = repo.GetAll().ToHashSet(comparer);
            var steamEntities = ModelConvert.ToSteamCategories(userAppModels).ToHashSet(comparer);

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamCategory>(), comparer);
        }

        private void SyncSteamGameCategories(List<UserAppModel> userAppModels) {
            SteamGameCategoryRepository repo = uow.GetRepo<SteamGameCategoryRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamGameCategories(userAppModels, uow.GetRepo<SteamCategoryRepository>().GetAll()).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamGameCategory>());
        }

        private void SyncSteamUserTags(List<UserAppModel> userAppModels) {
            SteamUserTagRepository repo = uow.GetRepo<SteamUserTagRepository>();
            SteamUserTagNameComparer comparer = new();
            var dbEntities = repo.GetAll().ToHashSet(comparer);
            var steamEntities = ModelConvert.ToSteamUserTags(userAppModels).ToHashSet(comparer);

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamUserTag>(), comparer);
        }

        private void SyncCompanies(List<UserAppModel> userAppModels) {
            GameCompanyRepository repo = uow.GetRepo<GameCompanyRepository>();
            var dbEntities = repo.GetAll().ToHashSet();

            List<GameCompany> gameCompanies = ModelConvert.ToGameDevelopers(userAppModels).ToList();
            gameCompanies.AddRange(ModelConvert.ToGamePublishers(userAppModels));
            var steamEntities = gameCompanies.ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<GameCompany>());

            List<long> syncedGameIDs = userAppModels.Select(x => x.SteamApp.ID).ToList();

            var syncedDbEnties = dbEntities.Where(x => syncedGameIDs.Contains(x.SteamGameID)).ToHashSet();

            foreach (GameCompany remove in syncedDbEnties.Except(steamEntities)) {
                repo.Remove(remove);
            }
        }

        private void SyncSteamGameUserTags(List<UserAppModel> userAppModels) {
            SteamGameUserTagRepository repo = uow.GetRepo<SteamGameUserTagRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamGameUserTags(userAppModels, uow.GetRepo<SteamUserTagRepository>().GetAll()).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamGameUserTag>());
        }

        private void SyncAchievementSchemas(List<UserAppModel> userAppModels) {
            SteamAchievementSchemaRepository repo = uow.GetRepo<SteamAchievementSchemaRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamAchievementSchemas(userAppModels).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamAchievementSchema>());


            var removed = dbEntities.Where(x => userAppModels.Select(y => y.OwnedApp.SteamAppID).Contains(x.SteamGameID)).Except(steamEntities);
            if (removed.SafeAny()) {

                foreach (SteamAchievementSchema schema in removed) {
                    if (!schema.RemovedFromSteam) {
                        schema.RemovedFromSteam = true;
                        repo.Update(schema);
                    }
                }
            }
        }

        private void SyncSteamUserAchievements(List<UserAppModel> userAppModels) {
            SteamUserAchievementRepository repo = uow.GetRepo<SteamUserAchievementRepository>();
            var dbEntities = repo.GetAll().ToHashSet();
            var steamEntities = ModelConvert.ToSteamUserAchievements(userAppModels).ToHashSet();

            SyncData(repo, dbEntities, steamEntities, new AllPropertyComparer<SteamUserAchievement>());
        }
        #endregion

        private void SyncData<T>(DbRepository<T> repo, HashSet<T> databaseEntities, HashSet<T> steamEntities, IEqualityComparer<T> updateComparer, IEqualityComparer<T> defaultComparer = null) {
            Type type = typeof(T);
            if (!type.IsAssignableTo(typeof(IEquatable<T>))) {
                throw new Exception($"Type '{type.Name}' does not implement the IEquatable interface.");
            }

            if (type.GetMethod("GetHashCode").DeclaringType != type) {
                throw new Exception($"Type '{type.Name}' does not override the GetHashCode method.");
            }

            var addList = steamEntities.Except(databaseEntities, defaultComparer);
            repo.Add(addList);

            var updateList = steamEntities.Intersect(databaseEntities).Except(databaseEntities, updateComparer);
            foreach (T toUpdate in updateList) {
                repo.Update(toUpdate);
            }

            //var removeList = databaseEntities.Except(steamEntities, defaultComparer);
            //foreach (T remove in removeList) {
            //    repo.Remove(remove);
            //}

            //logger.LogInformation($"{type.Name} Sync - Added: {addList.Count()} - Updated: {updateList.Count()} - Removed: {removeList.Count()}");
            logger.LogInformation($"{type.Name} Sync - Added: {addList.Count()} - Updated: {updateList.Count()}");
        }
    }
}
