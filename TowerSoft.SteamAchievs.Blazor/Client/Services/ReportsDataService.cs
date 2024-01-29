using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class ReportsDataService {
        private readonly HttpClient http;
        private readonly GameListDataService gameListDataService;

        public ReportsDataService(HttpClient http, GameListDataService gameListDataService) {
            this.http = http;
            this.gameListDataService = gameListDataService;
        }

        public async Task<GameListModel> GetMissingPerfectPossibleGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/Reports/MissingPerfectPossible");
            return await gameListDataService.GetGameListModel(games);
        }

        public async Task<GameListModel> GetMissingPlayNextGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/Reports/MissingPlayNext");
            return await gameListDataService.GetGameListModel(games);
        }
    }
}
