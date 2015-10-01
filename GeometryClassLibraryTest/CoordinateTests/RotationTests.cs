using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using static UnitClassLibrary.AngularDistance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class RotationTests
    {
        [Ignore]
        [Test]
        public void Rotation_Matrix_Constructor()
        {
            Line line = new Line(Point.MakePointWithInches(2, 3, -6), Direction.Out);
            AngularDistance angle = -173 * Degree;
            Rotation rotation = new Rotation(line, angle);

            Rotation fromMatrixConstructor = new Rotation(rotation.Matrix);

            (new Angle(fromMatrixConstructor.RotationAngle) == new Angle(angle)).Should().BeTrue();
            (fromMatrixConstructor.AxisOfRotation == line).Should().BeTrue();
        }
        

        [Test()]
        public void Rotation_EqualityTests()
        {
            Line axis1 = new Line(Point.MakePointWithInches(-2, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation1 = new Rotation(axis1, new Angle(AngleType.Degree, 42));

            Line axis2 = new Line(Point.MakePointWithInches(-2, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation2 = new Rotation(axis2, new Angle(AngleType.Degree, 42));

            Line axis3 = new Line(Point.MakePointWithInches(-3, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation3 = new Rotation(axis3, new Angle(AngleType.Degree, 42));

            Line axis4 = new Line(Point.MakePointWithInches(-3, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation4 = new Rotation(axis4, new Angle(AngleType.Degree, 63));

            (rotation1 == rotation2).Should().BeTrue();
            (rotation1 != rotation2).Should().BeFalse();
            (rotation1 == rotation3).Should().BeFalse();
            (rotation1 == rotation4).Should().BeFalse();
            (rotation3 != rotation4).Should().BeTrue();
        }

        [Test()]
        public void Rotation_Inverse()
        {
            Line axis = new Line(Point.MakePointWithInches(-2, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation = new Rotation(axis, new Angle(AngleType.Degree, 42));
            Point point = Point.MakePointWithInches(1, 1, 1);
            
            Point rotated = point.Rotate3D(rotation);
            Point unrotated = rotated.Rotate3D(rotation.Inverse());
           
            (point == unrotated).Should().BeTrue();
        }
    }
}
