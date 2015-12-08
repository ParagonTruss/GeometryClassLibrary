using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class EquilateralTriangle : RegularPolygon
    {
        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        private EquilateralTriangle()
            : base() { }

        /// <summary>
        /// Creates an equilateral triangle with the side the length of the passed in Distance
        /// </summary>
        /// <param name="passedSideLength"></param>
        public EquilateralTriangle(Distance passedSideLength)
            : base(3, passedSideLength) { }
    }
}
 