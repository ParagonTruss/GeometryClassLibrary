using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using static GeometryClassLibrary.Point;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PlaneRegionTests
    {
        [Test]
        public void PlaneRegion_Shift()
        {
            List<ISurface> planes = new List<ISurface>();

            List<LineSegment> polygonLines = new List<LineSegment>();
            polygonLines.Add(new LineSegment(MakePointWithInches(2, 3, 1)));
            polygonLines.Add(new LineSegment(MakePointWithInches(0, 2, 5)));
            polygonLines.Add(new LineSegment(MakePointWithInches(2, 3, 1), MakePointWithInches(0, 2, 5)));
            Polygon polygon = new Polygon(polygonLines);

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(MakePointWithInches(1, 5, 3)));
            nonPolygonEdges.Add(new LineSegment(MakePointWithInches(1, 5, 3), MakePointWithInches(2, 3, 0)));
            Arc arcToadd = new Arc(Origin, MakePointWithInches(2, 3, 0), Direction.Right);
            nonPolygonEdges.Add(arcToadd);
            PlaneRegion nonPolygon = new PlaneRegion(nonPolygonEdges);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(nonPolygon);

            Shift shift = new Shift(MakePointWithInches(2, 0, 0));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(MakePointWithInches(2, 0, 0), MakePointWithInches(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(MakePointWithInches(2, 0, 0), MakePointWithInches(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(MakePointWithInches(4, 3, 1), MakePointWithInches(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            Polygon polygonResult = (Polygon)planes[0].Shift(shift);
            (polygonResult == polygonExpected).Should().BeTrue();

            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(MakePointWithInches(2, 0, 0), MakePointWithInches(3, 5, 3)));
            nonPolygonExpectedEdges.Add(new LineSegment(MakePointWithInches(4, 3, 0), MakePointWithInches(3, 5, 3)));
            Arc arcExpected = new Arc(MakePointWithInches(2, 0, 0), MakePointWithInches(4, 3, 0), Direction.Right);
            nonPolygonExpectedEdges.Add(arcExpected);
            PlaneRegion nonPolygonExpected = new PlaneRegion(nonPolygonExpectedEdges);

            PlaneRegion nonPolygonResult = (PlaneRegion)planes[1].Shift(shift);
            (nonPolygonResult == nonPolygonExpected).Should().BeTrue();
        }    
    }
}
