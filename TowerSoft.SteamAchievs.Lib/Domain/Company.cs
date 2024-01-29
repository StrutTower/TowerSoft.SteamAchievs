using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class Company {
        [Autonumber]
        public long ID { get; set; }

        public string Name { get; set; }

        public long? RedirectToID { get; set; }


        [NotMapped]
        public bool IsDeveloper { get; set; }
    }
}
