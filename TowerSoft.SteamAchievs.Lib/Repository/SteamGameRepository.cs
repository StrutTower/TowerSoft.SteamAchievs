using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerSoft.Repository;
using TowerSoft.SteamAchievs.Lib.Domain;

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
                default:
                    throw new Exception("Unsupported GameListType");
            }
            return GetEntities(query);
        }
    }
}
