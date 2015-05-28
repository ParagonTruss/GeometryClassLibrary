using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;
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
                    
                    foreach(var edge in toReturn)
                    {
                        if (edge.DoesIntersect(newLine))
                        {
                            if (!(edge.Intersection(newLine) == newLine.BasePoint) && !(edge.Intersection(newLine) == newLine.EndPoint))
                            {
                                throw new Exception("Edges of a Polygon should not self intersect!");
                            }
                        }
                    }

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
            
            //Find the point with the lowest y value
            Point initial = pointList[0];
            for (int i = 1; i < pointList.Count; i++)
            {
                if (pointList[i].Y < initial.Y)
                {
                    initial = pointList[i];
                }
            }
            pointList.Remove(initial);

            //Now we sort the points by the angle the initial point to them makes with the x axis
            Dictionary<Angle, Point> myDictionary = new Dictionary<Angle, Point>();
            Vector rightVector = Direction.Right.UnitVector(DistanceType.Inch);
            foreach(Point vertex in pointList)
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
            myDictionary.Keys.ToList().Sort();

            //we can create the polygon now if we know that none of the points would make this concave
            if (allPointsShouldBeVertices)
            {
                List<Point> pointsToUse = myDictionary.Values.ToList();
                pointsToUse.Add(initial);
                return new Polygon(pointsToUse, false);
            }

            throw new NotImplementedException(); //ToDo: finish method

        }
    }
}
