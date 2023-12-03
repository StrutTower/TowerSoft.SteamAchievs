using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public class SearchModel {
        public string? Name { get; set; }

        [Display(Name = "Has Achievements")]
        public SearchBooleanType HasAchievements { get; set; }

        [Display(Name = "Achievement Completion Status")]
        public AchievementCompletionStatus? AchievementCompletionStatus { get; set; }

        [Display(Name = "Perfect Possible")]
        public SearchNullableBooleanType PerfectPossible { get; set; }
        public SearchComparisonType PerfectPossibleComparison { get; set; }

        public SearchNullableBooleanType Finished { get; set; }
        public SearchComparisonType FinishedComparison { get; set; }

        public SearchNullableBooleanType Delisted { get; set; }
        public SearchComparisonType DelistedComparison { get; set; }


        [Display(Name = "Play Next Score")]
        public int? PlayNextScore { get; set; }
        public SearchIntComparisonType PlayNextScoreComparison { get; set; }


        [Display(Name = "Steam Review Score", Prompt = "0-10"), Range(0, 10)]
        public int? SteamReviewScore { get; set; }
        public SearchIntComparisonType SteamReviewScoreComparison { get; set; }


        [Display(Name = "Metacritic Score", Prompt = "0-100"), Range(0, 100)]
        public int? MetacriticScore { get; set; }
        public SearchIntComparisonType MetacriticScoreComparison { get; set; }


        [Display(Name = "Main Story Time")]
        public double? MainTime { get; set; }
        public SearchIntComparisonType MainTimeComparison { get; set; }


        [Display(Name = "Main & Sides Time")]
        public double? MainAndSidesTime { get; set; }
        public SearchIntComparisonType MainAndSidesTimeComparison { get; set; }


        [Display(Name = "Completionist Time")]
        public double? CompletionistTime { get; set; }
        public SearchIntComparisonType CompletionistTimeComparison { get; set; }


        [Display(Name = "All Styles Time")]
        public double? AllStylesTime { get; set; }
        public SearchIntComparisonType AllStylesTimeComparison { get; set; }
    }
}
