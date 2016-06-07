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

using System.Collections.Generic;
using System.Linq;

namespace GeometryClassLibrary
{
    public static class PlaneRegionListExtensions
    {
        /// <summary>
        /// Rotates the list of plane regions with the given rotation
        /// </summary>
        public static List<PlaneRegion> Rotate(this IList<PlaneRegion> planeRegions, Rotation rotation)
        {
            return planeRegions.Select(p => p.Rotate(rotation)).ToList();
        }

        /// <summary>
        /// Translate the List of Plane Regions with the given translation
        /// </summary>
        //public static List<PlaneRegion> Translate(this IList<PlaneRegion> planeRegions, Point translation)
        //{
        //    return planeRegions.Select(p => p.Translate(translation)).ToList();
        //}

        /// <summary>
        /// Shifts the list of plane regions with the given Shift
        /// </summary>
        public static List<PlaneRegion> Shift(this List<PlaneRegion> planeRegions, Shift shift)
        {
            return planeRegions.Select(p => p.Shift(shift)).ToList();
        }
    }
}
