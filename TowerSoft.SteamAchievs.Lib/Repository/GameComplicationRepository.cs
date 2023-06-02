using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameComplicationRepository : DbRepository<GameComplication> {
        public GameComplicationRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public GameComplication Get(long steamGameID, long complicationID) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.ComplicationID, complicationID));
        }

        public List<GameComplication> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<GameComplication> GetByComplicationID(long complicationID) {
            return GetEntities(WhereEqual(x => x.ComplicationID, complicationID));
        }
    }
}
