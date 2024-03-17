using Microsoft.Extensions.Configuration;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.MySql;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class UnitOfWork : UnitOfWorkBase {
        public UnitOfWork(IConfiguration config) {
            DbAdapter = new MySqlDbAdapter(config.GetConnectionString("default"));
        }

        private readonly Dictionary<Type, object> repos = new();

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!repos.ContainsKey(type)) repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)repos[type];
        }
    }
}
