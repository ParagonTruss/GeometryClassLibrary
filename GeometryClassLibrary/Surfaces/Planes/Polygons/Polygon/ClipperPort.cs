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
                var vertices1 = polygon1.Vertices;
                var vertices2 = polygon2.Vertices;

                var depth = vertices1[0].Z;
                return Overlap_In_XY_Plane(vertices1, vertices2).Shift(depth*Direction.Out).ToPolygon();
            }

            var angle = normal.AngleBetween(Direction.Out);
            var axis = new Line(normal.CrossProduct(Direction.Out));
            var rotation = new Shift(new Rotation(axis, angle));

            var rotated1 = polygon1.Vertices.Shift(rotation);
            var rotated2 = polygon2.Vertices.Shift(rotation);

            var z_depth = rotated1[0].Z;

            var shiftBack = Shift.ComposeLeftToRight(z_depth*Direction.Out, rotation.Inverse());

            return Overlap_In_XY_Plane(rotated1, rotated2)?.Shift(shiftBack).ToPolygon();
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

            if (success.Not())
            {
                return null;
            }
            
            switch (soln.Count)
            {
                case 0:
                    return null;
                case 1:
                    return ToPoints(soln[0]);
                default:
                    throw new ArgumentException("The passed polygons had multiple regions of overlap.");
            }
           
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
