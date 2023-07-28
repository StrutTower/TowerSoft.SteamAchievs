using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class SteamGameModel {
        public SteamGame SteamGame { get; set; }

        public GameDetails GameDetails { get; set; }

        public SteamGameDescriptions GameDescriptions { get; set; }

        public List<AchievementModel> Achievements { get; set; }

        public List<Tag> ComplicationTags { get; set; }

        public List<SteamUserTagModel> UserTags { get; set; }

        public List<SteamCategoryModel> Categories { get; set; }
        public List<Tag> AchievementTags { get; internal set; }
    }
}
