using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TableRowRepository(UnitOfWork uow) : DbRepository<TableRow>(uow.DbAdapter) {
        public TableRow GetByID(long id) =>
            GetSingleEntity(WhereEqual(x => x.ID, id));

        public List<TableRow> GetByTableListID(long tableListID) =>
            GetEntities(WhereEqual(x => x.TableListID, tableListID));

        public List<TableRow> GetActiveByTableListID(long tableListID) =>
            GetEntities(Query
                .WhereEqual(x => x.TableListID, tableListID)
                .WhereEqual(x => x.IsActive, true));

        public List<TableRow> GetBySteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += 
                $"INNER JOIN tablelist tl ON {TableName}.TableListID = tl.ID " +
                $"WHERE tl.SteamGameID = @SteamGameID ";
            query.AddParameter("@SteamGameID", steamGameID);
            return GetEntities(query);
        }
    }
}
