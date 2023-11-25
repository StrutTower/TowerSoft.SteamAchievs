using Microsoft.Extensions.Logging;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamTower;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    internal class PerfectGameScan {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steamApi;
        private readonly SteamSyncService steamSyncService;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public PerfectGameScan(UnitOfWork uow, SteamApiClient steamApi, SteamSyncService steamSyncService, ILogger<PerfectGameScan> logger) {
            this.uow = uow;
            this.steamApi = steamApi;
            this.steamSyncService = steamSyncService;
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(PerfectGameScan)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(PerfectGameScan)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
            PerfectGameRepository repo = uow.GetRepo<PerfectGameRepository>();
            SteamUserAchievementRepository achievementRepo = uow.GetRepo<SteamUserAchievementRepository>();

            Dictionary<long, PerfectGame> storedPerfectGames = repo.GetAll().ToDictionary(x => x.SteamGameID);
            Dictionary<long, SteamGame> currentPerfectGames = uow.GetRepo<SteamGameRepository>().GetByGameListType(GameListType.Perfect).ToDictionary(x => x.ID);

            foreach (SteamGame current in currentPerfectGames.Values) {

                // Check if any incomplete games need to be set back to complete
                if (storedPerfectGames.ContainsKey(current.ID)) {
                    PerfectGame perfectGame = storedPerfectGames[current.ID];
                    if (currentPerfectGames.ContainsKey(current.ID) && perfectGame.IsIncompleteNow) {
                        perfectGame.IsIncompleteNow = false;
                        repo.Update(perfectGame);
                    } else {
                        continue;
                    }
                } else {

                    // Add the new perfect game to storage
                    List<SteamUserAchievement> achievements = achievementRepo.GetBySteamGameID(current.ID);
                    DateTime? lastDate = achievements.OrderByDescending(x => x.AchievedOn).First().AchievedOn;

                    PerfectGame newPerfectGame = new() {
                        SteamGameID = current.ID,
                        PerfectedOn = lastDate
                    };
                    repo.Add(newPerfectGame);
                }
            }

            // Check if any games need to be marked as incomplete
            foreach (PerfectGame stored in storedPerfectGames.Values) {
                if (currentPerfectGames.ContainsKey(stored.SteamGameID)) continue;

                stored.IsIncompleteNow = true;
                repo.Update(stored);
            }
        }
    }
}
