﻿using GeometryClassLibrary;
using UnitClassLibrary;

namespace GeometryStubLibrary
{
    public class TestLineSegment : LineSegment
    {
        public TestLineSegment()
        {
            Point point = PointGenerator.MakePointWithInches(0, 5);
            Distance distance = new Distance(DistanceType.Mile, 50);

            this.BasePoint = point;

            this.Length = distance; 

        }
    }
}