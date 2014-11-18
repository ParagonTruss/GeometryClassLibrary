using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    [Serializable]
    public class NonPolyhedron : Solid
    {
        /// <summary>
        /// The list of planeRegions that make up and define the nonPolyhedron
        /// </summary>
        public List<PlaneRegion> BoundaryPlanes
        {
            get { return this.Planes as List<PlaneRegion>; }
            set { this.Planes = value; }
        }
    }
}
