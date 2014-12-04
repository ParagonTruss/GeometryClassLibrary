using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public static class IListPolygonExtensionMethods
    {
        /// <summary>
        /// Rotates the list of polygons with the given rotation
        /// </summary>
        /// <param name="passedPolygons">The polygons to rotate</param>
        /// <param name="passedRotation">The Rotation to apply</param>
        /// <returns>A new list of new Polygons that have been rotated</returns>
        public static List<Polygon> Rotate(this IList<Polygon> passedPolygons, Rotation passedRotation)
        {
            List<Polygon> rotatedRegion = new List<Polygon>();
            foreach (var planeRegion in passedPolygons)
            {
                rotatedRegion.Add(planeRegion.Rotate(passedRotation));
            }
            return rotatedRegion;
        }

        /// <summary>
        /// Shifts the polygons in the list with the given shift
        /// </summary>
        /// <param name="passedPolygon">The polygons to Shift</param>
        /// <param name="passedShift">The shift to apply</param>
        /// <returns>A nes list of ne Polygons that have been shifted</returns>
        public static List<Polygon> Shift(this List<Polygon> passedPolygon, Shift passedShift)
        {
            List<Polygon> shiftedPolygons = new List<Polygon>();

            foreach (var region in passedPolygon)
            {
                shiftedPolygons.Add(region.Shift(passedShift));
            }

            return shiftedPolygons;
        }

        /// <summary>
        /// Returns whether or not the polygon has a common side with any of the polygons in this list
        /// </summary>
        /// <param name="polygonsList">The List of Polygons to check if the passed polygon shares a side with</param>
        /// <param name="polygonToCheckSidesOf">The polygon to see if any of the polygons in this list share a side with</param>
        /// <returns>Returns a bool of whether or not the Polyhedron has a common side with the polygon</returns>
        public static bool DoesShareSideWithPolygonInList(this List<Polygon> polygonsList, Polygon polygonToCheckSidesOf)
        {
            foreach (Polygon polygon in polygonsList)
            {
                if (polygonToCheckSidesOf.DoesShareSide(polygon))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
