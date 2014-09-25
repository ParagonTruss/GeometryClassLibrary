using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class IEnumerableLineSegmentExtensionMethods
    {
        public static List<PlaneRegion> MakeCoplanarLineSegmentsIntoRegions(this IEnumerable<LineSegment> passedLineSegments)
        {
            List<Plane> planesList = new List<Plane>();

            foreach (var segment1 in passedLineSegments)
            {
                foreach (var segment2 in passedLineSegments)
                {
                    if (segment1 != segment2 && segment1.DoesSharesABaseOrEndPointWith(segment2) && passedLineSegments.AreAllCoplanar())
                    {
                        planesList.Add(new Plane(segment1, segment2));
                    }
                }
            }

            List<PlaneRegion> returnList = new List<PlaneRegion>();

            foreach (var plane in planesList)
            {
                List<LineSegment> newRegionBoundaries = new List<LineSegment>();
                foreach (var segment in passedLineSegments)
                {
                    if (plane.Contains(segment))
                    {
                        newRegionBoundaries.Add(segment);
                    }
                }
                returnList.Add(new PlaneRegion(newRegionBoundaries));
            }

            return returnList;
        }

        /// <summary>
        /// Puts the boundaries in order and then checks if the end point of the last one is on the base point of the first one
        /// </summary>
        /// <param name="passedBoundaries"></param>
        /// <returns></returns>
        public static bool DoFormClosedRegion(this IEnumerable<LineSegment> passedBoundaries)
        {
            List<LineSegment> newList = new List<LineSegment>();
            foreach (LineSegment segment in passedBoundaries)
            {
                newList.Add(
                    new LineSegment(
                        PointGenerator.MakePointWithMillimeters(
                            Math.Round(segment.BasePoint.X.Millimeters, 3),
                            Math.Round(segment.BasePoint.Y.Millimeters, 3),
                            Math.Round(segment.BasePoint.Z.Millimeters, 3)
                        ),
                        PointGenerator.MakePointWithMillimeters(
                            Math.Round(segment.EndPoint.X.Millimeters, 3),
                            Math.Round(segment.EndPoint.Y.Millimeters, 3),
                            Math.Round(segment.EndPoint.Z.Millimeters, 3)
                        )
                    )
                );
            }

            return newList
                .SelectMany(segment => new[] { segment.BasePoint, segment.EndPoint })
                .GroupBy(point => new { point.X, point.Y, point.Z })
                .All(group => group.Count() == 2);
        }

        public static IEnumerable<LineSegment> RoundAllPoints(this IEnumerable<LineSegment> passedBoundaries, int passedNumberOfDecimals)
        {
            List<LineSegment> newSegments = new List<LineSegment>();

            foreach (LineSegment segment in passedBoundaries)
            {
                double newBaseX = Math.Round(segment.BasePoint.X.Millimeters, passedNumberOfDecimals);
                double newBaseY = Math.Round(segment.BasePoint.Y.Millimeters, passedNumberOfDecimals);
                double newBaseZ = Math.Round(segment.BasePoint.Z.Millimeters, passedNumberOfDecimals);
                Point newBasePoint = PointGenerator.MakePointWithMillimeters(newBaseX, newBaseY, newBaseZ);

                double newEndX = Math.Round(segment.EndPoint.X.Millimeters, passedNumberOfDecimals);
                double newEndY = Math.Round(segment.EndPoint.Y.Millimeters, passedNumberOfDecimals);
                double newEndZ = Math.Round(segment.EndPoint.Z.Millimeters, passedNumberOfDecimals);
                Point newEndPoint = PointGenerator.MakePointWithMillimeters(newEndX, newEndY, newEndZ);

                newSegments.Add(new LineSegment(newBasePoint, newEndPoint));
            }

            return newSegments;
        }

        /// <summary>
        /// Will break down all line segments into points and form them into clockwise traveling segments
        /// </summary>
        /// <param name="borders"></param>
        /// <returns></returns>
        public static IEnumerable<LineSegment> SortIntoClockWiseSegments(this IEnumerable<LineSegment> borders)
        {
            Point minXPoint = PointGenerator.MakePointWithMillimeters(double.MaxValue, double.MaxValue);

            // find the Point with the smallest X
            foreach (Point vertex in borders.GetAllPoints())
            {

            }

            foreach (LineSegment segment in borders)
            {
                // if the x's are equal but the y is higher, make the new point the one with the higher y
                if (segment.BasePoint.X == minXPoint.X)
                {
                    if (segment.BasePoint.Y > minXPoint.Y)
                        minXPoint = segment.BasePoint;
                }
                // else if this segment's base point has a lower x, make it the new minX
                else if (segment.BasePoint.X < minXPoint.X)
                    minXPoint = segment.BasePoint;
            }

            LineSegment firstChoice = null;
            LineSegment secondChoice = null;
            LineSegment startingSegment = null;

            // figure out which point to cross over to first
            foreach (LineSegment segment in borders)
            {
                if (segment.BasePoint.Equals(minXPoint))
                    firstChoice = segment;
                else if (segment.EndPoint.Equals(minXPoint))
                    secondChoice = segment;
            }

            // if the first choice is higher thna the second choice, choose it
            if (firstChoice.EndPoint.Y > secondChoice.BasePoint.Y)
            {
                startingSegment = firstChoice;
            }
            // else the second point is higher, so you need to reverse it and go on
            else
            {
                startingSegment = secondChoice;

            }

            // use loop to check each individual point and make sure the segment using it is clockwise
            return null;
        }

        /// <summary>
        /// takes the points from a list of line segments, rotates them, and turns them back into segments
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="axis"></param>
        /// <param name="rotateAngle"></param>
        /// <returns></returns>
        public static IEnumerable<LineSegment> RotatePointsAboutAnAxis(this IEnumerable<LineSegment> segments, Line axis, Angle rotateAngle)
        {
            List<Point> pointList = new List<Point>();

            // adds every point used; needs to check base AND end in case line segments don't form a closed region
            foreach (LineSegment segment in segments)
            {
                if (!pointList.Contains(segment.BasePoint))
                    pointList.Add(segment.BasePoint);

                if (!pointList.Contains(segment.EndPoint))
                    pointList.Add(segment.EndPoint);
            }

            List<Point> newList = new List<Point>();

            foreach (Point point in pointList)
            {
                newList.Add(point.Rotate3D(axis, rotateAngle));
            }

            return newList.MakeIntoLineSegmentsThatMeet();
        }

        /// <summary>
        /// Gets a list of all the unique Points represented in this list of LineSegments (both end and base points)
        /// </summary>
        /// <param name="passedSegments">The List of LineSegments to get the points of</param>
        /// <returns>Returns a list of Points containing all the unique Points in the LineSegments List</returns>
        public static List<Point> GetAllPoints(this IEnumerable<LineSegment> passedSegments)
        {
            List<Point> points = new List<Point>();

            //just cycle through each line and add the points to our list if they are not already there
            foreach (LineSegment line in passedSegments)
            {
                if (!points.Contains(line.BasePoint))
                {
                    points.Add(line.BasePoint);
                }
                if (!points.Contains(line.EndPoint))
                {
                    points.Add(line.EndPoint);
                }
            }

            return points;
        }
    

        /// <summary>
        /// finds the area of an irregular polygon.  ASSUMES THAT LINESEGMENTS ARE IN CLOCKWISE ORDER!!!!!  May need to change later
        /// </summary>
        /// <param name="passedBorders"></param>
        /// <returns></returns>
        public static Area FindAreaOfPolygon(this IEnumerable<LineSegment> passedBorders)
        {
            if (passedBorders.AreAllCoplanar())
            {
                Dimension areaDimension = new Dimension();
                Vector areaVector = new Vector();

                if (passedBorders != null && passedBorders.Count() > 2)
                {
                    //following the method here of projecting the triangles formed with an arbitrary point onto the plane: http://geomalgorithms.com/a01-_area.html

                    //get our verticies
                    List<Point> verticies = passedBorders.GetAllPoints();

                    //for each of our verticies compare it to the previous one
                    Point previousVertex = verticies[verticies.Count - 1];
                    foreach (Point vertex in verticies)
                    {
                        //take the cross product of them (relative to the origin)
                        Vector crossProduct = vertex.VectorFromOriginToPoint().CrossProduct(previousVertex.VectorFromOriginToPoint());

                        //now we add the magnitute to the area
                        //areaDimension += crossProduct.Magnitude;
                        areaVector += crossProduct;

                        previousVertex = vertex;
                    }
                }

                areaDimension = areaVector.Magnitude;

                if (areaDimension > new Dimension())
                {
                    //the area is really units squared so we need to return it that way - not as a dimension
                    //also we need to divide it in half (we were using triangles)
                    return new Area(AreaType.MillimetersSquared, areaDimension.Millimeters / 2);
                }
                else
                {
                    return new Area();
                }
            }
            return null;
        }

        public static List<LineSegment> Shift(this IEnumerable<LineSegment> passedLineSegments, Shift passedShift)
        {
            List<LineSegment> shiftedSegments = new List<LineSegment>();

            foreach (var segment in passedLineSegments)
            {
                shiftedSegments.Add(  segment.Shift(passedShift));
            }

            return shiftedSegments;
        }
    }
}