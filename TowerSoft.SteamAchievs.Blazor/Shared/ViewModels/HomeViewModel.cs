using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class HomeViewModel {
        public RecentGameModel[] RecentGames { get; set; }

        public int? PerfectLostCount { get; set; }
    }
}
