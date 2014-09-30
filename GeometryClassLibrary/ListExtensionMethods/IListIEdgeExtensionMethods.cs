using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IListIEdgeExtensionMethods
    {
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
