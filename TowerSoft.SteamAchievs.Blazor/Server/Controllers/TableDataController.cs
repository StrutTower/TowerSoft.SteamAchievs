using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TableDataController : ControllerBase {
        private readonly TableDataRepository repo;
        private readonly IMapper mapper;

        public TableDataController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<TableDataRepository>();
            this.mapper = mapper;
        }

        [HttpGet("{listRowID}/{listColumnID}")]
        public TableDataModel Get(long listRowID, long listColumnID) {
            TableData listValue = repo.Get(listRowID, listColumnID);
            if (listValue == null) return null;
            return mapper.Map<TableDataModel>(listValue);
        }

        [HttpGet]
        public IEnumerable<TableDataModel> Get() {
            List<TableData> listValues = repo.GetAll();
            return mapper.Map<TableDataModel[]>(listValues);
        }

        [HttpGet("steamGameID/{steamGameID}")]
        public IEnumerable<TableDataModel> GetBySteamGameID(long steamGameID) {
            return mapper.Map<TableDataModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpGet("tableListID/{tableListID}")]
        public IEnumerable<TableDataModel> GetByTableListID(long tableListID) {
            return mapper.Map<TableDataModel[]>(repo.GetByTableListID(tableListID));
        }
    }
}
