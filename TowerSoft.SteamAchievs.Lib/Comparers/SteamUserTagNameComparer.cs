using System.Diagnostics.CodeAnalysis;
using TowerSoft.SteamAchievs.Lib.Domain;

namespace TowerSoft.SteamAchievs.Lib.Comparers {
    public class SteamUserTagNameComparer : IEqualityComparer<SteamUserTag> {
        public bool Equals(SteamUserTag? x, SteamUserTag? y) {
            return x != null && y != null && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] SteamUserTag obj) {
            return obj.Name.GetHashCode();
        }
    }
}