using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public enum SearchComparisonType {
        [Display(Name = "=")]
        Equals,
        [Display(Name = "<")]
        LessThan,
        [Display(Name = "<=")]
        LessThanOrEqual,
        [Display(Name = ">")]
        GreaterThan,
        [Display(Name = ">=")]
        GreaterThanOrEqual,
    }
}
