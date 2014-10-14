using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class Cube : RectangularPrism
    {
        public Cube(Dimension passedSize):base(passedSize, passedSize, passedSize){ }
    }
}
