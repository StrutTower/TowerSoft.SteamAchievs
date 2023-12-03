using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels.Reports;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class SteamUserTagDataService : DataServiceBase {
        private readonly UnitOfWork uow;

        public SteamUserTagDataService(UnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
            this.uow = uow;
        }

        public SteamCategoriesListModel GetSteamUserTagsListModel() {
            SteamCategoriesListModel model = new() {
                ResultsActionName = nameof(Controllers.ReportsController.SteamUserTagGames),
                SteamCategories = new List<SteamCategoryReportModel>()
            };

            List<SteamUserTag> tags = uow.GetRepo<SteamUserTagRepository>().GetAll();
            List<SteamGameUserTag> gameTags = uow.GetRepo<SteamGameUserTagRepository>().GetAll();

            foreach(SteamUserTag tag in tags) {
                model.SteamCategories.Add(new() {
                    ID = tag.ID,
                    Name = tag.Name,
                    GameCount = gameTags.Where(x => x.SteamUserTagID == tag.ID).Count()
                });
            }

            return model;
        }
    }
}
