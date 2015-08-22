using System;
using System.Collections.Generic;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public abstract class Solid: IComparable<Solid>
    {
        #region Fields and Properties

        /// <summary>
        /// should return the list of all the edges that make up the solid
        /// </summary>
        public abstract IList<IEdge> Edges
        {
            get;
        }

        /// <summary>
        /// The plane Regions that represent and make up this solid
        /// </summary>
        public virtual IList<PlaneRegion> Faces { get; internal set; }

        public abstract List<Point> Vertices
        {
            get;
        }

        /// <summary>
        /// Accesses a point on the solid 
        /// </summary>
        public virtual Point PointOnSolid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Finds the Center Point of the Solid
        /// </summary>
        /// <returns>A Point that reperesents the center of the solid</returns>
        public virtual Point CenterPoint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The volume of this solid / the space enclosed by this solid
        /// </summary>
        public virtual Volume Volume
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual Point Centroid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion 

        #region Constructors

        /// <summary>
        /// Creates an empty solid object
        /// </summary>
        public Solid()
        {

        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other Solid
        /// NOTE: BASED SOLELY ON Volume.  MAY WANT TO CHANGE LATER
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Solid other)
        {
            if (this.Volume.Equals(other.Volume))
                return 0;
            else
                return this.Volume.CompareTo(other.Volume);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the given shift on the Solid
        /// </summary>
        /// <param name="passedShift">The Shift to preform</param>
        /// <returns>A new Solid that has been shifted</returns>
        public virtual Solid Shift(Shift passedShift)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a slive on the given solid, and returns two solids in a list
        /// with the larger piece first
        /// </summary>
        /// <param name="slicingPlane"></param>
        /// <returns></returns>
        public virtual List<Solid> Slice(Plane slicingPlane)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
