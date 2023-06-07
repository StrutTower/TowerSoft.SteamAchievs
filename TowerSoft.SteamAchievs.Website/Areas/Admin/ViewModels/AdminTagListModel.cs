using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TowerSoft.SteamAchievs.Website.Areas.Admin.ViewModels {
    public class AdminTagListModel {
        public List<TagGameModel> Tags { get; set; }

        public SelectList GameList { get; set; }
    }
}
