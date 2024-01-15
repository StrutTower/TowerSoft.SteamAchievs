using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase {
        private readonly TagRepository repo;
        private readonly IMapper mapper;

        public TagController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TagRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public TagModel[] Get() {
            return mapper.Map<TagModel[]>(repo.GetAll());
        }

        [HttpGet("{id}")]
        public TagModel GetByID(long id) {
            return mapper.Map<TagModel>(repo.GetByID(id));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public TagModel[] GetBySteamGameID(long steamGameID) {
            return mapper.Map<TagModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("Assigned/{steamGameID}")]
        public TagModel[] GetAssigned(long steamGameID) {
            return mapper.Map<TagModel[]>(repo.GetActiveForSteamGameID(steamGameID));
        }
    }
}
