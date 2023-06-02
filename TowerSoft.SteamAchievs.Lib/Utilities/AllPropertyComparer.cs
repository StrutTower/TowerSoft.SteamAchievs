using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Lib.Utilities {
    public class AllPropertyComparer<T> : IEqualityComparer<T> {
        private readonly PropertyReflectionHelper propertyReflectionHelper;
        readonly IList<string> keyProperties = new List<string>();

        public AllPropertyComparer(bool useCompareFields = false) {
            propertyReflectionHelper = new PropertyReflectionHelper();
            keyProperties = propertyReflectionHelper.GetPropertyNames<T>(useCompareFields);
        }

        public bool Equals(T x, T y) {
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            foreach (string propertyName in keyProperties) {
                object xValue = propertyReflectionHelper.GetPropertyValue(propertyName, x);
                object yValue = propertyReflectionHelper.GetPropertyValue(propertyName, y);

                if (xValue == null && yValue == null)
                    return true;
                else if (xValue == null && yValue != null)
                    return false;
                else if (xValue != null && yValue == null)
                    return false;
                else if (!xValue.Equals(yValue))
                    return false;
            }
            return true;
        }

        public int GetHashCode(T obj) {
            int hash = 17;
            foreach (string propertyName in keyProperties) {
                var value = propertyReflectionHelper.GetPropertyValue(propertyName, obj);
                if (value == null) {
                    hash = hash * 23 + "".GetHashCode();
                } else {
                    hash = hash * 23 + value.GetHashCode();
                }
            }
            return hash;
        }
    }
}
