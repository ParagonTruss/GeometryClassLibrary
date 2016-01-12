using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PolygonListExtensionTests
    {
        [Test]
        public void PolygonList_Shift()
        {
            List<Polygon> planes = new List<Polygon>();

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

            //add them to the generic list
            planes.Add(polygon);
            planes.Add(polygon2);

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

            List<Polygon> resultPlanes = planes.Shift(shift);
            (resultPlanes[0] == polygonExpected).Should().BeTrue();
            (resultPlanes[1] == polygon2Expected).Should().BeTrue();
        }

       
        [Test()]
        public void PolygonList_FindPolygonsTouchingPlane()
        {
            List<LineSegment> lineSegments1 = new List<LineSegment>();
            lineSegments1.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
            lineSegments1.Add(new LineSegment(Point.MakePointWithInches(0, 2, 4), Point.MakePointWithInches(0, 2, 0)));
            lineSegments1.Add(new LineSegment(Point.MakePointWithInches(0, 2, 4), Point.MakePointWithInches(0, 0, 4)));
            lineSegments1.Add(new LineSegment(Point.MakePointWithInches(0, 0, 4), Point.Origin));
            Polygon testPolygon1 = new Polygon(lineSegments1);

            List<LineSegment> lineSegments2 = new List<LineSegment>();
            lineSegments2.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(-3, 0, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-3, 0, 0), Point.Origin));
            Polygon testPolygon2 = new Polygon(lineSegments2);

            List<LineSegment> lineSegments3 = new List<LineSegment>();
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(0, 0, 4), Point.MakePointWithInches(0, 2, 4)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 4), Point.MakePointWithInches(0, 2, 4)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 4), Point.MakePointWithInches(-3, 0, 4)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(-3, 0, 4), Point.MakePointWithInches(0, 0, 4)));
            Polygon testPolygon3 = new Polygon(lineSegments3);

            List<Polygon> testList = new List<Polygon>() { testPolygon1, testPolygon2, testPolygon3};

            Plane planeToCheck = Plane.XY;

            List<Polygon> touchedInclusive = testList.FindPolygonsTouchingPlane(planeToCheck, true);
            (touchedInclusive.Count == 2).Should().BeTrue();
            touchedInclusive.Contains(testPolygon1).Should().BeTrue();
            touchedInclusive.Contains(testPolygon2).Should().BeTrue();

            List<Polygon> touchedExclusive = testList.FindPolygonsTouchingPlane(planeToCheck, false);
            (touchedExclusive.Count == 1).Should().BeTrue();
            touchedExclusive.Contains(testPolygon1).Should().BeTrue();
        }
    }
}
