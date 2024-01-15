namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class RecentGameModel {
        public long SteamGameID { get; set; }

        public DateTime FirstDetected { get; set; }

        public bool HasAchievements { get; set; }

        public int CompletedAchievements { get; set; }

        public int TotalAchievements { get; set; }
    }
}
