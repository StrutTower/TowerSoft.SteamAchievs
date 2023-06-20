using Microsoft.Extensions.Logging;
using TowerSoft.SteamAchievs.Cron.Services;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class HowLongToBeatSync {
        private readonly UnitOfWork uow;
        private readonly HowLongToBeatService howLongToBeatService;
        private readonly GameDetailsRepository detailsRepo;
        private readonly ILogger logger;

        public HowLongToBeatSync(UnitOfWork uow, HowLongToBeatService howLongToBeatService, ILogger<HowLongToBeatSync> logger) {
            this.uow = uow;
            this.howLongToBeatService = howLongToBeatService;
            this.detailsRepo = uow.GetRepo<GameDetailsRepository>();
            this.logger = logger;
        }

        public void StartJob() {
            Run();
        }

        private void Run() {
            GameDetailsRepository repo = uow.GetRepo<GameDetailsRepository>();

            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetAll().OrderBy(x => x.Name).ToList();
            Dictionary<long, GameDetails> detailsDictionary = repo.GetAll().ToDictionary(x => x.SteamGameID);

            foreach (SteamGame game in games) {
                //logger.LogInformation($"Updating {game.Name}");
                List<HltbModel> hltbModels = howLongToBeatService.Search(game.Name.Replace(" - ", " ")).Result;

                long? hltbID = null;
                if (detailsDictionary.ContainsKey(game.ID)) {
                    hltbID = detailsDictionary[game.ID].HowLongToBeatID;
                }

                if (hltbModels.SafeAny() && hltbModels.Count == 1) {
                    HltbModel model = hltbModels.First();

                    Update(game, model, detailsDictionary);

                } else if (hltbID.HasValue && hltbModels.SafeAny(x => x.ID == hltbID)) {
                    HltbModel model = hltbModels.Single(x => x.ID == hltbID);

                    Update(game, model, detailsDictionary);
                } else if (hltbModels.Where(x => x.Name.Replace(":", "").Equals(game.Name, StringComparison.OrdinalIgnoreCase)).Count() == 1) {
                    HltbModel model = hltbModels.Single(x => x.Name.Replace(":", "").Equals(game.Name, StringComparison.OrdinalIgnoreCase));
                    Update(game, model, detailsDictionary);
                } else {
                    logger.LogInformation($"Unable to find a match for {game.Name}");
                }
            }
        }

        private void Update(SteamGame game, HltbModel model, Dictionary<long, GameDetails> detailsDictionary) {

            GameDetails gameDetail;
            if (detailsDictionary.ContainsKey(game.ID)) {
                gameDetail = detailsDictionary[game.ID];
            } else {
                gameDetail = new() {
                    SteamGameID = game.ID
                };
                detailsRepo.Add(gameDetail);
            }

            gameDetail.HowLongToBeatID = model.ID;
            if (model.MainTime.HasValue)
                gameDetail.MainStoryTime = Math.Round((double)model.MainTime / 3600, 1);

            if (model.MainPlusTime.HasValue)
                gameDetail.MainAndSidesTime = Math.Round((double)model.MainPlusTime / 3600, 1);

            if (model.CompletionTime.HasValue)
                gameDetail.CompletionistTime = Math.Round((double)model.CompletionTime / 3600, 1);

            if (model.AllStylesTime.HasValue)
                gameDetail.AllStylesTime = Math.Round((double)model.AllStylesTime / 3600, 1);

            detailsRepo.Update(gameDetail);
        }
    }
}
