using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableListManageModel {
        public SteamGameModel SteamGame { get; set; }

        public List<TableListModel> TableLists { get; set; }
    }
}
