using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SteamAchievementSchemaController : ControllerBase {
        private readonly SteamAchievementSchemaRepository repo;
        private readonly IMapper mapper;

        public SteamAchievementSchemaController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamAchievementSchemaRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamAchievementSchemaModel[]> Get() {
            return mapper.Map<SteamAchievementSchemaModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}/{achievementKey}")]
        public async Task<SteamAchievementSchemaModel> Get(long steamGameID, string achievementKey) {
            return mapper.Map<SteamAchievementSchemaModel>(await repo.GetAsync(steamGameID, achievementKey));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public async Task<SteamAchievementSchemaModel[]> GetBySteamGameID(long steamGameID) {
            return mapper.Map<SteamAchievementSchemaModel[]>(await repo.GetBySteamGameIDAsync(steamGameID));
        }

        [HttpPost("[action]")]
        public async Task<SteamAchievementSchemaModel[]> GetBySteamGameIDs(IEnumerable<long> steamGameIDs) {
            return mapper.Map<SteamAchievementSchemaModel[]>(repo.GetByIDs(steamGameIDs));
        }
    }
}
