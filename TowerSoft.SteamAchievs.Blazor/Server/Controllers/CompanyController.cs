using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]"), ApiController]
    public class CompanyController : ControllerBase {
        CompanyRepository repo;
        private readonly IMapper mapper;

        public CompanyController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<CompanyRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public CompanyModel[] Get() {
            return mapper.Map<CompanyModel[]>(repo.GetAll());
        }

        [HttpGet("{id}")]
        public CompanyModel GetByID(long id) {
            return mapper.Map<CompanyModel>(repo.GetByID(id));
        }

        [HttpGet("[action]/{steamGameID}")]
        public CompanyModel[] SteamGameID(long steamGameID) {
            return mapper.Map<CompanyModel[]>(repo.GetBySteamGameID(steamGameID));
        }

        [HttpPut]
        public IActionResult Update(CompanyModel model) {
            Company company = mapper.Map<Company>(model);
            repo.Update(company);
            return Ok();
        }
    }
}
