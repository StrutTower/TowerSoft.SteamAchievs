using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TableRowController : ControllerBase {
        private readonly TableRowRepository repo;
        private readonly UnitOfWork uow;
        private readonly IMapper mapper;

        public TableRowController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TableRowRepository>();
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<TableRowModel> Get(long id) {
            TableRow listRow = repo.GetByID(id);
            if (listRow == null) return null;
            return mapper.Map<TableRowModel>(listRow);
        }

        [HttpGet]
        public async Task<IEnumerable<TableRowModel>> Get() {
            List<TableRow> listRows = repo.GetAll();
            return mapper.Map<TableRowModel[]>(listRows);
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public async Task<IEnumerable<TableRowModel>> GetBySteamGameID(long steamGameID) {
            return mapper.Map<TableRowModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("tableListID/{tableListID}")]
        public async Task<IEnumerable<TableRowModel>> GetByTableListID(long tableListID) {
            return mapper.Map<TableRowModel[]>(repo.GetByTableListID(tableListID));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TableRowEditModel model) {
            uow.BeginTransaction();
            try {
                TableRow tableRow = mapper.Map<TableRow>(model.TableRow);
                tableRow.TrimProperties();
                await repo.AddAsync(tableRow);
                AddOrUpdateTableData(model, tableRow);
                uow.CommitTransaction();
                return Ok();
            } catch {
                uow.RollbackTransaction();
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(TableRowEditModel model) {
            uow.BeginTransaction();
            try {
                TableRow tableRow = mapper.Map<TableRow>(model.TableRow);
                tableRow.TrimProperties();
                await repo.UpdateAsync(tableRow);
                AddOrUpdateTableData(model, tableRow);
                uow.CommitTransaction();
                return Ok();
            } catch {
                uow.RollbackTransaction();
                return BadRequest();
            }
        }

        [HttpPost("Completion")]
        public async Task<IActionResult> PostCompletion(TableRowModel model) {
            TableRowRepository repo = uow.GetRepo<TableRowRepository>();
            TableRow existing = repo.GetByID(model.ID);
            existing.IsComplete = model.IsComplete;
            repo.Update(existing);
            return Ok();
        }

        private void AddOrUpdateTableData(TableRowEditModel model, TableRow tableRow) {
            TableDataRepository tdRepo = uow.GetRepo<TableDataRepository>();
            foreach (var data in model.ColumnValues) {
                TableData existing = tdRepo.Get(tableRow.ID, data.ColumnID);
                TableData posted = mapper.Map<TableData>(data.Data);
                posted.TableRowID = tableRow.ID;
                posted.TableColumnID = data.ColumnID;
                posted.TrimProperties();
                if (existing == null) {
                    tdRepo.Add(posted);
                } else {
                    tdRepo.Update(posted);
                }
            }
        }
    }
}
