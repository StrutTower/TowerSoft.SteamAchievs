using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamGameUserTagRepository : DbRepository<SteamGameUserTag> {
        public SteamGameUserTagRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<SteamGameUserTag> Get(long steamGameID, long steamUserTagID) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }

        public async Task<List<SteamGameUserTag>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamGameUserTag>> GetBySteamUserTagID(long steamUserTagID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }
    }
}
