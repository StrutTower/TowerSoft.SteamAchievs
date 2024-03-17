using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableListViewModel {
        public TableListModel TableList { get; set; }
        public List<TableRowViewModel> Rows { get; set; }
    }
}
