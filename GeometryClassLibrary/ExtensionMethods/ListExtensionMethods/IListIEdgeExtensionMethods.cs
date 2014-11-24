using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IListIEdgeExtensionMethods
    {
        /// <summary>
        /// Shifts each Edge in the list using the given shift
        /// </summary>
        /// <param name="passedEdges">The lIst of IEdges to Shift</param>
        /// <param name="passedShift">The Shift to use</param>
        /// <returns>a new list of IEdge that has been shifted</returns>
        public static List<IEdge> Shift(this List<IEdge> passedEdges, Shift passedShift)
        {
            List<IEdge> returnEdges = new List<IEdge>();

            foreach (var segment in passedEdges)
            {
                returnEdges.Add(segment.Shift(passedShift));
            }

            return returnEdges;
        }

        /// <summary>
        /// Gets a list of all the unique Points represented in this list of LineSegments (both end and base points)
        /// </summary>
        /// <param name="passedEdges">The List of LineSegments to get the points of</param>
        /// <returns>Returns a list of Points containing all the unique Points in the LineSegments List</returns>
        public static List<Point> GetAllPoints(this IList<IEdge> passedEdges)
        {
            List<Point> points = new List<Point>();

            //just cycle through each line and add the points to our list if they are not already there
            foreach (LineSegment line in passedEdges)
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
    }
}
