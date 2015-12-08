using System;
using System.Collections.Generic;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class RegularPolygon : Polygon
    {

        #region Constructors

        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        protected RegularPolygon()
            : base() { }

        /// <summary>
        /// Creates a regular polygon centered at the origin in the XY-plane.
        /// </summary>
        public RegularPolygon(int numberOfSides, Distance sideLength)
            : base(Polygon.RegularPolygon(numberOfSides, sideLength)) { }

        
      

        #endregion

        #region Methods

       

        #endregion
    }
}
