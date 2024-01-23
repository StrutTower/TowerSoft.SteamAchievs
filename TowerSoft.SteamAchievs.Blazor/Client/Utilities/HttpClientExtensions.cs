using System.Net.Http.Json;

namespace TowerSoft.SteamAchievs.Blazor.Client.Utilities {
    public static class HttpClientExtensions {
        public async static Task<T> PostGetFromJson<T>(this HttpClient http, string url, object data) {
            HttpResponseMessage response = await http.PostAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response from the API.");
            T model = await response.Content.ReadFromJsonAsync<T>();
            return model;
        }
    }
}
