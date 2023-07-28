using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamAchievs.Website.ViewModels;
using TowerSoft.SteamTower;
using TowerSoft.SteamTower.Models;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class GameController : CustomController {
        private readonly GameDataService gameDataService;
        private readonly SteamApiClient steamApiClient;
        private readonly SteamSyncService steamSyncService;
        private readonly AppSettings appSettings;

        public GameController(GameDataService gameDataService, SteamApiClient steamApiClient, SteamSyncService steamSyncService,
            IOptionsSnapshot<AppSettings> appSettings) {
            this.gameDataService = gameDataService;
            this.steamApiClient = steamApiClient;
            this.steamSyncService = steamSyncService;
            this.appSettings = appSettings.Value;
        }

        public IActionResult View(long id) {
            return View(gameDataService.GetSteamGameModel(id));
        }

        [HttpGet]
        public IActionResult Edit(long id) {
            return PartialView(gameDataService.GetEditGameDetailsModel(id));
        }

        [HttpPost]
        public IActionResult Edit([Bind(Prefix = nameof(EditGameDetailsModel.GameDetails))] GameDetails model) {
            if (ModelState.IsValid) {
                GameDetails gameDetails = gameDataService.UpdateGameDetails(model);
                return Json(new {
                    success = true,
                    message = "Details updated",
                    view = RenderViewAsync("_GameDetails", gameDetails).Result
                });
            }
            return JsonModelErrorList(ModelState);
        }

        public IActionResult Sync(long id) {
            List<OwnedApp> ownedApps = steamApiClient.PlayerClient.GetOwnedApps(appSettings.DefaultSteamUserID).Result;
            List<OwnedApp> ownedApp = ownedApps.Where(x => x.SteamAppID == id).ToList();

            if (!ownedApp.SafeAny())
                return Message($"Unable to find a owned game with the ID {id}.");

            var userAppData = steamSyncService.LoadSteamData(ownedApp);

            steamSyncService.RunAllSyncs(userAppData);
            TempData["message"] = "Game Resynced";
            return RedirectToAction(nameof(View), new { id = id });
        }
    }
}
