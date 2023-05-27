using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGameCategory {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long SteamCategoryID { get; set; }
    }
}
