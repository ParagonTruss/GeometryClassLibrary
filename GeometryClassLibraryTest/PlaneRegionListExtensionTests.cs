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
        public void PlaneRegion_Shift()
        {
            List<PlaneRegion> planes = new List<PlaneRegion>();

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

            /*var arcTest = new Mock<Arc>();
            arcTest.Setup(x => x.Direction).Returns(new Direction());
            arcTest.Setup(x => x.BasePoint).Returns(new Point());
            arcTest.Setup(x => x.OriginPointTwo).Returns(PointGenerator.MakePointWithInches(1, 5, 3));*/

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 5, 3)));
            nonPolygonEdges.Add(new Arc(PointGenerator.MakePointWithInches(1, 5, 3))); ;
            //nonPolygonEdges.Add(arcTest.Object);
            NonPolygon nonPolygon = new NonPolygon(nonPolygonEdges);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(polygon2);
            //planes.Add(nonPolygon);

            Shift shift = new Shift(PointGenerator.MakePointWithInches(2, 0, 0));

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

            List<PlaneRegion> resultPlanes = planes.Shift(shift);
            (resultPlanes[0] == polygonExpected).Should().BeTrue();
            (resultPlanes[1] == polygon2Expected).Should().BeTrue();


            //Arcs are not implemetned
            /*var arcResult = new Mock<Arc>();
            arcResult.Setup(x => x.Direction).Returns(new Direction());
            arcResult.Setup(x => x.BasePoint).Returns(PointGenerator.MakePointWithInches(2, 0, 0));
            arcResult.Setup(x => x.OriginPointTwo).Returns(PointGenerator.MakePointWithInches(3, 5, 3));

            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(3, 5, 3)));
            nonPolygonExpectedEdges.Add(new Arc(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(3, 5, 3)));
            //nonPolygonExpectedEdges.Add(arcResult.Object);
            NonPolygon nonPolygonExpected = new NonPolygon(nonPolygonExpectedEdges);

            PlaneRegion nonPolygonResult = planes[1].Shift(shift);
            (nonPolygonResult == nonPolygonExpected).Should().BeTrue();
            nonPolygonResult = (NonPolygon)nonPolygonResult;*/

            
        }
    }
}
