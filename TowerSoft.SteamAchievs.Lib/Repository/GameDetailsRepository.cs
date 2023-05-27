using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class GameDetailsRepository : DbRepository<GameDetails> {
        public GameDetailsRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public async Task<GameDetails> GetBySteamGameID(long steamGameID) {
            return await GetSingleEntityAsync(WhereEqual(x => x.SteamGameID, steamGameID));
        }
    }
}
