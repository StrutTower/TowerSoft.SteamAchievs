using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamCategory {
        [Autonumber]
        public long ID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
