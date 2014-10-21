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
    public class NonPolygon : PlaneRegion
    {
        public virtual List<IEdge> PlaneEdges { get; set; } 

        #region Constructors
        public NonPolygon()
        {

        }
        
        /// <summary>
        /// Defines a nonPolygon usuing the boundaries
        /// NOTE: Should check if they are coplanar and form a closed region first!
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public NonPolygon(List<IEdge> passedBoundaries)
        {
            this.Edges = passedBoundaries;
        }

        #endregion

        /*public override List<IEdge> PlaneBoundaries
        {
            get { return _planeBoundaries; }
            set { _planeBoundaries = value; }
        }
        private List<IEdge> _planeBoundaries;*/

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
