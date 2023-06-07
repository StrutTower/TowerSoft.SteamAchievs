using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class SteamGameDescriptions : IEquatable<SteamGameDescriptions> {
        [Key]
        public long SteamGameID { get; set; }

        public string SupportedLanguages { get; set; }

        public string LegalNotice { get; set; }

        public string ExternalUserAccountNotice { get; set; }

        public string DetailedDescription { get; set; }

        public string AboutTheGame { get; set; }

        public string ShortDescription { get; set; }

        public string DlcIDs { get; set; }

        public bool Equals(SteamGameDescriptions? other) {
            return other != null && SteamGameID == other.SteamGameID;
        }

        public override int GetHashCode() {
            return SteamGameID.GetHashCode();
        }
    }
}
