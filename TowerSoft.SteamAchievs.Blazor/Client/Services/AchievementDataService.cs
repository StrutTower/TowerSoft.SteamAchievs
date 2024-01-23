using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class AchievementDataService {
        private readonly HttpClient http;

        public AchievementDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<List<AchievementViewModel>> GetBySteamGameID(long steamGameID) {
            SteamAchievementSchemaModel[] achievementSchemas = await http.GetFromJsonAsync<SteamAchievementSchemaModel[]>("SteamAchievementSchema/SteamGameID/" + steamGameID);
            List<AchievementViewModel> models = new();

            if (achievementSchemas?.Any() == true) {
                Dictionary<string, SteamUserAchievementModel> userAch =
                    (await http.GetFromJsonAsync<SteamUserAchievementModel[]>("SteamUserAchievement/SteamGameID/" + steamGameID)).ToDictionary(x => x.KeyName);

                Dictionary<string, AchievementDetailsModel> details =
                    (await http.GetFromJsonAsync<AchievementDetailsModel[]>("AchievementDetails/" + steamGameID)).ToDictionary(x => x.AchievementKey);

                Dictionary<long, TagModel> tag =
                    (await http.GetFromJsonAsync<TagModel[]>("Tag")).ToDictionary(x => x.ID);

                List<AchievementTagModel> achievementTags =
                    (await http.GetFromJsonAsync<AchievementTagModel[]>("AchievementTag/SteamGameID/" + steamGameID)).ToList();


                foreach (SteamAchievementSchemaModel schema in achievementSchemas) {
                    AchievementViewModel model = new() {
                        Schema = schema,
                        Description = schema.Description,
                        AchievementTags = new()
                    };

                    if (details.ContainsKey(schema.KeyName)) {
                        model.Details = details[schema.KeyName];
                        if (!string.IsNullOrWhiteSpace(model.Details.Description))
                            model.Description = model.Details.Description;
                    }

                    if (userAch.ContainsKey(schema.KeyName))
                        model.UserAchievement = userAch[schema.KeyName];

                    if (achievementTags?.Any() == true) {
                        foreach (AchievementTagModel aTag in achievementTags.Where(x => x.AchievementKey == schema.KeyName)) {
                            model.AchievementTags.Add(tag[aTag.TagID]);
                        }
                    }

                    models.Add(model);
                }
            }
            return models;
        }

        public async Task AddOrUpdateAchievementDetails(EditAchievementDetailsModel model) {
            await http.PostAsJsonAsync("AchievementDetails", model);
        }
    }
}
