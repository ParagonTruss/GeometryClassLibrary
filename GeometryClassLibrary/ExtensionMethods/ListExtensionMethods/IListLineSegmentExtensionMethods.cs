﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    if (segment1 != segment2 && segment1.IsCoplanarWith(segment2))
                    {
                        Plane possiblePlane = new Plane(segment1, segment2);
                        if (segment1.SharesABaseOrEndPointWith(segment2) && !planesList.Contains(possiblePlane))
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
        public static bool DoFormClosedRegion(this IList<LineSegment> passedBoundaries)
        {
            //changed from this:
            //the problem with this is that it uses hashcode, which is based on intrinsic value and thus does not get the rounding
            /*return newList
                .SelectMany(segment => new[] { segment.BasePoint, segment.EndPoint })
                .GroupBy(point => new { point.X, point.Y, point.Z })
                .All(group => group.Count() == 2);*/

            //find all the points we have used
            List<Point> points = passedBoundaries.GetAllPoints();

            //we should have the same number of points as line segments for it to even potentially form a closed region
            if (points.Count != passedBoundaries.Count)
            {
                return false;
            }

            //find out how many times each point is used
            int[] pointsUsedCount = new int[points.Count];

            foreach (LineSegment segment in passedBoundaries)
            {
                pointsUsedCount[points.IndexOf(segment.BasePoint)] ++;
                pointsUsedCount[points.IndexOf(segment.EndPoint)] ++;
            }

            //every point has to be used twice in order for it to be closed
            //Note: cant be large than two either to preserve same functionality as before
            foreach (int timesUsed in pointsUsedCount)
            {
                if (timesUsed != 2)
                {
                    return false;
                }
            }

            //if everything passed than we're good!
            return true;
        }

        public static bool AtleastOneIntersection(this IList<LineSegment> listOfSegments)
        {
            for (int i = 0; i < listOfSegments.Count; i++)
            {
                for (int j = i + 1; j < listOfSegments.Count; j++)
                {
                    if (listOfSegments[i].DoesIntersectNotTouching(listOfSegments[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the Polygon is valid (is a closed region and the LineSegments are all coplaner)
        /// </summary>
        /// <returns>returns true if the LineSegments form a closed area and they are all coplaner</returns>
        public static void ValidateForPolygon(this IList<LineSegment> segments )
        {
            bool notEnoughSegments = (segments == null || segments.Count < 3);
            if (notEnoughSegments)
            {
                throw new Exception("The passed list is either null or has less than 3 segments.");
            }
            bool notClosed = !segments.DoFormClosedRegion();
            if (notClosed)
            {
                throw new Exception("The passed list of segments do not form a closed region.");
            }
            bool areNotCoplanar = !segments.AreAllCoplanar();
            if (areNotCoplanar)
            {
                throw new Exception("The passed list of segments are not coplanar.");
            }
            bool selfIntersecting = segments.AtleastOneIntersection();
            if (selfIntersecting)
            {
                throw new Exception("The passed list of segments are self intersecting.");
            }
        }

        
        /// <summary>
        /// Creates a polygon if its valid to, otherwise returns null
        /// Use this over the ordinary constructor if you want to avoid a throw exception
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public static Polygon CreatePolygonIfValid(this List<LineSegment> segments)
        {
            try
            {
                var polygon = new Polygon(segments);
                return polygon;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Will break down all line segments into points and form them into clockwise traveling segments
        /// Segments must be coplanar and closed or else it will return null
        /// </summary>
        /// <param name="segments"></param>
        /// <returns>returns the LineSegmetns sorted in clockwise order all pointing in the clockwise direction</returns>
        public static List<LineSegment> SortIntoClockWiseSegments(this IList<LineSegment> segments)
        {
            segments.ValidateForPolygon();
            
            List<LineSegment> sortedSegments = new List<LineSegment>();
            Point minXPoint = null;

            // find the Point with the smallest X
            foreach (Point vertex in segments.GetAllPoints())
            {
                if (minXPoint == null || vertex.X < minXPoint.X)
                {
                    minXPoint = vertex;
                }
            }

            //find the line with the highest Y that contains the smallestXPoint
            LineSegment maxYLine = null;
            Point maxYPoint = null;

            foreach (LineSegment segment in segments)
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

            //create a shallow copy of the borders to find out which we have checked
            List<LineSegment> hasNotAdded = new List<LineSegment>(segments);
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

        /// <summary>
        /// takes the points from a list of line segments, rotates them, and turns them back into segments
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="rotationToApply">The Rotation(that stores the axis to rotate around and the angle to rotate) to apply to the list of LineSegments</param>
        /// <returns></returns>
        public static IList<LineSegment> RotatePointsAboutAnAxis(this IList<LineSegment> segments, Rotation rotationToApply)
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
                newList.Add(point.Rotate3D(rotationToApply));
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
        /// Shifts the List of LineSegments with the given shift
        /// </summary>
        /// <param name="passedLineSegments">The List of Line Segments to Shift</param>
        /// <param name="passedShift">the Shift to apply to the List of LineSegmnets</param>
        /// <returns>a new List of LineSegmnets that have been shifted with the given shift</returns>
        public static List<LineSegment> Shift(this IList<LineSegment> passedLineSegments, Shift passedShift)
        {
            List<LineSegment> shiftedSegments = new List<LineSegment>();

            foreach (var segment in passedLineSegments)
            {
                shiftedSegments.Add(segment.Shift(passedShift));
            }

            return shiftedSegments;
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed LineSegments</param>
        /// <returns></returns>
        public static bool AreAllCoplanar(this IList<LineSegment> passedLineList)
        {
            List<Point> vertices = passedLineList.GetAllPoints();
            Vector whatTheNormalShouldBe = null;
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = i + 1; j < vertices.Count(); j++)
                {
                    Vector vector1 = new Vector(vertices[i], vertices[j]);
                    for (int k = j + 1; k < vertices.Count(); k++)
                    {
                        Vector vector2 = new Vector(vertices[i], vertices[k]);
                        Vector normal = vector1.CrossProduct(vector2);
                        if (normal.Magnitude != new Distance())
                        {
                            //All the normal vectors have the origin as their base, because of how cross products are coded
                            if (whatTheNormalShouldBe == null)
                            {
                                whatTheNormalShouldBe = normal;
                            }
                            if (!normal.IsParallelTo(whatTheNormalShouldBe))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if all of the passed LineSegments are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed LineSegments</param>
        /// <returns></returns>
        public static bool AreAllParallel(this List<LineSegment> passedLineList)
        {
            for (int i = 0; i < passedLineList.Count - 1; i++)
            {
                if (!passedLineList[i].IsParallelTo(passedLineList[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds all points that are points adjacent to this point via one of these linesegments
        /// </summary>
        /// <param name="edgeList"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static List<Point> VerticesAdjacentTo(this List<LineSegment> edgeList, Point vertex)
        {
            List<Point> adjacentVertices = new List<Point>();
            foreach(LineSegment segment in edgeList)
            {
                if (vertex == segment.BasePoint)
                {
                    adjacentVertices.Add(segment.EndPoint);
                }
                else if (vertex == segment.EndPoint)
                {
                    adjacentVertices.Add(segment.BasePoint);
                }
            }
            return adjacentVertices;
        }

        /// <summary>
        /// given a list of edges, and a vertex, this returns all the edges that use the vertex as a base or end point
        /// NoteL this returns copies of those edges, and reorients them so that they use the vertex as a basepoint
        /// </summary>
        /// <param name="segmentList"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static List<LineSegment> SegmentsAdjacentTo(this List<LineSegment> segmentList, Point vertex)
        {
            List<LineSegment> segments = new List<LineSegment>();
            foreach(LineSegment segment in segmentList)
            {
                if (segment.BasePoint == vertex)
                {
                    segments.Add(new LineSegment(segment));
                }
                else if (segment.EndPoint == vertex)
                {
                    segments.Add(segment.Reverse());
                }
            }
            return segments;
        }

        /// <summary>
        /// Note this will return two copies of the segment itself as well
        /// </summary>
        /// <param name="segmentList"></param>
        /// <param name="lineSegment"></param>
        /// <returns></returns>
        public static List<LineSegment> SegmentsAdjacentTo(this List<LineSegment> segmentList, LineSegment lineSegment)
        {
            List<LineSegment> segments = new List<LineSegment>();
            segments.AddRange(segmentList.SegmentsAdjacentTo(lineSegment.BasePoint));
            segments.AddRange(segmentList.SegmentsAdjacentTo(lineSegment.EndPoint));
            
            Predicate<LineSegment> matching = (LineSegment s) => { return s == lineSegment; };
            segments.RemoveAll(matching);
            return segments;
        }

        public static List<LineSegment> ProjectAllOntoPlane(this IList<LineSegment> segmentList, Plane plane)
        {
            var results = new List<LineSegment>();
            foreach(var segment in segmentList)
            {
                var newSegment = segment.ProjectOntoPlane(plane);
                if (newSegment != null && !results.Contains(newSegment))
                {
                    results.Add(newSegment);
                }
            }
            return results;
        }

       
        public static Polygon ExteriorProfileFromSegments(this List<LineSegment> segments2D, Vector referenceNormal = null)
        {
            Point firstPoint = segments2D.GetAllPoints().OrderBy(p => p.X).ThenBy(p => p.Y).First();
            List<LineSegment> profileSegments = new List<LineSegment>();

            Point currentPoint = null;
            Vector referenceVector = new Vector(Point.MakePointWithInches(0, -1));
            while (currentPoint != firstPoint)
            {
                if (currentPoint == null)
                {
                    currentPoint = firstPoint;
                }
                var segmentsExtendingFromThisPoint = new List<LineSegment>();
                foreach (var segment in segments2D)
                {
                    if (segment.EndPoint == currentPoint)
                    {
                        segmentsExtendingFromThisPoint.Add(segment.Reverse());
                    }
                    else if (segment.BasePoint == currentPoint)
                    {
                        segmentsExtendingFromThisPoint.Add(segment);
                    }
                }
                var nextSegment = segmentsExtendingFromThisPoint.OrderBy(s => new Angle(s.SignedAngleBetween(referenceVector, referenceNormal))).First();
                profileSegments.Add(nextSegment);
                segments2D.Remove(nextSegment);
                referenceVector = nextSegment.Reverse();
                currentPoint = nextSegment.EndPoint;
            }
            return new Polygon(profileSegments, false);
        }
    }
}