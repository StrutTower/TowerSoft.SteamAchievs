using System.ComponentModel.DataAnnotations;
using TowerSoft.Repository.Attributes;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamUserTag {
        [Autonumber]
        public long ID { get; set; }

        [Required, MaxLength(45)]
        public string Name { get; set; }
    }
}
