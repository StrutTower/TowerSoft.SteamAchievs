using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GameCompanyController : ControllerBase {
        GameCompanyRepository repo;
        IMapper mapper;
        public GameCompanyController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<GameCompanyRepository>();
            this.mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<GameCompanyModel[]> GetByIDs(IEnumerable<long> ids) {
            return mapper.Map<GameCompanyModel[]>(repo.GetBySteamGameIDs(ids));
        }
    }
}
