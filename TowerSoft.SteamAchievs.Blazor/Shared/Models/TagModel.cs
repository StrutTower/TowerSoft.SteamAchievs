using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TagModel {
        public long ID { get; set; }

        public long? SteamGameID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, Display(Name = "Background Color")]
        public string BackgroundColor { get; set; }

        [Display(Name = "Complication")]
        public bool IsComplication { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
