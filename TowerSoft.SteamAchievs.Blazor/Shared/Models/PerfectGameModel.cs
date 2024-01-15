using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class PerfectGameModel {
        public long SteamGameID { get; set; }

        public DateTime? PerfectedOn { get; set; }

        public bool IsIncompleteNow { get; set; }
    }
}
