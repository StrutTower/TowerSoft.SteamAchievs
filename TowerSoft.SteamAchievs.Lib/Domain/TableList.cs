using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class TableList {
        [Autonumber]
        public long ID { get; set; }

        public long SteamGameID { get; set; }

        public string Name { get; set; }

        public ListType ListType { get; set; }

        public bool IsActive { get; set; }
    }
}
