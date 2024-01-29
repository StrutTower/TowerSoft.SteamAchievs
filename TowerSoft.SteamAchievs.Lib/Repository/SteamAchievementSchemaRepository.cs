using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamAchievementSchemaRepository : DbRepository<SteamAchievementSchema> {
        public SteamAchievementSchemaRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamAchievementSchema Get(long steamGameID, string achievementKey) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.KeyName, achievementKey));
        }

        public async Task<SteamAchievementSchema> GetAsync(long steamGameID, string achievementKey) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.KeyName, achievementKey));
        }

        public List<SteamAchievementSchema> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamAchievementSchema>> GetBySteamGameIDAsync(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<SteamAchievementSchema> GetByIDs(IEnumerable<long> ids) {
            if (ids == null || !ids.Any()) return default;

            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"WHERE SteamGameID IN (";

            int counter = 1;
            List<string> inStatements = new();
            foreach (long id in ids) {
                inStatements.Add($"@{counter}");
                query.AddParameter($"@{counter}", id);
                counter++;
            }

            query.SqlQuery += string.Join(",", inStatements) + ") ";

            return GetEntities(query);
        }
    }
}
