using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Client.Utilities;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class TagDataService {
        private readonly HttpClient http;

        public TagDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<TagModel> GetTag(long id) {
            return await http.GetFromJsonAsync<TagModel>("Tag/" + id);
        }

        public async Task<TagModel[]> GetActiveBySteamGameID(long steamGameID) {
            return await http.GetFromJsonAsync<TagModel[]>("Tag/Assigned/" + steamGameID);
        }

        public async Task<AdminTagListModel> GetAdminTagListModel() {
            AdminTagListModel model = new() { Tags = new() };

            TagModel[] tags = await http.GetFromJsonAsync<TagModel[]>("Tag");

            IEnumerable<long> steamGameIDs = tags.Where(x => x.SteamGameID.HasValue).Select(x => x.SteamGameID.Value);

            Dictionary<long, SteamGameModel> games = (await http.PostGetFromJson<SteamGameModel[]>("SteamGame/GetByIDs", steamGameIDs)).ToDictionary(x => x.ID);

            foreach (TagModel tag in tags.OrderBy(x => x.Name)) {
                TagViewModel tagViewModel = new() {
                    Tag = tag
                };

                if (tag.SteamGameID.HasValue && games.ContainsKey(tag.SteamGameID.Value)) {
                    tagViewModel.SteamGame = games[tag.SteamGameID.Value];
                }

                model.Tags.Add(tagViewModel);
            }

            return model;
        }

        public async Task AddOrUpdate(TagModel tag) {
            if (tag.ID == 0) {
                await http.PostAsJsonAsync("Tag", tag);
                
            } else {
                await http.PutAsJsonAsync("Tag", tag);
            }
        }
    }
}