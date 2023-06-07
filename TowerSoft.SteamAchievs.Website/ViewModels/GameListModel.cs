using System.Collections.Generic;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class GameListModel {
        public string PageTitle { get; set; }

        public List<SteamGameListModel> Games { get; set; }

        public List<KeyValuePair<string, string>> SortOptions { get; set; }
    }
}
