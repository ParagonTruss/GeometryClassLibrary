/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

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
    }

  
    public static class _
    {
        /// <summary>
        /// For easy null checking
        /// </summary>
        public static bool IsNull(this object obj) => obj == null;
        public static bool NotNull(this object obj) => obj != null;
        
        /// <summary>
        /// A function that returns its input.
        /// </summary>
        public static T Identity<T>(this T input) => input;
    }
}
