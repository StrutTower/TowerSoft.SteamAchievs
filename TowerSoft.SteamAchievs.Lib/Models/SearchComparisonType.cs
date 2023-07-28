using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public enum SearchComparisonType {
        [Display(Name = "=")]
        Equals,
        [Display(Name = "<>")]
        NotEquals,
        [Display(Name = "Null")]
        NullValue
    }
}
