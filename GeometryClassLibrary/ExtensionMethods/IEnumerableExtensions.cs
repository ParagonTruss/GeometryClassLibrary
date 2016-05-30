using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsReference<T>(this IEnumerable<T> list, T item)
        {
            return list.Any(itemInList => ReferenceEquals(itemInList, item));
        }
    }
}
