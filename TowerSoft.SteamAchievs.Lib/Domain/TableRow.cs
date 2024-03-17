using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class TableRow {
        [Autonumber]
        public long ID { get; set; }

        public long TableListID { get; set; }

        public bool? IsComplete { get; set; }

        public bool IsActive { get; set; }
    }
}
