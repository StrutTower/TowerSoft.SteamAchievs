using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamUserAchievementRepository : DbRepository<SteamUserAchievement> {
        public SteamUserAchievementRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamUserAchievement Get(long steamGameID, string key) {
            return GetSingleEntity(Query
                .WhereEqual(x => x.SteamGameID, steamGameID)
                .WhereEqual(x => x.KeyName, key));
        }

        public List<SteamUserAchievement> GetBySteamGameID(long steamGameID) {
            return GetEntities(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
