using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryClassLibrary
{
    public static class EdgeListExtensionMethods
    {
        /// <summary>
        /// Shifts each Edge in the list using the given shift
        /// </summary>
        public static List<IEdge> Shift(this IEnumerable<IEdge> passedEdges, Shift passedShift)
        {
            return passedEdges.Select(e => e.Shift(passedShift)).ToList();
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

        public static List<IEdge> FixEdgeOrientation(this List<IEdge> edges)
        {
            if (edges.Count < 2)
            {
                throw new Exception("too few edges");
            }
            var copy = edges.ToList();
           
            var current = copy.First();
            copy.RemoveAt(0);
            var sorted = new List<IEdge>();
            sorted.Add(current);
            for (int i = 1; i < edges.Count; i++)
            {
                var next = copy.FirstOrDefault(e => current.EndPoint == e.BasePoint);
                if (next == null)
                {
                    next = copy.FirstOrDefault(e => current.EndPoint == e.EndPoint);
                    if (next == null)
                    {
                        throw new Exception("The passed list of edges do not meet end to end.");
                    }
                    else
                    {
                       next = next.Reverse();
                    }
                }
                sorted.Add(next);
                copy.Remove(next);
                current = next;              
            }
            return sorted;
        }
    }
}
