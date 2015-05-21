﻿using GeometryClassLibrary;

namespace TestGeometryStubLibrary
{
        public class TestFirstLine : Line
        {
            public TestFirstLine()
            {
                Point testPoint1 = PointGenerator.MakePointWithInches(1, 1, 0);
                Direction testDirection1 = new Direction();

                this.BasePoint = testPoint1;
                this.Direction = testDirection1;

            }
    }
    //make into a new class
    public class TestSecondLine : Line
    {
        public TestSecondLine()
        {
            Point testPoint2 = PointGenerator.MakePointWithInches(4, 1, 0);
            Direction testDirection2 = new Direction();

            this.BasePoint = testPoint2;
            this.Direction = testDirection2;
        }
    }
}