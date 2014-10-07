using NUnit.Framework;
using GeometryClassLibrary;
using System.Collections.Generic;
using FluentAssertions;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PlaneRegionTests
    {
        [Test]
        public void PlaneRegion_GenericShiftTest()
        {
            List<PlaneRegion<IEdge>> planes = new List<PlaneRegion<IEdge>>();

            List<LineSegment> polygonLines = new List<LineSegment>();
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 3, 1)));
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 2, 5)));
            polygonLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 3, 1), PointGenerator.MakePointWithMillimeters(0, 2, 5)));
            Polygon polygon = new Polygon(polygonLines);

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 5, 3)));
            nonPolygonEdges.Add(new Arc(PointGenerator.MakePointWithMillimeters(1, 5, 3)));
            NonPolygon nonPolygon = new NonPolygon(nonPolygonEdges);

            //planes.Add(polygon as PlaneRegion<IEdge>);
            planes.Add(nonPolygon);

            Shift shift = new Shift(new Vector(PointGenerator.MakePointWithMillimeters(2, 0, 0)));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            //PlaneRegion<IEdge> polygonResult = planes[0].Shift<IEdge>(shift);
            //(polygonResult == polygonExpected).Should().BeTrue();


            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(3, 5, 3)));
            nonPolygonExpectedEdges.Add(new Arc(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(3, 5, 3)));
            NonPolygon nonPolygonExpected = new NonPolygon(nonPolygonExpectedEdges);

            PlaneRegion<IEdge> nonPolygonResult = planes[1].Shift<IEdge>(shift);
            (nonPolygonResult == nonPolygonExpected).Should().BeTrue();
            nonPolygonResult = (NonPolygon)nonPolygonResult;

            
        }      
    }
}
