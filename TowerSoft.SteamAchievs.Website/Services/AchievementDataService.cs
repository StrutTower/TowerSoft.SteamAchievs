using System.Collections.Generic;
using System.Linq;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.ViewModels;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Website.Services {
    public class AchievementDataService {
        private readonly UnitOfWork uow;

        public AchievementDataService(UnitOfWork uow) {
            this.uow = uow;
        }

        public IEnumerable<AchievementModel> GetAchievementModels(long steamGameID) {
            var schemas = uow.GetRepo<SteamAchievementSchemaRepository>().GetBySteamGameID(steamGameID);
            var userAchievements = uow.GetRepo<SteamUserAchievementRepository>().GetBySteamGameID(steamGameID).ToDictionary(x => x.KeyName);
            var details = uow.GetRepo<AchievementDetailsRepository>().GetBySteamGameID(steamGameID).ToDictionary(x => x.AchievementKey);
            var achievementTags = uow.GetRepo<AchievementTagRepository>().GetBySteamGameID(steamGameID);

            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetBySteamGameID(steamGameID).ToDictionary(x => x.ID);

            foreach(SteamAchievementSchema schema in schemas) {
                AchievementModel model = new AchievementModel {
                    Key = schema.KeyName,
                    SteamGameID = schema.SteamGameID,
                    Name = schema.Name,
                    IsHidden = schema.IsHidden,
                    Description = schema.Description,
                    IconUrl = schema.IconUrl,
                    IconGrayUrl = schema.IconGrayUrl,
                    GlobalCompletionPercentage = schema.GlobalCompletionPercentage,
                    //Achieved =
                    //AchievedOn =
                    Tags = new()
                };

                if (details.ContainsKey(schema.KeyName) && !string.IsNullOrWhiteSpace(details[schema.KeyName].Description)) {
                    model.Description = details[schema.KeyName].Description;
                }

                if (userAchievements.ContainsKey(schema.KeyName)) {
                    SteamUserAchievement userAchievement = userAchievements[schema.KeyName];
                    model.Achieved = userAchievement.Achieved;
                    model.AchievedOn = userAchievement.AchievedOn;
                }

                List<AchievementTag> currentTags = achievementTags.Where(x => x.AchievementKey == schema.KeyName).ToList();
                if (tags.SafeAny()) {
                    foreach(AchievementTag tag in currentTags) {
                        model.Tags.Add(tags[tag.TagID]);
                    }
                }

                yield return model;
            }
        }
    }
}
