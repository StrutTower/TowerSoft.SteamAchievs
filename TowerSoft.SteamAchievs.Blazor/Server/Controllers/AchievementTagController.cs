using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]"), ApiController]
    public class AchievementTagController : ControllerBase {
        private readonly AchievementTagRepository repo;
        private readonly IMapper mapper;

        public AchievementTagController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<AchievementTagRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public List<AchievementTagModel> Get() {
            return mapper.Map<List<AchievementTagModel>>(repo.GetAll());
        }

        [HttpGet("{steamGameID}/{achievementKey}/{tagID}")]
        public AchievementTagModel Get(long steamGameID, string achievementKey, long tagID) {
            return mapper.Map<AchievementTagModel>(repo.Get(steamGameID, achievementKey, tagID));
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public List<AchievementTagModel> Get(long steamGameID) {
            return mapper.Map<List<AchievementTagModel>>(repo.GetBySteamGameID(steamGameID));
        }

        //[HttpGet("achievementKey/{achievementKey}")]
        //public List<AchievementTagModel> Get(string achievementKey) {
        //    return mapper.Map<List<AchievementTagModel>>(repo.GetBySteamGameID(210));
        //}

        //[HttpGet("tagID/{tagID}")]
        //public List<AchievementTagModel> GetByTagID(long tagID) {
        //    return mapper.Map<List<AchievementTagModel>>(repo.GetBySteamGameID(210));
        //}
    }
}
