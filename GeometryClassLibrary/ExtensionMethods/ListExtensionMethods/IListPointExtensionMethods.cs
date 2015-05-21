using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
