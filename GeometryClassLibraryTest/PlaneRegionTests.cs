﻿using NUnit.Framework;
using GeometryClassLibrary;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PlaneRegionTests
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

            List<IEdge> nonPolygonEdges = new List<IEdge>();
            nonPolygonEdges.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 5, 3)));
            Arc arcToadd = new Arc(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(2, 3, 3), new Distance(DistanceType.Inch, 2));
            nonPolygonEdges.Add(arcToadd);
            NonPolygon nonPolygon = new NonPolygon(nonPolygonEdges);

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(nonPolygon);

            Shift shift = new Shift(PointGenerator.MakePointWithInches(2, 0, 0));

            List<LineSegment> polygonExpectedLines = new List<LineSegment>();
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(4, 3, 1)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(2, 2, 5)));
            polygonExpectedLines.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(2, 2, 5)));
            Polygon polygonExpected = new Polygon(polygonExpectedLines);

            PlaneRegion polygonResult = planes[0].ShiftAsPlaneRegion(shift);
            (polygonResult == polygonExpected).Should().BeTrue();


            //Arcs are not implemented

            List<IEdge> nonPolygonExpectedEdges = new List<IEdge>();
            nonPolygonExpectedEdges.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(3, 5, 3)));
            Arc arcExpected = new Arc(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(2, 3, 3), new Distance(DistanceType.Inch, 2));
            nonPolygonExpectedEdges.Add(arcExpected);
            NonPolygon nonPolygonExpected = new NonPolygon(nonPolygonExpectedEdges);

            PlaneRegion nonPolygonResult = planes[1].ShiftAsPlaneRegion(shift);
            (nonPolygonResult == nonPolygonExpected).Should().BeTrue();
        }    
    }
}
