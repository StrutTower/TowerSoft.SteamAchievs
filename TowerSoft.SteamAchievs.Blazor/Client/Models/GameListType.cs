using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Blazor.Client.Models {
    public enum GameListType {
        [Display(Name = "All Games")]
        All = 1,
        [Display(Name = "Perfect Games")]
        Perfect,
        [Display(Name = "Non-Perfect Games")]
        NonPerfect,
        [Display(Name = "Games without Complications")]
        NoComplications,
        [Display(Name = "Incomplete Games")]
        Incomplete,
        [Display(Name = "Perfect Possible Games")]
        PerfectPossible,
        [Display(Name = "With Play Next Score")]
        HasPlayNextScore,
    }
}
