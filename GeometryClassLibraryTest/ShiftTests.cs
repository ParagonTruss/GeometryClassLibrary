using NUnit.Framework;
using UnitClassLibrary;

using FluentAssertions;

using System.Collections.Generic;
using System;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{


    [TestFixture]
    public class ShiftTests
    {

        [Test]
        public void Shift_TranslateUsingToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, -4));

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftToThisFromWorld());

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 3, 4), PointGenerator.MakePointWithInches(-1, 5, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 3, 4), PointGenerator.MakePointWithInches(3, 3, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(3, 5, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 3, 4), PointGenerator.MakePointWithInches(3, 5, 4)));
            Polygon expectedPolygon = new Polygon(expectedBounds);

            shifted.Should().Be(expectedPolygon);
        }

        [Test]
        public void Shift_RotateToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 45), new Angle(), new Angle(AngleType.Degree, 45));

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(system.ShiftToThisFromWorld());

            Point expected = PointGenerator.MakePointWithInches(2.12132034, 1.5, -1.5);

            shifted.Should().Be(expected);

            //try to get the point we had before we switched order of shifting
            CoordinateSystem system2 = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 30), new Angle(AngleType.Degree, 54.7356104), new Angle());

            Point shifted2 = testPoint.Shift(system2.ShiftToThisFromWorld());

            Point expected2 = PointGenerator.MakePointWithInches(0, 2.59807621, -1.5);

            shifted2.Should().Be(expected2);
        }

        [Test]
        public void Shift_ShiftToCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, -4), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, -45), new Angle());

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 0);

            Point shifted = testPoint.Shift(system.ShiftToThisFromWorld());

            Point expected = PointGenerator.MakePointWithInches(2.12132034356, 3.53553391, -5);

            shifted.Should().Be(expected);
        }

        [Test]
        public void Shift_ShiftPolygonFromCoordinateSystem()
        {
            CoordinateSystem system = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 1, 0), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -45), new Angle());

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(4, 0, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 0), PointGenerator.MakePointWithInches(4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 2, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftFromThisToWorld());

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(-1, 2.41421356237, 1)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(2.82842712475, 1, 2.82842712475)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 2.41421356237, 1), PointGenerator.MakePointWithInches(1.82842712475, 2.41421356237, 3.82842712475)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2.82842712475, 1, 2.82842712475), PointGenerator.MakePointWithInches(1.82842712475, 2.41421356237, 3.82842712475)));
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

            Polygon shifted = testPolygon.Shift(system.ShiftToThisFromWorld());
            Polygon shifted2 = shifted.Shift(system.ShiftFromThisToWorld());

            testPolygon.Should().Be(shifted2);
        }
    }
}
