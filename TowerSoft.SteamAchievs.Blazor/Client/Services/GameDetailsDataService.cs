using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class GameDetailsDataService {
        private readonly HttpClient http;

        public GameDetailsDataService(HttpClient http) {
            this.http = http;
        }

        public async Task AddOrUpdateGameDetails(GameDetailsModel gameDetails) {
            await http.PostAsJsonAsync("api/GameDetails", gameDetails);
        }
    }
}
