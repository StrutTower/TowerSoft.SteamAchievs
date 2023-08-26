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
            SteamGame game = uow.GetRepo<SteamGameRepository>().GetByID(id);
            SteamGameModel model = new() {
                SteamGame = game,
                GameDetails = uow.GetRepo<GameDetailsRepository>().GetBySteamGameID(id),
                GameDescriptions = uow.GetRepo<SteamGameDescriptionsRepository>().GetBySteamGameID(id),
                Achievements = achievementDataService.GetAchievementModels(id).ToList(),
                ComplicationTags = new(),
                UserTags = GetSteamUserTagModels(game).ToList(),
                Categories = GetSteamCategoryModels(game).ToList(),
                AchievementTags = new List<Tag>()
            };

            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetAll().ToDictionary(x => x.ID);
            var aTags = uow.GetRepo<AchievementTagRepository>().GetBySteamGameID(id);
            foreach (var aTag in aTags) {
                Tag tag = tags[aTag.TagID];
                if (tag.IsComplication && !model.ComplicationTags.Select(x => x.ID).Contains(tag.ID))
                    model.ComplicationTags.Add(tag);
            }

            model.AchievementTags = tags.Values.Where(x => x.SteamGameID == null || x.SteamGameID == game.ID).ToList();

            return model;
        }

        public GameListModel GetGameListModel(GameListType gameListType) {
            GameListModel model = new() {
                PageTitle = EnumUtilities.GetEnumDisplayName(gameListType),
                Games = GetSteamGameListModel(uow.GetRepo<SteamGameRepository>().GetByGameListType(gameListType)).OrderBy(x => x.SteamGame.Name).ToList(),
                SortOptions = GetSortOptions()
            };

            if (gameListType == GameListType.HasPlayNextScore) {
                model.DefaultSortOption = "playNextSortDesc";
                model.Games = model.Games.OrderByDescending(x => x?.GameDetails?.PlayNextScore).ToList();
            }

            return model;
        }

        public GameListModel GetGameListModel(List<SteamGame> games, string pageTitle) {
            GameListModel model = new() {
                PageTitle = pageTitle,
                Games = GetSteamGameListModel(games).OrderBy(x => x.SteamGame.Name).ToList(),
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

        private IEnumerable<SteamCategoryModel> GetSteamCategoryModels(SteamGame steamGame) {
            Dictionary<long, SteamCategory> categories = uow.GetRepo<SteamCategoryRepository>().GetAll().ToDictionary(x => x.ID);
            List<SteamGameCategory> gameCategories = uow.GetRepo<SteamGameCategoryRepository>().GetBySteamGameID(steamGame.ID);

            foreach (SteamGameCategory gameCategory in gameCategories) {
                yield return new() {
                    SteamCategoryID = gameCategory.SteamCategoryID,
                    SteamGameID = gameCategory.SteamGameID,
                    Name = categories[gameCategory.SteamCategoryID].Name
                };
            }
        }

        private IEnumerable<SteamUserTagModel> GetSteamUserTagModels(SteamGame steamGame) {
            Dictionary<long, SteamUserTag> categories = uow.GetRepo<SteamUserTagRepository>().GetAll().ToDictionary(x => x.ID);
            List<SteamGameUserTag> gameCategories = uow.GetRepo<SteamGameUserTagRepository>().GetBySteamGameID(steamGame.ID);

            foreach (SteamGameUserTag gameCategory in gameCategories) {
                yield return new SteamUserTagModel {
                    SteamUserTagID = gameCategory.SteamUserTagID,
                    SteamGameID = gameCategory.SteamGameID,
                    Name = categories[gameCategory.SteamUserTagID].Name
                };
            }
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
                existing.HowLongToBeatID = gameDetails.HowLongToBeatID;
                existing.PerfectPossible = gameDetails.PerfectPossible;
                existing.PlayNextScore = gameDetails.PlayNextScore;
                existing.Finished = gameDetails.Finished;
                existing.NotifyIfAchievementsAdded = gameDetails.NotifyIfAchievementsAdded;
                repo.Update(existing);
            }
            return existing;
        }

        internal GameListModel GetPerfectLostGames() {
            List<PerfectGame> missingPerfectGames = uow.GetRepo<PerfectGameRepository>().GetIncompleteNowGames();
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetByIDs(missingPerfectGames.Select(x => x.SteamGameID));

            GameListModel model = new() {
                PageTitle = "Missing Perfect Game Status",
                Games = GetSteamGameListModel(games).OrderBy(x => x.SteamGame.Name).ToList(),
                SortOptions = GetSortOptions()
            };

            return model;
        }
    }
}
