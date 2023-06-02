using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamUserAchievement : IEquatable<SteamUserAchievement> {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public string KeyName { get; set; }

        public bool Achieved { get; set; }

        public DateTime? AchievedOn { get; set; }

        public bool Equals(SteamUserAchievement? other) {
            return other != null && SteamGameID == other.SteamGameID && KeyName == other.KeyName;
        }

        public override int GetHashCode() {
            return SteamGameID.GetHashCode() ^ KeyName.GetHashCode();
        }
    }
}
