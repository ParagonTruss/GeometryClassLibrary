using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Non polygons are shapes defined by finite lines. Unlike in polygons, these lines can intersect and be arcs.
    /// </summary>
    [Serializable]
    public class NonPolygon : PlaneRegion<IEdge>
    {
        #region Constructors
        public NonPolygon()
        {

        }
        #endregion

        public override List<IEdge> PlaneBoundaries
        {
            get { return _planeBoundaries; }
            set { _planeBoundaries = value; }
        }
        private List<IEdge> _planeBoundaries;

        public override Area Area
        {
            get { throw new NotImplementedException(); }
        }

        public override Polygon SmallestRectangleThatCanSurroundThisShape()
        {
            throw new NotImplementedException();
        }

        public override Point Centroid()
        {
            throw new NotImplementedException();
        }

        public override Solid Extrude(Dimension dimension)
        {
            throw new NotImplementedException();
        }
    }
}
