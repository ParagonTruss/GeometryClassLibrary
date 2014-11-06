using NUnit.Framework;
using GeometryClassLibrary;
using FluentAssertions;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class CoordinateSystemTest
    {
        /*
        [Test]
        public void CoordinateSystem_Negate()
        {
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 3), new Direction(new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -45)));

            CoordinateSystem result = test.Negate();

            CoordinateSystem expected = new CoordinateSystem(PointGenerator.MakePointWithInches(-1.79289322, -3.20710678, -0.70710612), new Direction(new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 45)));

            result.Should().Be(expected);

        }

        [Test]
        public void CoordinateSystem_DoubleNegate()
        {
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 3), new Direction(new Angle(AngleType.Degree, 34), new Angle(AngleType.Degree, -66)));

            CoordinateSystem result = test.Negate();
            CoordinateSystem doubleNegate = result.Negate();

            CoordinateSystem expected = new CoordinateSystem();

            test.Should().Be(doubleNegate);
        
        }*/
    }
}
