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

        public SteamGameUserTag Get(long steamGameID, long steamUserTagID) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }
        public async Task<SteamGameUserTag> GetAsync(long steamGameID, long steamUserTagID) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }

        public List<SteamGameUserTag> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<SteamGameUserTag>> GetBySteamGameIDAsync(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<SteamGameUserTag> GetBySteamUserTagID(long steamUserTagID) {
            return GetEntities(WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }

        public async Task<List<SteamGameUserTag>> GetBySteamUserTagIDAsync(long steamUserTagID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamUserTagID, steamUserTagID));
        }
    }
}
