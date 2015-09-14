using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class Triangle : Polygon
    {
        #region Constructors

        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        protected Triangle()
            : base() { }

        public Triangle(List<LineSegment> sides)
            : base(sides)
        {
            checkHasThreeSides(sides);
        }

        public Triangle(List<Point> corners)
            : base(corners)
        {
            checkHasThreeSides(corners);
        }

        #endregion

        #region Helper Methods

        private void checkHasThreeSides<T>(List<T> components)
        {
            if (components.Count != 3)
            {
                throw new ArgumentException("Must provide exactly three line segments to make a triangle.");
            }
        }

        #endregion
    }
}
