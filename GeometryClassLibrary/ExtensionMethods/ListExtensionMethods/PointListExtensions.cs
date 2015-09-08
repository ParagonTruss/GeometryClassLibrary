﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class PointListExtensions
    {
        public static Distance GreatestXDistance(this List<Point> points)
        {
            var min = points.Min(p => p.X);
            var max = points.Max(p => p.X);
            return max - min;
        }
        public static Distance GreatestYDistance(this List<Point> points)
        {
            var min = points.Min(p => p.Y);
            var max = points.Max(p => p.Y);
            return max - min;
        }
        public static Distance GreatestZDistance(this List<Point> points)
        {
            var min = points.Min(p => p.Z);
            var max = points.Max(p => p.Z);
            return max - min;
        }
        public static List<Point> Shift(this List<Point> pointList, Shift passedShift)
        {
            List<Point> shiftedPoints = new List<Point>();
            foreach(Point point in pointList)
            {
                shiftedPoints.Add(point.Shift(passedShift));
            }
            return shiftedPoints;
        }

        public static List<Point> ProjectAllOntoPlane(this IList<Point> pointList, Plane plane)
        {
            var results = new List<Point>();
            foreach (var point in pointList)
            {
                var pointInPlane = point.ProjectOntoPlane(plane);
                if (pointInPlane != null && !results.Contains(pointInPlane))
                {
                    results.Add(pointInPlane);
                }
            }
            return results;
        }

        /// <summary>
        /// Returns the two points that are furthest from each other in the list.
        /// </summary>
        public static List<Point> FurthestPoints(this IEnumerable<Point> pointList)
        {
            var points = pointList.ToList();
            if (points.Count < 2)
            {
                throw new Exception("Not enough points in the list");
            }
            Dictionary<List<Point>, Distance> distances = new Dictionary<List<Point>, Distance>();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    distances.Add(new List<Point>() { points[i], points[j] }, points[i].DistanceTo(points[j]));
                }
            }
            return distances.OrderBy(p => p.Value).Last().Key;
        }

        public static List<Point> Rotate(this IEnumerable<Point> pointList, Rotation rotation)
        {
            var results = new List<Point>();
            foreach (var point in pointList)
            {
                results.Add(point.Rotate3D(rotation));
            }
            return results;
        }
        /// <summary>
        /// determines if the points in the list, all lie on the same side of the dividing plane.
        /// Points on the plane are disregarded
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static bool AllPointsAreOnTheSameSideOf(this List<Point> pointList, Plane plane)
        {
            int index = 0;
            //find a reference point
            for (int i = 0; i < pointList.Count; i++)
		    {
			    if (!plane.Contains(pointList[i]))
                {
                    index = i;
                    break;
                }
		    }
            Point referencePoint = pointList[index];   
            
            //Now check the remaining points
            for (int i = index + 1; i < pointList.Count; i++)
            {
                if (!plane.Contains(pointList[i]) && !plane.PointIsOnSameSideAs(pointList[i], referencePoint))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Makes line segments that connect the given points in the order the appear in the list
        /// </summary>
        /// <param name="points">The points to make the segments out of</param>
        /// <returns>A new List of LineSegment that connect the points</returns>
        public static List<LineSegment> MakeIntoLineSegmentsThatMeet(this List<Point> points)
        {
            List<LineSegment> segments = new List<LineSegment>() { new LineSegment(points.Last(), points[0]) };

            LineSegment newSegment;
            for (int k = 0; k < points.Count() - 1; k++)
            {
                newSegment = new LineSegment(points[k], points[k + 1]);

                segments.Add(newSegment);
            }

            return segments;
        }

        public static Point CenterPoint(this List<Point> points)
        {
            Point sum = Point.Origin;
            foreach(Point point in points)
            {
                sum += point;
            }
            return ((new Vector(sum)) / points.Count).EndPoint;
        }

        /// <summary>
        /// Creates the largest convex polygon whose vertices are from this list
        /// Uses the Graham Scan: http://en.wikipedia.org/wiki/Graham_scan
        /// the suppressed boolean is for cases where you know all your points can be used
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static Polygon ConvexHull(this List<Point> passedPointList, bool allPointsShouldBeVertices = false)
        {
            if (passedPointList.Count < 3)
            {
                return null;
            }
            //First clone our list of points
            List<Point> pointList = new List<Point>(passedPointList);

            //Now sort the points by Y component (Z then X for tiebreakers)
            //and take the initial point out.
            pointList.Sort((new CompareInOrderYZX()));
            Point initial = pointList[0];
            pointList.Remove(initial);

            //the each point to the initial point forms a segment which has some angle against the x axis
            //sort by those angles
            List<Point> pointsInOrder = pointList._sortByAngles(initial);

            //we can create the polygon now if we know that none of the points would make this concave
            if (!allPointsShouldBeVertices)
            {
                //Now its time  to cut the chaff
                _removeInteriorPoints(initial, pointsInOrder);
            }
            //The initial point will be added to the end of the list, but since polygons are cyclic everything will still connect right
            pointsInOrder.Add(initial);
            return new Polygon(pointsInOrder, false);
        }

        private static List<Point> _sortByAngles(this List<Point> pointList, Point initial)
        {
            Dictionary<Angle, Point> myDictionary = new Dictionary<Angle, Point>();
            Vector referenceVector;
            if (pointList._xCompIsConstant())
            {
                referenceVector = Direction.Out.UnitVector(DistanceType.Inch);
            }
            else
            {
                referenceVector = Direction.Right.UnitVector(DistanceType.Inch);
            }
            var orderedList = pointList.OrderBy(v => referenceVector.AngleBetween(new Vector(initial, v))).ToList();
            return orderedList;
        }

        private static bool _xCompIsConstant(this List<Point> pointList)
        {
            Distance xComp = pointList[0].X;
            for (int i = 1; i < pointList.Count; i++)
            {
                if (pointList[i].X != xComp)
                { 
                    return false;
                }
            }
            return true;
        }

        private static void _removeInteriorPoints(Point initial, List<Point> pointsInOrder)
        {
            //First we compute what the normal actually is
            Vector last = new Vector(pointsInOrder[pointsInOrder.Count - 1], initial);
            Vector first = new Vector(initial, pointsInOrder[0]);
            Vector normal = last.CrossProduct(first);
            
            //Now we use a nested loop to allow us to actually edit the list we're looping through
            //The top level allows us to loop atleast as many times as we need to edit.
            //The inner loop restarts whenever we need to remove a point, but we keep track of where are index was
            //the flag tracks when we exited the inner loop normally or because we needed to remove an item
            //if we exited normally we're done
            int numberOfIterations = pointsInOrder.Count;
            int startingIndex = -1;
            for (int j = 0; j < numberOfIterations; j++)
            {
                bool flag = false;
                Point point1, point3, point2 = null;
                for (int i = startingIndex; i + 1 < pointsInOrder.Count; i++)
                {
                    if (i == -1)
                    {
                        point1 = initial;
                    }
                    point1 = pointsInOrder[i];
                    point2 = pointsInOrder[i + 1];
                    point3 = pointsInOrder[i + 2];

                    Vector vector1 = new Vector(point1, point2);
                    Vector vector2 = new Vector(point2, point3);
                    Vector shouldBeNormal = vector1.CrossProduct(vector2);

                    if (shouldBeNormal.Magnitude == new Distance() || !shouldBeNormal.HasSameDirectionAs(normal))
                    {
                        flag = true;
                        startingIndex = i;
                        break;
                    }
                }
                if (flag)
                {
                    pointsInOrder.Remove(point2);
                }
                else
                {
                    break;
                }
            }
        }

       
    }
}
