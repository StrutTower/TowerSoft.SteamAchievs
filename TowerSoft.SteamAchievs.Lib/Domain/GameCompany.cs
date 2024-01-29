using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class GameCompany : IEquatable<GameCompany> {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long CompanyID { get; set; }

        public bool IsDeveloper { get; set; }

        public bool Equals(GameCompany? other) {
            return SteamGameID == other.SteamGameID && CompanyID == other.CompanyID;
        }

        public override int GetHashCode() {
            return SteamGameID.GetHashCode() ^ CompanyID.GetHashCode();
        }
    }
}
