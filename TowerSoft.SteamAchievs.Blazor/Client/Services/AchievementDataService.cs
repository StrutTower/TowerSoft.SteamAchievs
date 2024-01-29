using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Client.Utilities;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class AchievementDataService {
        private readonly HttpClient http;

        public AchievementDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<List<AchievementViewModel>> GetBySteamGameID(long steamGameID) {
            SteamAchievementSchemaModel[] achievementSchemas = await http.GetFromJsonAsync<SteamAchievementSchemaModel[]>("api/SteamAchievementSchema/SteamGameID/" + steamGameID);
            List<AchievementViewModel> models = new();

            if (achievementSchemas?.Any() == true) {
                Dictionary<string, SteamUserAchievementModel> userAch =
                    (await http.GetFromJsonAsync<SteamUserAchievementModel[]>("api/SteamUserAchievement/SteamGameID/" + steamGameID)).ToDictionary(x => x.KeyName);

                Dictionary<string, AchievementDetailsModel> details =
                    (await http.GetFromJsonAsync<AchievementDetailsModel[]>("api/AchievementDetails/" + steamGameID)).ToDictionary(x => x.AchievementKey);

                Dictionary<long, TagModel> tag =
                    (await http.GetFromJsonAsync<TagModel[]>("api/Tag")).ToDictionary(x => x.ID);

                List<AchievementTagModel> achievementTags =
                    (await http.GetFromJsonAsync<AchievementTagModel[]>("api/AchievementTag/SteamGameID/" + steamGameID)).ToList();


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

        public async Task<Dictionary<long, List<AchievementViewModel>>> GetAllAchievementViewModels() {
            SteamAchievementSchemaModel[] achievementSchemas = await http.GetFromJsonAsync<SteamAchievementSchemaModel[]>("api/SteamAchievementSchema");

            Dictionary<string, SteamUserAchievementModel> userAch =
                (await http.GetFromJsonAsync<SteamUserAchievementModel[]>("api/SteamUserAchievement")).ToDictionary(x => x.SteamGameID + x.KeyName);

            Dictionary<string, AchievementDetailsModel> details =
                (await http.GetFromJsonAsync<AchievementDetailsModel[]>("api/AchievementDetails")).ToDictionary(x => x.SteamGameID + x.AchievementKey);

            Dictionary<long, TagModel> tags =
                (await http.GetFromJsonAsync<TagModel[]>("api/Tag")).ToDictionary(x => x.ID);

            List<AchievementTagModel> achievementTags =
                (await http.GetFromJsonAsync<AchievementTagModel[]>("api/AchievementTag")).ToList();

            Dictionary<long, List<AchievementViewModel>> output = new();

            foreach (SteamAchievementSchemaModel schema in achievementSchemas) {
                if (!output.ContainsKey(schema.SteamGameID)) {
                    output.Add(schema.SteamGameID, new List<AchievementViewModel>());
                }

                AchievementViewModel viewmodel = new() {
                    Schema = schema
                };

                if (userAch.ContainsKey(schema.SteamGameID + schema.KeyName)) {
                    viewmodel.UserAchievement = userAch[schema.SteamGameID + schema.KeyName];
                }

                if (details.ContainsKey(schema.SteamGameID + schema.KeyName)) {
                    viewmodel.Details = details[schema.SteamGameID + schema.KeyName];
                }

                if (string.IsNullOrWhiteSpace(viewmodel?.Details?.Description))
                    viewmodel.Description = schema.Description;
                else
                    viewmodel.Description = viewmodel.Details.Description;

                var atags = achievementTags.Where(x => x.AchievementKey == schema.KeyName);
                if (atags.SafeAny()) {
                    viewmodel.AchievementTags = [];
                    foreach (AchievementTagModel atag in atags) {
                        if (tags.ContainsKey(atag.TagID))
                            viewmodel.AchievementTags.Add(tags[atag.TagID]);
                    }
                }

                output[schema.SteamGameID].Add(viewmodel);
            }

            return output;
        }

        public async Task<List<RecentlyUnlockedAchievementModel>> GetLatestUnlocked() {
            List<SteamUserAchievementModel> userAchievs = await http.GetFromJsonAsync<List<SteamUserAchievementModel>>("api/SteamUserAchievement/LatestUnlocked");

            Dictionary<long, SteamGameModel> games =
                (await http.PostGetFromJson<List<SteamGameModel>>("api/SteamGame/GetByIDs", userAchievs.Select(x => x.SteamGameID))).ToDictionary(x => x.ID);

            Dictionary<string, SteamAchievementSchemaModel> schemas =
                (await http.PostGetFromJson<List<SteamAchievementSchemaModel>>("api/SteamAchievementSchema/GetBySteamGameIDs", userAchievs.Select(x => x.SteamGameID)))
                .ToDictionary(x => x.SteamGameID + x.KeyName);

            List<RecentlyUnlockedAchievementModel> models = [];
            foreach (SteamUserAchievementModel userAchievement in userAchievs.OrderByDescending(x => x.AchievedOn)) {
                RecentlyUnlockedAchievementModel model = new() {
                    UserAchievement = userAchievement,
                    SteamGame = games[userAchievement.SteamGameID],
                    Schema = schemas[userAchievement.SteamGameID + userAchievement.KeyName],
                };

                models.Add(model);
            }

            return models;
        }

        public async Task AddOrUpdateAchievementDetails(EditAchievementDetailsModel model) {
            await http.PostAsJsonAsync("api/AchievementDetails", model);
        }
    }
}
