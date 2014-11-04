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
        /// <summary>
        /// Creates a new cube with with the given length of one of the sides
        /// </summary>
        /// <param name="passedSize"></param>
        public Cube(Dimension passedSize):base(passedSize, passedSize, passedSize){ }
    }
}
