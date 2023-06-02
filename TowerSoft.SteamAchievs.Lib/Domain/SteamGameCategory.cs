using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGameCategory : IEquatable<SteamGameCategory> {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long SteamCategoryID { get; set; }

        public bool Equals(SteamGameCategory? other) {
            return other != null && SteamGameID == other.SteamGameID && SteamCategoryID == other.SteamCategoryID;
        }

        public override int GetHashCode() {
            return SteamGameID.GetHashCode() ^ SteamCategoryID.GetHashCode();
        }
    }
}
