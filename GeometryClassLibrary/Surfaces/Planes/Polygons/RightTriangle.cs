/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

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
            if (!((new Line(sides[0])).IsPerpendicularTo(new Line(sides[1])) || (new Line(sides[0])).IsPerpendicularTo(new Line(sides[2])) || (new Line(sides[1])).IsPerpendicularTo(new Line(sides[2]))))
            {
                throw new ArgumentException("No right angle in this triangle.");
            }
        }

        #endregion
    }
}
