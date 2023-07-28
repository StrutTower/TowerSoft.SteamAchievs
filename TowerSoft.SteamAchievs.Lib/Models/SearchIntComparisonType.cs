using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public enum SearchIntComparisonType {
        [Display(Name = "=")]
        Equals,
        [Display(Name = "<>")]
        NotEquals,
        [Display(Name = "<")]
        LessThan,
        [Display(Name = "<=")]
        LessThanOrEqual,
        [Display(Name = ">")]
        GreaterThan,
        [Display(Name = ">=")]
        GreaterThanOrEqual,
        [Display(Name = "Null")]
        NullValue
    }
}
