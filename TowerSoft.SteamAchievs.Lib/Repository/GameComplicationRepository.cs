using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameComplicationRepository : DbRepository<GameComplication> {
        public GameComplicationRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<GameComplication> Get(long steamGameID, long complicationID) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.ComplicationID, complicationID));
        }

        public async Task<List<GameComplication>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<GameComplication>> GetByComplicationID(long complicationID) {
            return await GetEntitiesAsync(WhereEqual(x => x.ComplicationID, complicationID));
        }
    }
}
