using NUnit.Framework;
using GeometryClassLibrary;
using FluentAssertions;
using UnitClassLibrary;
using System.Collections.Generic;
using System;

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

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -1, -4), PointGenerator.MakePointWithInches(1, 1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -1, -4), PointGenerator.MakePointWithInches(5, -1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -4), PointGenerator.MakePointWithInches(5, 1, -4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(5, -1, -4), PointGenerator.MakePointWithInches(5, 1, -4)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
        }

        [Test]
        public void Shift_RotateUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 45), new Angle(), new Angle(AngleType.Degree, 45));

            Point testPoint = PointGenerator.MakePointWithInches(3, 0, 0);

            Point shifted = testPoint.Shift(new Shift(system));

            Point expected = PointGenerator.MakePointWithInches(1.5, -2.12132034, 1.5);

            shifted.Should().Be(expected);

            //try to get the point we had before we switched order of shifting
            CoordinateSystem system2 = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 30), new Angle(), new Angle(AngleType.Degree, 54.7356104));

            Point shifted2 = testPoint.Shift(new Shift(system2));

            Point expected2 = PointGenerator.MakePointWithInches(1.5, -1.5, 2.12132034);

            shifted2.Should().Be(expected2);
        }

        [Test]
        public void Shift_ShiftUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 4), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45), new Angle());

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(new Shift(system));

            Point expected = PointGenerator.MakePointWithInches(2.12132034 + 1, 1.5 - 2, -1.5 - 4);

            shifted.Should().Be(expected);
        }

        [Test]
        public void Shift_ShiftPolygonUsingCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(0.75, -2, -1.5), new Angle(AngleType.Degree, 45), new Angle(), new Angle(AngleType.Degree, 45));
            
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(new Shift(system));

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5 - 0.75, 0.707106781 + 2, .5 + 1.5), PointGenerator.MakePointWithInches(1.5 - 0.75, 2.12132034 + 2, 1.5 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5 - 0.75, 0.707106781 + 2, .5 + 1.5), PointGenerator.MakePointWithInches(2.5 - 0.75, -2.12132034 + 2, 2.5 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1.5 - 0.75, 2.12132034 + 2, 1.5 + 1.5), PointGenerator.MakePointWithInches(3.5 - 0.75, -0.707106781 + 2, 3.5 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2.5 - 0.75, -2.12132034 + 2, 2.5 + 1.5), PointGenerator.MakePointWithInches(3.5 - 0.75, -0.707106781 + 2, 3.5 + 1.5)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
        }

        [Test]
        public void Shift_CoordinateShiftTheUnShift()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 4), new Angle(AngleType.Degree, 123), new Angle(AngleType.Degree, -22), new Angle(AngleType.Degree, 78));

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(new Shift(system));
            Polygon shifted2 = shifted.Shift(system.ShiftThatReturnsThisToWorldCoordinateSystem());

            testPolygon.Should().Be(shifted2);
        }
    }
}
