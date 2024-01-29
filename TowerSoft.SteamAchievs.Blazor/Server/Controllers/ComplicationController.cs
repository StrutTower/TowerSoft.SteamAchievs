using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]"), ApiController]
    public class ComplicationController : ControllerBase {
        private readonly ComplicationRepository repo;
        private readonly IMapper mapper;

        public ComplicationController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<ComplicationRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public List<ComplicationModel> Get() {
            return mapper.Map<List<ComplicationModel>>(repo.GetAll());
        }

        [HttpGet("{id}")]
        public ComplicationModel Get(long id) {
            return mapper.Map<ComplicationModel>(repo.GetByID(id));
        }
    }
}
