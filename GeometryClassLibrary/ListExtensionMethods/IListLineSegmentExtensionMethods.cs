using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class ListLineSegmentExtensionMethods
    {
        public static List<Polygon> MakeCoplanarLineSegmentsIntoPolygons(this List<LineSegment> passedLineSegments)
        {
            List<Plane> planesList = new List<Plane>();

            foreach (var segment1 in passedLineSegments)
            {
                foreach (var segment2 in passedLineSegments)
                {
                    if (segment1.IsCoplanarWith(segment2))
                    {
                        Plane possiblePlane = new Plane(segment1, segment2);
                        if (segment1 != segment2 && segment1.DoesSharesABaseOrEndPointWith(segment2) && !planesList.Contains(possiblePlane))
                        {
                            planesList.Add(possiblePlane);
                        }
                    }

                }
            }

            List<Polygon> returnList = new List<Polygon>();

            foreach (var plane in planesList)
            {
                List<LineSegment> newRegionBoundaries = new List<LineSegment>();
                foreach (var segment in passedLineSegments)
                {
                    if (plane.Contains(segment) && !newRegionBoundaries.Contains(segment))
                    {
                        newRegionBoundaries.Add(segment);
                    }
                }
                returnList.Add(new Polygon(newRegionBoundaries));
            }

            return returnList;
        }

        /// <summary>
        /// Puts the boundaries in order and then checks if the end point of the last one is on the base point of the first one
        /// </summary>
        /// <param name="passedBoundaries"></param>
        /// <returns></returns>
        public static bool DoFormClosedRegion(this List<LineSegment> passedBoundaries)
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

        /// <summary>
        /// Will break down all line segments into points and form them into clockwise traveling segments
        /// Segments must be coplanar and closed or else it will return null
        /// </summary>
        /// <param name="borders"></param>
        /// <returns>returns the LineSegmetns sorted in clockwise order all pointing in the clockwise direction</returns>
        public static IList<LineSegment> SortIntoClockWiseSegments(this List<LineSegment> borders)
        {
            if (borders.AreAllCoplanar() && borders.DoFormClosedRegion())
            {
                List<LineSegment> sortedSegments = new List<LineSegment>();
                Point minXPoint = null;

                // find the Point with the smallest X
                foreach (Point vertex in borders.GetAllPoints())
                {
                    if (minXPoint == null || vertex.X < minXPoint.X)
                    {
                        minXPoint = vertex;
                    }
                }

                //find the line with the highest Y that contains the smallestXPoint
                LineSegment maxYLine = null;
                Point maxYPoint = null;

                foreach (LineSegment segment in borders)
                {
                    if (segment.BasePoint == minXPoint)
                    {
                        if (maxYPoint == null || maxYPoint.Y < segment.EndPoint.Y)
                        {
                            maxYPoint = segment.EndPoint;
                            maxYLine = segment;
                        }
                    }
                    else if (segment.EndPoint == minXPoint)
                    {
                        if (maxYPoint == null || maxYPoint.Y < segment.BasePoint.Y)
                        {
                            maxYPoint = segment.BasePoint;
                            maxYLine = segment;
                        }
                    }
                }

                //now start at the first line and make sure it starts at the min and goes to the max y point
                if (maxYLine.BasePoint == minXPoint)
                {
                    sortedSegments.Add(new LineSegment(maxYLine));
                }
                else
                {
                    sortedSegments.Add(maxYLine.Reverse());
                }

                //create a shalloe copy of the borders to find out which we have checked
                List<LineSegment> hasNotAdded = new List<LineSegment>(borders);
                hasNotAdded.Remove(maxYLine);

                //get our endpoint to find out which segment to process next
                Point previousEndPoint = sortedSegments[0].EndPoint;

                //now go through the rest and find the next Segment to use
                for (int i = 0; i < hasNotAdded.Count; i++)
                {
                    LineSegment currentLine = hasNotAdded[i];

                    //if our line contains the previous endpoint than it comes next and add it in the right direction
                    if (currentLine.BasePoint == previousEndPoint)
                    {
                        sortedSegments.Add(new LineSegment(currentLine));
                        previousEndPoint = currentLine.EndPoint;
                        hasNotAdded.Remove(currentLine);
                        //restart the looping (note it will increment before starting again hence the -1)
                        i = -1;
                    }
                    else if (currentLine.EndPoint == previousEndPoint)
                    {
                        sortedSegments.Add(currentLine.Reverse());
                        previousEndPoint = currentLine.BasePoint;
                        hasNotAdded.Remove(currentLine);
                        //restart the looping 
                        i = -1;
                    }
                }

                //now were all done and return the sorted segments
                return sortedSegments;
            }
            return null;
        }

        /// <summary>
        /// takes the points from a list of line segments, rotates them, and turns them back into segments
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="axis"></param>
        /// <param name="rotateAngle"></param>
        /// <returns></returns>
        public static IList<LineSegment> RotatePointsAboutAnAxis(this IList<LineSegment> segments, Line axis, Angle rotateAngle)
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
        public static List<Point> GetAllPoints(this IList<LineSegment> passedSegments)
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
        public static Area FindAreaOfPolygon(this List<LineSegment> passedBorders)
        {
            if (passedBorders.AreAllCoplanar())
            {
                Dimension areaDimension = new Dimension();
                Vector areaVector = new Vector();

                if (passedBorders != null && passedBorders.Count() > 2)
                {
                    //following the method here of projecting the triangles formed with an arbitrary point onto the plane: http://geomalgorithms.com/a01-_area.html

                    //first sort them clockwise
                    List<LineSegment> sortedBorders = (List<LineSegment>)passedBorders.SortIntoClockWiseSegments();

                    //get our verticies
                    List<Point> verticies = sortedBorders.GetAllPoints();

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

        public static List<LineSegment> Shift(this IList<LineSegment> passedLineSegments, Shift passedShift)
        {
            List<LineSegment> shiftedSegments = new List<LineSegment>();

            foreach (var segment in passedLineSegments)
            {
                shiftedSegments.Add(  segment.Shift(passedShift));
            }

            return shiftedSegments;
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed LineSegments</param>
        /// <returns></returns>
        public static bool AreAllCoplanar(this List<LineSegment> passedLineList)
        {
            List<Line> passedLineListCasted = passedLineList.ConvertAll(x => (Line)x);

            return passedLineListCasted.AreAllCoplanar();
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed LineSegments</param>
        /// <returns></returns>
        public static bool AreAllParallel(this List<LineSegment> passedLineList)
        {
            List<Line> passedLineListCasted = passedLineList.ConvertAll(x => (Line)x);

            return passedLineListCasted.AreAllParallel();
        }
    }
}