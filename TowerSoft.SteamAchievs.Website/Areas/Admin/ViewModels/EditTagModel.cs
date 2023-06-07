using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.Areas.Admin.ViewModels {
    public class EditTagModel {
        public Tag Tag { get; set; }

        public SteamGame SteamGame { get; set; }

        public bool ReturnToGameView { get; set; }
    }
}
