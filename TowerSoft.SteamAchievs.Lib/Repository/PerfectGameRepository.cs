using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class PerfectGameRepository : DbRepository<PerfectGame> {
        public PerfectGameRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public PerfectGame GetBySteamGameID(long steamGameID) {
            return GetSingleEntity(WhereEqual(x => x.SteamGameID, steamGameID));
        }

        public List<PerfectGame> GetIncompleteNowGames() {
            return GetEntities(WhereEqual(x => x.IsIncompleteNow, true));
        }
    }
}
