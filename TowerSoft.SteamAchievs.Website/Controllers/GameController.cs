using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Website.Services;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class GameController : Controller {
        private readonly GameDataService gameDataService;

        public GameController(GameDataService gameDataService) {
            this.gameDataService = gameDataService;
        }

        public IActionResult View(long id) {
            return View(gameDataService.GetSteamGameModel(id));
        }
    }
}
