using NUnit.Framework;
using GeometryClassLibrary;
using FluentAssertions;
using UnitClassLibrary;
using System.Collections.Generic;
using System;

namespace GeometryClassLibraryTest
{
    private class LegoBlock
    {
        public Polyhedron Geometry;
        public CoordinateSystem BlockSystem;
    }

    private class LegoSet
    {
        public List<LegoBlock> Blocks;
        public CoordinateSystem SetSystem;
    }

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

        [Test]
        public void Shift_CoordinateSystemSwitching()
        {
            Assert.Fail();
        }


        [Test]
        public void Shift_TranslateUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 4)); 

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(new Shift(system));

            //now negate the coordinate sytem and try that
            Polygon shifted2 = shifted.Shift(new Shift(CoordinateSystem.WorldCoordinateSystem));

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -1, -4), PointGenerator.MakePointWithInches(1, 1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -1, -4), PointGenerator.MakePointWithInches(5, -1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -4), PointGenerator.MakePointWithInches(5, 1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(5, -1, -4), PointGenerator.MakePointWithInches(5, 1, -4)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
            shifted2.Should().Be(testPolygon);
        }

        [Test]
        public void Shift_RotateUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(new Point(), new Direction(new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45)));

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(new Shift(system));

            Point shifted2 = shifted.Shift(new Shift(CoordinateSystem.WorldCoordinateSystem));

            Point expected = PointGenerator.MakePointWithInches(1.5, 1.5, 2.12132034);

            shifted.Should().Be(expected);
            shifted2.Should().Be(testPoint);
        }

        [Test]
        public void Shift_ShiftUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 4), new Direction(new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45)));

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(new Shift(system));

            Point shifted2 = shifted.Shift(new Shift(CoordinateSystem.WorldCoordinateSystem));

            Point expected = PointGenerator.MakePointWithInches(2.5, -0.5, 2.12132034 - 4);

            shifted.Should().Be(expected);
            shifted2.Should().Be(testPoint);
        }

        [Test]
        public void Shift_ShiftPolygonUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(new Point(), new Direction(new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45)));
            
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(new Shift(system));

            Polygon shifted2 = shifted.Shift(new Shift(CoordinateSystem.WorldCoordinateSystem));

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5, .5, 0.707106781), PointGenerator.MakePointWithInches(1.5, 1.5, 2.12132034)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5, .5, 0.707106781), PointGenerator.MakePointWithInches(3.32847125, -2.328427125, 0.707106781)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1.5, 1.5, 2.12132034), PointGenerator.MakePointWithInches(4.328427125, -1.328427125, 2.12132034)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(3.32847125, -2.328427125, 0.707106781), PointGenerator.MakePointWithInches(4.328427125, -1.328427125, 2.12132034)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
            shifted2.Should().Be(testPolygon);
        }
    }
}
