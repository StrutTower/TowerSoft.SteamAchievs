using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class AchievementDetailsRepository : DbRepository<AchievementDetails> {
        public AchievementDetailsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public AchievementDetails Get(long steamGameID, string achievementKey) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.AchievementKey, achievementKey));
        }

        public List<AchievementDetails> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
