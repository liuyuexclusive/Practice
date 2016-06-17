using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Common
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> arr)
        {
            return arr == null || arr.Count() == 0;
        }
    }
}
