using System.ComponentModel.DataAnnotations;
using TowerSoft.SteamAchievs.Blazor.Shared.Enums;

namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class TableListModel {
        public long ID { get; set; }

        public long SteamGameID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ListType ListType { get; set; }

        public bool IsActive { get; set; }
    }
}
