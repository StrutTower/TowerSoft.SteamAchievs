using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Lib.Domain {
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
        PerfectPossible
    }
}
