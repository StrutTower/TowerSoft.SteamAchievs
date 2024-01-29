using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class GameCompanyModel {
        public long SteamGameID { get; set; }
        public long CompanyID { get; set; }
        public bool IsDeveloper { get; set; }
    }
}
