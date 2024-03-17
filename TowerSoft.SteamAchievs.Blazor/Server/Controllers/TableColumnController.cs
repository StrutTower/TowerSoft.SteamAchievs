using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TableColumnController : ControllerBase {
        private readonly TableColumnRepository repo;
        private readonly IMapper mapper;

        public TableColumnController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TableColumnRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public TableColumnModel Get(long id) {
            TableColumn listColumn = repo.GetByID(id);
            if (listColumn == null) return null;
            return mapper.Map<TableColumnModel>(listColumn);
        }

        [HttpGet]
        public IEnumerable<TableColumnModel> Get() {
            List<TableColumn> listColumns = repo.GetAll();
            return mapper.Map<TableColumnModel[]>(listColumns);
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public List<TableColumnModel> GetBySteamID(long steamGameID) {
            return mapper.Map<List<TableColumnModel>>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("tableListID/{tableListID}")]
        public IEnumerable<TableColumnModel> GetByTableListID(long tableListID) {
            return mapper.Map<TableColumnModel[]>(repo.GetByTableListID(tableListID));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TableColumnModel model) {
            TableColumn tableColumn = mapper.Map<TableColumn>(model);
            await repo.AddAsync(tableColumn);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(TableColumnModel model) {
            TableColumn tableColumn = mapper.Map<TableColumn>(model);
            await repo.UpdateAsync(tableColumn);
            return Ok();
        }
    }
}
