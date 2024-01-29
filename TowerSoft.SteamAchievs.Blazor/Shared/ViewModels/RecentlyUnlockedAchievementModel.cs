using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class RecentlyUnlockedAchievementModel {
        public SteamAchievementSchemaModel Schema { get; set; }

        public SteamUserAchievementModel UserAchievement { get; set; }

        public SteamGameModel SteamGame { get; set; }
    }
}
