using GeometryClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabricatedComponentTypeLibrary
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
        public static void Shift(this IShiftableCollection shiftableObject, Shift passedShift)
        {
            shiftableObject.ShiftGeometry(passedShift);
            shiftableObject.CoordinateSystem = shiftableObject.CoordinateSystem.RelativeShift(passedShift, shiftableObject.CurrentCoordinateSystem);
        }

        /// <summary>
        /// This shifts the object from the perspective of the current coordinate system to the passed system, moving the geometry but not its coordinate system
        /// </summary>
        /// <param name="shiftableObject">The shiftable object to change the Coordinate System to</param>
        /// <param name="toShiftTo">The CoordianteSystem to shift this object into the perspective of</param>
        public static void ShiftToAnotherCoordinateSystem(this IShiftableCollection shiftableObject, CoordinateSystem toShiftTo)
        {
            shiftableObject.ShiftGeometryToAnotherCoordinateSystem(toShiftTo);
            shiftableObject.CurrentCoordinateSystem = toShiftTo;
        }
    }
}
