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

        public async Task<SteamGameCategory> Get(long steamGameID, long steamCategoryID) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.SteamCategoryID, steamCategoryID));
        }

        public async Task<List<SteamGameCategory>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamGameCategory>> GetBySteamCategoryID(long steamCategoryID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamCategoryID, steamCategoryID));
        }
    }
}
