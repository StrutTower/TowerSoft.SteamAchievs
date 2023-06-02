using Newtonsoft.Json;
using TowerSoft.SteamAchievs.Lib.Models;

namespace TowerSoft.SteamAchievs.Lib.Services {
    public class ProtonDbService {
        private readonly HttpClient httpClient;

        public ProtonDbService(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task<ProtonDbGame?> GetGame(long steamGameID) {
            try {
                string json = await httpClient.GetStringAsync($"api/v1/reports/summaries/{steamGameID}.json");
                ProtonDbGame protonDbGame = JsonConvert.DeserializeObject<ProtonDbGame>(json);
                return protonDbGame;
            } catch {
                return null;
            }
        }
    }
}
