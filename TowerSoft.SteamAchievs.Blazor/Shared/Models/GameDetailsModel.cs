﻿namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class GameDetailsModel : ICloneable {
        public long SteamGameID { get; set; }

        public long? HowLongToBeatID { get; set; }

        public bool? PerfectPossible { get; set; }

        public int? PlayNextScore { get; set; }

        public bool? Finished { get; set; }

        public bool NotifyIfAchievementsAdded { get; set; }

        public string? ProtonDbRating { get; set; }

        public double? MainStoryTime { get; set; }

        public double? MainAndSidesTime { get; set; }

        public double? CompletionistTime { get; set; }

        public double? AllStylesTime { get; set; }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
