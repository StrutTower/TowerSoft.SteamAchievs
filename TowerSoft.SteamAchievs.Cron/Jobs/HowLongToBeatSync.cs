using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class HowLongToBeatSync {
        private readonly UnitOfWork uow;
        private readonly HowLongToBeatService howLongToBeatService;
        private readonly AppSettings appSettings;
        private readonly GameDetailsRepository detailsRepo;
        private readonly ILogger logger;

        private List<long> skipAppIDs = new List<long> { 42710, 10190, 42690, 325180 };

        public HowLongToBeatSync(UnitOfWork uow, HowLongToBeatService howLongToBeatService, IOptionsSnapshot<AppSettings> appSettings, ILogger<HowLongToBeatSync> logger) {
            this.uow = uow;
            this.howLongToBeatService = howLongToBeatService;
            this.appSettings = appSettings.Value;
            this.detailsRepo = uow.GetRepo<GameDetailsRepository>();
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(HowLongToBeatSync)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(HowLongToBeatSync)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
            GameDetailsRepository repo = uow.GetRepo<GameDetailsRepository>();

            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetAll().OrderBy(x => x.Name).ToList();
            Dictionary<long, GameDetails> detailsDictionary = repo.GetAll().ToDictionary(x => x.SteamGameID);

            foreach (SteamGame game in games) {
                if (skipAppIDs.Contains(game.ID)) continue;

                string searchName = game.NameClean
                    .Replace("™", "")
                    .Replace(" - ", " ")
                    .Replace("®", "")
                    .Replace("–", " ")
                    .Replace("-", " ")
                    .Replace(":", " ")
                    .Replace("+", " ")
                    .Replace("!", " ");
                if (searchName.EndsWith(" HD", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 3);
                }

                if (searchName.EndsWith(" - Remastered", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 13);
                }

                if (searchName.EndsWith(" GOTY Edition", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 13);
                }

                if (searchName.EndsWith(" Steam Edition", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 14);
                }

                if (searchName.EndsWith(" Complete Edition", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 17);
                }

                if (searchName.EndsWith(" Enhanced Edition", StringComparison.OrdinalIgnoreCase)) {
                    searchName = searchName.Substring(0, searchName.Length - 17);
                }

                if (searchName.Contains("(")) {
                    searchName = searchName.Substring(0, searchName.IndexOf("("));
                }
                
                List<HltbModel> hltbModels = howLongToBeatService.Search(searchName).Result;

                AttemptMatchAndSync(game, hltbModels, detailsDictionary, searchName);
            }
        }

        private void AttemptMatchAndSync(SteamGame game, List<HltbModel> hltbModels, Dictionary<long, GameDetails> detailsDictionary, string searchName) {
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
            } else if (hltbModels.Where(x => x.Name.Equals(game.NameClean, StringComparison.OrdinalIgnoreCase)).Count() == 1) {
                HltbModel model = hltbModels.Single(x => x.Name.Equals(game.NameClean, StringComparison.OrdinalIgnoreCase));
                Update(game, model, detailsDictionary);

            } else if (hltbModels.Where(x => x.Name.Replace(":", "").Equals(game.NameClean.Trim(), StringComparison.OrdinalIgnoreCase)).Count() == 1) {
                HltbModel model = hltbModels.Single(x => x.Name.Replace(":", "").Equals(game.NameClean, StringComparison.OrdinalIgnoreCase));
                Update(game, model, detailsDictionary);

            } else {
                if (game.NameClean.Contains("-")) {
                    string searchName2 = game.Name.Substring(0, game.NameClean.IndexOf("-") - 1).SafeTrim();
                    List<HltbModel> hltbModels2 = howLongToBeatService.Search(searchName2).Result;

                    AttemptMatchAndSync(game, hltbModels2, detailsDictionary, searchName2);
                } else {
                    logger.LogInformation($"Unable to find a match for {searchName}.\n   Possible matches " + string.Join(", ", hltbModels.Select(x => $"{x.Name} ({x.ReleaseYear})")));
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

            double? mainStoryTime = null;
            double? mainAndSidesTime = null;
            double? completionistTime = null;
            double? allStylesTime = null;

            gameDetail.HowLongToBeatID = model.ID;
            if (model.MainTime.HasValue)
                mainStoryTime = Math.Round((double)model.MainTime / 3600, 1);

            if (model.MainPlusTime.HasValue)
                mainAndSidesTime = Math.Round((double)model.MainPlusTime / 3600, 1);

            if (model.CompletionTime.HasValue)
                completionistTime = Math.Round((double)model.CompletionTime / 3600, 1);

            if (model.AllStylesTime.HasValue)
                allStylesTime = Math.Round((double)model.AllStylesTime / 3600, 1);

            if (mainStoryTime == 0) mainStoryTime = null;
            if (mainAndSidesTime == 0) mainAndSidesTime = null;
            if (completionistTime == 0) completionistTime = null;
            if (allStylesTime == 0) allStylesTime = null;

            if (gameDetail.MainStoryTime != mainStoryTime ||
                gameDetail.MainAndSidesTime != mainAndSidesTime ||
                gameDetail.CompletionistTime != completionistTime ||
                gameDetail.AllStylesTime != allStylesTime) {

                gameDetail.MainStoryTime = mainStoryTime;
                gameDetail.MainAndSidesTime = mainAndSidesTime;
                gameDetail.CompletionistTime = completionistTime;
                gameDetail.AllStylesTime = allStylesTime;

                logger.LogInformation($"Updating '{game.NameClean}' HLTB times");
                detailsRepo.Update(gameDetail);
            }
        }
    }
}
