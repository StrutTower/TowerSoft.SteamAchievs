using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGameUserTag {
        [Key]
        public long SteamGameID { get; set; }

        [Key]
        public long SteamUserTagID { get; set; }
    }
}
