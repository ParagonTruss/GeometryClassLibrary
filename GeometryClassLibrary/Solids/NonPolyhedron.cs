using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public class NonPolyhedron : Solid
    {
        public override IList<IEdge> Edges
        {
            get { throw new NotImplementedException(); }
        }
    }
}
