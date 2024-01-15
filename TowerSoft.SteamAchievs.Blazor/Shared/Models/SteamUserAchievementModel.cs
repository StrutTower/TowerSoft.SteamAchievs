namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class SteamUserAchievementModel {
        public long SteamGameID { get; set; }

        public string KeyName { get; set; }

        public bool Achieved { get; set; }

        public DateTime? AchievedOn { get; set; }
    }
}
