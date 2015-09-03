using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public static class IEdgeListExtensionMethods
    {
        /// <summary>
        /// Shifts each Edge in the list using the given shift
        /// </summary>
        /// <param name="passedEdges">The lIst of IEdges to Shift</param>
        /// <param name="passedShift">The Shift to use</param>
        /// <returns>a new list of IEdge that has been shifted</returns>
        public static List<IEdge> Shift(this IEnumerable<IEdge> passedEdges, Shift passedShift)
        {
            List<IEdge> returnEdges = new List<IEdge>();

            foreach (var segment in passedEdges)
            {
                returnEdges.Add(segment.Shift(passedShift));
            }

            return returnEdges;
        }

        /// <summary>
        /// Gets a list of all the points in this collection of edges.
        /// </summary>
        public static List<Point> GetAllPoints(this IList<IEdge> passedEdges)
        {
            List<Point> points = new List<Point>();

            //just cycle through each line and add the points to our list if they are not already there
            foreach (IEdge edge in passedEdges)
            {
                if (!points.Contains(edge.BasePoint))
                {
                    points.Add(edge.BasePoint);
                }
                if (!points.Contains(edge.EndPoint))
                {
                    points.Add(edge.EndPoint);
                }
            }

            return points;
        }
    }
}
