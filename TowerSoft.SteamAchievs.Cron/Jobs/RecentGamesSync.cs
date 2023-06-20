using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Cron.Models;
using TowerSoft.SteamAchievs.Cron.Utilities;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamTower;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    internal class RecentGamesSync {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steamApi;
        private readonly SteamDataService steamDataService;
        private readonly AppSettings appSettings;

        public RecentGamesSync(UnitOfWork uow, SteamApiClient steamApi, SteamDataService steamDataService,
            IOptionsSnapshot<AppSettings> appSettings) {
            this.uow = uow;
            this.steamDataService = steamDataService;
            this.steamApi = steamApi;
            this.appSettings = appSettings.Value;
        }

        public void StartJob() {
            Run();
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
                if (!existingRecentGames.Select(x => x.SteamGameID).Contains(model.SteamApp.ID)) {
                    bool hasAchievements = false;
                    if (model.GameStatsSchema != null && model.GameStatsSchema.Achievements.SafeAny())
                        hasAchievements = true;

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
