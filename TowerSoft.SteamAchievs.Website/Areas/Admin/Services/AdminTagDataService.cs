using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Areas.Admin.ViewModels;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Website.Areas.Admin.Services {
    public class AdminTagDataService {
        private readonly UnitOfWork uow;
        private readonly ITempDataDictionary tempData;

        public AdminTagDataService(UnitOfWork uow, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory) {
            this.uow = uow;
            tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
        }

        internal async Task<AdminTagListModel> GetAdminTagListModel() {
            Dictionary<long, SteamGame> games = (await uow.GetRepo<SteamGameRepository>().GetAllAsync()).ToDictionary(x => x.ID);

            AdminTagListModel model = new AdminTagListModel {
                Tags = new()
            };

            foreach (Tag tag in await uow.GetRepo<TagRepository>().GetAllAsync()) {
                TagGameModel tagModel = new() {
                    Tag = tag,
                };
                if (tag.SteamGameID.HasValue) {
                    tagModel.SteamGame = games[tag.SteamGameID.Value];
                }
                model.Tags.Add(tagModel);
            }

            model.GameList = new SelectList(model.Tags
                .Where(x => x.SteamGame != null)
                .Select(x => x.SteamGame).OrderBy(x => x.Name)
                .DistinctBy(x => x.ID), "ID", "Name");

            return model;
        }

        internal async Task<EditTagModel> GetEditTagModel(long? id, Tag tagModel = null, long? steamGameID = null) {
            EditTagModel model = new();

            if (steamGameID.HasValue) {
                model.Tag = new() {
                    IsActive = true,
                    BackgroundColor = "#268d96",
                    SteamGameID = steamGameID.Value
                };
                model.ReturnToGameView = true;
            } else if (tagModel != null) {
                model.Tag = tagModel;
            } else if (id.HasValue) {
                model.Tag = uow.GetRepo<TagRepository>().GetByID(id.Value);
            } else
                model.Tag = new() { 
                    IsActive = true,
                    BackgroundColor = "#268d96"
                };

            if (model.Tag.SteamGameID.HasValue) {
                model.SteamGame = uow.GetRepo<SteamGameRepository>().GetByID(model.Tag.SteamGameID.Value);
            }

            return model;
        }

        internal async Task AddOrUpdateTag(Tag tag) {
            TagRepository repo = uow.GetRepo<TagRepository>();

            tag.TrimProperties();
            if (tag.ID == 0) {
                await repo.AddAsync(tag);
                tempData["message"] = "Tag Added";
            } else {
                await repo.UpdateAsync(tag);
                tempData["message"] = "Tag Updated";
            }
        }
    }
}
