using TowerSoft.SteamAchievs.Blazor.Shared.Enums;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableRowColEditModel {
        public long ColumnID { get; set; }
        public string Title { get; set; }
        public ColumnDataType DataType { get; set; }
        public int SortValue { get; set; }
        public TableDataModel? Data { get; set; }
        public List<TableColumnChoiceModel> Choices { get; set; }
    }
}
