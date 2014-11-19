using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    
    public class NonPolyhedron : Solid
    {
        /// <summary>
        /// The list of planeRegions that make up and define the nonPolyhedron
        /// </summary>
        public List<PlaneRegion> BoundaryPlanes
        {
            get { return this.PlaneRegions as List<PlaneRegion>; }
            set { this.PlaneRegions = value; }
        }
    }
}
