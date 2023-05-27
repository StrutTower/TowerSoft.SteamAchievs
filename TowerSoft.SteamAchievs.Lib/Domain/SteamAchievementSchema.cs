using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamAchievementSchema {
        [Key]
        public string Key { get; set; }

        [Key]
        public long SteamGameID { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string IconGrayUrl { get; set; }
    }
}
