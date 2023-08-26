using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class UpdateGameDetails {
        private readonly UnitOfWork uow;
        private readonly ILogger logger;

        public UpdateGameDetails(UnitOfWork uow, ILogger<UpdateGameDetails> logger) {
            this.uow = uow;
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(UpdateGameDetails)} Job");
            DateTime startTime = DateTime.Now;
            Run();
            logger.LogInformation($"Finished {nameof(UpdateGameDetails)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
            GameDetailsRepository repo = uow.GetRepo<GameDetailsRepository>();

            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetByGameListType(GameListType.Perfect);
            Dictionary<long, GameDetails> detailsDictionary = repo.GetAll().ToDictionary(x => x.SteamGameID);
            foreach(SteamGame game in games) {

                GameDetails gameDetail;
                if (detailsDictionary.ContainsKey(game.ID)) {
                    gameDetail = detailsDictionary[game.ID];
                } else {
                    gameDetail = new() {
                        SteamGameID = game.ID
                    };
                    repo.Add(gameDetail);
                }

                bool needsUpdate = false;

                if (gameDetail.PerfectPossible != true) {
                    gameDetail.PerfectPossible = true;
                    needsUpdate = true;
                }

                if (gameDetail.PlayNextScore != 0) {
                    gameDetail.PlayNextScore = 0;
                    needsUpdate = true;
                }

                if (gameDetail.Finished != true) {
                    gameDetail.Finished = true;
                    needsUpdate = true;
                }

                if (needsUpdate) {
                    repo.Update(gameDetail);
                }
            }

            games = uow.GetRepo<SteamGameRepository>().GetWithoutAchievements();
            foreach(SteamGame game in games) {
                GameDetails gameDetail;
                if (detailsDictionary.ContainsKey(game.ID)) {
                    gameDetail = detailsDictionary[game.ID];
                } else {
                    gameDetail = new() {
                        SteamGameID = game.ID
                    };
                    repo.Add(gameDetail);
                }

                bool needsUpdate = false;

                if (gameDetail.PerfectPossible != false) {
                    gameDetail.PerfectPossible = false;
                    needsUpdate = true;
                }

                if (needsUpdate) {
                    repo.Update(gameDetail);
                }
            }
        }
    }
}
