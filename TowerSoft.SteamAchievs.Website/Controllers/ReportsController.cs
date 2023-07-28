using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamAchievs.Website.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class ReportsController : Controller {
        private readonly UnitOfWork uow;
        private readonly GameDataService gameDataService;

        public ReportsController(UnitOfWork uow, GameDataService gameDataService) {
            this.uow = uow;
            this.gameDataService = gameDataService;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Search(SearchModel model) {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().AdvancedSearch(model);
            return View("GameList", gameDataService.GetGameListModel(games, "Advanced Search"));
        }

        public IActionResult NullPerfectPossible() {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetWithNullPerfectPossible();
            return View("GameList", gameDataService.GetGameListModel(games, "Missing Perfect Possible"));
        }

        public IActionResult NullPlayPriority() {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetWithNullPlayPriority();
            return View("GameList", gameDataService.GetGameListModel(games, "Missing Play Priority"));
        }

        public IActionResult NullFinished() {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetWithNullFinished();
            return View("GameList", gameDataService.GetGameListModel(games, "Missing Finished"));
        }

        public IActionResult NullTimes() {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetWithoutHltbData();
            return View("GameList", gameDataService.GetGameListModel(games, "Missing How Long to Beat Data"));
        }
    }
}
