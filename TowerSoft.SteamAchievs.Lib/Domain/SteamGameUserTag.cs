using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGameUserTag : IEquatable<SteamGameUserTag> {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long SteamUserTagID { get; set; }

        public bool Equals(SteamGameUserTag? other) {
            return other != null && SteamGameID == other.SteamGameID && SteamUserTagID == other.SteamUserTagID;
        }

        public override int GetHashCode() {
            return SteamGameID.GetHashCode() ^ SteamUserTagID.GetHashCode();
        }
    }
}
