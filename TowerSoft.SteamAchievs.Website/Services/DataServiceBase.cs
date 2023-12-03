using Microsoft.AspNetCore.Http;

namespace TowerSoft.SteamAchievs.Website.Services {
    public abstract class DataServiceBase {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DataServiceBase(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
