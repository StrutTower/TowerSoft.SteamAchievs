using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamCategory : IEquatable<SteamCategory> {
        [Key]
        public long ID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool Equals(SteamCategory? other) {
            return other != null && ID == other.ID;
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }
    }
}
