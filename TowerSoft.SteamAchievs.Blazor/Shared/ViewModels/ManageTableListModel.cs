using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class ManageTableListModel {
        public SteamGameModel SteamGame { get; set; }

        public TableListModel TableList { get; set; }

        public List<ManageTableColumnViewModel> Columns { get; set; }
        public List<TableRowViewModel> Rows { get; set; }
    }
}
