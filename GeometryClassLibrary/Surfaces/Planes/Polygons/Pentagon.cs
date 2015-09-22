using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Pentagon : RegularPolygon
    {
        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        private Pentagon()
            : base() { }

        /// <summary>
        /// Creates a regular Pentagon where all sides are the passed length
        /// </summary>
        /// <param name="passedSideLength"></param>
        public Pentagon(Distance passedSideLength)
            : base(5, passedSideLength) { }
    }
}
