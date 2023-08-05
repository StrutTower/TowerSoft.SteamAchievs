using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class GameDetails {
        [Key]
        public long SteamGameID { get; set; }

        public long? HowLongToBeatID { get; set; }

        [Display(Name = "Perfect Possible")]
        public bool? PerfectPossible { get; set; }

        [Display(Name = "Play Next Score")]
        public int? PlayNextScore { get; set; }

        public bool? Finished { get; set; }

        [Display(Name = "Notify If Achievemnts Are Added")]
        public bool NotifyIfAchievementsAdded { get; set; }

        public string? ProtonDbRating { get; set; }

        public double? MainStoryTime { get; set; }

        public double? MainAndSidesTime { get; set; }

        public double? CompletionistTime { get; set; }

        public double? AllStylesTime { get; set; }
    }
}
