using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamTower.Models;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Lib.Utilities {
    internal static class ModelConvert {
        internal static IEnumerable<SteamGame> ToSteamGames(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                yield return new SteamGame() {
                    ID = userAppModel.OwnedApp.SteamAppID,
                    Name = userAppModel.OwnedApp.Name,
                    RequiredAge = userAppModel.SteamApp.RequiredAge,
                    ControllerSupport = userAppModel.SteamApp.ControllerSupport,
                    Delisted = userAppModel.Delisted,
                    HeaderImageUrl = userAppModel.SteamApp.HeaderImage,
                    CapsuleImageUrl = userAppModel.SteamApp.CapsuleImage,
                    CapsuleImageV5Url = userAppModel.SteamApp.CapsuleImageV5,
                    IconUrl = userAppModel.OwnedApp.IconUrl,
                    BackgroundUrl = userAppModel.SteamApp.BackgroundUrl,
                    BackgroundRawUrl = userAppModel.SteamApp.BackgroundRawUrl,
                    GridImageUrl = userAppModel?.GridImage?.Url,
                    GridThumbUrl = userAppModel?.GridImage?.ThumbnailUrl,
                    HeroImageUrl = userAppModel?.HeroImage?.Url,
                    HeroThumbUrl = userAppModel?.HeroImage?.ThumbnailUrl,
                    Website = userAppModel.SteamApp.Website,
                    WindowsSupported = userAppModel.SteamApp?.Platforms?.Windows,
                    MacSupported = userAppModel.SteamApp?.Platforms?.Mac,
                    LinuxSupported = userAppModel.SteamApp?.Platforms?.Linux,
                    MetacriticScore = userAppModel.SteamApp.MetacriticScore,
                    RecommendationCount = userAppModel.SteamApp.RecommendationCount,
                    ReviewScore = userAppModel.ReviewSummary.ReviewScore,
                    ReviewDescription = userAppModel.ReviewSummary.ReivewDescription,
                    AchievementCount = userAppModel.GameStatsSchema?.Achievements?.Count ?? 0,
                    ReleaseDate = userAppModel.SteamApp?.ReleaseDate?.Date,
                    IsComingSoon = userAppModel.SteamApp?.ReleaseDate?.IsComingSoon,
                    DeckCompatibility = userAppModel.DeckCompatibility?.Compatibility,
                    RecentPlaytime = userAppModel.OwnedApp.RecentPlaytime,
                    TotalPlaytime = userAppModel.OwnedApp.TotalPlaytime,
                    WindowsTotalPlaytime = userAppModel.OwnedApp.WindowsTotalPlaytime,
                    MacTotalPlaytime = userAppModel.OwnedApp.MacTotalPlaytime,
                    LinuxTotalPlaytime = userAppModel.OwnedApp.LinuxTotalPlaytime,
                    LastPlayedOn = userAppModel.OwnedApp.LastPlayedOn
                };
            }
        }

        internal static IEnumerable<SteamGameDescriptions> ToSteamGameDescriptions(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                SteamGameDescriptions model = new SteamGameDescriptions {
                    SteamGameID = userAppModel.OwnedApp.SteamAppID,
                    SupportedLanguages = userAppModel.SteamApp?.SupportedLanguages,
                    LegalNotice = userAppModel.SteamApp?.LegalNotice,
                    ExternalUserAccountNotice = userAppModel.SteamApp?.ExternalUserAccountNotice,
                    DetailedDescription = userAppModel.SteamApp?.DetailedDescription,
                    AboutTheGame = userAppModel.SteamApp?.AboutTheGame,
                    ShortDescription = userAppModel.SteamApp?.ShortDescription
                };

                if (userAppModel.SteamApp.DlcIDs.SafeAny()) {
                    model.DlcIDs = string.Join(",", userAppModel.SteamApp.DlcIDs);
                }

                yield return model;
            }
        }

        internal static IEnumerable<SteamCategory> ToSteamCategories(List<UserAppModel> userAppModels) {
            var test = userAppModels.Where(x => x.SteamApp.Categories.SafeAny()).SelectMany(x => x.SteamApp.Categories).Select(x => x.Description).Distinct();

            foreach (string name in test) {
                yield return new SteamCategory {
                    Name = name
                };
            }
        }

        internal static IEnumerable<SteamGameCategory> ToSteamGameCategories(List<UserAppModel> userAppModels, List<SteamCategory> categories) {
            var categoryDictionary = categories.ToDictionary(x => x.Name);
            var categoryGameModels = userAppModels
                .Where(x => x.SteamApp.Categories.SafeAny())
                .SelectMany(x => x.SteamApp.Categories
                    .Select(y => new CategoryGameModel { SteamAppID = x.SteamApp.ID, CategoryName = y.Description }));

            foreach (CategoryGameModel categoryGameModel in categoryGameModels) {
                yield return new SteamGameCategory {
                    SteamCategoryID = categoryDictionary[categoryGameModel.CategoryName].ID,
                    SteamGameID = categoryGameModel.SteamAppID
                };
            }
        }

        internal static IEnumerable<SteamUserTag> ToSteamUserTags(List<UserAppModel> userAppModels) {
            var test = userAppModels.Where(x => x.UserTags.SafeAny()).SelectMany(x => x.UserTags).Distinct();

            foreach (string name in test) {
                yield return new SteamUserTag {
                    Name = name
                };
            }
        }

        internal static IEnumerable<SteamGameUserTag> ToSteamGameUserTags(List<UserAppModel> userAppModels, List<SteamUserTag> steamUserTags) {
            var userTagDictionary = steamUserTags.ToDictionary(x => x.Name);

            List<CategoryGameModel> categoryGameModels = new();
            foreach (UserAppModel userAppModel in userAppModels) {
                if (!userAppModel.UserTags.SafeAny()) continue;

                foreach (string userTag in userAppModel.UserTags) {
                    categoryGameModels.Add(new CategoryGameModel {
                        SteamAppID = userAppModel.OwnedApp.SteamAppID,
                        CategoryName = userTag
                    });
                }
            }

            foreach (CategoryGameModel categoryGameModel in categoryGameModels) {
                yield return new SteamGameUserTag {
                    SteamUserTagID = userTagDictionary[categoryGameModel.CategoryName].ID,
                    SteamGameID = categoryGameModel.SteamAppID
                };
            }
        }

        internal static IEnumerable<SteamAchievementSchema> ToSteamAchievementSchemas(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                if (userAppModel.GameStatsSchema == null || !userAppModel.GameStatsSchema.Achievements.SafeAny()) continue;

                foreach (AchievementSchema schema in userAppModel.GameStatsSchema.Achievements) {
                    GlobalAchievementStat globalAchievementStat = userAppModel.GlobalAchievementStats.SingleOrDefault(x => x.Key == schema.Key);
                    yield return new SteamAchievementSchema {
                        KeyName = schema.Key,
                        SteamGameID = userAppModel.OwnedApp.SteamAppID,
                        Name = schema.Name,
                        DefaultValue = schema.DefaultValue,
                        IsHidden = schema.Hidden,
                        Description = schema.Description,
                        IconUrl = schema.IconUrl,
                        IconGrayUrl = schema.IconGrayUrl,
                        GlobalCompletionPercentage = globalAchievementStat.Percentage
                    };
                }
            }
        }

        internal static IEnumerable<SteamUserAchievement> ToSteamUserAchievements(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                if (!userAppModel.UserAchievements.SafeAny()) continue;
                foreach (UserAchievement userAchievement in userAppModel.UserAchievements) {
                    yield return new SteamUserAchievement {
                        SteamGameID = userAppModel.OwnedApp.SteamAppID,
                        KeyName = userAchievement.AchievementKey,
                        Achieved = userAchievement.Achieved,
                        AchievedOn = userAchievement.AchievedOn,
                    };
                }
            }
        }

        internal static IEnumerable<GameCompany> ToGameDevelopers(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                if (!userAppModel.Developers.SafeAny()) continue;
                foreach (long devID in userAppModel.Developers) {
                    yield return new GameCompany {
                        SteamGameID = userAppModel.SteamApp.ID,
                        CompanyID = devID,
                        IsDeveloper = true
                    };
                }
            }
        }

        internal static IEnumerable<GameCompany> ToGamePublishers(List<UserAppModel> userAppModels) {
            foreach (UserAppModel userAppModel in userAppModels) {
                if (!userAppModel.Publishers.SafeAny()) continue;
                foreach (long pubID in userAppModel.Publishers) {
                    yield return new GameCompany {
                        SteamGameID = userAppModel.SteamApp.ID,
                        CompanyID = pubID,
                        IsDeveloper = false
                    };
                }
            }
        }
    }
}