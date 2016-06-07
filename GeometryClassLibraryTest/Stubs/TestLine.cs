using GeometryClassLibrary;

namespace GeometryClassLibraryTest.Stubs
{
    public class TestLine : Line
    {
        public TestLine()
        {
            Point testPoint = Point.MakePointWithInches(1, 0, 2);
            Direction testDirection = Direction.Right;

            this.BasePoint = testPoint;
            this.Direction = testDirection;
        }
        

    }
    public class TestLine2 : Line
    {
        public TestLine2()
        {
            Point testPoint2 = Point.MakePointWithInches(2, 3, 1);
            Direction testDirection2 = Direction.Right;
            this.BasePoint = testPoint2;
            this.Direction = testDirection2;

        }

    }
}
