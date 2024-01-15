using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class SteamCategoryController : ControllerBase {
        private readonly SteamCategoryRepository repo;
        private readonly IMapper mapper;

        public SteamCategoryController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<SteamCategoryRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<SteamCategoryModel[]> Get() {
            return mapper.Map<SteamCategoryModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<SteamCategoryModel> Get(long id) {
            return mapper.Map<SteamCategoryModel>(await repo.GetByIDAsync(id));
        }
    }
}
