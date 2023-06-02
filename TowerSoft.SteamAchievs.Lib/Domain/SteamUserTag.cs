using System.ComponentModel.DataAnnotations;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamUserTag : IEquatable<SteamUserTag> {
        [Key]
        public long ID { get; set; }

        [Required, MaxLength(45)]
        public string Name { get; set; }

        public bool Equals(SteamUserTag? other) {
            return other != null && ID == other.ID;
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }
    }
}
