using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PerfectGameController : ControllerBase {
        private readonly PerfectGameRepository repo;
        private readonly IMapper mapper;

        public PerfectGameController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<PerfectGameRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public PerfectGameModel Get(long id) {
            PerfectGame game = repo.GetBySteamGameID(id);
            if (game == null) return null;
            return mapper.Map<PerfectGameModel>(game);
        }

        [HttpGet]
        public List<PerfectGameModel> Get() {
            return mapper.Map<List<PerfectGameModel>>(repo.GetAll());
        }

        [HttpGet("IncompleteCount")]
        public int GetIncompleteCount() {
            return repo.GetIncompleteNowGames().Count;
        }

        [HttpGet("Incomplete")]
        public List<PerfectGameModel> GetIncomplete() {
            return mapper.Map<List<PerfectGameModel>>(repo.GetIncompleteNowGames());
        }
    }
}
