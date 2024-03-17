using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TableColumnChoiceController : ControllerBase {
        private readonly TableColumnChoiceRepository repo;
        private readonly IMapper mapper;

        public TableColumnChoiceController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TableColumnChoiceRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public TableColumnChoiceModel Get(long id) {
            TableColumnChoice choice = repo.GetByID(id);
            if (choice == null) return null;
            return mapper.Map<TableColumnChoiceModel>(choice);
        }

        [HttpGet]
        public IEnumerable<TableColumnChoiceModel> Get() {
            List<TableColumnChoice> choices = repo.GetAll();
            return mapper.Map<TableColumnChoiceModel[]>(choices);
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public IEnumerable<TableColumnChoiceModel> GetBySteamGameID(long steamGameID) {
            return mapper.Map<TableColumnChoiceModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("tableListID/{tableListID}")]
        public IEnumerable<TableColumnChoiceModel> GetByTableListID(long tableListID) {
            return mapper.Map<TableColumnChoiceModel[]>(repo.GetByTableListID(tableListID));
        }

        [HttpGet("tableColumnID/{tableColumnID}")]
        public IEnumerable<TableColumnChoiceModel> GetByTableColumnID(long tableColumnID) {
            return mapper.Map<TableColumnChoiceModel[]>(repo.GetByTableColumnID(tableColumnID));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TableColumnChoiceModel model) {
            TableColumnChoice tableColumnChoice = mapper.Map<TableColumnChoice>(model);
            await repo.AddAsync(tableColumnChoice);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(TableColumnChoice model) {
            TableColumnChoice tableColumnChoice = mapper.Map<TableColumnChoice>(model);
            await repo.UpdateAsync(tableColumnChoice);
            return Ok();
        }
    }
}
