using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class GameComplication {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long ComplicationID { get; set; }

        public string Description { get; set; }
    }
}
