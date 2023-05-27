using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamCategoryRepository : DbRepository<SteamCategory> {
        public SteamCategoryRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<SteamCategory> GetByID(long id) {
            return await GetSingleEntityAsync(WhereEqual(x => x.ID, id));
        }
    }
}
