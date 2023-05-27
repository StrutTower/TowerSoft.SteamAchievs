using System.ComponentModel.DataAnnotations;

namespace TowerSoft.SteamAchievs.Lib.Domain {
    public class GameDetails {
        [Key]
        public long SteamGameID { get; set; }

        public long HowLongToBeathID { get; set; }

        public bool PerfectPossible { get; set; }

        public int PlayNextScore { get; set; }

        public bool Finished { get; set; }

        public string ProtonDbRating { get; set; }

        public double MainStoryTime { get; set; }

        public double MainAndSidesTime { get; set; }

        public double CompletionistTime { get; set; }

        public double AllStylesTime { get; set; }
    }
}
