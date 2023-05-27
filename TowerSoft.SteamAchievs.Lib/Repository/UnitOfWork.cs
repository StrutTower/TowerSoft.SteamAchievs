using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.MySql;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class UnitOfWork : IRepositoryUnitOfWork {
        public UnitOfWork(IConfiguration config) {
            DbAdapter = new MySqlDbAdapter(config.GetConnectionString("default"));
        }

        public IDbAdapter DbAdapter { get; }

        public void BeginTransaction() => DbAdapter.BeginTransaction();

        public void CommitTransaction() => DbAdapter.CommitTransaction();

        public void RollbackTransaction() => DbAdapter.RollbackTransaction();

        public void Dispose() => DbAdapter.Dispose();


        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();

        public TRepo GetRepo<TRepo>() where TRepo : IDbRepository {
            Type type = typeof(TRepo);

            if (!repos.ContainsKey(type)) repos[type] = Activator.CreateInstance(type, this);
            return (TRepo)repos[type];
        }
    }
}
