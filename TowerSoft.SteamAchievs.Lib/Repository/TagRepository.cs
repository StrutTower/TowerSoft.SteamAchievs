using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class TagRepository : DbRepository<Tag> {
        public TagRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public Tag GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public List<Tag> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        /// <summary>
        /// Returns all active global tags and tags assigned to the specified game
        /// </summary>
        /// <param name="steamGameID"></param>
        /// <returns></returns>
        public List<Tag> GetActiveForSteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"WHERE IsActive = 1 " +
                $"AND (" +
                    $"SteamGameID = @SteamGameID " +
                    $"OR SteamGameID IS NULL" +
                $")";
            query.AddParameter("@SteamGameID", steamGameID);
            return GetEntities(query);
        }
    }
}
