using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class Square : RegularPolygon
    {
        /// <summary>
        /// creates a new square in the XY plane with the given dimesnion as the length and height
        /// </summary>
        /// <param name="passedSideLength"></param>
        public Square(Distance passedSideLength)
            :base(4,passedSideLength) {  }
    }
}
