using GeometryClassLibrary;
using System;
namespace PrefabricatedComponentTypeLibrary
{
    public interface IShiftable
    {
        /// <summary>
        /// The coordinate system that has this object at the base point
        /// </summary>
        CoordinateSystem HomeCoordinateSystem { get; set; }

        /// <summary>
        /// The coordinate system that this object is currently oriented in
        /// We keep track of this so we always know how to get back to our home coordinates
        /// and to simplify methods and higher level functionalities
        /// </summary>
        CoordinateSystem CurrentCoordinateSystem { get; set; }
    }
}
