using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class Polygon_Validation
    {
        [Test]
        public void Polygon_Constructor_NoSelfIntersections_ErrorCase_1()
        {
            var points = new List<Point>
           {
               new Point(new Distance(168, Inches), new Distance(88.1875, Inches)),
               new Point(new Distance(168, Inches), new Distance(84.2743810393754, Inches)),
               new Point(new Distance(-12, Inches), new Distance(-1.8125, Inches)),
               new Point(new Distance(-12, Inches), new Distance(-5.72561896062463, Inches))
           };
            Assert.Throws<SelfIntersectionPolygonException>(() => new Polygon(points));
        }
        [Test]
        public void Polygon_Constructor_NoSelfIntersections()
        {
            Point point1 = Point.MakePointWithInches(0, 0);
            Point point2 = Point.MakePointWithInches(1, 0);
            Point point3 = Point.MakePointWithInches(1, 1);
            Point point4 = Point.MakePointWithInches(0, 1);

            List<Point> verticesInCorrectOrder = new List<Point> { point1, point2, point3, point4 };
            List<Point> verticesInWrongOrder = new List<Point> { point1, point2, point4, point3 };

            Polygon correctPolygon = new Polygon(verticesInCorrectOrder);
            Area area = correctPolygon.Area;
            area.Should().Be(new Area(new SquareInch(), 1));


            Assert.Throws<SelfIntersectionPolygonException>(() => new Polygon(verticesInWrongOrder));
        }

        [Test]
        public void Polygon_Constructor_CoplanarPoints_ErrorCase_1()
        {
            var segments = new[]
            {
                new LineSegment(new Point(Inches, 178.25, 29.3556923011416, 0),
                    new Point(Inches, 180.011803133622, 30.25, 0)),
                new LineSegment(new Point(Inches, 180, 30.25, 1.5),
                    new Point(Inches, 178.25, 29.3556923011416, 1.5)),
                new LineSegment(new Point(Inches, 178.25, 29.3556923011416, 0),
                    new Point(Inches, 178.25, 29.3556923011416, 1.5)),
                new LineSegment(new Point(Inches, 180, 30.25, 0),
                    new Point(Inches, 180.011803133622, 30.25, 1.5))
            };
            Assert.DoesNotThrow(() => new Polygon(true, segments));
        }
    }
}
