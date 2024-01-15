using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class GameListItemModel {
        public SteamGameModel SteamGame { get; set; }
        public GameDetailsModel Details { get; set; }
        public List<SteamUserAchievementModel> UserAchievements { get; set; }
        public int AchievedCount { get; set; }
        public double AchievProgressWidth { get; set; }
        public bool Perfect { get; set; }

        public List<TagModel> ComplicationTags { get; set; } = new();
    }
}
