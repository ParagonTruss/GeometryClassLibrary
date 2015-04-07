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
        /// Shifts this non polygon as a more generic plane region
        /// </summary>
        /// <param name="passedShift">The shift to apply to this nonPolygon</param>
        /// <returns><Returns a  new nonPolygon as a PlaneRegion that has been shifted/returns>
        public override PlaneRegion ShiftAsPlaneRegion(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Shifts the NonPolygon with the given shift
        /// </summary>
        /// <param name="passedShift">The shift to apply to the nonPolygon</param>
        /// <returns>A new nonPolygon that has been shifted</returns>
        public NonPolygon Shift(Shift passedShift)
        {
            return new NonPolygon(this.Edges.Shift(passedShift));
        }

        /// <summary>
        /// Rotate the NonPolygon as a more generic PlaneRegion 
        /// </summary>
        /// <param name="passedRotation">The rotation to apply to the nonPolygon</param>
        /// <returns>A New nonPolygon as a PlaneRegion that has been rotated<returns>
        public override PlaneRegion RotateAsPlaneRegion(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        /// <summary>
        /// Rotates the NonPolygon using the given rotation
        /// </summary>
        /// <param name="passedRotation">The rotation to apply to the NonPolygon</param>
        /// <returns>A new NonPolgon that has been rotated with the given rotation</returns>
        public new NonPolygon Rotate(Rotation passedRotation)
        {
            List<IEdge> newEdges = new List<IEdge>();
            foreach (var edge in this.Edges)
            {
                newEdges.Add(edge.Rotate(passedRotation));
            }

            return new NonPolygon(newEdges);
        }

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
        /// Extrudes this NonPolygon into a 3 Distanceal prism
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns>Returns this NonPolygon extruded into 3D</returns>
        public override Solid Extrude(Vector directionVector)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
