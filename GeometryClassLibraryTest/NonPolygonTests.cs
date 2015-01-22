using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class NonPolygonTests
    {
        [Test()]
        public void NonPolygon_Ellipse()
        {
            Ellipse e1 = new Ellipse();

            List<Point> foci = new List<Point>() { PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(-1, 0, 0) };
            Ellipse e2 = new Ellipse(foci, Distance.Inch);
        }

        [Test()]
        public void NonPolygon_Circle()
        {
            Circle c1 = new Circle();

            Point center = PointGenerator.Origin;
            Distance radius = Distance.Inch * 3;
            Circle c2 = new Circle(center, radius);
            c2.Center.Equals(PointGenerator.Origin).Should().BeTrue();
            c2.Radius.Inches.Should().Be(3);
        }
    }
}