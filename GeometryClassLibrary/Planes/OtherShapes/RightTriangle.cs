using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class RightTriangle : Triangle
    {
        #region Constructors

        public RightTriangle(List<LineSegment> sides) : base(sides)
        {
            checkHasRightAngle(sides);
        }

        public RightTriangle(List<Point> corners)
            : base(corners)
        {
            checkHasRightAngle(this.LineSegments);
        }

        #endregion

        #region Properties

        public LineSegment Hypotenuse
        {
            get
            {
                LineSegment hypotenuse = new LineSegment();
                foreach (LineSegment side in this.LineSegments)
                {
                    if (side.Length > hypotenuse.Length)
                    {
                        hypotenuse = side;
                    }
                }

                return hypotenuse;
            }
        }

        public LineSegment ShortLeg
        {
            get
            {
                LineSegment shortLeg = LineSegments[0];
                foreach (LineSegment side in this.LineSegments)
                {
                    if (side.Length < shortLeg.Length)
                    {
                        shortLeg = side;
                    }
                }

                return shortLeg;
            }
        }

        public LineSegment LongLeg
        {
            get
            {
                List<LineSegment> legs = this.LineSegments;
                legs.Remove(Hypotenuse);
                
                LineSegment longLeg = new LineSegment();
                foreach (LineSegment side in legs)
                {
                    if (side.Length > longLeg.Length)
                    {
                        longLeg = side;
                    }
                }

                return longLeg;
            }
        }

        #endregion

        #region Helper Methods

        private void checkHasRightAngle(List<LineSegment> sides)
        {
            if (!(((Line)(sides[0])).IsPerpendicularTo((Line)(sides[1])) || ((Line)(sides[0])).IsPerpendicularTo((Line)(sides[2])) || ((Line)(sides[1])).IsPerpendicularTo((Line)(sides[2]))))
            {
                throw new ArgumentException("No right angle in this triangle.");
            }
        }

        #endregion
    }
}
