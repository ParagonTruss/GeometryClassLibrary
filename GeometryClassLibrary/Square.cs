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
        public Square(Dimension passedSideLength)
            :base(4,passedSideLength) {  }
    }
}
