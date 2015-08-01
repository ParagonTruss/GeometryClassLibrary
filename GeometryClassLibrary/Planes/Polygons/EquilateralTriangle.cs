using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class EquilateralTriangle : RegularPolygon
    {
        /// <summary>
        /// Creates an equilateral triangle with the side the length of the passed in Distance
        /// </summary>
        /// <param name="passedSideLength"></param>
        public EquilateralTriangle(Distance passedSideLength)
            : base(3, passedSideLength) {  }
    }
}
 