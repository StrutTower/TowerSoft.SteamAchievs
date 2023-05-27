using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class AchievementDetailsRepository :DbRepository<AchievementDetails> {
        public AchievementDetailsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<AchievementDetails> Get(long steamGameID, string achievementKey) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.AchievementKey, achievementKey));
        }

        public async Task<List<AchievementDetails>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
