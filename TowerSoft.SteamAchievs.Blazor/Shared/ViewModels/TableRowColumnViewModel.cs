using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableRowColumnViewModel {
        public TableColumnModel Column { get; set; }
        public TableDataModel? Value { get; set; }

        public TableColumnChoiceModel SelectedChoice { get; set; }
        public long RowID { get; set; }
    }
}
