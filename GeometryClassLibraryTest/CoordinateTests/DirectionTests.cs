using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class DirectionTests
    {
        [Test]
        public void Direction_Equality_Tests()
        {
            Direction direction1 = Direction.Up;
            Direction direction2 = new Direction(new Angle(new Degree(), 33));
            Direction direction3 = Direction.Out;
            Direction direction4 = new Direction(new Angle(new Degree(), 33), Angle.RightAngle);
            Direction direction5 = null;

            (direction2 == direction4).Should().BeTrue();
           
            //Error margin of 0.1 degrees
            Point testPoint = Point.MakePointWithInches(0.002, 0.001, -1.5);
            Direction testErrorHandling = new Direction(testPoint);
            Direction expectedErrorDirection = Direction.Back;
            (testErrorHandling == expectedErrorDirection).Should().BeTrue();

            //now try the reverse Direction too
            Point testPoint0 = Point.MakePointWithInches(0.002, 0.001, 1.5);
            Direction testDirection1 = new Direction(testPoint0);
            Direction expected = Direction.Out;
            (testDirection1 == expected).Should().BeTrue();

            Point testPoint2 = Point.MakePointWithInches(0.003, 0.002, 1.5);
            Direction testDirection2 = new Direction(testPoint2);
            //Just barely more than a 0.1 degree angle between them
            (testDirection2 == expected).Should().BeFalse();
        }

        [Test]
        public void Direction_Inequalties()
        {
            Direction direction1 = Direction.Up;
            Direction direction2 = new Direction(new Angle(new Degree(), 33));
            Direction direction3 = Direction.Out;
            Direction direction4 = new Direction(new Angle(new Degree(), 33), Angle.RightAngle);
            Direction direction5 = null;


            direction1.Should().NotBe(direction2);
            direction1.Should().NotBe(direction3);
            direction1.Should().NotBe(direction4);

            direction2.Should().NotBe(direction3);
           

            direction3.Should().NotBe(direction4);
        }

        [Test]
        public void Direction_Null_Tests()
        {
            Direction direction1 = Direction.Up;
            Direction direction2 = new Direction(new Angle(new Degree(), 33));
            Direction direction3 = Direction.Out;
            Direction direction4 = new Direction(new Angle(new Degree(), 33), Angle.RightAngle);
            Direction direction5 = null;

            //now check null stuff
            (direction1 == null).Should().BeFalse();
            (direction5 == null).Should().BeTrue();
            (direction1 == direction5).Should().BeFalse();
            (direction5 == direction1).Should().BeFalse();

            (direction1 != null).Should().BeTrue();
            (direction5 != null).Should().BeFalse();
            (direction1 != direction5).Should().BeTrue();
            (direction5 != direction1).Should().BeTrue();
        }

    }
}
