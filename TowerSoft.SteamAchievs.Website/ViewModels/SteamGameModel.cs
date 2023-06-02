using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class SteamGameModel {
        public SteamGame SteamGame { get; set; }

        public GameDetails GameDetails { get; set; }

        public List<AchievementModel> Achievements { get; set; }

        public List<Complication> Complications { get; set; }
    }
}
