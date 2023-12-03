using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels.Reports;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class SteamCategoryDataService : DataServiceBase {
        private readonly UnitOfWork uow;

        public SteamCategoryDataService(UnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
            this.uow = uow;
        }

        internal SteamCategoriesListModel GetSteamCategoriesListModel() {
            SteamCategoriesListModel model = new() {
                ResultsActionName = nameof(Controllers.ReportsController.SteamCategoryGames),
                SteamCategories = new()
            };

            List<SteamCategory> categories = uow.GetRepo<SteamCategoryRepository>().GetAll();
            List<SteamGameCategory> gameCategories = uow.GetRepo<SteamGameCategoryRepository>().GetAll();

            foreach(SteamCategory category in categories) {
                model.SteamCategories.Add(new() {
                    ID = category.ID,
                    Name = category.Name,
                    GameCount = gameCategories.Where(x => x.SteamCategoryID == category.ID).Count()
                });
            }
            return model;
        }
    }
}
