using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [Serializable]
    public class Solid
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
    }
}
