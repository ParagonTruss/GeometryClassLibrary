﻿using GeometryClassLibrary;


namespace GeometryClassLibrary
{
    public static class IShiftableExtensions
    {
        /// <summary>
        /// Shifts this object using the given shift
        /// Note: This shifts both the Geometry and the Coordinate System of this Shiftable Object and should only be used whrn
        /// moving this object around in the world. If you are just swithcing Cooordinate Systems then ShiftToAnotherCoordinateSystem() 
        /// should be used instead.
        /// </summary>
        /// <param name="shiftableObject">The shiftable object to shift the coodinate system of</param>
        /// <param name="passedShift">The shift to apply to this object's coordinate system</param>
        /// <param name="systemShiftIsBasedOn">The coordinate system the shift is based on and to shift in. If left out it defaults to the current coordinate system</param>
        public static void Shift(this IShiftable shiftableObject, Shift passedShift, CoordinateSystem systemShiftIsBasedOn = null)
        {
            if(systemShiftIsBasedOn == null)
            {
                systemShiftIsBasedOn = shiftableObject.CurrentCoordinateSystem;
            }
            shiftableObject.HomeCoordinateSystem = shiftableObject.HomeCoordinateSystem.RelativeShift(passedShift, shiftableObject.CurrentCoordinateSystem);
        }

        /// <summary>
        /// This shifts the object from the perspective of the current coordinate system to the passed system, moving the geometry but not its coordinate system
        /// </summary>
        /// <param name="shiftableObject">The shiftable object to change the Coordinate System to</param>
        /// <param name="toShiftTo">The CoordianteSystem to shift this object into the perspective of</param>
        public static void ShiftToCoordinateSystem(this IShiftable shiftableObject, CoordinateSystem toShiftTo)
        {
            shiftableObject.CurrentCoordinateSystem = toShiftTo;
        }
    }
}
