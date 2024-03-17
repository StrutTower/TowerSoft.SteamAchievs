using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TableRowModel {
        public long ID { get; set; }

        public long TableListID { get; set; }

        public bool IsComplete { get; set; }

        public bool IsActive { get; set; }
    }
}
