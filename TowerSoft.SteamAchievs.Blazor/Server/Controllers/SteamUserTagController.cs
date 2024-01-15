using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class SteamUserTagController : ControllerBase {
        private readonly SteamUserTagRepository repo;
        private readonly IMapper mapper;

        public SteamUserTagController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamUserTagRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamUserTagModel[]> Get() {
            return mapper.Map<SteamUserTagModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<SteamUserTagModel> Get(long id) {
            return mapper.Map<SteamUserTagModel>(await repo.GetByIDAsync(id));
        }
    }
}
