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
        [Test()]
        public void Negate_Test()

        {
            var originalAngle = new Angle(AngleType.Degree, 45);
            var oppositeAngle = new Angle(AngleType.Degree, -45);

            var negatedOriginalAngle = originalAngle.Negate();
            var negatedNegatedOriginalAngle = negatedOriginalAngle.Negate();

            (originalAngle == negatedNegatedOriginalAngle).Should().BeTrue();
            (negatedOriginalAngle == oppositeAngle).Should().BeTrue();

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
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5 - 0.75, .5 + 2, -0.707106781 + 1.5), PointGenerator.MakePointWithInches(1.5 - 0.75, 1.5 + 2, -2.12132034 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(.5 - 0.75, .5 + 2, -0.707106781 + 1.5), PointGenerator.MakePointWithInches(3.32847125 - 0.75, -2.328427125 + 2, -0.707106781 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1.5 - 0.75, 1.5 + 2, -2.12132034 + 1.5), PointGenerator.MakePointWithInches(4.328427125 - 0.75, -1.328427125 + 2, -2.12132034 + 1.5)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(3.32847125 - 0.75, -2.328427125 + 2, -0.707106781 + 1.5), PointGenerator.MakePointWithInches(4.328427125 - 0.75, -1.328427125 + 2, -2.12132034 + 1.5)));
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
