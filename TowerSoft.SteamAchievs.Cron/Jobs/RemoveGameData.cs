using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Cron.Jobs {
    public class RemoveGameData {
        private readonly UnitOfWork uow;
        private readonly AppSettings appSettings;

        public RemoveGameData(UnitOfWork uow, IOptionsSnapshot<AppSettings> appSettings) {
            this.uow = uow;
            this.appSettings = appSettings.Value;
        }

        public void Run() {
#if DEBUG
            foreach (long appID in appSettings.IgnoredAppIDs) {
                RemoveApp(appID);
            }
#else
            throw new System.NotImplementedException();
#endif
        }

        private void RemoveApp(long id) {
            SteamGameRepository steamGameRepo = uow.GetRepo<SteamGameRepository>();
            SteamAchievementSchemaRepository steamAchievementSchemaRepo = uow.GetRepo<SteamAchievementSchemaRepository>();
            AchievementDetailsRepository achievementDetailsRepo = uow.GetRepo<AchievementDetailsRepository>();
            AchievementTagRepository achievementTagRepo = uow.GetRepo<AchievementTagRepository>();
            GameComplicationRepository gameComplicationRepo = uow.GetRepo<GameComplicationRepository>();
            GameDetailsRepository gameDetailsRepo = uow.GetRepo<GameDetailsRepository>();
            PerfectGameRepository perfectGameRepo = uow.GetRepo<PerfectGameRepository>();
            RecentGameRepository recentGameRepo = uow.GetRepo<RecentGameRepository>();
            SteamGameCategoryRepository steamGameCategoryRepository = uow.GetRepo<SteamGameCategoryRepository>();
            SteamGameDescriptionsRepository steamGameDescriptionsRepo = uow.GetRepo<SteamGameDescriptionsRepository>();
            SteamGameUserTagRepository steamGameUserTagRepo = uow.GetRepo<SteamGameUserTagRepository>();
            SteamUserAchievementRepository steamUserAchievementRepo = uow.GetRepo<SteamUserAchievementRepository>();
            TagRepository tagRepo = uow.GetRepo<TagRepository>();

            SteamGame steamGame = steamGameRepo.GetByID(id);
            if (steamGame != null) {
                GameDetails gameDetails = gameDetailsRepo.GetBySteamGameID(steamGame.ID);
                if (gameDetails != null)
                    gameDetailsRepo.Remove(gameDetails);

                PerfectGame perfectGame = perfectGameRepo.GetBySteamGameID(steamGame.ID);
                if (perfectGame != null)
                    perfectGameRepo.Remove(perfectGame);

                RecentGame recentGame = recentGameRepo.GetBySteamGameID(steamGame.ID);
                if (recentGame != null)
                    recentGameRepo.Remove(recentGame);

                SteamGameDescriptions steamGameDescriptions = steamGameDescriptionsRepo.GetBySteamGameID(steamGame.ID);
                if (steamGameDescriptions != null)
                    steamGameDescriptionsRepo.Remove(steamGameDescriptions);


                List<SteamUserAchievement> steamUserAchievements = steamUserAchievementRepo.GetBySteamGameID(steamGame.ID);
                if (steamUserAchievements.SafeAny())
                    steamUserAchievements.ForEach(x => steamUserAchievementRepo.Remove(x));

                List<AchievementDetails> achievementDetails = achievementDetailsRepo.GetBySteamGameID(steamGame.ID);
                if (achievementDetails.SafeAny())
                    achievementDetails.ForEach(x => achievementDetailsRepo.Remove(x));

                List<AchievementTag> achievementTags = achievementTagRepo.GetBySteamGameID(steamGame.ID);
                if (achievementTags.SafeAny())
                    achievementTags.ForEach(x => achievementTagRepo.Remove(x));

                List<SteamAchievementSchema> achievementSchemas = steamAchievementSchemaRepo.GetBySteamGameID(steamGame.ID);
                if (achievementSchemas.SafeAny())
                    achievementSchemas.ForEach(x => steamAchievementSchemaRepo.Remove(x));

                List<GameComplication> gameComplications = gameComplicationRepo.GetBySteamGameID(steamGame.ID);
                if (gameComplications.SafeAny())
                    gameComplications.ForEach(x => gameComplicationRepo.Remove(x));

                List<SteamGameCategory> steamGameCategories = steamGameCategoryRepository.GetBySteamGameID(steamGame.ID);
                if (steamGameCategories.SafeAny())
                    steamGameCategories.ForEach(x => steamGameCategoryRepository.Remove(x));

                List<SteamGameUserTag> steamGameUserTags = steamGameUserTagRepo.GetBySteamGameID(steamGame.ID);
                if (steamGameUserTags.SafeAny())
                    steamGameUserTags.ForEach(x => steamGameUserTagRepo.Remove(x));

                List<Tag> tags = tagRepo.GetBySteamGameID(steamGame.ID);
                if (tags.SafeAny())
                    tags.ForEach(x => tagRepo.Remove(x));

                steamGameRepo.Remove(steamGame);
            }
        }
    }
}
