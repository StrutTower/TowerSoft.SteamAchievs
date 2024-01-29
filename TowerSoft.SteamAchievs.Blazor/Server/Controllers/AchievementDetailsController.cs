using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]"), ApiController]
    public class AchievementDetailsController : ControllerBase {
        private readonly AchievementDetailsRepository repo;
        private readonly UnitOfWork uow;
        private readonly IMapper mapper;

        public AchievementDetailsController(UnitOfWork uow, IMapper mapper) {
            repo = uow.GetRepo<AchievementDetailsRepository>();
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<AchievementDetailsModel[]> GetAll() {
            return mapper.Map<AchievementDetailsModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}/{achievementKey}")]
        public AchievementDetailsModel Get(long steamGameID, string achievementKey) {
            return mapper.Map<AchievementDetailsModel>(repo.Get(steamGameID, achievementKey));
        }

        [HttpGet("{steamGameID}")]
        public List<AchievementDetailsModel> Get(long steamGameID) {
            return mapper.Map<List<AchievementDetailsModel>>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpPost]
        public IActionResult AddOrUpdate(EditAchievementDetailsModel model) {
            AchievementDetails existing = repo.Get(model.SteamGameID, model.AchievementKey);
            bool isNew = false;
            bool needsUpdate = false;
            if (existing == null) {
                isNew = true;
                existing = new() {
                    SteamGameID = model.SteamGameID,
                    AchievementKey = model.AchievementKey
                };
            }

            if (existing.Description != model.ManualDescription) {
                needsUpdate = true;
                existing.Description = model.ManualDescription;
            }

            if (isNew)
                repo.Add(existing);
            else if (needsUpdate)
                repo.Update(existing);

            AchievementTagRepository atRepo = uow.GetRepo<AchievementTagRepository>();
            List<AchievementTag> selectedTags = [];
            foreach(long tagID in model.TagIDs) {
                selectedTags.Add(new() {
                    AchievementKey = model.AchievementKey,
                    SteamGameID = model.SteamGameID,
                    TagID = tagID
                });
            }

            List<AchievementTag> existingTags = atRepo.GetByKeyAndSteamGameID(model.AchievementKey, model.SteamGameID);

            foreach(AchievementTag toRemove in existingTags.Except(selectedTags)) {
                atRepo.Remove(toRemove);
            }

            foreach(AchievementTag toAdd in selectedTags.Except(existingTags)) {
                atRepo.Add(toAdd);
            }

            return Ok();
        }
    }
}
