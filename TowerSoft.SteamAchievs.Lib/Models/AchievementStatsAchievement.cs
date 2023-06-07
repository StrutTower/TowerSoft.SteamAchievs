using Newtonsoft.Json;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public class AchievementStatsAchievement {
        [JsonProperty("apiName")]
        public string AchievementKey { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
