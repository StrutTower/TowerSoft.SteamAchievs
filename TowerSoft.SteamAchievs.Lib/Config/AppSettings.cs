namespace TowerSoft.SteamAchievs.Lib.Config {
    public class AppSettings {
        /// <summary>
        /// Name of this application.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Name of the associated company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Homepage URL for the associated company.
        /// </summary>
        public string CompanyHomepageUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string LogPath { get; set; }

        public long DefaultSteamUserID { get; set; }

        public string DefaultSteamUserProfileUrl { get; set; }
    }
}
