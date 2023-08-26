using System.Collections.Generic;
using System.Linq;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class HomeDataService {
        private readonly UnitOfWork uow;

        public HomeDataService(UnitOfWork uow) {
            this.uow = uow;
        }

        internal HomeViewModel GetHomeViewModel() {
            HomeViewModel model = new() {
                RecentGames = uow.GetRepo<RecentGameRepository>().GetAll().OrderByDescending(x => x.FirstDetected).ToList(),
                PerfectLostGames = new()
            };

            List<PerfectGame> incompleteGames = uow.GetRepo<PerfectGameRepository>().GetIncompleteNowGames();
            if (incompleteGames.SafeAny()) {
                Dictionary<long, SteamGame> games = uow.GetRepo<SteamGameRepository>().GetByIDs(incompleteGames.Select(x => x.SteamGameID)).ToDictionary(x => x.ID);

                foreach (PerfectGame incompleteGame in incompleteGames) {
                    model.PerfectLostGames.Add(new() {
                        Game = games[incompleteGame.SteamGameID],
                        PerfectGame = incompleteGame
                    });
                }
            }

            return model;
        }
    }
}
