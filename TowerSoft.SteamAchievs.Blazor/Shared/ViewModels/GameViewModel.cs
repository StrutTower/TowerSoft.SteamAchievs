﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class GameViewModel {
        public SteamGameModel? SteamGame { get; set; }
        public GameDetailsModel? GameDetails { get; set; }
        public SteamGameDescriptionsModel? Descriptions { get; set; }
        public List<TagModel> ComplicationTags { get; set; }
        public List<SteamUserTagModel> UserTags { get; set; }
        public List<SteamCategoryModel> Categories { get; set; }
        public List<AchievementViewModel> Achievements { get; set; }
    }
}