using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class EditTableRowModel {
        public TableRowEditModel? EditModel { get; set; }

        public TableListModel? TableList { get; set; }
        public SteamGameModel? SteamGame { get; set; }
    }
}
