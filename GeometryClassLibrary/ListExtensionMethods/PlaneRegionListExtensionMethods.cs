using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class IEnumerablePlaneRegionExtensionMethods
    {
        public static void Rotate(this IEnumerable<Polygon> passedPlaneRegions, Line passedAxisLine, Angle passedRotationAngle)
        {
            foreach (var planeRegion in passedPlaneRegions)
            {
                planeRegion.Rotate(passedAxisLine, passedRotationAngle);
            }
        }

        public static void Translate(this IEnumerable<Polygon> passedPlaneRegions, Dimension xTranslate, Dimension yTranslate, Dimension zTranslate)
        {
            throw new NotImplementedException();
        }

        public static void Translate(this List<Polygon> passedPlaneRegions, Vector passedVector)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Polygon> Shift(this IEnumerable<Polygon> passedPlaneRegions, Shift passedShift)
        {
            List<Polygon> shiftedRegions = new List<Polygon>();

            foreach (var region in passedPlaneRegions)
            {
                shiftedRegions.Add(region.Shift(passedShift));
            }

            return shiftedRegions;
        }
    }
}
