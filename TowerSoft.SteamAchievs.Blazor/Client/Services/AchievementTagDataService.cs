namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class AchievementTagDataService {
        private readonly HttpClient http;

        public AchievementTagDataService(HttpClient http) {
            this.http = http;
        }
    }
}
