using Microsoft.Extensions.Logging;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class HiddenAchievementSync {
        private readonly UnitOfWork uow;
        private readonly AchievementStatsService achievementStatsService;
        private readonly ILogger logger;

        public HiddenAchievementSync(UnitOfWork uow, AchievementStatsService achievementStatsService, ILogger<HiddenAchievementSync> logger) {
            this.uow = uow;
            this.achievementStatsService = achievementStatsService;
            this.logger = logger;
        }

        public void StartJob() {
            logger.LogInformation($"Starting {nameof(HiddenAchievementSync)} Job");
            DateTime startTime = DateTime.Now;
            try {
                Run();
            } catch (Exception ex) {
                logger.LogWarning($"Exception occurred during {nameof(HiddenAchievementSync)} job." + Environment.NewLine + ex);
            }
            logger.LogInformation($"Finished {nameof(HiddenAchievementSync)} Job. Total Runtime: {(int)Math.Floor(DateTime.Now.Subtract(startTime).TotalSeconds)}");
        }

        private void Run() {
            List<SteamGame> games = uow.GetRepo<SteamGameRepository>().GetWithAchievementsNeedingDescription();
            logger.LogInformation($"Running HiddenAchievementSync on {games.Count} games.");

            SteamAchievementSchemaRepository schemaRepo = uow.GetRepo<SteamAchievementSchemaRepository>();
            AchievementDetailsRepository detailsRepo = uow.GetRepo<AchievementDetailsRepository>();
            SteamGameDescriptionsRepository descriptionRepo = uow.GetRepo<SteamGameDescriptionsRepository>();

            foreach (SteamGame game in games) {
                int updateCount = 0;
                List<SteamAchievementSchema> schemas = schemaRepo.GetBySteamGameID(game.ID);
                List<AchievementDetails> details = detailsRepo.GetBySteamGameID(game.ID);
                List<AchievementStatsAchievement> statsAchievements = achievementStatsService.GetAchievements(game.ID).Result;

                if (schemas.Count > statsAchievements.Count) {
                    // Find DLC achievements
                    SteamGameDescriptions descriptions = descriptionRepo.GetBySteamGameID(game.ID);
                    if (string.IsNullOrWhiteSpace(descriptions.DlcIDs)) continue;

                    string[] parts = descriptions.DlcIDs.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts) {
                        if (long.TryParse(part, out var value)) {
                            List<AchievementStatsAchievement> dlcAchievements = achievementStatsService.GetAchievements(value).Result;
                            if (dlcAchievements.SafeAny())
                                statsAchievements.AddRange(dlcAchievements);
                        }
                    }
                }

                foreach (SteamAchievementSchema schema in schemas) {
                    if (!string.IsNullOrWhiteSpace(schema.Description)) continue;

                    AchievementDetails detail = details.SingleOrDefault(x => x.AchievementKey == schema.KeyName);
                    bool isNewDetails = false;
                    if (detail == null) {
                        isNewDetails = true;
                        detail = new AchievementDetails {
                            AchievementKey = schema.KeyName,
                            SteamGameID = game.ID,
                        };
                    }


                    AchievementStatsAchievement achievementStatsAchievement = statsAchievements
                        .SingleOrDefault(x => x.AchievementKey.Equals(schema.KeyName, StringComparison.OrdinalIgnoreCase));
                    if (achievementStatsAchievement == null) continue;

                    detail.Description = achievementStatsAchievement.Description;

                    if (isNewDetails)
                        detailsRepo.Add(detail);
                    else
                        detailsRepo.Update(detail);
                    updateCount++;
                }

                logger.LogInformation($"{game.NameClean}: {updateCount} achievement descriptions updated");
            }
        }
    }
}
