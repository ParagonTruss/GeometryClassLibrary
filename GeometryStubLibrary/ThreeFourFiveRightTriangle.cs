﻿using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryStubLibrary
{
    public class ThreeFourFiveRightTriangle : Triangle
    {
        public ThreeFourFiveRightTriangle(List<LineSegment> sides)
            : base(sides)
        {
            // Build right triangle with line segments
            LineSegment rt_sideOne = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(3, 0));
            LineSegment rt_sideTwo = new LineSegment(PointGenerator.MakePointWithInches(3, 0), PointGenerator.MakePointWithInches(3, 4));
            LineSegment rt_sideThree = new LineSegment(PointGenerator.MakePointWithInches(3, 4), PointGenerator.MakePointWithInches(0, 0));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);
        }


    }
}
