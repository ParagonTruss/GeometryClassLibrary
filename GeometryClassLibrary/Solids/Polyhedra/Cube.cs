using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class Cube : RectangularPrism
    {
        /// <summary>
        /// Creates a new cube with with the given length of one of the sides
        /// </summary>
        /// <param name="passedSize"></param>
        public Cube(Distance passedSize):base(passedSize, passedSize, passedSize){ }
    }
}
