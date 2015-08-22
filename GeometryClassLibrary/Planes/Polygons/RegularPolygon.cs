using System;
using System.Collections.Generic;
using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class RegularPolygon : Polygon
    {

        #region Constructors
        /// <summary>
        /// Creates a regular polygon centered at the origin in the XY-plane.
        /// </summary>
        public RegularPolygon(int numberOfSides, Distance sideLength)
            : base(_createRegularPolygonPoints(numberOfSides, sideLength)) { }

        private static List<Point> _createRegularPolygonPoints(int numberOfSides, Distance sideLength)
        {
            if (numberOfSides < 3)
            {
                throw new ArgumentException("A polygon must have more than 2 sides.");
            }
            
            AngularDistance step = Angle.Degree * 360.0 / numberOfSides;
            AngularDistance otherAngle = (Angle.Degree*180 - step) / 2;

            //Law of Sines
            Distance length = sideLength * Math.Sin(otherAngle.Radians)/Math.Sin((step.Radians));

            Point firstPoint = new Point(length, new Distance());
            List<Point> points = new List<Point>(){ firstPoint };

            for (int i = 1; i < numberOfSides; i++)
            {
                points.Add(firstPoint.Rotate2D(new Point(), step*i)); //code snippet from above
            }

            return points;
        }

      

        #endregion

        #region Methods

       

        #endregion
    }
}
