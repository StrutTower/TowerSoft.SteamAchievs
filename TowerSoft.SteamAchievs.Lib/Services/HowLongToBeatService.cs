using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using TowerSoft.SteamAchievs.Lib.Models;

namespace TowerSoft.SteamAchievs.Lib.Services {
    public class HowLongToBeatService {
        private readonly HttpClient httpClient;

        public HowLongToBeatService(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task<List<HltbModel>> Search(string gameName) {
            string gameNameParsed = Regex.Replace(gameName, "\\(\\d{4}\\)$", "");
            gameNameParsed = gameNameParsed.Replace("™", "");

            string[] nameParts = gameNameParsed.Split(' ');

            string searchTerms = "\"" + string.Join("\",\"", nameParts) + "\"";

            string postModel = "{\"searchType\":\"games\",\"searchTerms\":[" + searchTerms +
                "],\"searchPage\":1,\"size\":20,\"searchOptions\":{\"games\":{\"userId\":0,\"platform\":\"\",\"sortCategory\":\"popular\"," +
                "\"rangeCategory\":\"main\",\"rangeTime\":{\"min\":null,\"max\":null},\"gameplay\":{\"perspective\":\"\",\"flow\":\"\"," +
                "\"genre\":\"\"},\"rangeYear\":{\"min\":\"\",\"max\":\"\"},\"modifier\":\"\"},\"users\":{\"sortCategory\":\"postcount\"}," +
                "\"filter\":\"\",\"sort\":0,\"randomizer\":0}}";

            var result = httpClient.PostAsync("api/search", new StringContent(postModel, Encoding.UTF8, "application/json")).Result;

            string json = await result.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(json);

            List<HltbModel> games = obj["data"].ToObject<List<HltbModel>>();

            if (games.Count == 0 && gameNameParsed.Contains(":")) {
                string shortName = gameNameParsed.Split(":").First();
                return await Search(shortName);
            }

            return games;
        }
    }
}
