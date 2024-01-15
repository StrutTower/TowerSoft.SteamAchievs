using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AchievementDetailsController : ControllerBase {
        private readonly AchievementDetailsRepository repo;
        private readonly IMapper mapper;

        public AchievementDetailsController(UnitOfWork uow, IMapper mapper) {
            repo = uow.GetRepo<AchievementDetailsRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{steamGameID}/{achievementKey}")]
        public AchievementDetailsModel Get(long steamGameID, string achievementKey) {
            return mapper.Map<AchievementDetailsModel>(repo.Get(steamGameID, achievementKey));
        }

        [HttpGet("{steamGameID}")]
        public List<AchievementDetailsModel> Get(long steamGameID) {
            return mapper.Map<List<AchievementDetailsModel>>(repo.GetBySteamGameID(steamGameID));
        }
    }
}
