using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class AchievementTagRepository : DbRepository<AchievementTag> {
        public AchievementTagRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public AchievementTag Get(long steamGameID, string achievementKey, long tagID) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.AchievementKey, achievementKey)
                .WhereEqual(x => x.TagID, tagID));
        }

        public List<AchievementTag> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<AchievementTag> GetByKeyAndSteamGameID(string achievementKey, long steamGameID) {
            return GetEntities(Query
                .WhereEqual(x => x.AchievementKey, achievementKey)
                .WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<AchievementTag> GetBySteamGameIDAndTagID(long steamGameID, long tagID) {
            return GetEntities(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.TagID, tagID));
        }
    }
}
