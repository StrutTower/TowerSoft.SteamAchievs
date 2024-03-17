using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class TableRowEditModel {
        public TableRowModel TableRow { get; set; }

        public List<TableRowColEditModel> ColumnValues { get; set; } = [];
    }
}
