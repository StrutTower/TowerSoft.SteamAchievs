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

        public List<SteamGame> GetWithoutHltbData() {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"INNER JOIN gamedetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE gd.MainStoryTime IS NULL " +
                $"AND gd.MainAndSidesTime IS NULL " +
                $"AND gd.CompletionistTime IS NULL ";
            return GetEntities(query);
        }

        public List<SteamGame> AdvancedSearch(SearchModel model) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"LEFT JOIN gamedetails gd ON {TableName}.ID = gd.SteamGameID " +
                $"WHERE ";

            List<string> statements = new();

            if (!string.IsNullOrWhiteSpace(model.Name)) {
                statements.Add($"Name LIKE @Name");
                query.AddParameter("@Name", $"%{model.Name}%");
            }

            GetIntSearchQuery(query, statements, model.SteamReviewScore, model.SteamReviewScoreComparison, $"{TableName}.ReviewScore", "@SteamReviewScore");
            GetIntSearchQuery(query, statements, model.MetacriticScore, model.MetacriticScoreComparison, $"{TableName}.MetacriticScore", "@MetacriticScore");
            GetIntSearchQuery(query, statements, model.PlayNextScore, model.PlayNextScoreComparison, "gd.PlayNextScore", "@PlayNextScore");

            GetDoubleSearchQuery(query, statements, model.MainTime, model.MainTimeComparison, "gd.MainStoryTime", "@MainTime");
            GetDoubleSearchQuery(query, statements, model.MainAndSidesTime, model.MainAndSidesTimeComparison, "gd.MainAndSidesTime", "@MainAndSidesTime");
            GetDoubleSearchQuery(query, statements, model.CompletionistTime, model.CompletionistTimeComparison, "gd.CompletionistTime", "@CompletionistTime");
            GetDoubleSearchQuery(query, statements, model.AllStylesTime, model.AllStylesTimeComparison, "gd.AllStylesTime", "@AllStylesTime");

            GetNullableBooleanQuery(query, statements, model.PerfectPossible, model.PerfectPossibleComparison, "gd.PerfectPossible", "@PerfectPossible");
            GetNullableBooleanQuery(query, statements, model.Finished, model.FinishedComparison, "gd.Finished", "@Finished");
            GetNullableBooleanQuery(query, statements, model.Delisted, model.DelistedComparison, $"{TableName}.Delisted", "@Delisted");

            switch (model.HasAchievements) {
                case SearchBooleanType.True:
                    statements.Add($"{TableName}.AchievementCount > 0");
                    break;
                case SearchBooleanType.False:
                    statements.Add($"{TableName}.AchievementCount = 0");
                    break;
            }

            query.SqlQuery += string.Join(" AND ", statements);

            return GetEntities(query);
        }

        private void GetIntSearchQuery(QueryBuilder query, List<string> statements, long? value, SearchIntComparisonType comparisonType, string columnName, string placeholderName) {
            if (comparisonType == SearchIntComparisonType.NullValue) {
                statements.Add($"{columnName} IS NULL");
            } else if (value.HasValue) {
                statements.Add($"{columnName} {GetComparisonOperator(comparisonType)} {placeholderName}");
                query.AddParameter(placeholderName, value);
            }
        }
        private void GetDoubleSearchQuery(QueryBuilder query, List<string> statements, double? value, SearchIntComparisonType comparisonType, string columnName, string placeholderName) {
            if (comparisonType == SearchIntComparisonType.NullValue) {
                statements.Add($"{columnName} IS NULL");
            } else if (value.HasValue) {
                statements.Add($"{columnName} {GetComparisonOperator(comparisonType)} {placeholderName}");
                query.AddParameter(placeholderName, value);
            }
        }

        private void GetNullableBooleanQuery(QueryBuilder query, List<string> statements, SearchNullableBooleanType value, SearchComparisonType comparisonType, string columnName, string placeholderName) {

            if (value == SearchNullableBooleanType.Null && comparisonType == SearchComparisonType.Equals) {
                statements.Add($"{columnName} IS NULL");
            } else if (value == SearchNullableBooleanType.Null && comparisonType == SearchComparisonType.NotEquals) {
                statements.Add($"{columnName} IS NOT NULL");
            } else if (value != SearchNullableBooleanType.Skip) {

                if (comparisonType == SearchComparisonType.NotEquals) {
                    statements.Add($"NOT {columnName} <=> {placeholderName}");
                } else {
                    statements.Add($"{columnName} {GetComparisonOperator(comparisonType)} {placeholderName}");
                }
                if (value == SearchNullableBooleanType.True) {
                    query.AddParameter(placeholderName, true);
                } else if (value == SearchNullableBooleanType.False) {
                    query.AddParameter(placeholderName, false);
                }
            }

        }

        private string GetComparisonOperator(SearchComparisonType searchComparisonType) {
            switch (searchComparisonType) {
                case SearchComparisonType.Equals:
                    return "=";
                case SearchComparisonType.NotEquals:
                    return "<>";
                default:
                    return null;
            }
        }

        private string GetComparisonOperator(SearchIntComparisonType searchIntComparisonType) {
            switch (searchIntComparisonType) {
                case SearchIntComparisonType.Equals:
                    return "=";
                case SearchIntComparisonType.NotEquals:
                    return "<>";
                case SearchIntComparisonType.LessThan:
                    return "<";
                case SearchIntComparisonType.LessThanOrEqual:
                    return "<=";
                case SearchIntComparisonType.GreaterThan:
                    return ">";
                case SearchIntComparisonType.GreaterThanOrEqual:
                    return ">=";
                default:
                    return null;
            }
        }

        public List<SteamGame> GetByIDs(IEnumerable<long> ids) {
            if (!ids.SafeAny()) return default;

            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery +=
                $"WHERE ID IN (";

            int counter = 1;
            List<string> inStatements = new();
            foreach(long id in ids) {
                inStatements.Add($"@{counter}");
                query.AddParameter($"@{counter}", id);
            }

            query.SqlQuery += string.Join(",", inStatements) + ") ";

            return GetEntities(query);
        }
    }
}
