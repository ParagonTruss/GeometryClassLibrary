using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary.ExtensionMethods
{
    /// <summary>
    /// Singleton class for asynchronous file logging
    /// </summary>
    public class Logging
    {
        #region Properties & Destructor
        private static readonly BlockingCollection<Tuple<string,string>> _log = new BlockingCollection<Tuple<string,string>>();
       
        private Logging(){ }
        private static readonly Logging _instance = new Logging();
        // destructor relies on singleton instance
        ~Logging()
        {
            foreach (var tuple in _log)
            {
                // Write all logged values to files, delete existing records in those files.
                File.WriteAllText($"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)}\\{tuple.Item1}.txt", tuple.Item2);
            }
        }
        #endregion
        
        public static void WriteToFile(string fileName, string contents)
        {
            _log.Add(Tuple.Create(fileName,contents));
        }

        public static void WriteToFile(string fileName, IEnumerable<string> contents)
        {
            _log.Add(Tuple.Create(fileName,contents.Join("\r\n")));
        }

        public static void WriteSliceTest(string fileName, Polyhedron polyhedron, Plane slicingPlane)
        {
            WriteToFile(fileName, new[]
            {
                $"// Automatically generated at {DateTime.Now}",
                $"[Test]",
                $"public void SliceError_Case_{Hash(polyhedron, slicingPlane)}()",
                "{",
                $"var polyhedron = {polyhedron.AsString()};",
                $"var slicingPlane = {slicingPlane.AsString()};",
                "var sliceResults = polyhedron.Slice(slicingPlane);",
                "}",
            });
        }
        private static string Hash<T1,T2>(T1 item1, T2 item2) => (item1.GetHashCode() ^ item2.GetHashCode()).ToString("X8");
    }

    public static class LoggingUtilities
    {
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }
        internal static string AsString(this IEnumerable<object> source)
        {
            return "new[]\r\n{\r\n\t" + source.Select(AsString).Join(",\r\n") + "\r\n}";
        }
        
        #region AsString methods
        // Technically we could use an interface (i.e. ILoggable with method AsString), 
        // but that would require maintaining across several files. 
        // This way has all the logging behavior confined to this one extension file.
        internal static string AsString(this object obj)
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
        internal static string AsString(this Polyhedron polyhedron)
        {
            return $"new Polyhedron(true, {AsString(polyhedron.Polygons)})";
        }
        internal static string AsString(this Polygon polygon)
        {
            return $"new Polygon(true,\r\n{polygon.Vertices.Select(AsString).Join(",\r\n")})";
        }
        internal static string AsString(this Point point)
        {
            return $"new Point(Distance.Inches, {point.X.ValueInInches}, {point.Y.ValueInInches}, {point.Z.ValueInInches})";
        }
        internal static string AsString(this Distance distance)
        {
            return $"new Distance(Distance.Inches, {distance.ValueInInches})";
        }
        internal static string AsString(this LineSegment segment)
        {
            return $"new LineSegment({AsString(segment.BasePoint)}, {AsString(segment.EndPoint)})";
        }
        internal static string AsString(this Vector vector)
        {
            return $"new Vector({AsString(vector.BasePoint)}, {AsString(vector.EndPoint)})";
        }
        internal static string AsString(this Line line)
        {
            return $"new Line({AsString(line.BasePoint)}, {AsString(line.Direction)})";
        }
        internal static string AsString(this Plane plane)
        {
            return $"new Plane({AsString(plane.BasePoint)},{AsString(plane.NormalDirection)})";
        }
        internal static string AsString(this Direction direction)
        {
            return $"new Direction({direction.X}, {direction.Y}, {direction.Z})";
        }
        #endregion
    }
}
