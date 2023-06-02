using System;
using System.Collections.Generic;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class AchievementModel {
        public string Key { get; set; }

        public long SteamGameID { get; set; }

        public string Name { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string IconGrayUrl { get; set; }

        public double GlobalCompletionPercentage { get; set; }

        public bool Achieved { get; set; }

        public DateTime? AchievedOn { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
