using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TowerSoft.SteamTower.Models;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGame : IEquatable<SteamGame> {
        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public string RequiredAge { get; set; }

        public string ControllerSupport { get; set; }

        public bool Delisted { get; set; }

        public string? HeaderImageUrl { get; set; }

        public string? CapsuleImageUrl { get; set; }

        public string? CapsuleImageV5Url { get; set; }

        public string IconUrl { get; set; }

        public string? BackgroundUrl { get; set; }

        public string? BackgroundRawUrl { get; set; }

        public string? GridImageUrl { get; set; }

        public string? GridThumbUrl { get; set; }

        public string? HeroImageUrl { get; set; }

        public string? HeroThumbUrl { get; set; }

        public string? Website { get; set; }

        public bool? WindowsSupported { get; set; }

        public bool? MacSupported { get; set; }

        public bool? LinuxSupported { get; set; }

        public int? MetacriticScore { get; set; }

        public int RecommendationCount { get; set; }

        public int ReviewScore { get; set; }

        public string ReviewDescription { get; set; }

        public int AchievmentCount { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool? IsComingSoon { get; set; }

        public DeckCompatibilityType? DeckCompatibility { get; set; }

        public long? RecentPlaytime { get; set; }

        public long TotalPlaytime { get; set; }

        public long WindowsTotalPlaytime { get; set; }

        public long MacTotalPlaytime { get; set; }

        public long LinuxTotalPlaytime { get; set; }

        public DateTime? LastPlayedOn { get; set; }


        public bool Equals([AllowNull] SteamGame other) {
            return other != null && ID == other.ID;
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj) {
            return obj != null && obj is SteamGame other && ID == other.ID;
        }

        public override string ToString() {
            return $"{ID}: {Name}";
        }
    }
}
