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
