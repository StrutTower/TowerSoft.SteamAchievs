using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class RarestAchievementGameModel {
        public SteamGameModel Game { get; set; }

        public List<AchievementViewModel> Achievements { get; set; }

        public int AchievedCount { get; set; }
        public double AchievProgressWidth { get; set; }
        public bool Perfect { get; set; }

        public double RarestAchievementPercentage {
            get {
                if (Achievements == null || Achievements.Count == 0) return 0;
                return Achievements.OrderBy(x => x.Schema.GlobalCompletionPercentage).First().Schema.GlobalCompletionPercentage;
            }
        }

        public GameDetailsModel Details { get; set; }
    }
}
