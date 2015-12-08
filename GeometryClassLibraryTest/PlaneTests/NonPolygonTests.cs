using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

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
            Ellipse e2 = new Ellipse(foci, Distance.Inch);
        }

        [Test()]
        public void NonPolygon_Circle()
        {
            Point center = Point.Origin;
            Distance radius = Distance.Inch * 3;
            Circle c2 = new Circle(center, radius);
            c2.Center.Equals(Point.Origin).Should().BeTrue();
            c2.Radius.Inches.Should().Be(3);
        }
    }
}