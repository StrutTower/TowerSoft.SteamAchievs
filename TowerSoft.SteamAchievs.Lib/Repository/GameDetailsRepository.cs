using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameDetailsRepository : DbRepository<GameDetails> {
        public GameDetailsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public GameDetails GetBySteamGameID(long steamGameID) {
            return GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
