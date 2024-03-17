using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class ManageChoicesModel {
        public TableColumnModel TableColumn { get; set; }
        public List<TableColumnChoiceModel>? Choices { get; set; }
        public TableListModel? TableList { get; set; }
        public SteamGameModel? SteamGame { get; set; }
    }
}
