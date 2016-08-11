using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    public static class LoggingUtilities
    {
        public static void Log(this List<Polygon> polygons)
        {
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ForTrussDisplay.txt",
                PolygonArrayString(polygons));
        }

        private static string PolygonArrayString(List<Polygon> polygons)
        {
            return "new[]\n {\n\t" + string.Join(",\n\t", polygons.Select(PolygonString)) + "\n};";
        }
        private static string PolygonString(Polygon polygon)
        {
            return $"Polygon.CreateInXYPlane(Inches, {Coordinates(polygon)})";
        }

        private static string Coordinates(Polygon polygon)
        {
            return string.Join(", ", polygon.Vertices
                 .SelectMany(point => new[] { point.X.ValueInInches, point.Y.ValueInInches })
                 .Select(d => d.ToString()));
        }
    }
}
