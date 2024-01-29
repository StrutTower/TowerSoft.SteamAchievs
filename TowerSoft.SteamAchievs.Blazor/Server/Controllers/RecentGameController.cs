using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RecentGameController : ControllerBase {
        private readonly RecentGameRepository repo;
        private readonly IMapper mapper;

        public RecentGameController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<RecentGameRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public RecentGameModel Get(long id) {
            RecentGame game = repo.GetBySteamGameID(id);
            if (game == null) return null;
            return mapper.Map<RecentGameModel>(game);
        }

        [HttpGet]
        public IEnumerable<RecentGameModel> Get() {
            List<RecentGame> games = repo.GetAll();
            return mapper.Map<List<RecentGameModel>>(games);
        }
    }
}
