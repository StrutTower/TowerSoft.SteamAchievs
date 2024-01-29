using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase {
        private readonly UnitOfWork uow;
        private readonly IMapper mapper;

        public ReportsController(UnitOfWork uow, IMapper mapper) {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<SteamGameModel[]> MissingPerfectPossible() {
            List<SteamGame> games = await uow.GetRepo<SteamGameRepository>().GetWithNullPerfectPossibleAsync();
            return mapper.Map<SteamGameModel[]>(games);
        }
        

        [HttpGet("[action]")]
        public async Task<SteamGameModel[]> MissingPlayNext() {
            List<SteamGame> games = await uow.GetRepo<SteamGameRepository>().GetWithNullPlayPriorityAsync();
            return mapper.Map<SteamGameModel[]>(games);
        }
    }
}
