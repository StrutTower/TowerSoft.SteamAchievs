using System.ComponentModel.DataAnnotations;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamAchievementSchema : IEquatable<SteamAchievementSchema> {
        [Key]
        public string KeyName { get; set; }

        [Key]
        public long SteamGameID { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string IconGrayUrl { get; set; }

        public double GlobalCompletionPercentage { get; set; }

        public bool Equals(SteamAchievementSchema? other) {
            return other != null && KeyName == other.KeyName && SteamGameID == other.SteamGameID;
        }

        public override int GetHashCode() {
            return KeyName.GetHashCode() ^ SteamGameID.GetHashCode();
        }
    }
}
