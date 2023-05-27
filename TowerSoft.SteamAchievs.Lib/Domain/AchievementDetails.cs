using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class AchievementDetails {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public string AchievementKey { get; set; }

        public string Description { get; set; }
    }
}
