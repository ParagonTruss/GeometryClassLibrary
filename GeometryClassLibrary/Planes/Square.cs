using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class Square : RegularPolygon
    {
        /// <summary>
        /// creates a new square in the XY plane with the given dimesnion as teh length and height
        /// </summary>
        /// <param name="passedSideLength"></param>
        public Square(Distance passedSideLength)
            :base(4,passedSideLength) {  }
    }
}
