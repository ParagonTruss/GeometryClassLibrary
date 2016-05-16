using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using System.Diagnostics;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;


namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class RotationTests
    { 
        [Test]
        public void Rotation_Constructor_Matrix()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(2, 3, -6), Direction.Out, new Distance(1, Inches));
            Vector vector2 = new Vector(Point.MakePointWithInches(-5, 3, 7));

            //A very randomish axis
            Line testAxis = vector1.CrossProduct(vector2).Translate(Point.MakePointWithInches(3, -9, 10));

            Angle angle = new Angle(new Radian(), -173);

            Rotation rotation = new Rotation(testAxis, angle);
            Rotation fromMatrixConstructor = Rotation.RotationFromMatrix(rotation.Matrix);

            (fromMatrixConstructor.RotationAngle == angle.ProperAngle).Should().BeTrue();
            (fromMatrixConstructor.AxisOfRotation == testAxis).Should().BeTrue();
        }
        

        [Test()]
        public void Rotation_EqualityTests()
        {
            Line axis1 = new Line(Point.MakePointWithInches(-2, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation1 = new Rotation(axis1, new Angle(new Degree(), 42));

            Line axis2 = new Line(Point.MakePointWithInches(-2, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation2 = new Rotation(axis2, new Angle(new Degree(), 42));

            Line axis3 = new Line(Point.MakePointWithInches(-3, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation3 = new Rotation(axis3, new Angle(new Degree(), 42));

            Line axis4 = new Line(Point.MakePointWithInches(-3, 7, 0), Point.MakePointWithInches(0, 1, 1));
            Rotation rotation4 = new Rotation(axis4, new Angle(new Degree(), 63));

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
            Rotation rotation = new Rotation(axis, new Angle(new Degree(), 42));
            Point point = Point.MakePointWithInches(1, 1, 1);
            
            Point rotated = point.Rotate3D(rotation);
            Point unrotated = rotated.Rotate3D(rotation.Inverse());
           
            (point == unrotated).Should().BeTrue();
        }
    }
}
