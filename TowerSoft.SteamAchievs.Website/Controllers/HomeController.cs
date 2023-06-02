using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamTower;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class HomeController : Controller {
        private readonly UnitOfWork uow;
        private readonly SteamApiClient steam;
        private readonly GameDataService gameDataService;

        public HomeController(UnitOfWork uow, SteamApiClient steam, GameDataService gameDataService) {
            this.uow = uow;
            this.steam = steam;
            this.gameDataService = gameDataService;
        }

        public async Task<IActionResult> Index() {
            return RedirectToAction("View", "Game", new { id = 377160 });
            //return View();
        }

        public IActionResult GameList(GameListType id) {
            return View(gameDataService.GetGameListModel(id));
        }
    }
}
