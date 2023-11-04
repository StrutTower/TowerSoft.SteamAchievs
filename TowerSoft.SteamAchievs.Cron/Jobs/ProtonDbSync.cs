using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class ProtonDbSync {
        private readonly UnitOfWork uow;
        private readonly ProtonDbService protonDbService;
        private readonly ILogger logger;

        public ProtonDbSync(UnitOfWork uow, ProtonDbService protonDbService, ILogger<ProtonDbSync> logger) {
            this.uow = uow;
            this.protonDbService = protonDbService;
            this.logger = logger;
        }
        public void StartJob() {
            logger.LogInformation($"Starting {nameof(ProtonDbSync)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(ProtonDbSync)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
            GameDetailsRepository repo = uow.GetRepo<GameDetailsRepository>();

            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetAll().OrderBy(x => x.NameClean).ToList();
            Dictionary<long, GameDetails> detailsDictionary = repo.GetAll().ToDictionary(x => x.SteamGameID);

            foreach (SteamGame game in games) {
                logger.LogInformation($"Running proton db sync on {game.NameClean}");
                ProtonDbGame protonDbGame = protonDbService.GetGame(game.ID).Result;

                if (protonDbGame != null && !string.IsNullOrWhiteSpace(protonDbGame.Tier)) {
                    GameDetails gameDetail;
                    if (detailsDictionary.ContainsKey(game.ID)) {
                        gameDetail = detailsDictionary[game.ID];
                    } else {
                        gameDetail = new() {
                            SteamGameID = game.ID
                        };
                        repo.Add(gameDetail);
                    }

                    gameDetail.ProtonDbRating = protonDbGame.Tier;
                    repo.Update(gameDetail);
                }
            }
        }
    }
}
