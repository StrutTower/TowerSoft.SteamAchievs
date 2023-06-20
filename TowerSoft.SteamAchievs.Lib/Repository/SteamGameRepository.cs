using Org.BouncyCastle.Math.EC.Endo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;
using TowerSoft.SteamAchievs.Lib.Models;
using TowerSoft.Utilities;

namespace TowerSoft.SteamAchievs.Lib.Repository {
    public class SteamGameRepository : DbRepository<SteamGame> {
        public SteamGameRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

        public SteamGame GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public List<SteamGame> GetByGameListType(GameListType gameListType) {
            QueryBuilder query = GetQueryBuilder();

            switch (gameListType) {
                case GameListType.All:
                    break;
                case GameListType.Perfect:
                    query.SqlQuery += $"" +
                        $"INNER JOIN SteamUserAchievement a ON {TableName}.ID = a.SteamGameID " +
                        $"GROUP BY {TableName}.ID " +
                        $"HAVING group_concat(DISTINCT a.Achieved) = 1";
                    break;
                case GameListType.NonPerfect:
                    query.SqlQuery += $"" +
                        $"INNER JOIN SteamUserAchievement a ON {TableName}.ID = a.SteamGameID " +
                        $"GROUP BY {TableName}.ID " +
                        $"HAVING group_concat(DISTINCT a.Achieved) <> 1";
                    break;
                case GameListType.NoComplications:
                    query.SqlQuery +=
                        $"LEFT JOIN gamecomplication gc ON {TableName}.ID = gc.SteamGameID " +
                        $"WHERE gc.SteamGameID IS NULL ";
                    break;
                case GameListType.Incomplete:
                    query.SqlQuery +=
                        $"INNER JOIN SteamUserAchievement a ON {TableName}.ID = a.SteamGameID " +
                        $"GROUP BY {TableName}.ID " +
                        $"HAVING group_concat(DISTINCT a.Achieved) = '0,1' ";
                    break;
                case GameListType.PerfectPossible:
                    query.SqlQuery +=
                        $"INNER JOIN GameDetails gd ON {TableName}.ID = gd.SteamGameID " +
                        $"WHERE gd.PerfectPossible = 1 ";
                    break;
                case GameListType.HasPlayNextScore:
                    query.SqlQuery +=
                        $"INNER JOIN GameDetails gd ON {TableName}.ID = gd.SteamGameID " +
                        $"WHERE gd.PlayNextScore > 0 ";
                    break;
                default:
                    throw new Exception("Unsupported GameListType");
            }
            return GetEntities(query);
        }

        public List<SteamGame> Search(string q) {
            return GetEntities(Where(x => x.Name, Comparison.LikeBothSidesWildcard, q));
        }

        public List<SteamGame> GetWithAchievementsNeedingDescription() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"INNER JOIN SteamAchievementSchema sas ON {TableName}.ID = sas.SteamGameID " +
                $"LEFT JOIN AchievementDetails ad ON {TableName}.ID = ad.SteamGameID " +
                $"WHERE sas.Description IS NULL " +
                $"AND ad.Description IS NULL " +
                $"GROUP BY {TableName}.ID ";
            return GetEntities(query);
        }

        public List<SteamGame> GetWithNullPerfectPossible() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"LEFT JOIN GameDetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE PerfectPossible IS NULL ";
            return GetEntities(query);
        }

        public List<SteamGame> GetWithNullPlayPriority() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"LEFT JOIN GameDetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE PlayPriority IS NULL ";
            return GetEntities(query);
        }

        public List<SteamGame> GetWithNullFinished() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"LEFT JOIN GameDetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE Finished IS NULL ";
            return GetEntities(query);
        }

        public List<SteamGame> GetWithoutAchievements() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"" +
                $"WHERE AchievementCount = 0";
            return GetEntities(query);
        }

        public List<SteamGame> AdvancedSearch(SearchModel model) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"INNER JOIN gamedetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE ";

            List<string> statements = new();

            if (!string.IsNullOrWhiteSpace(model.Name)) {
                statements.Add($"Name LIKE @Name");
                query.AddParameter("@Name", $"%{model.Name}%");
            }

            if (model.PlayNextScore.HasValue) {
                statements.Add($"gd.PlayNextScore {EnumUtilities.GetEnumDisplayName(model.PlayNextScoreComparison)} @PlayNextScore");
                query.AddParameter("@PlayNextScore", model.PlayNextScore);
            }

            if (model.MainTime.HasValue) {
                statements.Add($"gd.MainStoryTime {EnumUtilities.GetEnumDisplayName(model.MainTimeComparison)} @MainTime");
                query.AddParameter("@MainTime", model.MainTime);
            }

            if (model.MainAndSidesTime.HasValue) {
                statements.Add($"gd.MainAndSidesTime {EnumUtilities.GetEnumDisplayName(model.MainAndSidesTimeComparison)} @MainAndSidesTime");
                query.AddParameter("@MainAndSidesTime", model.MainAndSidesTime);
            }

            if (model.CompletionistTime.HasValue) {
                statements.Add($"gd.CompletionistTime {EnumUtilities.GetEnumDisplayName(model.CompletionistTimeComparison)} @CompletionistTime");
                query.AddParameter("@CompletionistTime", model.CompletionistTime);
            }

            if (model.AllStylesTime.HasValue) {
                statements.Add($"gd.AllStylesTime {EnumUtilities.GetEnumDisplayName(model.AllStylesTimeComparison)} @AllStylesTime");
                query.AddParameter("@AllStylesTime", model.AllStylesTime);
            }

            query.SqlQuery += string.Join(" AND ", statements);

            return GetEntities(query);
        }
    }
}
