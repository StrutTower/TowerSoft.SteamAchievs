using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class GameComplicationController : ControllerBase {
        private readonly GameComplicationRepository repo;
        private readonly IMapper mapper;

        public GameComplicationController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<GameComplicationRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public List<GameComplicationModel> Get() {
            return mapper.Map<List<GameComplicationModel>>(repo.GetAll());
        }

        [HttpGet("{steamGameID}/{complicationID}")]
        public List<GameComplicationModel> Get(long steamGameID, long complicationID) {
            return mapper.Map<List<GameComplicationModel>>(repo.Get(steamGameID, complicationID));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public List<GameComplicationModel> GetBySteamGameID(long steamGameID) {
            return mapper.Map<List<GameComplicationModel>>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("ComplicationID/{complicationID}")]
        public List<GameComplicationModel> GetByComplicationID(long complicationID) {
            return mapper.Map<List<GameComplicationModel>>(repo.GetByComplicationID(complicationID));
        }
    }
}
