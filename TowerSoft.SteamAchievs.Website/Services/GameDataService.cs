using System.Collections.Generic;
using System.Linq;
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
                //AchievementSchemas = ,
                //UserAchievements = uow.GetRepo<SteamUserAchievementRepository>().GetBySteamGameID(id),
                Complications = new(),
                Achievements = achievementDataService.GetAchievementModels(id).ToList()
            };

            Dictionary<long, Complication> complications = uow.GetRepo<ComplicationRepository>().GetAll().ToDictionary(x => x.ID);
            var gameComps = uow.GetRepo<GameComplicationRepository>().GetBySteamGameID(id);
            foreach (GameComplication gameComp in gameComps) {
                model.Complications.Add(complications[gameComp.ComplicationID]);
            }

            return model;
        }

        public GameListModel GetGameListModel(GameListType gameListType) {
            GameListModel model = new() {
                PageTitle = EnumUtilities.GetEnumDisplayName(gameListType),
                Games = GetSteamGameListModel(uow.GetRepo<SteamGameRepository>().GetByGameListType(gameListType))
            };
            return model;
        }

        public List<SteamGameListModel> GetSteamGameListModel(List<SteamGame> games) {
            List<SteamGameListModel> models = new();

            List<SteamUserAchievement> userAchievements = uow.GetRepo<SteamUserAchievementRepository>().GetAll();
            List<GameDetails> gameDetails = uow.GetRepo<GameDetailsRepository>().GetAll();
            Dictionary<long, Complication> complications = uow.GetRepo<ComplicationRepository>().GetAll().ToDictionary(x => x.ID);
            List<GameComplication> gameComplications = uow.GetRepo<GameComplicationRepository>().GetAll();

            foreach (SteamGame game in games) {
                SteamGameListModel model = new() {
                    SteamGame = game,
                    GameDetails = gameDetails.SingleOrDefault(x => x.SteamGameID == game.ID),
                    UserAchievements = userAchievements.Where(x => x.SteamGameID == game.ID).ToList(),
                    Complications = new()
                };

                var gameComps = gameComplications.Where(x => x.SteamGameID == game.ID);
                foreach (GameComplication gameComp in gameComps) {
                    model.Complications.Add(complications[gameComp.ComplicationID]);
                }

                models.Add(model);
            }
            return models;
        }
    }
}
