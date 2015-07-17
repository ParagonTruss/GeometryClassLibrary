using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class RotationTests
    {
        /* Does not make sense with how we were adding and subtracting rotations because we didnt take into account the axis
        [Test()]
        public void Rotation_AdditionAndSubtractionTest()
        {
            Line axis = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 1, 1));

            Rotation baseRotation = new Rotation(axis, new Angle(AngleType.Degree, 42));
            Angle toAdd = new Angle(AngleType.Degree, 21);
            Angle toSubtract = new Angle(AngleType.Radian, Math.PI / 4); //45 degrees

            Rotation addedRotation = baseRotation + toAdd;
            Rotation subtractedRotation = baseRotation - toSubtract;

            Rotation expectedAddition = new Rotation(axis, new Angle(AngleType.Degree, 63));
            Rotation expectedSubtraction = new Rotation(axis, new Angle(AngleType.Degree, -3));

            (addedRotation == expectedAddition).Should().BeTrue();
            (subtractedRotation == expectedSubtraction).Should().BeTrue();
        
        }*/

        [Test()]
        public void Rotation_EqualityTests()
        {
            Line axis1 = new Line(PointGenerator.MakePointWithInches(-2, 7, 0), PointGenerator.MakePointWithInches(0, 1, 1));
            Rotation rotation1 = new Rotation(axis1, new Angle(AngleType.Degree, 42));

            Line axis2 = new Line(PointGenerator.MakePointWithInches(-2, 7, 0), PointGenerator.MakePointWithInches(0, 1, 1));
            Rotation rotation2 = new Rotation(axis2, new Angle(AngleType.Degree, 42));

            Line axis3 = new Line(PointGenerator.MakePointWithInches(-3, 7, 0), PointGenerator.MakePointWithInches(0, 1, 1));
            Rotation rotation3 = new Rotation(axis3, new Angle(AngleType.Degree, 42));

            Line axis4 = new Line(PointGenerator.MakePointWithInches(-3, 7, 0), PointGenerator.MakePointWithInches(0, 1, 1));
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
            Line axis = new Line(PointGenerator.MakePointWithInches(-2, 7, 0), PointGenerator.MakePointWithInches(0, 1, 1));
            Rotation rotation = new Rotation(axis, new Angle(AngleType.Degree, 42));
            Point point = PointGenerator.MakePointWithInches(1, 1, 1);
            
            Point rotated = point.Rotate3D(rotation);
            Point unrotated = rotated.Rotate3D(rotation.Inverse());
           
            (point == unrotated).Should().BeTrue();
        }
    }
}
