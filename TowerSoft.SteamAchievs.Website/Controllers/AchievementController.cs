using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Website.Infrastructure;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamAchievs.Website.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    public class AchievementController : CustomController {
        private readonly AchievementDataService achievementDataService;

        public AchievementController(AchievementDataService achievementDataService) {
            this.achievementDataService = achievementDataService;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(long steamGameID, string achievementKey) {
            return PartialView(achievementDataService.GetEditAchievementModel(steamGameID, achievementKey));
        }

        [HttpPost]
        public IActionResult Edit(EditAchievementModel model) {
            achievementDataService.UpdateAchievementDetails(model);

            string view = RenderViewAsync("_Achievement", achievementDataService.GetAchievementModel(model.SteamGameID, model.AchievementKey)).Result;

            return Json(new {
                success = true,
                message = "Achievement Updated",
                view
            });
        }
    }
}
