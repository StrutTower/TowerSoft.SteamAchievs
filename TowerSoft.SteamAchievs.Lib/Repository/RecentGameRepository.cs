using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class RecentGameRepository : DbRepository<RecentGame> {
        public RecentGameRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public override List<RecentGame> GetAll() {
            return GetEntities(Query
                .OrderByDescending(x => x.FirstDetected));
        }

        public RecentGame GetBySteamGameID(long steamGameID) {
            return GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
