using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Website.Areas.Admin.Services;
using TowerSoft.SteamAchievs.Website.Areas.Admin.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Areas.Admin.Controllers {
    [Area("Admin")]
    public class TagController : Controller {
        private readonly AdminTagDataService adminTagDataService;

        public TagController(AdminTagDataService adminTagDataService) {
            this.adminTagDataService = adminTagDataService;
        }

        public async Task<IActionResult> Index() {
            return View(await adminTagDataService.GetAdminTagListModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? id = null) {
            return View(await adminTagDataService.GetEditTagModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind(Prefix = nameof(EditTagModel.Tag))]Tag tag) {
            if (ModelState.IsValid) {
                await adminTagDataService.AddOrUpdateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(await adminTagDataService.GetEditTagModel(tag.ID, tag));
        }
    }
}
