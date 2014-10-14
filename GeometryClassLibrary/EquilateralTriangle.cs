using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class EquilateralTriangle : RegularPolygon
    {
        public EquilateralTriangle(Dimension passedSideLength)
            :base(3,passedSideLength) {  }
    }
}
