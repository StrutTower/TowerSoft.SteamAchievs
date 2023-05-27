using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class PerfectGame {
        [Key]
        public long SteamGameID { get; set; }

        public DateTime? PerfectedOn { get; set; }

        public bool IsIncompleteNow { get; set; }
    }
}
