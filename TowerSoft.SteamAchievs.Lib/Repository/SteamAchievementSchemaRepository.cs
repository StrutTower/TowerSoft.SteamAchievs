using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamAchievementSchemaRepository : DbRepository<SteamAchievementSchema> {
        public SteamAchievementSchemaRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<SteamAchievementSchema> Get(long steamGameID, string achievementKey) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.Key, achievementKey));
        }

        public async Task<List<SteamAchievementSchema>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
