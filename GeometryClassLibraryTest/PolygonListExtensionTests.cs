using NUnit.Framework;
using GeometryClassLibrary;
using UnitClassLibrary;
using FluentAssertions;
using System.Collections.Generic;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PolygonListExtensionTests
    {
        [Test]
        public void Polygon_Shift()
        {
            List<Polygon> planes = new List<Polygon>();

            List<LineSegment> polygonLines = new List<LineSegment>();
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 3, 1)));
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 5)));
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 3, 1), PointGenerator.MakePointWithInches(0, 2, 5)));
            Polygon polygon = new Polygon(polygonLines);


            List<LineSegment> polygon2Lines = new List<LineSegment>();
            polygon2Lines.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, -5, 7)));
            polygon2Lines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 3, 2)));
            polygon2Lines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 3, 2), PointGenerator.MakePointWithInches(-1, -5, 7)));
            Polygon polygon2 = new Polygon(polygon2Lines);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(polygon2);

            Shift shift = new Shift(new Vector(PointGenerator.MakePointWithInches(2, 0, 0)));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            List<LineSegment> polygon2ExpectedLines = new List<LineSegment>();
            polygon2ExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(1, -5, 7)));
            polygon2ExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(4, 3, 2)));
            polygon2ExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 2), PointGenerator.MakePointWithInches(1, -5, 7)));
            Polygon polygon2Expected = new Polygon(polygon2ExpectedLines);

            List<Polygon> resultPlanes = planes.Shift(shift);
            (resultPlanes[0] == polygonExpected).Should().BeTrue();
            (resultPlanes[1] == polygon2Expected).Should().BeTrue();
        }
    }
}
