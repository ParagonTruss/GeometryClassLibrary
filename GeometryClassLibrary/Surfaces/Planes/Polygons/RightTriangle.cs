using System;
using System.Collections.Generic;

namespace GeometryClassLibrary
{
    public class RightTriangle : Triangle
    {
        #region Constructors

        /// <summary>
        /// Null constructor for the benefit of Entity Framework
        /// </summary>
        private RightTriangle()
            : base() { }

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
                LineSegment hypotenuse = LineSegments[0];
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

                LineSegment longLeg = null;
                
                if (legs[0].Length > legs[1].Length)
                {
                    longLeg = legs[0];
                }
                else
                {
                    longLeg = legs[1];
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
