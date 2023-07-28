using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TowerSoft.SteamAchievs.Lib.Comparers;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Utilities;
using TowerSoft.SteamGridDbWrapper.Models;
using TowerSoft.SteamGridDbWrapper;
using TowerSoft.SteamTower.Models;
using TowerSoft.Utilities;
using TowerSoft.SteamTower;
using TowerSoft.Repository;

namespace TowerSoft.SteamAchievs.Lib.Services {
    public class SteamSyncService {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steamApi;
        private readonly SteamGridClient steamGridClient;
        private readonly ProtonDbService protonDbService;
        private readonly HowLongToBeatService howLongToBeatService;
        private readonly ILogger logger;
        private readonly AppSettings appSettings;

        public SteamSyncService(UnitOfWork uow, SteamApiClient steamApi, SteamGridClient steamGridClient,
            ProtonDbService protonDbService, HowLongToBeatService howLongToBeatService,
            IOptionsSnapshot<AppSettings> appSettings, ILogger<SteamSyncService> logger) {
            this.uow = uow;
            this.steamApi = steamApi;
            this.steamGridClient = steamGridClient;
            this.protonDbService = protonDbService;
            this.howLongToBeatService = howLongToBeatService;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public List<UserAppModel> LoadSteamData(List<OwnedApp> ownedApps) {
            List<UserAppModel> userAppModels = new();
            int count = 1;
            foreach (OwnedApp ownedApp in ownedApps.OrderBy(x => x.Name)) {
                ownedApp.Name = ownedApp.Name.SafeTrim();

                if (appSettings.IgnoredAppIDs.Contains(ownedApp.SteamAppID)) {
                    logger.LogInformation($"ID {ownedApp.SteamAppID} is ignored. Skipping {ownedApp.Name}");
                    continue;
                }
                // Making requests too quickly seems to cause a short soft ban from Steam's API

                SteamApp steamApp = steamApi.StoreClient.GetSteamApp(ownedApp.SteamAppID).Result;
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

                SteamGridGame steamGridGame = steamGridClient.GetGameBySteamID(ownedApp.SteamAppID);
                SteamGridImage gridImage = null;
                SteamGridImage heroImage = null;
                if (steamGridGame != null) {
                    gridImage = steamGridClient.GetBestGridImage(steamGridGame.ID);
                    heroImage = steamGridClient.GetBestHeroImage(steamGridGame.ID);
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
                    GridImage = gridImage,
                    HeroImage = heroImage,
                    HltbModel = matchedHltbModel,
                    ProtonDbGame = protonDbGame,
                    UserTags = userTags,
                    Delisted = delisted
                };

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
            //foreach(T remove in removeList) {
            //    repo.Remove(remove);
            //}

            //logger.LogInformation($"{type.Name} Sync - Added: {addList.Count()} - Updated: {updateList.Count()} - Removed: {removeList.Count()}");
            logger.LogInformation($"{type.Name} Sync - Added: {addList.Count()} - Updated: {updateList.Count()}");
        }
    }
}
