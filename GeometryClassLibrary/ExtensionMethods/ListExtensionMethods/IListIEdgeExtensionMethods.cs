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
    }
}
