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
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using MoreLinq;

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

        #region IGrouping Implementation
        public class Group<TKey,TElement> : IGrouping<TKey,TElement>
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

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source,
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
                    if (group.Key.Equals(key))
                    {
                        group.Elements.Add(item);
                        itemHasBeenGrouped = true;
                        break;
                    }
                }
                if (!itemHasBeenGrouped)
                {
                    groups.Add(new Group<TKey,TSource>(key, new List<TSource>{}));
                }
            }
            return groups;
        }

        public static TSource MaxBy<TSource, TUnit, TUnitType>(this IEnumerable<TSource> enumerable, Func<TSource,TUnit> selector)
            where TUnitType : IUnitType
            where TUnit : Unit<TUnitType>
        {
            return enumerable.MaxBy(x => selector(x).ValueIn(new Inch()));
        }

        public static TSource MinBy<TSource, TUnit, TUnitType>(this IEnumerable<TSource> enumerable, Func<TSource,TUnit> selector)
            where TUnitType : IUnitType
            where TUnit : Unit<TUnitType>
        {
            return enumerable.MinBy(x => selector(x).ValueIn(new Inch()));
        }

        public static IEnumerable<TSource> OrderBy<TSource, TUnit, TUnitType>(this IEnumerable<TSource> enumerable, Func<TSource,TUnit> selector)
            where TUnitType : IUnitType
            where TUnit : Unit<TUnitType>
        {
            return enumerable.OrderBy(x => selector(x).ValueIn(new Inch()));
        }

        public static IEnumerable<TSource> OrderByDescending<TSource, TUnit, TUnitType>(this IEnumerable<TSource> enumerable, Func<TSource,TUnit> selector)
            where TUnitType : IUnitType
            where TUnit : Unit<TUnitType>
        {
            return enumerable.OrderByDescending(x => selector(x).ValueIn(new Inch()));
        }

    }

    public static class _
    {
        /// <summary>
        /// For easy null checking
        /// </summary>
        public static bool IsNull<T>(this T obj)
            where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// For easy null checking
        /// </summary>
        public static bool NotNull<T>(this T obj)
           where T : class
        {
            return obj != null;
        }

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
