using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class Complication {
        [Autonumber]
        public long ID { get; set; }

        [Required, MaxLength(75)]
        public string Name { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsActive { get; set; }
    }
}
