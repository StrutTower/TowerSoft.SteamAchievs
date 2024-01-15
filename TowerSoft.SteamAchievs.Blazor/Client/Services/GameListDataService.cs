using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class GameListDataService {
        private readonly HttpClient http;

        public GameListDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<GameListModel> GetGameListModel(IEnumerable<long> steamGameIDs) {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("SteamGame/GetByIDs/" + string.Join(",", steamGameIDs));

            GameDetailsModel[] details = await http.GetFromJsonAsync<GameDetailsModel[]>("GameDetails/GetByIDs/" + string.Join(",", steamGameIDs));
            Dictionary<long, GameDetailsModel> detailDictionary = details.ToDictionary(x => x.SteamGameID);

            SteamUserAchievementModel[] achievements = await http.GetFromJsonAsync<SteamUserAchievementModel[]>("SteamUserAchievement/GetByIDs/" + string.Join(",", steamGameIDs));

            TagModel[] tags = await http.GetFromJsonAsync<TagModel[]>("Tag");
            Dictionary<long, TagModel> tagDictionary = tags.ToDictionary(x => x.ID);

            AchievementTagModel[] achievementTags = await http.GetFromJsonAsync<AchievementTagModel[]>("AchievementTag");

            GameListModel model = new() {  Games = new() };
            foreach(SteamGameModel game in games) {
                GameListItemModel itemModel = new() {
                    SteamGame = game
                };

                if (detailDictionary.ContainsKey(game.ID))
                    itemModel.Details = detailDictionary[game.ID];

                if (game.AchievementCount > 0) {
                    itemModel.UserAchievements = achievements.Where(x => x.SteamGameID == game.ID).ToList();
                    itemModel.AchievedCount = itemModel.UserAchievements.Where(x => x.Achieved).Count();
                    itemModel.AchievProgressWidth = (double)itemModel.AchievedCount / (double)itemModel.SteamGame.AchievementCount * 100;
                    itemModel.Perfect = itemModel.AchievedCount == itemModel.SteamGame.AchievementCount;
                }

                IEnumerable<AchievementTagModel> at = achievementTags.Where(x => x.SteamGameID == game.ID);
                if (at?.Any() == true)
                foreach(var atag in at) {
                    TagModel tag = tagDictionary[atag.TagID];
                    if (tag.IsComplication && !itemModel.ComplicationTags.Select(x => x.ID).Contains(tag.ID))
                        itemModel.ComplicationTags.Add(tag);
                }

                model.Games.Add(itemModel);
            }
            model.Games = model.Games.OrderBy(x => x.SteamGame.NameClean).ToList();
            return model;
        }
    }
}
