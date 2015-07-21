using NUnit.Framework;
using GeometryClassLibrary;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using UnitClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture]
    public class PlaneRegionTests
    {
        [Test]
        public void PlaneRegion_Shift()
        {
            List<PlaneRegion> planes = new List<PlaneRegion>();

            List<LineSegment> polygonLines = new List<LineSegment>();
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 1)));
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(0, 2, 5)));
            polygonLines.Add(new LineSegment(Point.MakePointWithInches(2, 3, 1), Point.MakePointWithInches(0, 2, 5)));
            Polygon polygon = new Polygon(polygonLines);

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(Point.MakePointWithInches(1, 5, 3)));
            Arc arcToadd = new Arc(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(2, 3, 0), Direction.Right);
            nonPolygonEdges.Add(arcToadd);
            NonPolygon nonPolygon = new NonPolygon(nonPolygonEdges);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(nonPolygon);

            Shift shift = new Shift(Point.MakePointWithInches(2, 0, 0));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(Point.MakePointWithInches(4, 3, 1), Point.MakePointWithInches(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            PlaneRegion polygonResult = planes[0].ShiftAsPlaneRegion(shift);
            (polygonResult == polygonExpected).Should().BeTrue();

            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(3, 5, 3)));
            Arc arcExpected = new Arc(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(4, 3, 0), Direction.Right);
            nonPolygonExpectedEdges.Add(arcExpected);
            NonPolygon nonPolygonExpected = new NonPolygon(nonPolygonExpectedEdges);

            PlaneRegion nonPolygonResult = planes[1].ShiftAsPlaneRegion(shift);
            (nonPolygonResult == nonPolygonExpected).Should().BeTrue();
        }    
    }
}
