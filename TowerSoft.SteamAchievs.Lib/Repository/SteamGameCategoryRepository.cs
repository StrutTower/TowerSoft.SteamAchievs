using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamGameCategoryRepository : DbRepository<SteamGameCategory> {
        public SteamGameCategoryRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamGameCategory Get(long steamGameID, long steamCategoryID) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.SteamCategoryID, steamCategoryID));
        }

        public List<SteamGameCategory> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<SteamGameCategory> GetBySteamCategoryID(long steamCategoryID) {
            return GetEntities(WhereEqual(x => x.SteamCategoryID, steamCategoryID));
        }
    }
}
