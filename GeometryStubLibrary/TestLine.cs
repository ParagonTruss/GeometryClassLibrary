﻿using GeometryClassLibrary;

namespace GeometryStubLibrary
{
    public class TestLine : Line
    {
        public TestLine()
        {
            Point testPoint = PointGenerator.MakePointWithInches(1, 0, 2);
            Direction testDirection = new Direction();

            this.BasePoint = testPoint;
            this.Direction = testDirection;
        }
        

    }
    public class TestLine2 : Line
    {
        public TestLine2()
        {
            Point testPoint2 = PointGenerator.MakePointWithInches(2, 3, 1);
            Direction testDirection2 = new Direction();
            this.BasePoint = testPoint2;
            this.Direction = testDirection2;

        }

    }
}