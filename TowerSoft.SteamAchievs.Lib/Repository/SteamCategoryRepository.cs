﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamCategoryRepository : DbRepository<SteamCategory> {
        public SteamCategoryRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamCategory GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public async Task<SteamCategory> GetByIDAsync(long id) {
            return await GetSingleEntityAsync(WhereEqual(x => x.ID, id));
        }
    }
}
