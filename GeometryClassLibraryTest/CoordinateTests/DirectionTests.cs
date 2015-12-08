using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class DirectionTests
    {
        [Test]
        public void Direction_ConstuctorsAndEquality()
        {
            Direction direction1 = Direction.Up;
            Direction direction2 = new Direction(new Angle(new Degree(), 33));
            Direction direction3 = Direction.Out;
            Direction direction4 = new Direction(new Angle(new Degree(), 33), new Angle(new Degree(), 90));
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

            Point testPoint = Point.MakePointWithInches(0.006, 0.003, -1.4999);
            Direction testErrorHandling = new Direction(testPoint);
            Direction expectedErrorDirection = Direction.Back;
            (testErrorHandling == expectedErrorDirection).Should().BeTrue();

            //now try the 0 Direction too
            Point testPoint0 = Point.MakePointWithInches(0.006, 0.003, 1.4999);
            Direction testErrorHandling0 = new Direction(testPoint0);
            Direction expectedErrorDirection0 = Direction.Out;
            (testErrorHandling0 == expectedErrorDirection0).Should().BeTrue();

            Point testPointPassedEqualityTo0 = Point.MakePointWithInches(1.0 / 32, 0.006, 1.488);
            Direction testErrorNotEqualTo0 = new Direction(testPointPassedEqualityTo0);
            (testErrorNotEqualTo0 == expectedErrorDirection0).Should().BeFalse();
        }

        [Test]
        public void Direction_JSON()
        {
            Direction direction = new Direction(new Angle(new Degree(), 14), new Angle(new Degree(), 37));

            var json = JsonConvert.SerializeObject(direction);
            Direction deserializedDirection = JsonConvert.DeserializeObject<Direction>(json);

            bool areEqual = (direction == deserializedDirection);
            areEqual.Should().BeTrue();
        }
    }
}
