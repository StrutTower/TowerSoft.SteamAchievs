using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class AchievementTagRepository : DbRepository<AchievementTag> {
        public AchievementTagRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<AchievementTag> Get(long steamGameID, string achievementKey, long tagID) {
            return await GetSingleEntityAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.AchievementKey, achievementKey)
                .WhereEqual(x => x.TagID, tagID));
        }

        public async Task<List<AchievementTag>> GetBySteamGameID(long steamGameID) {
            return await GetEntitiesAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public async Task<List<AchievementTag>> GetBySteamGameIDAndTagID(long steamGameID, long tagID) {
            return await GetEntitiesAsync(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.TagID, tagID));
        }
    }
}
