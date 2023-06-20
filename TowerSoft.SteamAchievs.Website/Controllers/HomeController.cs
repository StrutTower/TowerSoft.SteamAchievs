using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.Services;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class HomeController : CustomController {
        private readonly UnitOfWork uow;
        private readonly HomeDataService homeDataService;
        private readonly GameDataService gameDataService;

        public HomeController(UnitOfWork uow, HomeDataService homeDataService, GameDataService gameDataService) {
            this.uow = uow;
            this.homeDataService = homeDataService;
            this.gameDataService = gameDataService;
        }

        public IActionResult Index() {
            return View(homeDataService.GetHomeViewModel());
        }

        public IActionResult GameList(GameListType id) {
            return View(gameDataService.GetGameListModel(id));
        }
    }
}
