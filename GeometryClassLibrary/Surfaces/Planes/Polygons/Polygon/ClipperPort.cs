using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Ports Clippers operations to our library.
    /// </summary>
    internal static class ClipperPort
    {
        public static Polygon Overlap(Polygon polygon1, Polygon polygon2)
        {
            if (polygon1.IsCoplanarTo(polygon2).Not())
            {
                return null;
            }
            var normal = polygon1.NormalDirection;
            if (normal.IsParallelTo(Direction.Out))
            {
                return Overlap_In_XY_Plane(polygon1.Vertices, polygon2.Vertices).ToPolygon();
            }

            var angle = normal.AngleBetween(Direction.Out);
            var axis = new Line(normal.CrossProduct(Direction.Out));
            var rotation = new Rotation(axis,angle);
            var inverse = new Rotation(axis,angle.Negate());


            return Overlap_In_XY_Plane(polygon1.Rotate(rotation).Vertices, polygon2.Rotate(rotation).Vertices).Rotate(inverse).ToPolygon();
        }

       
        // Only works if the points are in the XY plane.
        // Link to their Documentation: http://www.angusj.com/delphi/clipper/documentation/Docs/_Body.htm
        private static List<Point> Overlap_In_XY_Plane(List<Point> points1, List<Point> points2)
        {
            var clipper = new Clipper();
            clipper.AddPath(ToClipperPath(points1), PolyType.ptClip, true);
            clipper.AddPath(ToClipperPath(points2), PolyType.ptSubject, true);

            var soln = new Paths();
            // clipper offers 4 operations: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/ClipType.htm
            var success = clipper.Execute(ClipType.ctIntersection, soln);

            var polygon =  success ? ToPoints(soln[0]) : null;
            return polygon;
        }

       
        // Clipper rounds off doubles into longs. We scale up, and then scale down when converting between, to keep digits of precision.
        // The current scalefactor of a 1000 will preserve 3 digits of precision. 
        private const int scaling = 1000;
        private static IntPoint ToClipperPoint(Point point) => new IntPoint(point.X.ValueInInches * scaling, point.Y.ValueInInches * scaling);
        private static Point ToGCLPoint(IntPoint point) => new Point(Inches, Convert.ToDouble(point.X) / scaling, Convert.ToDouble(point.Y) / scaling);
   
        private static List<IntPoint> ToClipperPath(List<Point> p) => p.Select(ToClipperPoint).ToList();
        private static List<Point> ToPoints(Path path) => path.Select(ToGCLPoint).ToList();
    }
   
}
