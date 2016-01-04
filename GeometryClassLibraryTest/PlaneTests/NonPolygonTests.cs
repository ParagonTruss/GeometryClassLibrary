using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class NonPolygonTests
    {
        [Test()]
        public void NonPolygon_Ellipse()
        {
            Ellipse e1 = new Ellipse();

            List<Point> foci = new List<Point>() { Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(-1, 0, 0) };
            Ellipse e2 = new Ellipse(foci, new Distance(1, Inches));
        }

        [Test()]
        public void NonPolygon_Circle()
        {
            Point center = Point.Origin;
            Distance radius = new Distance(3, Inches);
            Circle c2 = new Circle(center, radius);
            c2.Center.Equals(Point.Origin).Should().BeTrue();
            c2.Radius.InInches.Should().Be(3);
        }
    }
}