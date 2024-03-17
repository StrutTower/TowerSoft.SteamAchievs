using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TableColumnChoiceRepository(UnitOfWork uow) : DbRepository<TableColumnChoice>(uow.DbAdapter) {
        public TableColumnChoice GetByID(long id) =>
            GetSingleEntity(WhereEqual(x => x.ID, id));

        public List<TableColumnChoice> GetByTableColumnID(long tableColumnID) =>
            GetEntities(WhereEqual(x => x.TableColumnID, tableColumnID));

        public List<TableColumnChoice> GetActiveByTableColumnID(long tableColumnID) =>
            GetEntities(Query
                .WhereEqual(x => x.TableColumnID, tableColumnID)
                .WhereEqual(x => x.IsActive, true));

        public List<TableColumnChoice> GetBySteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += 
                $"INNER JOIN tablecolumn tc ON {TableName}.TableColumnID = tc.ID " +
                $"INNER JOIN tablelist tl ON tc.TableListID = tl.ID " +
                $"WHERE tl.SteamGameID = @SteamGameID ";
            query.AddParameter("@SteamGameID", steamGameID);
            return GetEntities(query);
        }

        public List<TableColumnChoice> GetByTableListID(long tableListID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"INNER JOIN tablecolumn tc ON {TableName}.TableColumnID = tc.ID " +
                $"WHERE tc.TableListID = @TableListID ";
            query.AddParameter("@TableListID", tableListID);
            return GetEntities(query);
        }
    }
}
