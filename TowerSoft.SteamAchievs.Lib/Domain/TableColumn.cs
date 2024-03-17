using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class TableColumn {
        [Autonumber]
        public long ID { get; set; }

        public long TableListID { get; set; }

        public string Title { get; set; }

        public ColumnDataType DataType { get; set; } 

        public int SortValue { get; set; }

        public bool IsActive { get; set; }
    }
}
