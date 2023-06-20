using Microsoft.AspNetCore.Mvc.Rendering;
using TowerSoft.SteamAchievs.Lib.Models;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class ReportsIndexModel {
        public SearchModel SearchModel { get; set; }

        public SelectList SearchComparisonList { get; set; }
    }
}
