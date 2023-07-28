﻿using Microsoft.Extensions.Logging;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class HowLongToBeatSync {
        private readonly UnitOfWork uow;
        private readonly HowLongToBeatService howLongToBeatService;
        private readonly GameDetailsRepository detailsRepo;
        private readonly ILogger logger;

        private List<long> skipAppIDs = new List<long> { 42710, 10190, 42690, 325180 };

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
                if (skipAppIDs.Contains(game.ID)) continue;

                string searchName = game.Name.Replace("™", "").Replace(" - ", " ").Replace("®", "");

                List<HltbModel> hltbModels = howLongToBeatService.Search(searchName).Result;

                AttemptMatchAndSync(game, hltbModels, detailsDictionary);
            }
        }

        private void AttemptMatchAndSync(SteamGame game, List<HltbModel> hltbModels, Dictionary<long, GameDetails> detailsDictionary, string searchName = null) {
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
            } else if (hltbModels.Where(x => x.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase)).Count() == 1) {
                HltbModel model = hltbModels.Single(x => x.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase));
                Update(game, model, detailsDictionary);

            } else if (hltbModels.Where(x => x.Name.Replace(":", "").Equals(game.Name.Trim(), StringComparison.OrdinalIgnoreCase)).Count() == 1) {
                HltbModel model = hltbModels.Single(x => x.Name.Replace(":", "").Equals(game.Name, StringComparison.OrdinalIgnoreCase));
                Update(game, model, detailsDictionary);

            } else {
                if (searchName != null) {
                    if (searchName.Contains("-")) {
                        string searchName2 = searchName.Substring(0, searchName.IndexOf("-") - 1).SafeTrim();
                        List<HltbModel> hltbModels2 = howLongToBeatService.Search(searchName2).Result;

                        AttemptMatchAndSync(game, hltbModels2, detailsDictionary, searchName2);
                    }
                } else if (game.Name.Contains("-")) {
                    string searchName2 = game.Name.Substring(0, game.Name.IndexOf("-") - 1).SafeTrim();
                    List<HltbModel> hltbModels2 = howLongToBeatService.Search(searchName2).Result;

                    AttemptMatchAndSync(game, hltbModels2, detailsDictionary, searchName2);
                } else {
                    logger.LogInformation($"Unable to find a match for {game.Name}.\n   Possible matches " + string.Join(", ", hltbModels.Select(x => $"{x.Name} ({x.ReleaseYear})")));
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

                logger.LogInformation($"Updating '{game.Name}' HLTB times");
                detailsRepo.Update(gameDetail);
            }
        }
    }
}