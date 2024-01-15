using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamGameDescriptionsRepository : DbRepository<SteamGameDescriptions> {
        public SteamGameDescriptionsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamGameDescriptions GetBySteamGameID(long steamGameID) {
            return GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<SteamGameDescriptions> GetBySteamGameIDAsync(long steamGameID) {
            return await GetSingleEntityAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
