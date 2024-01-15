using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class SteamGameController : Controller {
        private readonly SteamGameRepository repo;
        private readonly IMapper mapper;

        public SteamGameController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamGameRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public SteamGameModel Get(long id) {
            SteamGame game = repo.GetByID(id);
            if (game == null) return null;
            return mapper.Map<SteamGameModel>(game);
        }

        [HttpGet]
        public List<SteamGameModel> Get() {
            return mapper.Map<List<SteamGameModel>>(repo.GetAll());
        }

        [HttpGet("GetByIDs/{ids}")]
        public List<SteamGameModel> GetByIDs(string ids) {
            List<long> ids2 = ids.Split(',').Select(x => long.Parse(x)).ToList();
            return mapper.Map<List<SteamGameModel>>(repo.GetByIDs(ids2));
        }
    }
}
