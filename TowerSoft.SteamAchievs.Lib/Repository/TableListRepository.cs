using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TableListRepository(UnitOfWork uow) : DbRepository<TableList>(uow.DbAdapter) {
        public TableList GetByID(long id) =>
            GetSingleEntity(WhereEqual(x => x.ID, id));

        public List<TableList> GetActive() =>
            GetEntities(WhereEqual(x => x.IsActive, true));

        public List<TableList> GetActiveBySteamGameID(long steamGameID) =>
            GetEntities(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.IsActive, true));

        public List<TableList> GetBySteamGameID(long steamGameID) =>
            GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
    }
}
