using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamUserAchievementRepository : DbRepository<SteamUserAchievement> {
        public SteamUserAchievementRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamUserAchievement Get(long steamGameID, string key) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.KeyName, key));
        }

        public async Task<SteamUserAchievement> GetAsync(long steamGameID, string key) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.KeyName, key));
        }

        public List<SteamUserAchievement> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamUserAchievement>> GetBySteamGameIDAsync(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamUserAchievement>> GetBySteamGameIDsAsync(IEnumerable<long> steamGameIDs) {
            if (!steamGameIDs.SafeAny()) return default;

            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"WHERE SteamGameID IN (";

            int counter = 1;
            List<string> inStatements = new();
            foreach (long id in steamGameIDs) {
                inStatements.Add($"@{counter}");
                query.AddParameter($"@{counter}", id);
                counter++;
            }

            query.SqlQuery += string.Join(",", inStatements) + ") ";

            return await GetEntitiesAsync(query);
        }
    }
}
