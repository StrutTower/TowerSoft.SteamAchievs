using System.Diagnostics.CodeAnalysis;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Comparers {
    public class SteamCategoryNameComparer : IEqualityComparer<SteamCategory> {
        public bool Equals(SteamCategory? x, SteamCategory? y) {
            return x != null && y != null && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] SteamCategory obj) {
            return obj.Name.GetHashCode();
        }
    }
}
