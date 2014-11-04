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
        #region Properties and Fields

        public virtual List<IEdge> PlaneEdges { get; set; }

        #endregion

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

        #region Methods

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

        public override Polyhedron Extrude(Dimension dimension)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
