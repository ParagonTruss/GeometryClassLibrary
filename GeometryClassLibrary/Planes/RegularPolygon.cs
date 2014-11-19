using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class RegularPolygon : Polygon
    {
        #region Constructors

        /// <summary>
        /// Creates a regular polygon from the given line segments if they are are equal and have the same angles between
        /// neighboring segments
        /// </summary>
        /// <param name="passedLineSegments"></param>
        public RegularPolygon(List<LineSegment> passedLineSegments)
        {
            //make sure the linesegments are equidistant and all angles equal
            if (AllSidesAreEqualandAllAnglesBetweenLinesAreTheSame(passedLineSegments))
            {
                this.PlaneBoundaries = passedLineSegments;
            }
        }

        /// <summary>
        /// Creates a regular polygon in the XY-plane with the given number of sides where each side is the given length and the 
        /// polygon is rotated by the given angle
        /// </summary>
        /// <param name="passedNumberOfSides"></param>
        /// <param name="passedSideLength"></param>
        /// <param name="passedRotationAngle"></param>
        public RegularPolygon(int passedNumberOfSides, Distance passedSideLength, Angle passedRotationAngle = null)
        {
            if (passedNumberOfSides < 3)
            {
                throw new ArgumentException("A polygon must have more than 2 sides.");
            }

            if (passedRotationAngle == null)
            {
                passedRotationAngle = new Angle();
            }

            List<Point> points = new List<Point>();
            Angle step = new Angle(AngleType.Degree, 360.0 / passedNumberOfSides);

            for (Angle i = passedRotationAngle; i < passedRotationAngle + new Angle(AngleType.Degree, 360.0); i += step) //go in a full circle
            {
                points.Add(DegreesToXY(passedRotationAngle, passedSideLength, PointGenerator.Origin)); //code snippet from above
                passedRotationAngle += step;
            }

            points.MakeIntoLineSegmentsThatMeet();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates a point that is at an angle from the passedPoint, in XY (0 is to the right)
        /// </summary>
        private Point DegreesToXY(Angle passedAngle, Distance distance, Point passedPoint)
        {
            Point newPoint = new Point(
                new Distance(DistanceType.Inch, Math.Cos(passedAngle.Radians) * distance.Inches + passedPoint.X.Inches),
            new Distance(DistanceType.Inch, Math.Sin(passedAngle.Negate().Radians) * distance.Inches + passedPoint.Y.Inches));

            return newPoint;
        }

        public bool AllSidesAreEqualandAllAnglesBetweenLinesAreTheSame(List<LineSegment> passedLineSegments)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
