using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IListPolygonExtensionMethods
    {
        public static List<Polygon> Shift(this List<Polygon> passedPolygon, Shift passedShift)
        {
            List<Polygon> shiftedPolygons = new List<Polygon>();

            foreach (var region in passedPolygon)
            {
                shiftedPolygons.Add(region.Shift(passedShift));
            }

            return shiftedPolygons;
        }
    }
}
