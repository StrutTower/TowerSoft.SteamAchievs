using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class SteamGameListModel {
        public SteamGame SteamGame { get; set; }

        public GameDetails GameDetails { get; set; }

        public List<SteamUserAchievement> UserAchievements { get; set; }

        public List<Complication> Complications { get; set; }
    }
}
