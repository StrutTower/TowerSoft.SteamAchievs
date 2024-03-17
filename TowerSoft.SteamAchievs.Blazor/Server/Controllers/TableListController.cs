using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TableListController : ControllerBase {
        private readonly TableListRepository repo;
        private readonly IMapper mapper;

        public TableListController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TableListRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public TableListModel Get(long id) {
            TableList gameList = repo.GetByID(id);
            if (gameList == null) return null;
            return mapper.Map<TableListModel>(gameList);
        }

        [HttpGet]
        public IEnumerable<TableListModel> Get() {
            List<TableList> gameLists = repo.GetAll();
            return mapper.Map<TableListModel[]>(gameLists);
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public IEnumerable<TableListModel> GetBySteamID(long steamGameID) {
            return mapper.Map<TableListModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TableListModel model) {
            TableList tableList = mapper.Map<TableList>(model);
            await repo.AddAsync(tableList);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(TableListModel model) {
            TableList tableList = mapper.Map<TableList>(model);
            await repo.UpdateAsync(tableList);
            return Ok();
        }
    }
}
