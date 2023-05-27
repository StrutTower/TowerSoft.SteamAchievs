using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class Tag {
        [Autonumber]
        public long ID { get; set; }

        public long? SteamGameID { get; set; }

        public string Name { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsActive { get; set; }
    }
}
