using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamGridDbWrapper.Models;
using TowerSoft.SteamTower.Models;

namespace TowerSoft.SteamAchievs.Cron.Models {
    public class UserAppModel {
        public SteamApp SteamApp { get; set; }

        public OwnedApp OwnedApp { get; set; }

        public List<GlobalAchievementStat> GlobalAchievementStats { get; set; }

        public GameStatsSchema GameStatsSchema { get; set; }

        public List<UserAchievement> UserAchievements { get; set; }

        public DeckCompatibility DeckCompatibility { get; set; }

        public ReviewSummary ReviewSummary { get; set; }

        public SteamGridImage? GridImage { get; set; }

        public SteamGridImage? HeroImage { get; set; }

        public HltbModel? HltbModel { get; set; }

        public ProtonDbGame? ProtonDbGame { get; set; }

        public List<string> UserTags { get; set; }

        public bool Delisted { get; set; }
    }
}
