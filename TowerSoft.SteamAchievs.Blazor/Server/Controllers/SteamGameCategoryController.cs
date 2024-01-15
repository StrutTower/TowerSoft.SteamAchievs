using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class SteamGameCategoryController : ControllerBase {
        private readonly SteamGameCategoryRepository repo;
        private readonly IMapper mapper;

        public SteamGameCategoryController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamGameCategoryRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamGameCategoryModel[]> Get() {
            return mapper.Map<SteamGameCategoryModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{steamGameID}/{steamCategoryID}")]
        public async Task<SteamGameCategoryModel> Get(long steamGameID, long steamCategoryID) {
            return mapper.Map<SteamGameCategoryModel>(await repo.GetAsync(steamGameID, steamCategoryID));
        }

        [HttpGet("SteamGameID/{steamGameID}")]
        public async Task<SteamGameCategoryModel[]> GetBySteamGameID(long steamGameID) {
            return mapper.Map<SteamGameCategoryModel[]>(await repo.GetBySteamGameIDAsync(steamGameID));
        }

        [HttpGet("SteamCategoryID/{steamCategoryID}")]
        public async Task<SteamGameCategoryModel[]> GetBySteamCategoryID(long steamCategoryID) {
            return mapper.Map<SteamGameCategoryModel[]>(await repo.GetBySteamCategoryIDAsync(steamCategoryID));
        }
    }
}
