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

        /// <summary>
        /// The list of Edges that make up and define the non-polygon
        /// </summary>
        public virtual List<IEdge> NonPolygonBoundaries { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty nonPolygon
        /// </summary>
        public NonPolygon()
        {
            this.Edges = new List<IEdge>();
        }

        /// <summary>
        /// Defines a nonPolygon usuing the boundaries
        /// NOTE: Should check if they are coplanar and form a closed region first!
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public NonPolygon(List<IEdge> passedBoundaries)
        {
            this.NonPolygonBoundaries = new List<IEdge>();
            foreach (IEdge edge in passedBoundaries)
            {
                this.NonPolygonBoundaries.Add(edge.Copy());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// returns the area enclosed by this nonPolygon
        /// </summary>
        public override Area Area
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Finds the smalles rectangle that can contain this nonPolygon that is coplanar with it
        /// </summary>
        /// <returns>Returns a Polygon that contains this NonPolygon</returns>
        public override Polygon SmallestRectangleThatCanSurroundThisShape()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the centriod (geometric center) of this nonPolygon
        /// </summary>
        /// <returns>Returns a Point that is the centroid of this NonPolygon</returns>
        public override Point Centroid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Extrudes this NonPolygon into a 3 dimensional prism
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns>Returns this NonPolygon extruded into 3D</returns>
        public override Solid Extrude(Vector directionVector)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
