using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Website.Infrastructure;

namespace TowerSoft.SteamAchievs.Website.Areas.Admin.Controllers {
    [Area("Admin")]
    [Role(RoleType.Admin)]
    public class HomeController : CustomController {
        public IActionResult Index() {
            return View();
        }
    }
}
