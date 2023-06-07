using System.Linq;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class HomeDataService {
        private readonly UnitOfWork uow;

        public HomeDataService(UnitOfWork uow) {
            this.uow = uow;
        }

        internal HomeViewModel GetHomeViewModel() {
            HomeViewModel model = new() {
                RecentGames = uow.GetRepo<RecentGameRepository>().GetAll().OrderByDescending(x => x.FirstDetected).ToList()
            };

            return model;
        }
    }
}
