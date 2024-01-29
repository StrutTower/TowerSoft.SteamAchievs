using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameCompanyRepository  : DbRepository<GameCompany>{
        public GameCompanyRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public GameCompany Get(long steamGameID, long companyID) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.CompanyID, companyID));
        }

        public GameCompany GetBySteamGameID(long steamGameID) =>
            GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));

        public GameCompany GetByCompanyID(long comanyID) =>
            GetSingleEntity(WhereEqual(x => x.CompanyID, comanyID));

        public List<GameCompany> GetBySteamGameIDs(IEnumerable<long> ids) {
            if (!ids.SafeAny()) return default;

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
