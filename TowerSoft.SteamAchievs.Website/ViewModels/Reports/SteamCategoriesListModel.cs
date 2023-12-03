using System.Collections.Generic;

namespace TowerSoft.SteamAchievs.Website.ViewModels.Reports {
    public class SteamCategoriesListModel {
        public string ResultsActionName { get; set; }

        public List<SteamCategoryReportModel> SteamCategories { get; internal set; }
    }
}
