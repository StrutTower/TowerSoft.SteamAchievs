using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class TableColumnChoice {
        [Autonumber]
        public long ID { get; set; }

        public long TableColumnID { get; set; }

        public string Title { get; set; }

        public int SortValue { get; set; }

        public bool IsActive { get; set; }
    }
}
