using Microsoft.VisualBasic;
using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Client.Pages.Admin;
using TowerSoft.SteamAchievs.Blazor.Client.Shared;
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
            return await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + id);
        }

        public async Task<GameViewModel> GetGameViewModel(long id) {
            GameViewModel model = new() {
                SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + id),
                Descriptions = await http.GetFromJsonAsync<SteamGameDescriptionsModel>("api/SteamGameDescriptions/" + id),
                Achievements = await achievementDataService.GetBySteamGameID(id),
                ComplicationTags = new(),
                Categories = new(),
                UserTags = new(),
            };

            try {
                model.GameDetails = await http.GetFromJsonAsync<GameDetailsModel>("api/GameDetails/" + id);
            } catch { }

            var companies = await http.GetFromJsonAsync<List<CompanyModel>>("api/Company/SteamGameID/" + id);
            model.Developers = companies.Where(x => x.IsDeveloper).ToList();
            model.Publishers = companies.Where(x => !x.IsDeveloper).ToList();

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

            SteamGameCategoryModel[]? gameCategories = await http.GetFromJsonAsync<SteamGameCategoryModel[]>("api/SteamGameCategory/SteamGameID/" + id);
            if (gameCategories?.Any() == true) {
                Dictionary<long, SteamCategoryModel> categories = (await http.GetFromJsonAsync<SteamCategoryModel[]>("api/SteamCategory")).ToDictionary(x => x.ID);

                foreach (SteamGameCategoryModel gameCategory in gameCategories) {
                    SteamCategoryModel category = categories[gameCategory.SteamCategoryID];
                    model.Categories.Add(category);
                }
            }

            SteamGameUserTagModel[]? gameUserTags = await http.GetFromJsonAsync<SteamGameUserTagModel[]>("api/SteamGameUserTag/SteamGameID/" + id);
            if (gameUserTags?.Any() == true) {
                Dictionary<long, SteamUserTagModel> userTags = (await http.GetFromJsonAsync<SteamUserTagModel[]>("api/SteamUserTag")).ToDictionary(x => x.ID);

                foreach (SteamGameUserTagModel gameUserTag in gameUserTags) {
                    SteamUserTagModel userTag = userTags[gameUserTag.SteamUserTagID];
                    model.UserTags.Add(userTag);
                }
            }

            return model;
        }

        public async Task<GameViewModel> ResyncGame(long id) {
            await http.GetAsync("api/SteamGame/Resync/" + id);
            return await GetGameViewModel(id);
        }

        public async Task<List<RarestAchievementGameModel>> GetRarestAchievementGameModels() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame");
            Dictionary<long, GameDetailsModel> details = (await http.GetFromJsonAsync<GameDetailsModel[]>("api/GameDetails")).ToDictionary(x => x.SteamGameID);
            Dictionary<long, List<AchievementViewModel>> allAchievements = await achievementDataService.GetAllAchievementViewModels();

            List<RarestAchievementGameModel> models = [];

            foreach(SteamGameModel game in games) {
                if (game.AchievementCount == 0) continue;

                RarestAchievementGameModel model = new() {
                    Game = game,
                    Achievements = allAchievements[game.ID]
                };

                if (details.ContainsKey(game.ID))
                    model.Details = details[game.ID];


                model.AchievedCount = model.Achievements.Where(x => x.UserAchievement.Achieved).Count();
                model.AchievProgressWidth = (double)model.AchievedCount / (double)model.Game.AchievementCount * 100;
                model.Perfect = model.AchievedCount == model.Game.AchievementCount;

                models.Add(model);
            }

            return models.OrderByDescending(x => x.RarestAchievementPercentage).ToList();
        }
    }
}
