using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class GameDataService {
        private readonly HttpClient http;
        private readonly AchievementDataService achievementDataService;

        public GameDataService(HttpClient http, AchievementDataService achievementDataService) {
            this.http = http;
            this.achievementDataService = achievementDataService;
        }

        public async Task<SteamGameModel> GetSteamGame(long id) {
            return await http.GetFromJsonAsync<SteamGameModel>("SteamGame/" + id);
        }

        public async Task<GameViewModel> GetGameViewModel(long id) {
            GameViewModel model = new() {
                SteamGame = await http.GetFromJsonAsync<SteamGameModel>("SteamGame/" + id),
                GameDetails = await http.GetFromJsonAsync<GameDetailsModel>("GameDetails/" + id),
                Descriptions = await http.GetFromJsonAsync<SteamGameDescriptionsModel>("SteamGameDescriptions/" + id),
                Achievements = await achievementDataService.GetBySteamGameID(id),
                ComplicationTags = new(),
                Categories = new(),
                UserTags = new()
            };

            if (model?.Achievements?.Any() == true) {
                foreach (AchievementViewModel aModel in model.Achievements) {
                    List<TagModel> complicationTags = aModel.AchievementTags.Where(x => x.IsComplication).ToList();

                    foreach (TagModel tag in complicationTags) {
                        if (!model.ComplicationTags.Select(x => x.ID).Contains(tag.ID)) {
                            model.ComplicationTags.Add(tag);
                        }
                    }
                }
            }

            SteamGameCategoryModel[]? gameCategories = await http.GetFromJsonAsync<SteamGameCategoryModel[]>("SteamGameCategory/SteamGameID/" + id);
            if (gameCategories?.Any() == true) {
                Dictionary<long, SteamCategoryModel> categories = (await http.GetFromJsonAsync<SteamCategoryModel[]>("SteamCategory")).ToDictionary(x => x.ID);

                foreach (SteamGameCategoryModel gameCategory in gameCategories) {
                    SteamCategoryModel category = categories[gameCategory.SteamCategoryID];
                    model.Categories.Add(category);
                }
            }

            SteamGameUserTagModel[]? gameUserTags = await http.GetFromJsonAsync<SteamGameUserTagModel[]>("SteamGameUserTag/SteamGameID/" + id);
            if (gameUserTags?.Any() == true) {
                Dictionary<long, SteamUserTagModel> userTags = (await http.GetFromJsonAsync<SteamUserTagModel[]>("SteamUserTag")).ToDictionary(x => x.ID);

                foreach (SteamGameUserTagModel gameUserTag in gameUserTags) {
                    SteamUserTagModel userTag = userTags[gameUserTag.SteamUserTagID];
                    model.UserTags.Add(userTag);
                }
            }

            return model;
        }

        public async Task<GameViewModel> ResyncGame(long id) {
            await http.GetAsync("SteamGame/Resync/" + id);
            return await GetGameViewModel(id);
        }
    }
}
