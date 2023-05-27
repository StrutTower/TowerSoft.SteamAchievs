using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class RecentGame {
        [Key]
        public long SteamGameID { get; set; }

        public DateTime FirstDetected { get; set; }

        public bool HasAchievements { get; set; }

        public int CompletedAchievements { get; set; }

        public int TotalAchievements { get; set; }
    }
}
