using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableRowViewModel {
        public TableRowModel Row { get; set; }

        public List<TableRowColumnViewModel> Columns { get; set; }
    }
}
