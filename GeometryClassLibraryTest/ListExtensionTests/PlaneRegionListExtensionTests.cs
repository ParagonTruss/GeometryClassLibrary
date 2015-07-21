using NUnit.Framework;
using GeometryClassLibrary;
using UnitClassLibrary;
using FluentAssertions;
using System.Collections.Generic;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PlaneRegionListExtensionTests
    {
        [Test]
        public void PlaneRegion_ShiftList()
        {
            List<PlaneRegion> planes = new List<PlaneRegion>();

            List<LineSegment> polygonLines = new List<LineSegment>();
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 1)));
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(0, 2, 5)));
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 1), Point.MakePointWithInches(0, 2, 5)));
            Polygon polygon = new Polygon(polygonLines);

            List<LineSegment> polygon2Lines = new List<LineSegment>();
            polygon2Lines.Add(new LineSegment(Point.MakePointWithInches(-1, -5, 7)));
            polygon2Lines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 2)));
            polygon2Lines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 2), Point.MakePointWithInches(-1, -5, 7)));
            Polygon polygon2 = new Polygon(polygon2Lines);

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(Point.MakePointWithInches(1, 5, 3)));
            Arc arcToadd = new Arc(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(2, 3, 3), Direction.Right);
            nonPolygonEdges.Add(arcToadd);
            NonPolygon nonPolygon = new NonPolygon(nonPolygonEdges);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(polygon2);
            planes.Add(nonPolygon);

            Shift shift = new Shift(Point.MakePointWithInches(2, 0, 0));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(4, 3, 1), Point.MakePointWithInches(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            List<LineSegment> polygon2ExpectedLines = new List<LineSegment>();
            polygon2ExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(1, -5, 7)));
            polygon2ExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(4, 3, 2)));
            polygon2ExpectedLines.Add(new LineSegment(Point.MakePointWithInches(4, 3, 2), Point.MakePointWithInches(1, -5, 7)));
            Polygon polygon2Expected = new Polygon(polygon2ExpectedLines);

            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(3, 5, 3)));
            Arc arcExpected = new Arc(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(2, 3, 3), Direction.Right);
            nonPolygonExpectedEdges.Add(arcExpected);
            NonPolygon nonPolygonExpected = new NonPolygon(nonPolygonExpectedEdges);

            List<PlaneRegion> resultPlanes = planes.Shift(shift);
            (resultPlanes[0] == polygonExpected).Should().BeTrue();
            (resultPlanes[1] == polygon2Expected).Should().BeTrue();
            (resultPlanes[2] == nonPolygonExpected).Should().BeTrue();
        }  
    }
}
