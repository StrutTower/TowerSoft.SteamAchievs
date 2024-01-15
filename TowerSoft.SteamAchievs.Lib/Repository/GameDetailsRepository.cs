using Org.BouncyCastle.Crypto;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameDetailsRepository : DbRepository<GameDetails> {
        public GameDetailsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public GameDetails GetBySteamGameID(long steamGameID) {
            return GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<GameDetails> GetBySteamGameIDAsync(long steamGameID) {
            return await GetSingleEntityAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<GameDetails>> GetByIDsAsync(IEnumerable<long> steamGameIDs) {
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
