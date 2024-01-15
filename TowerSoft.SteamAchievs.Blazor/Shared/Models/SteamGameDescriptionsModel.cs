namespace TowerSoft.SteamAchievs.Blazor.Shared.Models {
    public class SteamGameDescriptionsModel {
        public long SteamGameID { get; set; }

        public string SupportedLanguages { get; set; }

        public string LegalNotice { get; set; }

        public string ExternalUserAccountNotice { get; set; }

        public string DetailedDescription { get; set; }

        public string AboutTheGame { get; set; }

        public string ShortDescription { get; set; }

        public string DlcIDs { get; set; }
    }
}
