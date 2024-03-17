using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class HomeDataService {
        private readonly HttpClient http;
        private readonly GameListDataService gameListDataService;

        public HomeDataService(HttpClient http, GameListDataService gameListDataService) {
            this.http = http;
            this.gameListDataService = gameListDataService;
        }

        public async Task<HomeViewModel> GetHomeViewModel() {
            return new HomeViewModel() {
                RecentGames = await http.GetFromJsonAsync<RecentGameModel[]>("api/RecentGame"),
                PerfectLostCount = await http.GetFromJsonAsync<int>("api/PerfectGame/IncompleteCount")
            };
        }

        public async Task<GameListViewModel> GetPerfectLostGameListModel() {
            PerfectGameModel[] incompleteGames = await http.GetFromJsonAsync<PerfectGameModel[]>("api/PerfectGame/Incomplete");
            return await gameListDataService.GetGameListModel(incompleteGames.Select(x => x.SteamGameID));
        }
    }
}
