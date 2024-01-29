using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SteamGameDescriptionsController : ControllerBase {
        private readonly SteamGameDescriptionsRepository repo;
        private readonly IMapper mapper;

        public SteamGameDescriptionsController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamGameDescriptionsRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamGameDescriptionsModel[]> Get() {
            return mapper.Map<SteamGameDescriptionsModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}")]
        public async Task<SteamGameDescriptionsModel> Get(long steamGameID) {
            return mapper.Map<SteamGameDescriptionsModel>(await repo.GetBySteamGameIDAsync(steamGameID));
        }
    }
}
