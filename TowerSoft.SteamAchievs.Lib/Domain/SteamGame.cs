using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TowerSoft.SteamTower.Models;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGame {
        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string HeaderImageUrl { get; set; }

        public string BackgroundImageUrl { get; set; }

        public string GridImageUrl { get; set; }

        public string GridThumbUrl { get; set; }

        public string HeroImageUrl { get; set; }

        public string HeroThumbUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? PlaytimeMinutes { get; set; }

        public int? ReviewScore { get; set; }

        public string ReviewScoreDescription { get; set; }

        public int? MetacriticScore { get; set; }

        public DeckCompatibilityType? DeckCompatibility { get; set; }

        public bool WindowsSupported { get; set; }

        public bool LinuxSupported { get; set; }

        public bool MacSupported { get; set; }

        public bool IsFree { get; set; }


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
