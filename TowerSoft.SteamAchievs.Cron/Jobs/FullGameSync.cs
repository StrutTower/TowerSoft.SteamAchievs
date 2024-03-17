using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamTower;
using TowerSoft.SteamTower.Models;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class FullGameSync {
        private readonly SteamApiClient steamApi;
        private readonly SteamSyncService steamDataService;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public FullGameSync(SteamApiClient steamApi, SteamSyncService steamDataService,
            IOptions<AppSettings> appSettings, ILogger<FullGameSync> logger) {
            this.steamApi = steamApi;
            this.steamDataService = steamDataService;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(FullGameSync)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(FullGameSync)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        public void Run() {
            List<UserAppModel> userAppModels = GetOwnedAppData();

            //steamDataService.RunAllSyncs(userAppModels);

            IEnumerable<IEnumerable<UserAppModel>> batchList = userAppModels.Batch(100);
            foreach(IEnumerable<UserAppModel> batch in batchList) {
                steamDataService.RunAllSyncs(batch.ToList());
            }
        }

        private List<UserAppModel> GetOwnedAppData() {
            logger.LogInformation("Pulling Owned App Data");

            List<OwnedApp> ownedApps = steamApi.PlayerClient.GetOwnedApps(appSettings.DefaultSteamUserID).Result;

#if DEBUG
            //ownedApps = ownedApps.Where(x => x.Name.StartsWith("Torchlight", StringComparison.OrdinalIgnoreCase)).ToList();
#endif

            return steamDataService.LoadSteamData(ownedApps);

            //IEnumerable<IEnumerable<OwnedApp>> batchedApps = ownedApps.Batch(250);
            //List<UserAppModel> models = [];
            //foreach (IEnumerable<OwnedApp> batch in batchedApps) {
            //   models.AddRange(steamDataService.LoadSteamData(batch.ToList()));
            //}
            //return models;
        }
    }
}
