using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryClassLibrary;
using System.Diagnostics;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class Solid: IComparable<Solid>
    {
        #region Fields and Properties

        /// <summary>
        /// The plane Regions that represent and make up this solid
        /// </summary>
        public virtual IList<IEdge> Edges { get; set; }

        /// <summary>
        /// The plane Regions that represent and make up this solid
        /// </summary>
        public virtual IList<PlaneRegion> Planes { get; set; }

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
        /// The volume of this solid / the space enclosed by this solid
        /// </summary>
        public virtual Volume Volume
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
        /// Finds the Center Point of the Solid
        /// </summary>
        /// <returns>A Point that reperesents the center of the solid</returns>
        public virtual Point CenterPoint()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the midline of the Solid that goes parallel to the longest side?
        /// </summary>
        /// <returns></returns>
        public virtual Line MidLine()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the given shift on the Solid
        /// </summary>
        /// <param name="passedShift">The Shift to preform</param>
        /// <returns>A new Solid that has been shifted</returns>
        public virtual Solid Shift(Shift passedShift)
        {
            throw new NotImplementedException();
        }

        public virtual List<Solid> Slice(Plane slicingPlane)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
