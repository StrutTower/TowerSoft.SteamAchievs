using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public enum AchievementCompletionStatus {
        Perfect = 1,
        [Display(Name = "Incomplete (Have at least 1 achievement)")]
        Incomplete,
        [Display(Name = "Not Started (No Achievements)")]
        NotStarted,
        [Display(Name = "Not Perfect")]
        NotPerfect
    }
}
