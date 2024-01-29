using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SteamGameUserTagController : ControllerBase {
        private readonly SteamGameUserTagRepository repo;
        private readonly IMapper mapper;

        public SteamGameUserTagController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamGameUserTagRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamGameUserTagModel[]> Get() {
            return mapper.Map<SteamGameUserTagModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}/{steamUserTagID}")]
        public async Task<SteamGameUserTagModel> Get(long steamGameID, long steamUserTagID) {
            return mapper.Map<SteamGameUserTagModel>(await repo.GetAsync(steamGameID, steamUserTagID));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public async Task<SteamGameUserTagModel[]> GetBySteamGameID(long steamGameID) {
            return mapper.Map<SteamGameUserTagModel[]>(await repo.GetBySteamGameIDAsync(steamGameID));
        }

        [HttpGet("SteamUserTagID/{steamUserTagID}")]
        public async Task<SteamGameUserTagModel[]> GetBySteamUserTagID(long steamUserTagID) {
            return mapper.Map<SteamGameUserTagModel[]>(await repo.GetBySteamUserTagIDAsync(steamUserTagID));
        }
    }
}
