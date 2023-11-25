using Microsoft.AspNetCore.Mvc.Rendering;
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

            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetAll().ToDictionary(x => x.ID);

            foreach (SteamAchievementSchema schema in schemas) {
                AchievementModel model = new() {
                    Key = schema.KeyName,
                    SteamGameID = schema.SteamGameID,
                    Name = schema.Name,
                    IsHidden = schema.IsHidden,
                    Description = schema.Description,
                    IconUrl = schema.IconUrl,
                    IconGrayUrl = schema.IconGrayUrl,
                    GlobalCompletionPercentage = schema.GlobalCompletionPercentage,
                    RemovedFromSteam = schema.RemovedFromSteam,
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
                    foreach (AchievementTag tag in currentTags) {
                        model.Tags.Add(tags[tag.TagID]);
                    }
                }

                yield return model;
            }
        }

        public AchievementModel GetAchievementModel(long steamGameID, string achievementKey) {
            SteamAchievementSchema schema = uow.GetRepo<SteamAchievementSchemaRepository>().Get(steamGameID, achievementKey);
            AchievementDetails details = uow.GetRepo<AchievementDetailsRepository>().Get(steamGameID, achievementKey);
            SteamUserAchievement userAchievement = uow.GetRepo<SteamUserAchievementRepository>().Get(steamGameID, achievementKey);
            var achievementTags = uow.GetRepo<AchievementTagRepository>().GetByKeyAndSteamGameID(achievementKey, steamGameID);
            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetAll().ToDictionary(x => x.ID);

            AchievementModel model = new AchievementModel {
                Key = schema.KeyName,
                SteamGameID = schema.SteamGameID,
                Name = schema.Name,
                IsHidden = schema.IsHidden,
                Description = schema.Description,
                IconUrl = schema.IconUrl,
                IconGrayUrl = schema.IconGrayUrl,
                GlobalCompletionPercentage = schema.GlobalCompletionPercentage,
                RemovedFromSteam = schema.RemovedFromSteam,
                Tags = new()
            };

            if (details != null && !string.IsNullOrWhiteSpace(details.Description)) {
                model.Description = details.Description;
            }

            if (userAchievement != null) {
                model.Achieved = userAchievement.Achieved;
                model.AchievedOn = userAchievement.AchievedOn;
            }

            if (tags.SafeAny()) {
                foreach (AchievementTag tag in achievementTags) {
                    model.Tags.Add(tags[tag.TagID]);
                }
            }

            return model;
        }

        public EditAchievementModel GetEditAchievementModel(long steamGameID, string achievementKey) {
            EditAchievementModel model = new() {
                SteamGameID = steamGameID,
                AchievementKey = achievementKey,
                SteamGame = uow.GetRepo<SteamGameRepository>().GetByID(steamGameID),
                AchievementSchema = uow.GetRepo<SteamAchievementSchemaRepository>().Get(steamGameID, achievementKey)
            };

            AchievementDetails details = uow.GetRepo<AchievementDetailsRepository>().Get(steamGameID, achievementKey);
            if (details != null) {
                model.DescriptionOverride = details.Description;
                model.TagIDs = new();
            }

            List<AchievementTag> achievementTags = uow.GetRepo<AchievementTagRepository>().GetByKeyAndSteamGameID(achievementKey, steamGameID);
            Dictionary<long, Tag> tags = uow.GetRepo<TagRepository>().GetActiveForSteamGameID(steamGameID).ToDictionary(x => x.ID);

            model.TagList = new SelectList(tags.Values.Where(x => x.IsActive).OrderBy(x => x.Name), "ID", "Name");

            if (achievementTags.SafeAny()) {
                foreach (AchievementTag achievementTag in achievementTags) {
                    model.TagIDs.Add(tags[achievementTag.TagID].ID);
                }
            }

            return model;
        }

        internal void UpdateAchievementDetails(EditAchievementModel model) {
            AchievementDetailsRepository repo = uow.GetRepo<AchievementDetailsRepository>();
            AchievementDetails existing = repo.Get(model.SteamGameID, model.AchievementKey);

            if (existing == null) {
                AchievementDetails details = new() {
                    SteamGameID = model.SteamGameID,
                    AchievementKey = model.AchievementKey,
                    Description = string.IsNullOrWhiteSpace(model.DescriptionOverride) ? null : model.DescriptionOverride
                };
                repo.Add(details);
            } else {
                if (existing.Description != model.DescriptionOverride) {
                    existing.Description = model.DescriptionOverride;
                    repo.Update(existing);
                }
            }

            AchievementTagRepository tagRepo = uow.GetRepo<AchievementTagRepository>();
            List<AchievementTag> existingTags = tagRepo.GetByKeyAndSteamGameID(model.AchievementKey, model.SteamGameID);

            List<long> existingIDs = existingTags.Select(x => x.TagID).ToList();
            if (model.TagIDs == null)
                model.TagIDs = new();

            foreach (long removeID in existingIDs.Except(model.TagIDs)) {
                tagRepo.Remove(new AchievementTag {
                    SteamGameID = model.SteamGameID,
                    AchievementKey = model.AchievementKey,
                    TagID = removeID
                });
            }

            foreach (long addID in model.TagIDs.Except(existingIDs)) {
                tagRepo.Add(new AchievementTag {
                    SteamGameID = model.SteamGameID,
                    AchievementKey = model.AchievementKey,
                    TagID = addID
                });
            }
        }
    }
}
