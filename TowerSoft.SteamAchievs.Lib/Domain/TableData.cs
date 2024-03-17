using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class TableData {
        [Key]
        public long TableRowID { get; set; }

        [Key]
        public long TableColumnID { get; set; }

        public string? Text { get; set; }

        public long? Number { get; set; }

        public double? DoubleNumber { get; set; }

        public DateTime? DateTime { get; set; }

        public long? ChoiceID { get; set; }
    }
}
