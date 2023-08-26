using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class HomeViewModel {
        public List<RecentGame> RecentGames { get; set; }
        public List<PerfectLostGameModel> PerfectLostGames { get; internal set; }
    }
}
