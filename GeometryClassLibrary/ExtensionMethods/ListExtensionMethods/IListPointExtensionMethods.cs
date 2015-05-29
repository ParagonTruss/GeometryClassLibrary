using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;
using GeometryClassLibrary.ComparisonMethods;
//

namespace GeometryClassLibrary
{
    public static class IListPointExtensionMethods
    {
        /// <summary>
        /// Makes line segments that connect the given points in the order the appear in the list
        /// </summary>
        /// <param name="points">The points to make the segments out of</param>
        /// <returns>A new List of LineSegment that connect the points</returns>
        public static List<LineSegment> MakeIntoLineSegmentsThatMeet(this List<Point> points)
        {
            List<LineSegment> toReturn = new List<LineSegment>();

            for (int k = 0; k < points.Count(); k++)
            {
                if (k != points.Count() - 1)
                {
                    LineSegment newLine = new LineSegment(points[k], points[k + 1]);

                    toReturn.Add(newLine);
                }
                else
                {
                    toReturn.Add(new LineSegment(points[k], points[0]));
                }
            }

            return toReturn;
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
            //First clone our list of points
            List<Point> pointList = new List<Point>(passedPointList);

            //Now sort the points by Y component (Z then X for tiebreakers)
            //and take the initial point out.
            pointList.Sort((new CompareInOrderYZX()));
            Point initial = pointList[0];
            pointList.Remove(initial);

            //the each point to the initial point forms a segment which has some angle against the x axis
            //sort by those angles
            List<Point> pointsInOrder = _sortByAngles(pointList, initial);

            //we can create the polygon now if we know that none of the points would make this concave
            if (!allPointsShouldBeVertices)
            {
                //Now its time  to cut the chaff
                _removeInteriorPoints(initial, pointsInOrder);
            }
            //The initial point will added to the end of the list, but since polygons are cyclic everything will still connect right
            pointsInOrder.Add(initial);
            return new Polygon(pointsInOrder, false);
        }

        private static List<Point> _sortByAngles(List<Point> pointList, Point initial)
        {
            Dictionary<Angle, Point> myDictionary = new Dictionary<Angle, Point>();
            Vector rightVector = Direction.Right.UnitVector(DistanceType.Inch);
            foreach (Point vertex in pointList)
            {
                Vector currentVector = new Vector(initial, vertex);
                Angle currentAngle = rightVector.AngleBetween(currentVector);

                //This is to catch error margins
                //We don't want a "359 degree" angle to accidentally be sorted as largest.
                //angles larger than 180 shouldn't happen, so this sets those just shy of zero, to zero
                if (currentAngle == new Angle())
                {
                    currentAngle = new Angle();
                }
                try
                {
                    myDictionary.Add(currentAngle, vertex);
                }
                catch { }
            }
            return (from entry in myDictionary orderby entry.Key ascending select entry)
            .ToDictionary(x => x.Key, x => x.Value).Values.ToList(); 
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
