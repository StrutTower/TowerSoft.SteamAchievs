using TowerSoft.SteamAchievs.Blazor.Shared.Enums;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TableColumnModel {
        public long ID { get; set; }

        public long TableListID { get; set; }

        public string Title { get; set; }

        public ColumnDataType DataType { get; set; }

        public int SortValue { get; set; }

        public bool IsActive { get; set; }
    }
}
