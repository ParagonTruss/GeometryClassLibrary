using System;
using NUnit.Framework;
using System.Collections.Generic;

using FluentAssertions;
using GeometryClassLibrary;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    class RectangularPrismTest
    {
        [Test]
        public void RectangularPrism_ConstructorTest()
        {
            var height = new Distance(DistanceType.Inch, 5);
            var width = new Distance(DistanceType.Inch, 5);
            var length = new Distance(DistanceType.Inch, 5);
            RectangularPrism prism = new RectangularPrism(width, height, length);

            //make sure it was made right
            (prism.LineSegments.Count == 12).Should().BeTrue();
            (prism.Polygons.Count == 6).Should().BeTrue();
            (prism.Vertices.Count == 8).Should().BeTrue();

            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 5, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(5, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(5, 5, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 5);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 5, 5);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(5, 0, 5);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(5, 5, 5);

            List<Point> expectedPoints = new List<Point>() { basePoint, topLeftPoint, bottomRightPoint, topRightPoint, backbasepoint, backtopleftpoint, backbottomrightpoint, backtoprightpoint };

            foreach (Point point in expectedPoints)
            {
                prism.Vertices.Contains(point).Should().BeTrue();
            }
        }
    }
}