using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TowerSoft.SteamAchievs.Cron.Models;
using TowerSoft.SteamAchievs.Cron.Utilities;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamTower;
using TowerSoft.SteamTower.Models;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class FullGameSync {
        private readonly SteamApiClient steamApi;
        private readonly SteamDataService steamDataService;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public FullGameSync(SteamApiClient steamApi, SteamDataService steamDataService,
            IOptions<AppSettings> appSettings, ILogger<FullGameSync> logger) {
            this.steamApi = steamApi;
            this.steamDataService = steamDataService;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public void StartJob() {
            Run();
        }

        public void Run() {
            List<UserAppModel> userAppModels = GetOwnedAppData();

            //string json = JsonConvert.SerializeObject(userAppModels);
            //File.WriteAllText(appSettings.CacheJsonPath, json);

            //List<UserAppModel> userAppModels =
            //    JsonConvert.DeserializeObject<List<UserAppModel>>(
            //        File.ReadAllText("tempData.json"));

            steamDataService.RunAllSyncs(userAppModels);
        }

        private List<UserAppModel> GetOwnedAppData() {
            logger.LogInformation("Pulling Owned App Data");

            List<OwnedApp> ownedApps = steamApi.PlayerClient.GetOwnedApps(appSettings.DefaultSteamUserID).Result;

            return steamDataService.LoadSteamData(ownedApps);
        }
    }
}
