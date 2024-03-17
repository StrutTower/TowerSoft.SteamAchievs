using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TableDataRepository(UnitOfWork uow) : DbRepository<TableData>(uow.DbAdapter) {
        public TableData Get(long tableRowID, long tableColumnID) =>
            GetSingleEntity(Query
                .WhereEqual(x => x.TableRowID, tableRowID)
                .WhereEqual(x => x.TableColumnID, tableColumnID));

        public List<TableData> GetByTableRowID(long tableRowID) =>
            GetEntities(WhereEqual(x => x.TableRowID, tableRowID));

        public List<TableData> GetByTableColumnID(long listColumnID) =>
            GetEntities(WhereEqual(x => x.TableColumnID, listColumnID));

        public List<TableData> GetBySteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += 
                $"INNER JOIN tablerow tr ON {TableName}.TableRowID = tr.ID " +
                $"INNER JOIN tablelist tl ON tr.TableListID = tl.ID " +
                $"WHERE tl.SteamGameID = @SteamGameID ";
            query.AddParameter("@SteamGameID", steamGameID);
            return GetEntities(query);
        }

        public List<TableData> GetByTableListID(long tableListID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"INNER JOIN tablerow tr ON {TableName}.TableRowID = tr.ID " +
                $"WHERE tr.TableListID = @TableListID ";
            query.AddParameter("@TableListID", tableListID);
            return GetEntities(query);
        }
    }
}
