using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SteamUserAchievementController : ControllerBase {
        private readonly SteamUserAchievementRepository repo;
        private readonly IMapper mapper;

        public SteamUserAchievementController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamUserAchievementRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamUserAchievementModel[]> Get() {
            return mapper.Map<SteamUserAchievementModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}/{key}")]
        public async Task<SteamUserAchievementModel> Get(long steamGameID, string key) {
            return mapper.Map<SteamUserAchievementModel>(await repo.GetAsync(steamGameID, key));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public async Task<SteamUserAchievementModel[]> GetBySteamGameID(long steamGameID) {
            return mapper.Map<SteamUserAchievementModel[]>(await repo.GetBySteamGameIDAsync(steamGameID));
        }

        [HttpGet("[action]")]
        public async Task<SteamUserAchievementModel[]> LatestUnlocked() {
            return mapper.Map<SteamUserAchievementModel[]>(await repo.GetLatestUnlocked());  
        }

        [HttpPost("[action]")]
        public async Task<SteamUserAchievementModel[]> GetByIDs(IEnumerable<long> ids) {
            return mapper.Map<SteamUserAchievementModel[]>(await repo.GetBySteamGameIDsAsync(ids));
        }
    }
}
