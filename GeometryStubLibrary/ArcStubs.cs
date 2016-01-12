using GeometryClassLibrary;

namespace GeometryStubLibrary
{
    public class TestQuarterArc : Arc
    {
        static Point basePoint = Point.Origin;
        static Point endPoint = Point.MakePointWithInches(3, 3, 4.24264);
        static Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));
        public TestQuarterArc()
            : base(basePoint, endPoint, directionAtStart)
        {

        }
    }

    public class TestHalfArc : Arc
    {
        static Point basePoint = Point.Origin;
        static Point endPoint = Point.MakePointWithInches(0, 0, 6);
        static Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

        public TestHalfArc()
            : base(basePoint, endPoint, directionAtStart)
        {

        }
    }

    public class TestThreeQuarterArc : Arc
    {
        static Point basePoint = Point.Origin;
        static Point endPoint = Point.MakePointWithInches(-3, -3, 4.24264);
        static Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));
        public TestThreeQuarterArc()
            : base(basePoint, endPoint, directionAtStart)
        {

        }
    }
}
