namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class SteamAchievementSchemaModel {
        public string KeyName { get; set; }

        public long SteamGameID { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsHidden { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string IconGrayUrl { get; set; }

        public double GlobalCompletionPercentage { get; set; }

        public bool RemovedFromSteam { get; set; }
    }
}
