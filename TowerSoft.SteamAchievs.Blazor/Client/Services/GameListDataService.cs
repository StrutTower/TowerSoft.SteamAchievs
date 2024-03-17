using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Client.Models;
using TowerSoft.SteamAchievs.Blazor.Client.Utilities;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class GameListDataService {
        private readonly HttpClient http;

        public GameListDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<GameListViewModel> Search(string searchTerm) {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/Search?q=" + searchTerm);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetGameListModel(IEnumerable<long> steamGameIDs) {
            SteamGameModel[] games = await http.PostGetFromJson<SteamGameModel[]>("api/SteamGame/GetByIDs", steamGameIDs);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetGameListModel(IEnumerable<SteamGameModel> games) {
            List<long> steamGameIDs = games.Select(x => x.ID).ToList();

            GameDetailsModel[] details = await http.PostGetFromJson<GameDetailsModel[]>("api/GameDetails/GetByIDs", steamGameIDs);
            Dictionary<long, GameDetailsModel> detailDictionary = details.ToDictionary(x => x.SteamGameID);
            SteamUserAchievementModel[] achievements = await http.PostGetFromJson<SteamUserAchievementModel[]>("api/SteamUserAchievement/GetByIDs", steamGameIDs);

            TagModel[] tags = await http.GetFromJsonAsync<TagModel[]>("api/Tag");
            Dictionary<long, TagModel> tagDictionary = tags.ToDictionary(x => x.ID);

            AchievementTagModel[] achievementTags = await http.GetFromJsonAsync<AchievementTagModel[]>("api/AchievementTag");

            GameListViewModel model = new() { Games = new() };
            foreach (SteamGameModel game in games) {
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
                    foreach (var atag in at) {
                        TagModel tag = tagDictionary[atag.TagID];
                        if (tag.IsComplication && !itemModel.ComplicationTags.Select(x => x.ID).Contains(tag.ID))
                            itemModel.ComplicationTags.Add(tag);
                    }

                model.Games.Add(itemModel);
            }
            model.Games = model.Games.OrderBy(x => x.SteamGame.NameClean).ToList();
            return model;
        }

        public async Task<GameListViewModel> GetPerfectGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.Perfect);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetNonPerfectGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.NonPerfect);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetIncompleteGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.Incomplete);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetPerfectPossibleGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.PerfectPossible);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetNoComplicationsGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.NoComplications);
            return await GetGameListModel(games);
        }

        public async Task<GameListViewModel> GetPlayNextScoreGames() {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GameListType/" + GameListType.HasPlayNextScore);
            return await GetGameListModel(games);
        }
    }
}
