using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class Cube : RectangularPrism
    {
        /// <summary>
        /// Private null constructor for the use of data frameworks like Entity Framework and Json.NET
        /// </summary>
        private Cube() { }

        /// <summary>
        /// Creates a new cube with with the given length of one of the sides
        /// </summary>
        /// <param name="passedSize"></param>
        public Cube(Distance passedSize):base(passedSize, passedSize, passedSize){ }
    }
}
