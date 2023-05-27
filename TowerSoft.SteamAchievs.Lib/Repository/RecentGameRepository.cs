using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class RecentGameRepository : DbRepository<RecentGame> {
        public RecentGameRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public override async Task<List<RecentGame>> GetAllAsync() {
            return await GetEntitiesAsync(Query
                .OrderByDescending(x => x.FirstDetected));
        }

        public async Task<RecentGame> GetBySteamGameID(long steamGameID) {
            return await GetSingleEntityAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
