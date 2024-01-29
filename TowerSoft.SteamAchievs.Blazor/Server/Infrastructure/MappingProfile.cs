using AutoMapper;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Blazor.Server.Infrastructure {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<AchievementDetails, AchievementDetailsModel>().ReverseMap();
            CreateMap<AchievementTag, AchievementTagModel>().ReverseMap();
            CreateMap<Complication, ComplicationModel>().ReverseMap();
            CreateMap<Company, CompanyModel>().ReverseMap();
            CreateMap<GameCompany, GameCompanyModel>().ReverseMap();
            CreateMap<GameDetails, GameDetailsModel>().ReverseMap();
            CreateMap<PerfectGame, PerfectGameModel>().ReverseMap();
            CreateMap<RecentGame, RecentGameModel>().ReverseMap();
            CreateMap<SteamAchievementSchema, SteamAchievementSchemaModel>().ReverseMap();
            CreateMap<SteamCategory, SteamCategoryModel>().ReverseMap();
            CreateMap<SteamGame, SteamGameModel>().ReverseMap();
            CreateMap<SteamGameCategory, SteamGameCategoryModel>().ReverseMap();
            CreateMap<SteamGameDescriptions, SteamGameDescriptionsModel>().ReverseMap();
            CreateMap<SteamGameUserTag, SteamGameUserTagModel>().ReverseMap();
            CreateMap<SteamUserAchievement, SteamUserAchievementModel>().ReverseMap();
            CreateMap<SteamUserTag, SteamUserTagModel>().ReverseMap();
            CreateMap<Tag, TagModel>().ReverseMap();
        }
    }
}
