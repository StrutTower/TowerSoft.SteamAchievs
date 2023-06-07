using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Models;

namespace TowerSoft.SteamAchievs.Lib.Services {
    public class AchievementStatsService {
        private readonly HttpClient httpClient;
        private readonly string apiKey;

        public AchievementStatsService(HttpClient httpClient, string apiKey) {
            this.httpClient = httpClient;
            this.apiKey = apiKey;
        }

        public async Task<List<AchievementStatsAchievement>> GetAchievements(long steamGameID) {
            string url = $"https://api.achievementstats.com/games/{steamGameID}/achievements?key={apiKey}";
            string json = await httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<List<AchievementStatsAchievement>>(json);
        }
    }
}
