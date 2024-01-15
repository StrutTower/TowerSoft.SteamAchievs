using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
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

        [HttpGet("GetByIDs/{ids}")]
        public async Task<SteamUserAchievementModel[]> GetByIDs(string ids) {
            List<long> ids2 = ids.Split(',').Select(x => long.Parse(x)).ToList();
            return mapper.Map<SteamUserAchievementModel[]>(await repo.GetBySteamGameIDsAsync(ids2));
        }
    }
}
