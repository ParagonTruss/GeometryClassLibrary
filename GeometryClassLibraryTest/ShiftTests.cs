using NUnit.Framework;
using GeometryClassLibrary;
using FluentAssertions;
using UnitClassLibrary;
using System.Collections.Generic;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class ShiftTests
    {
        [Test]
        public void Shift_EqualityTests()
        {
            Rotation rotation1 = new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 45));
            Rotation rotation2 = new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 45));

            Shift testShift1 = new Shift(rotation1, PointGenerator.MakePointWithInches(2, 1, 3));
            Shift testShift2 = new Shift(rotation2, PointGenerator.MakePointWithInches(2, 1, 3));

            testShift1.Equals(testShift2).Should().BeTrue();

            Rotation rotation3 = new Rotation(Line.XAxis, new Angle(AngleType.Degree, 23));
            Rotation rotation4 = new Rotation(new Line(PointGenerator.MakePointWithInches(0, 2 ,3), PointGenerator.MakePointWithInches(-2,-6,7)), 
                new Angle(AngleType.Degree, 47));
            Rotation rotation5 = new Rotation(Line.YAxis, new Angle(AngleType.Degree, -112));

            Shift testShift3 = new Shift(new List<Rotation>() { rotation3, rotation1, rotation4, rotation2, rotation5 }, PointGenerator.MakePointWithInches(-3,4,0));
            Shift testShift4 = new Shift(new List<Rotation>() { rotation3, rotation1, rotation4, rotation2, rotation5 }, PointGenerator.MakePointWithInches(-3,4,0));

            testShift3.Equals(testShift2).Should().BeFalse();
            testShift3.Equals(testShift4).Should().BeTrue();
        }
    }
}
