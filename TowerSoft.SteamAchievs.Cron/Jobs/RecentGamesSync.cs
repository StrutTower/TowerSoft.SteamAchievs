using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamTower;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    internal class RecentGamesSync {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steamApi;
        private readonly SteamSyncService steamDataService;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public RecentGamesSync(UnitOfWork uow, SteamApiClient steamApi, SteamSyncService steamDataService,
            IOptionsSnapshot<AppSettings> appSettings, ILogger<RecentGamesSync> logger) {
            this.uow = uow;
            this.steamDataService = steamDataService;
            this.steamApi = steamApi;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(RecentGamesSync)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(RecentGamesSync)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
           var games = steamApi.PlayerClient.GetRecentlyPlayedGames(appSettings.DefaultSteamUserID).Result;


            var userAppDatas = steamDataService.LoadSteamData(games);

            RecentGameRepository recentGameRepo = uow.GetRepo<RecentGameRepository>();

            List<RecentGame> existingRecentGames = recentGameRepo.GetAll();

            foreach(RecentGame existing in existingRecentGames) {
                if(!userAppDatas.Select(x => x.SteamApp.ID).Contains(existing.SteamGameID)) {
                    recentGameRepo.Remove(existing);
                }
            }

            steamDataService.SyncSteamGames(userAppDatas);

            foreach (UserAppModel model in userAppDatas) {
                bool hasAchievements = false;
                if (model.GameStatsSchema != null && model.GameStatsSchema.Achievements.SafeAny())
                    hasAchievements = true;

                if (existingRecentGames.Select(x => x.SteamGameID).Contains(model.SteamApp.ID)) {
                    RecentGame recentGame = existingRecentGames.Single(x => x.SteamGameID == model.SteamApp.ID);
                    recentGame.HasAchievements = hasAchievements;
                    recentGame.TotalAchievements = model.GameStatsSchema?.Achievements?.Count ?? 0;
                    recentGame.CompletedAchievements = model.UserAchievements?.Where(x => x.Achieved).Count() ?? 0;
                    recentGameRepo.Update(recentGame);

                } else {
                    RecentGame newRecent = new() {
                        SteamGameID = model.SteamApp.ID,
                        FirstDetected = DateTime.Now,
                        HasAchievements = hasAchievements,
                        TotalAchievements = model.GameStatsSchema?.Achievements?.Count ?? 0,
                        CompletedAchievements = model.UserAchievements?.Where(x => x.Achieved).Count() ?? 0
                    };
                    recentGameRepo.Add(newRecent);
                }
            }

            steamDataService.RunAllSyncs(userAppDatas);
        }
    }
}
