using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class AchievementTag {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public string AchievementKey { get; set; }

        [Key]
        public long TagID { get; set; }
    }
}
