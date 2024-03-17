using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TableDataModel {
        public long TableRowID { get; set; }

        public long TableColumnID { get; set; }

        [MaxLength(200)]
        public string? Text { get; set; }

        public long? Number { get; set; }

        public double? Double { get; set; }

        public DateTime? DateTime { get; set; }

        public long? ChoiceID { get; set; }
    }
}
