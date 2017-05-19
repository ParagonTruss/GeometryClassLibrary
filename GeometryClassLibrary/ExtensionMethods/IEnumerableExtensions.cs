/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary;
using MoreLinq;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsReference<T>(this IEnumerable<T> list, T item)
        {
            return list.Any(itemInList => ReferenceEquals(itemInList, item));
        }

        public static bool RemoveByReference<T>(this IList<T> list, T item)
        {
            var index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (ReferenceEquals(list[i], item))
                {
                    index = i;
                }
            }
            if (index == -1)
            {
                return false;
            }
            list.RemoveAt(index);
            return true;
        }
        
        public static string Join<T>(this IEnumerable<T> list, string separator)
        {
            return string.Join(separator, list);
        }
        
        #region IGrouping Implementation
        public class Group<TKey,TElement> : IGrouping<TKey,TElement>
            where TKey : IEquatable<TKey>
        {
            public IEnumerator<TElement> GetEnumerator() => Elements.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();

            public TKey Key { get; }
            public List<TElement> Elements { get; }

            public Group(TKey key, List<TElement> elements)
            {
                Key = key;
                Elements = elements;
            }
        }
        #endregion

        /// <summary>
        /// A safe version of Linq's GroupBy that relies on IEquatable instead of GetHashCode.
        /// </summary>
        public static List<Group<TKey, TSource>> GroupByEquatable<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
        {
            var groups = new List<Group<TKey,TSource>>();
            foreach (var item in source)
            {
                var key = selector(item);
                var itemHasBeenGrouped = false;
                foreach (var group in groups)
                {
                    if (key.Equals(group.Key))
                    {
                        group.Elements.Add(item);
                        itemHasBeenGrouped = true;
                        break;
                    }
                }
                if (!itemHasBeenGrouped)
                {
                    groups.Add(new Group<TKey,TSource>(key, new List<TSource>{item}));
                }
            }
            return groups;
        }

        /// <summary>
        /// A safe version of Linq's Distinct that relies on IEquatable instead of GetHashCode.
        /// </summary>
        public static IEnumerable<TSource> DistinctEquatable<TSource>(this IEnumerable<TSource> source)
            where TSource : IEquatable<TSource>
        {
            return source.DistinctByEquatable(_.Identity);
        }

        /// <summary>
        /// A safe version of MoreLinq's DistinctBy that relies on IEquatable instead of GetHashCode.
        /// </summary>
        public static IEnumerable<TSource> DistinctByEquatable<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
            where TKey : IEquatable<TKey>
        {
            var list = new List<TKey>();
            foreach (var item in source)
            {
                var key = selector(item);
                if (!list.Contains(key))
                {
                    list.Add(key);
                    yield return item;
                }
            }
        }

        /// <summary>
        /// A safe alternative to Max when working with Units. Returns null for empty enumerables.
        /// </summary>
        public static TUnit MaxUnitOrNull<TUnit>(this IEnumerable<TUnit> source)
            where TUnit : Unit
        {
            return source.MaxByUnitOrDefault(_.Identity);
        }

        /// <summary>
        /// A safe alternative to Min when working with Units. Returns null for empty enumerables.
        /// </summary>
        public static TUnit MinUnitOrNull<TUnit>(this IEnumerable<TUnit> source)
            where TUnit : Unit
        {
            return source.MinByUnitOrDefault(_.Identity);
        }

        /// <summary>
        /// A safe alternative to MaxBy when working with Units. Returns the default value for empty enumerables.
        /// </summary>
        public static TSource MaxByUnitOrDefault<TSource,TUnit>(this IEnumerable<TSource> source, Func<TSource,TUnit> selector)
            where TUnit : Unit
        {
            return source.MaxByOrDefault(x => selector(x).ValueIn(Distance.Inches));
        }

        /// <summary>
        /// A safe alternative to MinBy when working with Units. Returns the default value for empty enumerables.
        /// </summary>
        public static TSource MinByUnitOrDefault<TSource,TUnit>(this IEnumerable<TSource> source, Func<TSource,TUnit> selector)
            where TUnit : Unit
        {
            return source.MinByOrDefault(x => selector(x).ValueIn(Distance.Inches));
        }
        /// <summary>
        /// A safe alternative to MaxBy. Returns the default value for empty enumerables.
        /// </summary>
        public static TSource MaxByOrDefault<TSource,TKey>(this IEnumerable<TSource> source, Func<TSource,TKey> selector)
            where TKey : IComparable<TKey>
        {
            return source.Any() ? source.MaxBy(selector) : default(TSource);
        }

        /// <summary>
        /// A safe alternative to MinBy. Returns the default value for empty enumerables.
        /// </summary>
        public static TSource MinByOrDefault<TSource,TKey>(this IEnumerable<TSource> source, Func<TSource,TKey> selector)
            where TKey : IComparable<TKey>
        {
            return source.Any() ? source.MinBy(selector) : default(TSource);
        }

        /// <summary>
        /// Orders by Units (ascending), ignoring the error margins.
        /// </summary>
        public static IEnumerable<TSource> OrderByUnit<TSource,TUnit>(this IEnumerable<TSource> source, Func<TSource,TUnit> selector)
            where TUnit : Unit
        {
            return source.OrderBy(x => selector(x).ValueIn(Distance.Inches));
        }

        /// <summary>
        /// Orders by Units (descending), ignoring the error margins.
        /// </summary>
        public static IEnumerable<TSource> OrderByUnitDescending<TSource,TUnit>(this IEnumerable<TSource> source, Func<TSource,TUnit> selector)
            where TUnit : Unit
        {
            return source.OrderByDescending(x => selector(x).ValueIn(Distance.Inches));
        }
    }

    public static class _
    {
        /// <summary>
        /// For easy null checking
        /// </summary>
        public static bool IsNull(this object obj) => obj == null;

        /// <summary>
        /// For easy null checking
        /// </summary>
        public static bool NotNull(this object obj) => obj != null;

        /// <summary>
        /// Returns the opposite boolean value.
        /// Wraps the prefix operator "!". 
        /// </summary>
        public static bool Not(this bool boolean) => !boolean;

        /// <summary>
        /// A function that returns its input.
        /// </summary>
        public static T Identity<T>(this T input) => input;

        /// <summary>
        /// Create a new function by passing the ouput of the first function as input to the second.
        /// </summary>
        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f1, Func<B, C> f2) => a => f2(f1(a));
    }
}
