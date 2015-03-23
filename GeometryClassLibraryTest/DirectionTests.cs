using NUnit.Framework;
using UnitClassLibrary;
using GeometryClassLibrary;
using FluentAssertions;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class DirectionTests
    {
        [Test]
        public void Direction_ConstuctorsAndEquality()
        {
            Direction direction1 = new Direction();
            Direction direction2 = new Direction(new Angle(AngleType.Degree, 33));
            Direction direction3 = new Direction(new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 0));
            Direction direction4 = new Direction(new Angle(AngleType.Degree, 33), new Angle(AngleType.Degree, 90));
            Direction direction5 = null;

            direction1.Should().NotBe(direction2);
            direction1.Should().NotBe(direction3);
            direction1.Should().NotBe(direction4);

            direction2.Should().NotBe(direction3);
            (direction2 == direction4).Should().BeTrue();
            direction2.Should().Be(direction4);

            direction3.Should().NotBe(direction4);

            //now check null stuff
            (direction1 == null).Should().BeFalse();
            (direction5 == null).Should().BeTrue();
            (direction1 == direction5).Should().BeFalse();
            (direction5 == direction1).Should().BeFalse();

            (direction1 != null).Should().BeTrue();
            (direction5 != null).Should().BeFalse();
            (direction1 != direction5).Should().BeTrue();
            (direction5 != direction1).Should().BeTrue();

            Point testPoint = PointGenerator.MakePointWithInches(0.006, 0.003, -1.4999);
            Direction testErrorHandling = new Direction(testPoint);
            Direction expectedErrorDirection = new Direction(new Angle(), new Angle(AngleType.Degree, 180));
            (testErrorHandling == expectedErrorDirection).Should().BeTrue();

            //now try the 0 Direction too
            Point testPoint0 = PointGenerator.MakePointWithInches(0.006, 0.003, 1.4999);
            Direction testErrorHandling0 = new Direction(testPoint0);
            Direction expectedErrorDirection0 = new Direction(new Angle(), new Angle());
            (testErrorHandling0 == expectedErrorDirection0).Should().BeTrue();

            Point testPointPassedEqualityTo0 = PointGenerator.MakePointWithInches(1.0 / 32, 0.006, 1.488);
            Direction testErrorNotEqualTo0 = new Direction(testPointPassedEqualityTo0);
            (testErrorNotEqualTo0 == expectedErrorDirection0).Should().BeFalse();
        }
    }
}
