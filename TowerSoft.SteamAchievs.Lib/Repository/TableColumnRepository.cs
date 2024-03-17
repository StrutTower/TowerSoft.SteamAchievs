using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TableColumnRepository(UnitOfWork uow) : DbRepository<TableColumn>(uow.DbAdapter) {
        public TableColumn GetByID(long id) =>
            GetSingleEntity(WhereEqual(x => x.ID, id));

        public List<TableColumn> GetByTableListID(long tableListID) =>
            GetEntities(WhereEqual(x => x.TableListID, tableListID));

        public List<TableColumn> GetActiveByTableListID(long tableListID) =>
            GetEntities(Query
                .WhereEqual(x => x.TableListID, tableListID)
                .WhereEqual(x => x.IsActive, true));

        public List<TableColumn> GetBySteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += 
                $"INNER JOIN tablelist tl ON {TableName}.TableListID = tl.ID " +
                $"WHERE tl.SteamGameID = @SteamGameID ";
            query.AddParameter("@SteamGameID", steamGameID);
            return GetEntities(query);
        }
    }
}
