using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TableColumnChoiceModel {
        public long ID { get; set; }

        public long TableColumnID { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public int SortValue { get; set; }

        public bool IsActive { get; set; }
    }
}
