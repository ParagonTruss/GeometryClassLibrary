using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class RectangularPrismTests
    {
        [Test]
        public void RectangularPrism_ConstructorTest()
        {
            var height = new Distance(new Inch(), 5);
            var width = new Distance(new Inch(), 5);
            var length = new Distance(new Inch(), 5);
            RectangularPrism prism = new RectangularPrism(width, height, length);

            //make sure it was made right
            (prism.LineSegments.Count == 12).Should().BeTrue();
            (prism.Polygons.Count == 6).Should().BeTrue();
            (prism.Vertices.Count == 8).Should().BeTrue();

            Point basePoint = Point.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = Point.MakePointWithInches(0, 5, 0);
            Point bottomRightPoint = Point.MakePointWithInches(5, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(5, 5, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 5);
            Point backtopleftpoint = Point.MakePointWithInches(0, 5, 5);
            Point backbottomrightpoint = Point.MakePointWithInches(5, 0, 5);
            Point backtoprightpoint = Point.MakePointWithInches(5, 5, 5);

            List<Point> expectedPoints = new List<Point>() { basePoint, topLeftPoint, bottomRightPoint, topRightPoint, backbasepoint, backtopleftpoint, backbottomrightpoint, backtoprightpoint };

            foreach (Point point in expectedPoints)
            {
                prism.Vertices.Contains(point).Should().BeTrue();
            }
        }
    }
}