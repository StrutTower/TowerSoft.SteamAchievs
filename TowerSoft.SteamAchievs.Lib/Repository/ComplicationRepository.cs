using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class ComplicationRepository : DbRepository<Complication> {
        public ComplicationRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<Complication> GetByID(long id) {
            return await GetSingleEntityAsync(WhereEqual(x => x.ID, id));
        }
    }
}
