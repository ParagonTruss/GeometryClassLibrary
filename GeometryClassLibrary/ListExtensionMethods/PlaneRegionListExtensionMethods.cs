using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class IEnumerablePlaneRegionExtensionMethods
    {
        public static void Rotate(this IEnumerable<PlaneRegion> passedPlaneRegions, Line passedAxisLine, Angle passedRotationAngle)
        {
            foreach (var planeRegion in passedPlaneRegions)
            {
                planeRegion.Rotate(passedAxisLine, passedRotationAngle);
            }
        }

        public static void Translate(this IEnumerable<PlaneRegion> passedPlaneRegions, Dimension xTranslate, Dimension yTranslate, Dimension zTranslate)
        {
            throw new NotImplementedException();
        }

        public static void Translate(this List<PlaneRegion> passedPlaneRegions, Vector passedVector)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<PlaneRegion> Shift(this IEnumerable<PlaneRegion> passedPlaneRegions, Shift passedShift)
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
