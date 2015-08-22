using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class NonPolyhedron : Solid
    {
        public override IList<IEdge> Edges
        {
            get { throw new NotImplementedException(); }
        }

        public override List<Point> Vertices
        {
            get { throw new NotImplementedException(); }
        }
    }
}
