using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class EditAchievementDetailsModel {
        public long SteamGameID { get; set; }

        public string AchievementKey { get; set; }

        [Display(Name = "Manual Description")]
        public string? ManualDescription { get; set; }

        [Display(Name = "Tags")]
        public IEnumerable<string> TagIDstrings { get; set; }

        public IEnumerable<long> TagIDs {
            get {
                return TagIDstrings.Select(x => long.Parse(x)).ToArray();
            }
        }
    }
}
