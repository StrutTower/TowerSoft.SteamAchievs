using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamAchievs.Website.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class GameController : CustomController {
        private readonly GameDataService gameDataService;

        public GameController(GameDataService gameDataService) {
            this.gameDataService = gameDataService;
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
    }
}
