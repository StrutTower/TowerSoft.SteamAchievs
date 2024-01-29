using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamTower.Models;
using TowerSoft.SteamTower;
using TowerSoft.Utilities;
using Microsoft.Extensions.Options;

namespace TowerSoft.SteamAchievs.Blazor.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SteamGameController : Controller {
        private readonly SteamGameRepository repo;
        private readonly SteamApiClient steamApiClient;
        private readonly SteamSyncService steamSyncService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;

        public SteamGameController(UnitOfWork uow, SteamApiClient steamApiClient, SteamSyncService steamSyncService,
            IMapper mapper, IOptionsSnapshot<AppSettings> appSettings) {

            this.repo = uow.GetRepo<SteamGameRepository>();
            this.steamApiClient = steamApiClient;
            this.steamSyncService = steamSyncService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        [HttpGet("{id}")]
        public SteamGameModel Get(long id) {
            SteamGame game = repo.GetByID(id);
            if (game == null) return null;
            return mapper.Map<SteamGameModel>(game);
        }

        [HttpGet]
        public List<SteamGameModel> Get() {
            return mapper.Map<List<SteamGameModel>>(repo.GetAll());
        }

        [HttpPost("[action]")]
        public List<SteamGameModel> GetByIDs(IEnumerable<long> ids) {
            return mapper.Map<List<SteamGameModel>>(repo.GetByIDs(ids));
        }

        [HttpGet("Search")]
        public async Task<SteamGameModel[]> Search(string q) {
            return mapper.Map<SteamGameModel[]>(await repo.SearchAsync(q));
        }

        [HttpGet("GameListType/{gameListType}")]
        public async Task<SteamGameModel[]> GetByGameListType(GameListType gameListType) {
            return mapper.Map<SteamGameModel[]>(await repo.GetByGameListTypeAsync(gameListType));
        }

        [HttpGet("[action]/{companyID}")]
        public async Task<SteamGameModel[]> GetByCompanyID(long companyID) {
            return mapper.Map<SteamGameModel[]>(await repo.GetByCompanyID(companyID));
        }

        [HttpGet("Resync/{id}")]
        public async Task<IActionResult> ResyncGame(long id) {
            List<OwnedApp> ownedApps = steamApiClient.PlayerClient.GetOwnedApps(appSettings.DefaultSteamUserID).Result;
            List<OwnedApp> ownedApp = ownedApps.Where(x => x.SteamAppID == id).ToList();

            var userAppData = steamSyncService.LoadSteamData(ownedApp);

            steamSyncService.RunAllSyncs(userAppData);
            return Ok();
        }
    }
}
