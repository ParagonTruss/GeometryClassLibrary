using GeometryClassLibrary;

namespace GeometryClassLibraryTest.Stubs
{
        public class TestFirstLine : Line
        {
            public TestFirstLine()
            {
                Point testPoint1 = Point.MakePointWithInches(1, 1, 0);
                Direction testDirection1 = Direction.Right;

                this.BasePoint = testPoint1;
                this.Direction = testDirection1;

            }
    }
    //make into a new class
    public class TestSecondLine : Line
    {
        public TestSecondLine()
        {
            Point testPoint2 = Point.MakePointWithInches(4, 1, 0);
            Direction testDirection2 = Direction.Right;

            this.BasePoint = testPoint2;
            this.Direction = testDirection2;
        }
    }
}
