using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class GameCompanyViewModel {
        public SteamGameModel SteamGame { get; set; }

        public List<CompanyModel> Developers { get; set; }

        public List<CompanyModel> Publishers { get; set; }
    }
}
