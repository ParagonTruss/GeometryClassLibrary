using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeometryClassLibrary
{
    public static class LoggingUtilities
    {
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }

        public static void Log(this string contents, string fileName)
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}.txt", contents);
        }

        public static void Log(this IEnumerable<string> contents, string fileName)
        {
            File.WriteAllLines($"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}.txt", contents);
        }

        public static void Log(this List<Polygon> polygons, string fileName)
        {
            PolygonArrayString(polygons).Log(fileName);
        }

        public static void Log(this Polygon polygon, string fileName)
        {
            PolygonString(polygon).Log(fileName);
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
                .SelectMany(point => new[] {point.X.ValueInInches, point.Y.ValueInInches}));
        }
    }
}
