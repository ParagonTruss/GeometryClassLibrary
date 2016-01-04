using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{


    [TestFixture]
    public class ShiftTests
    {
        [Test]
        public void Shift_TranslateUsingToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(Point.MakePointWithInches(1, -2, -4));

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftToThisFrom());

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(-1, 3, 4), Point.MakePointWithInches(-1, 5, 4)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(-1, 3, 4), Point.MakePointWithInches(3, 3, 4)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 4), Point.MakePointWithInches(3, 5, 4)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(3, 3, 4), Point.MakePointWithInches(3, 5, 4)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
        }

        [Test]
        public void Shift_RotateToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(Point.Origin, new Angle(new Degree(), 45), Angle.ZeroAngle, new Angle(new Degree(), 45));

            Point testPoint = Point.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(system.ShiftToThisFrom());

            Point expected = Point.MakePointWithInches(2.12132034, 1.5, -1.5);

            shifted.Should().Be(expected);

            //try to get the point we had before we switched order of shifting
            CoordinateSystem system2 = new CoordinateSystem(Point.Origin, new Angle(new Degree(), 30), new Angle(new Degree(), 54.7356104), Angle.ZeroAngle);

            Point shifted2 = testPoint.Shift(system2.ShiftToThisFrom());

            Point expected2 = Point.MakePointWithInches(0, 2.59807621, -1.5);

            shifted2.Should().Be(expected2);
        }

        [Test]
        public void Shift_ShiftToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(Point.MakePointWithInches(1, -2, -4), new Angle(new Degree(), 90), new Angle(new Degree(), -45), Angle.ZeroAngle);

            Point testPoint = Point.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(system.ShiftToThisFrom());

            Point expected = Point.MakePointWithInches(2.12132034356, 3.53553391, -5);

            shifted.Should().Be(expected);
        }

        [Test]
        public void Shift_ShiftPolygonFromCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(Point.MakePointWithInches(0, 1, 0), new Angle(new Degree(), 45), new Angle(new Degree(), -45), Angle.ZeroAngle);

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(4, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(4, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 2, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftFromThisTo());

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(-1, 2.41421356237, 1)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(2.82842712475, 1, 2.82842712475)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(-1, 2.41421356237, 1), Point.MakePointWithInches(1.82842712475, 2.41421356237, 3.82842712475)));
            expectedBounds.Add(new LineSegment(Point.MakePointWithInches(2.82842712475, 1, 2.82842712475), Point.MakePointWithInches(1.82842712475, 2.41421356237, 3.82842712475)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
        }

        [Test]
        public void Shift_CoordinateShiftTheUnShift()
        {
            CoordinateSystem system = new CoordinateSystem(Point.MakePointWithInches(-1, 2, 4), new Angle(new Degree(), 123), new Angle(new Degree(), -22), new Angle(new Degree(), 78));

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftToThisFrom());
            Polygon shifted2 = shifted.Shift(system.ShiftFromThisTo());

            testPolygon.Should().Be(shifted2);
        }
    }
}
