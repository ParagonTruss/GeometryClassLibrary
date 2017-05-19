using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary.ExtensionMethods
{
    public static class LoggingUtilities
    {
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }

        public static void Log(this string contents, string fileName)
        {
            File.WriteAllText($"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)}\\{fileName}.txt", contents);
        }

        public static void Log(this IEnumerable<string> contents, string fileName)
        {
            File.WriteAllLines($"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)}\\{fileName}.txt", contents);
        }

        public static void WriteSliceTest(Polyhedron polyhedron, Plane slicingPlane, string fileName)
        {
            new []
            {
                $"// Automatically generated at {DateTime.Now}",
                $"[Test]",
                $"public void SliceError_Case_{Hash(polyhedron,slicingPlane)}()",
                "{",
                $"var polyhedron = {AsString(polyhedron)};",
                $"var slicingPlane = {AsString(slicingPlane)};",
                "var sliceResults = polyhedron.Slice(slicingPlane);",
                "}",
            }.Log(fileName);
        }
        private static string Hash<T1,T2>(T1 item1, T2 item2) => (item1.GetHashCode() ^ item2.GetHashCode()).ToString("X8");

        public static void Log(this IEnumerable<object> source, string fileName)
        {
            AsString(source).Log(fileName);
        }

        public static void Log(this object item, string fileName)
        {
            AsString(item).Log(fileName);
        }
        private static string AsString(IEnumerable<object> source)
        {
            return "new[]\r\n{\r\n\t" + source.Select(AsString).Join(",\r\n") + "\r\n}";
        }
        
        #region AsString methods
        // Technically we could use an interface (i.e. ILoggable with method AsString), 
        // but that would require maintaining across several files. 
        // This way has all the logging behavior confined to this one extension file.
        private static string AsString(object obj)
        {
            // dynamic keyword doesn't work with extension methods. So we do this instead.
            if (obj is Polyhedron) return AsString(obj as Polyhedron);
            if (obj is Polygon) return AsString(obj as Polygon);
            if (obj is Plane) return AsString(obj as Plane);
            if (obj is Point) return AsString(obj as Point);
            if (obj is Distance) return AsString(obj as Distance);
            if (obj is LineSegment) return AsString(obj as LineSegment);
            if (obj is Vector) return AsString(obj as Vector);
            if (obj is Line) return AsString(obj as Line);
            if (obj is Direction) return AsString(obj as Direction);
            throw new NotImplementedException();
        }
        private static string AsString(Polyhedron polyhedron)
        {
            return $"new Polyhedron(true, {AsString(polyhedron.Polygons)})";
        }
        private static string AsString(Polygon polygon)
        {
            return $"new Polygon(true,\r\n{polygon.Vertices.Select(AsString).Join(",\r\n")})";
        }
        private static string AsString(Point point)
        {
            return $"new Point(Distance.Inches, {AsString(point.X)}, {AsString(point.Y)}, {AsString(point.Z)})";
        }
        private static string AsString(Distance distance)
        {
            return $"new Point(Distance.Inches, {distance.ValueInInches})";
        }
        private static string AsString(LineSegment segment)
        {
            return $"new LineSegment({AsString(segment.BasePoint)}, {AsString(segment.EndPoint)})";
        }
        private static string AsString(Vector vector)
        {
            return $"new Vector({AsString(vector.BasePoint)}, {AsString(vector.EndPoint)})";
        }
        private static string AsString(Line line)
        {
            return $"new Line({AsString(line.BasePoint)}, {AsString(line.Direction)})";
        }
        private static string AsString(Plane plane)
        {
            return $"new Plane({AsString(plane.BasePoint)},{AsString(plane.NormalDirection)})";
        }
        private static string AsString(Direction direction)
        {
            return $"new Direction({direction.X}, {direction.Y}, {direction.Z})";
        }
        #endregion
    }
}
