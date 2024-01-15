using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class GameDetailsController : ControllerBase {
        private readonly GameDetailsRepository repo;
        private readonly IMapper mapper;

        public GameDetailsController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<GameDetailsRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<GameDetailsModel[]> Get() {
            return mapper.Map<GameDetailsModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<GameDetailsModel> Get(long id) {
            return mapper.Map<GameDetailsModel>(await repo.GetBySteamGameIDAsync(id));
        }

        [HttpGet("GetByIDs/{ids}")]
        public async Task<GameDetailsModel[]> GetByIDs(string ids) {
            List<long> ids2 = ids.Split(',').Select(x => long.Parse(x)).ToList();
            return mapper.Map<GameDetailsModel[]>(await repo.GetByIDsAsync(ids2));
        }
    }
}
