using TowerSoft.SteamAchievs.Blazor.Shared.Models;

namespace TowerSoft.SteamAchievs.Blazor.Shared.ViewModels {
    public class GameViewModel {
        public SteamGameModel? SteamGame { get; set; }
        public GameDetailsModel? GameDetails { get; set; }
        public SteamGameDescriptionsModel? Descriptions { get; set; }
        public List<TagModel> ComplicationTags { get; set; }
        public List<SteamUserTagModel> UserTags { get; set; }
        public List<SteamCategoryModel> Categories { get; set; }
        public List<AchievementViewModel> Achievements { get; set; }
        public List<GameCompanyViewModel> Companies { get; set; }
        public List<CompanyModel> Developers { get; set; }
        public List<CompanyModel> Publishers { get; set; }
        public List<ManageTableListModel> Lists { get; set; }
    }
}
