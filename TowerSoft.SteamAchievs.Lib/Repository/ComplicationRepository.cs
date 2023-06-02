using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class ComplicationRepository : DbRepository<Complication> {
        public ComplicationRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public Complication GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }
    }
}
