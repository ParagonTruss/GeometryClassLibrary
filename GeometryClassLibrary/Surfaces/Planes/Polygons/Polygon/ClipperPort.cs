using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using UnitClassLibrary.AngleUnit;
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
            var rotation = GetRotation(normal);
         
            var rotated1 = polygon1.Vertices.Shift(rotation);
            var rotated2 = polygon2.Vertices.Shift(rotation);

            var overlapped = Overlap_In_XY_Plane(rotated1, rotated2);
            if (overlapped.IsNull())
            {
                return null;
            }

            overlapped = RemovePointsTooCloseToEachOther(overlapped);
           
            var depth = rotated1[0].Z;
            var shiftBack = Shift.ComposeLeftToRight(depth*Direction.Out, rotation.Inverse());
            
            return new Polygon(overlapped.Shift(shiftBack));
        }
        public static List<Polygon> RemoveOverlap(Polygon polygon1, List<Polygon> polygons)
        {
            foreach (var polygon in polygons)
            {
                if (!polygon1.IsCoplanarTo(polygon))
                {
                    return null;
                }
            }
            var normal = polygon1.NormalDirection;
            //For clipper, everything must be converted to 2d, so any references to rotation are dealing with that
            var rotation = GetRotation(normal);

            var rotated1 = polygon1.Vertices.Shift(rotation);
            var rotatedPolygons = new List<List<Point>>();
            foreach (var polygon in polygons)
            {
                rotatedPolygons.Add(polygon.Vertices.Shift(rotation));
            }
       
            var overlapped = Remove_Overlap_In_XY_Plane(rotated1, rotatedPolygons);
            if (overlapped.IsNull())
            {
                return null;
            }
            for (int i = 0; i < overlapped.Count; i++)
            {
                overlapped[i] = RemovePointsTooCloseToEachOther(overlapped[i]);
            }
            

            var depth = rotated1[0].Z;
            var shiftBack = Shift.ComposeLeftToRight(depth * Direction.Out, rotation.Inverse());
            var finalPolygons = new List<Polygon>();

            //for some reason, Clipper will sometimes return very thin areas that we consider invalid and overlapping
            //this function loops over all points in each polygon, and if the angle between any two
            //segments is greater than 179 degrees, it gets rid of the intermediary point
            for (int i = 0; i < overlapped.Count; i++)
            {
                var pointList = overlapped[i].Shift(shiftBack);
                for (int j = 0; j < overlapped[i].Count; j++)
                {
                    var index1 = j;
                    var index2 = j + 1;
                    var index3 = j + 2;
                    if (j == overlapped[i].Count - 1)
                    {
                        index2 = 0;
                        index3 = 1;
                    }
                    if (j == overlapped[i].Count - 2)
                    {
                        index3 = 0;
                    }
                    var seg1 = new LineSegment(pointList[index1], pointList[index2]);
                    var seg2 = new LineSegment(pointList[index2], pointList[index3]);
                    var angleBetween = seg1.AngleBetween(seg2);
                    if (Math.Abs(angleBetween.InDegrees.Value) >= 179)
                    {
                        overlapped[i].RemoveAt(index2);
                    }
                }
                
            }
            //In rare cases, Clipper will return a polygon with no points, this removes those
            for (int i = 0; i < overlapped.Count; i++)
            {
                if (overlapped[i].Count == 0)
                {
                    overlapped.RemoveAt(i);
                } 
            }
            foreach (var polygonPointList in overlapped)
            {
                finalPolygons.Add(new Polygon(polygonPointList.Shift(shiftBack)));
            }
            return finalPolygons;
        }

        private static Shift GetRotation(Direction normal)
        {
            if (normal.IsParallelTo(Direction.Out))
            {
                return Rotation.Identity;
            }
            var angle = normal.AngleBetween(Direction.Out);
            var axis = new Line(normal.CrossProduct(Direction.Out));
            var rotation = new Shift(new Rotation(axis, angle));
            return rotation;
        }

        private static List<Point> RemovePointsTooCloseToEachOther(List<Point> overlapped)
        {
            var newPoints = new List<Point>();
            foreach (var point in overlapped)
            {
                if (point != newPoints.LastOrDefault())
                {
                   newPoints.Add(point);
                }
            }
            if (newPoints[0] == newPoints.Last())
            {
                newPoints.RemoveAt(newPoints.Count-1);
            }
            return newPoints;
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
        private static List<List<Point>> Remove_Overlap_In_XY_Plane(List<Point> points1, List<List<Point>> otherPolygonsPoints)
        {
            var clipper = new Clipper();
            
            clipper.StrictlySimple = true; //we only handle so-called simple polygons, this helps ensure that the only results clipper returns are simple polygons
            
            clipper.AddPath(ToClipperPath(points1), PolyType.ptSubject, true);
            foreach (var pointList in otherPolygonsPoints)
            { 
                clipper.AddPath(ToClipperPath(pointList), PolyType.ptClip, true);
            }

            var soln = new Paths();
            // clipper offers 4 operations: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/ClipType.htm
            var success = clipper.Execute(ClipType.ctDifference, soln);
            
            var resultantPolygonPoints = new List<List<Point>>();
            if (success.Not())
            {
                return null;
            }
            foreach (var pointList in soln)
            {
                resultantPolygonPoints.Add(ToPoints(pointList));
            }
            return resultantPolygonPoints;
        }


        // Clipper rounds off doubles into longs. We scale up, and then scale down when converting between, to keep digits of precision.
        // The current scalefactor of a 1000 will preserve 3 digits of precision. 
        private const int scaling = 1000;
        private static IntPoint ToClipperPoint(Point point) => new IntPoint(Convert.ToInt64(point.X.ValueInInches * scaling),Convert.ToInt64(point.Y.ValueInInches * scaling));
        private static Point ToGCLPoint(IntPoint point) => new Point(Inches, Convert.ToDouble(point.X) / scaling, Convert.ToDouble(point.Y) / scaling);
   
        private static List<IntPoint> ToClipperPath(List<Point> p) => p.Select(ToClipperPoint).ToList();
        private static List<Point> ToPoints(Path path) => path.Select(ToGCLPoint).ToList();
    }
   
}
