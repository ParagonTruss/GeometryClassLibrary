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

namespace GeometryClassLibrary
{
    public static class IShiftableCollectionExtensions
    {
        /// <summary>
        /// Shifts this object using the given shift
        /// Note: This shifts both the Geometry and the Coordinate System of this Shiftable Object and should only be used whrn
        /// moving this object around in the world. If you are just swithcing Cooordinate Systems then ShiftToAnotherCoordinateSystem() 
        /// should be used instead.
        /// </summary>
        /// <param name="shiftableObject">The shiftable object to shift the geometry and coodinate system of</param>
        /// <param name="passedShift">The shift to apply to this object's geometry and coordinate system</param>
        /// <param name="systemShiftIsBasedOn">The coordinate system the shift is based on and to shift in. If left out it defaults to the current coordinate system</param>
        public static void Shift(this IShiftableCollection shiftableObject, Shift passedShift, CoordinateSystem systemShiftIsBasedOn = null)
        {
            shiftableObject.ShiftGeometry(passedShift, systemShiftIsBasedOn);
            shiftableObject.HomeCoordinateSystem = shiftableObject.HomeCoordinateSystem.RelativeShift(passedShift, systemShiftIsBasedOn);
        }

        /// <summary>
        /// This shifts the object from the perspective of the current coordinate system to the passed system, moving the geometry but not its coordinate system
        /// </summary>
        /// <param name="shiftableObject">The shiftable object to change the Coordinate System to</param>
        /// <param name="toShiftTo">The CoordianteSystem to shift this object into the perspective of</param>
        public static void ShiftToCoordinateSystem(this IShiftableCollection shiftableObject, CoordinateSystem toShiftTo)
        {
            shiftableObject.ShiftGeometryToCoordinateSystem(toShiftTo);
            shiftableObject.CurrentCoordinateSystem = toShiftTo;
        }
    }
}
