using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class IListPlaneRegionExtensionMethods
    {
        /// <summary>
        /// Rotates the list of plane regions with the given rotation
        /// </summary>
        /// <param name="passedPlaneRegions">The plane Regions to rotate</param>
        /// <param name="passedRotation">The Rotation to apply</param>
        /// <returns>A new list of new PlaneRegions that have been rotated</returns>
        public static List<PlaneRegion> Rotate(this IList<PlaneRegion> passedPlaneRegions, Rotation passedRotation)
        {
            List<PlaneRegion> rotatedRegion = new List<PlaneRegion>();
            foreach (var planeRegion in passedPlaneRegions)
            {
                rotatedRegion.Add(planeRegion.Rotate(passedRotation));
            }
            return rotatedRegion;
        }

        /// <summary>
        /// Translate the List of Plane Regions with the given translation
        /// </summary>
        /// <param name="passedPlaneRegions">The planeregions to translate</param>
        /// <param name="passedTranslation">The Translation to apply</param>
        /// <returns>A new list of new PlaneRegions that have been translated</returns>
        public static List<PlaneRegion> Translate(this IList<PlaneRegion> passedPlaneRegions, Point passedTranslation)
        {
            List<PlaneRegion> translatedRegion = new List<PlaneRegion>();
            foreach (var planeRegion in passedPlaneRegions)
            {
                translatedRegion.Add(planeRegion.Translate(passedTranslation));
            }
            return translatedRegion;
        }

        /// <summary>
        /// Shifts the list of plane regions with the given Shift
        /// </summary>
        /// <param name="passedPlaneRegions">The plane Regions to shift</param>
        /// <param name="passedShift">The Shift to apply</param>
        /// <returns>A new list of new Polygons that have been shifted</returns>
        public static List<PlaneRegion> Shift(this List<PlaneRegion> passedPlaneRegions, Shift passedShift)
        {
            List<PlaneRegion> shiftedRegions = new List<PlaneRegion>();

            foreach (var region in passedPlaneRegions)
            {
                shiftedRegions.Add(region.Shift(passedShift));
            }

            return shiftedRegions;
        }
    }
}
