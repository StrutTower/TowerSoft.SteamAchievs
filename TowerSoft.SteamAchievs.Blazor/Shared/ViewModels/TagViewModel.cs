using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TagViewModel {
        public TagModel Tag { get; set; }

        public SteamGameModel? SteamGame { get; set; }
    }
}
