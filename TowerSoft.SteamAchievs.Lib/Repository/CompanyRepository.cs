using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class CompanyRepository : DbRepository<Company> {
        public CompanyRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public Company GetByID(long id) =>
            GetSingleEntity(WhereEqual(x => x.ID, id));

        public Company GetByName(string name) =>
            GetSingleEntity(WhereEqual(x => x.Name, name));

        public Dictionary<long, Company> GetAllDictionary() =>
            GetAll().ToDictionary(x => x.ID);

        public List<Company> GetBySteamGameID(long steamGameID) {
            QueryBuilder query = GetQueryBuilder(new[] { "gc.IsDeveloper"});
            query.SqlQuery +=
                $"INNER JOIN gamecompany gc ON {TableName}.ID = gc.CompanyID " +
                $"WHERE gc.SteamGameID = @SteamGameID ";
            query.AddParameter("@SteamGameID", steamGameID);
            var test =  GetEntities(query);
            return test;
        }
    }
}
