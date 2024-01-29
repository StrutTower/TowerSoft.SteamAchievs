using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class CompanyModel {
        public long ID { get; set; }

        public string Name { get; set; }

        public long? RedirectToID { get; set; }

        public bool IsDeveloper { get; set; }
    }
}
