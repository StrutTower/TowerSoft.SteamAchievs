using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.Services;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class SearchController : CustomController {
        private readonly UnitOfWork uow;
        private readonly GameDataService gameDataService;

        public SearchController(UnitOfWork uow, GameDataService gameDataService) {
            this.uow = uow;
            this.gameDataService = gameDataService;
        }

        [HttpGet]
        public IActionResult Basic(string q) {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().Search(q);
            return View("GameList", gameDataService.GetGameListModel(games, $"Search Results for '{q}'"));
        }
    }
}
