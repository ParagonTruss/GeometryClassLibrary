using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Polygons are shapes defined by three or more line segments that do not touch each other except at their endpoints. 
    /// Shapes defined by arcs or line segments that intersect each other at places other than their ends are non polygons. 
    /// Therefore a circle and an hourglass shape are non polygons.
    /// </summary>
    public class Polygon : PlaneRegion
    {
        #region Constructors
        public Polygon()
        {

        }
        #endregion
    }
}
