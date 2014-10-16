using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class RegularPolygon : Polygon
    {
        public RegularPolygon(List<LineSegment> passedLineSegments)
        {
            //make sure the linesegments are equidistant and all angles equal
            if (AllSidesAreEqualandAllAnglesBetweenLinesAreTheSame(passedLineSegments))
            {
                this.PlaneBoundaries = passedLineSegments;
            }
        }

        public RegularPolygon(int passedNumberOfSides, Dimension passedSideLength, Angle passedRotationAngle = null)
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



        /// <summary>
        /// Calculates a point that is at an angle from the passedPoint, in XY (0 is to the right)
        /// </summary>
        private Point DegreesToXY(Angle passedAngle, Dimension distance, Point passedPoint)
        {
            Point newPoint = new Point(
                new Dimension(DimensionType.Inch, Math.Cos(passedAngle.Radians) * distance.Inches + passedPoint.X.Inches),
            new Dimension(DimensionType.Inch, Math.Sin(passedAngle.Negate().Radians) * distance.Inches + passedPoint.Y.Inches));

            return newPoint;
        }

        public bool AllSidesAreEqualandAllAnglesBetweenLinesAreTheSame(List<LineSegment> passedLineSegments)
        {
            throw new NotImplementedException();
        }
    }
}
