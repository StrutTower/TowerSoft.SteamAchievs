using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class EditAchievementModel {
        public long SteamGameID { get; set; }

        public string AchievementKey { get; set; }

        [Display(Name = "Description Override")]
        public string DescriptionOverride { get; set; }

        [Display(Name = "Tags")]
        public List<long> TagIDs { get; set; }

        public SelectList TagList { get; set; }
    }
}
