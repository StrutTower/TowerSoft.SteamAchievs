using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class GameDataService {
        private readonly UnitOfWork uow;
        private readonly AchievementDataService achievementDataService;

        public GameDataService(UnitOfWork uow, AchievementDataService achievementDataService) {
            this.uow = uow;
            this.achievementDataService = achievementDataService;
        }

        public SteamGameModel GetSteamGameModel(long id) {
            SteamGameModel model = new() {
                SteamGame = uow.GetRepo<SteamGameRepository>().GetByID(id),
                GameDetails = uow.GetRepo<GameDetailsRepository>().GetBySteamGameID(id),
                GameDescriptions = uow.GetRepo<SteamGameDescriptionsRepository>().GetBySteamGameID(id),
                Achievements = achievementDataService.GetAchievementModels(id).ToList(),
                ComplicationTags = new()
            };

            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetAll().ToDictionary(x => x.ID);
            var aTags = uow.GetRepo<AchievementTagRepository>().GetBySteamGameID(id);
            foreach (var aTag in aTags) {
                Tag tag = tags[aTag.TagID];
                if (tag.IsComplication && !model.ComplicationTags.Select(x => x.ID).Contains(tag.ID))
                    model.ComplicationTags.Add(tag);
            }

            return model;
        }

        public GameListModel GetGameListModel(GameListType gameListType) {
            GameListModel model = new() {
                PageTitle = EnumUtilities.GetEnumDisplayName(gameListType),
                Games = GetSteamGameListModel(uow.GetRepo<SteamGameRepository>().GetByGameListType(gameListType)),
                SortOptions = GetSortOptions()
            };
            return model;
        }

        public GameListModel GetGameListModel(List<SteamGame> games, string pageTitle) {
            GameListModel model = new() {
                PageTitle = pageTitle,
                Games = GetSteamGameListModel(games),
                SortOptions = GetSortOptions()
            };
            return model;
        }

        private List<KeyValuePair<string, string>> GetSortOptions() {
            return new() {
                new KeyValuePair<string, string>("nameAZ", "Name (A-Z)"),
                new KeyValuePair<string, string>("nameZA", "Name (Z-A)"),
                new KeyValuePair<string, string>("countSortAsc", "Achievement Count"),
                new KeyValuePair<string, string>("countSortDesc", "Achievement Count (Descending)"),
                new KeyValuePair<string, string>("percentSortAsc", "Completion Percentage"),
                new KeyValuePair<string, string>("percentSortDesc", "Completion Percentage (Descending)"),
                new KeyValuePair<string, string>("playtimeSortAsc", "Playtime"),
                new KeyValuePair<string, string>("playtimeSortDesc", "Playtime (Descending)"),
                new KeyValuePair<string, string>("reviewSortAsc", "Review Score"),
                new KeyValuePair<string, string>("reviewSortDesc", "Review Score (Descending)"),
                new KeyValuePair<string, string>("metacriticScoreAsc", "Metacritic Score"),
                new KeyValuePair<string, string>("metacriticScoreDesc", "Metacritic Score (Descending)"),
                new KeyValuePair<string, string>("playNextSortAsc", "Play Next Score"),
                new KeyValuePair<string, string>("playNextSortDesc", "Play Next Score (Descending)"),
            };
        }

        private List<SteamGameListModel> GetSteamGameListModel(List<SteamGame> games) {
            List<SteamGameListModel> models = new();

            List<SteamUserAchievement> userAchievements = uow.GetRepo<SteamUserAchievementRepository>().GetAll();
            List<GameDetails> gameDetails = uow.GetRepo<GameDetailsRepository>().GetAll();
            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetAll().ToDictionary(x => x.ID);
            List<AchievementTag> achievementTags = uow.GetRepo<AchievementTagRepository>().GetAll();

            foreach (SteamGame game in games) {
                SteamGameListModel model = new() {
                    SteamGame = game,
                    GameDetails = gameDetails.SingleOrDefault(x => x.SteamGameID == game.ID),
                    UserAchievements = userAchievements.Where(x => x.SteamGameID == game.ID).ToList(),
                    ComplicationTags = new()
                };

                var aTags = achievementTags.Where(x => x.SteamGameID == game.ID).ToList();
                foreach (var aTag in aTags) {
                    Tag tag = tags[aTag.TagID];
                    if (tag.IsComplication && !model.ComplicationTags.Select(x => x.ID).Contains(tag.ID))
                        model.ComplicationTags.Add(tag);
                }
                models.Add(model);
            }
            return models;
        }

        internal EditGameDetailsModel GetEditGameDetailsModel(long steamGameID) {
            EditGameDetailsModel model = new() {
                SteamGame = uow.GetRepo<SteamGameRepository>().GetByID(steamGameID),
                GameDetails = uow.GetRepo<GameDetailsRepository>().GetBySteamGameID(steamGameID)
            };
            if (model.GameDetails == null) {
                model.GameDetails = new GameDetails {
                    SteamGameID = steamGameID
                };
            }
            return model;
        }

        internal GameDetails UpdateGameDetails(GameDetails gameDetails) {
            GameDetailsRepository repo = uow.GetRepo<GameDetailsRepository>();
            GameDetails existing = repo.GetBySteamGameID(gameDetails.SteamGameID);
            if (existing == null) {
                repo.Add(gameDetails);
            } else {
                existing.PerfectPossible = gameDetails.PerfectPossible;
                existing.PlayNextScore = gameDetails.PlayNextScore;
                existing.Finished = gameDetails.Finished;
                repo.Update(existing);
            }
            return existing;
        }
    }
}
