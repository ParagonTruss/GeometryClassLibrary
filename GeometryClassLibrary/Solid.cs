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
        /// Accesses a point on the solid 
        /// </summary>
        public virtual Point PointOnSolid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual Volume Volume
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion 

        #region Constructors
        public Solid()
        {

        }
        #endregion

        #region Methods
        public virtual Point CenterPoint()
        {
            throw new NotImplementedException();
        }

        public virtual Line MidLine()
        {
            throw new NotImplementedException();
        }

        public Polyhedron Shift(Shift passedShift)
        {
            throw new NotImplementedException();
        }
        #endregion

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
    }
}
