using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class AchievementTag : IEquatable<AchievementTag> {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public string AchievementKey { get; set; }

        [Key]
        public long TagID { get; set; }


        public override int GetHashCode() {
            return SteamGameID.GetHashCode() ^ AchievementKey.GetHashCode() ^ TagID.GetHashCode();
        }

        public override bool Equals(object? obj) {
            return obj is AchievementTag other && SteamGameID == other.SteamGameID && AchievementKey == other.AchievementKey && TagID == other.TagID;
        }

        public bool Equals(AchievementTag? other) {
            return SteamGameID == other.SteamGameID && AchievementKey == other.AchievementKey && TagID == other.TagID;
        }
    }
}
