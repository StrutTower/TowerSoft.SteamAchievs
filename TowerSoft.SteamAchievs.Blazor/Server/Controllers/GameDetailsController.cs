using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class GameDetailsController : ControllerBase {
        private readonly GameDetailsRepository repo;
        private readonly IMapper mapper;

        public GameDetailsController(UnitOfWork uow, IMapper mapper) {
            this.repo = uow.GetRepo<GameDetailsRepository>();
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<GameDetailsModel[]> Get() {
            return mapper.Map<GameDetailsModel[]>(await repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<GameDetailsModel> Get(long id) {
            return mapper.Map<GameDetailsModel>(await repo.GetBySteamGameIDAsync(id));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetByIDs(IEnumerable<long> ids) {
            return Ok(mapper.Map<GameDetailsModel[]>(await repo.GetByIDsAsync(ids)));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(GameDetailsModel model) {
            GameDetails existing = repo.GetBySteamGameID(model.SteamGameID);
            bool isNew = false;
            bool needsUpdate = false;
            if (existing == null) {
                isNew = true;
                existing = new() {
                    SteamGameID = model.SteamGameID
                };
            }

            if (existing.PerfectPossible != model.PerfectPossible) {
                needsUpdate = true;
                existing.PerfectPossible = model.PerfectPossible;
            }

            if (existing.PlayNextScore != model.PlayNextScore) {
                needsUpdate = true;
                existing.PlayNextScore = model.PlayNextScore;
            }

            if (existing.Finished != model.Finished) {
                needsUpdate = true;
                existing.Finished = model.Finished;
            }

            if (existing.HowLongToBeatID != model.HowLongToBeatID) {
                needsUpdate = true;
                existing.HowLongToBeatID = model.HowLongToBeatID;
            }

            if (isNew)
                repo.Add(existing);
            else if (needsUpdate)
                repo.Update(existing);

            return Ok();
        }
    }
}
