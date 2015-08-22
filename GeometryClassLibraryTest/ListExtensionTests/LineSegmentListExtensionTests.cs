using System;
using GeometryClassLibraryTests;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture]
    public class IEdgeListExtensionMethodTests
    {

        [Test()]
        public void LineSegmentList_DoFormClosedRegionTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));

            lineSegments.DoFormClosedRegion().Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_GetPoints()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));

            List<Point> vertices = lineSegments.GetAllPoints();

            vertices.Count.Should().Be(3);
            vertices.Contains(Point.MakePointWithInches(0, 2, 3)).Should().BeTrue();
            vertices.Contains(Point.MakePointWithInches(-3, -2, 0)).Should().BeTrue();
            vertices.Contains(Point.MakePointWithInches(1, 1, -1)).Should().BeTrue();

            List<LineSegment> nonClosed = new List<LineSegment>();
            nonClosed.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(1, 0, 0)));
            nonClosed.Add(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 2, 0)));

            List<Point> points = nonClosed.GetAllPoints();

            points.Count.Should().Be(4);
            points.Contains(Point.MakePointWithInches(0, 0, 0)).Should().BeTrue();
            points.Contains(Point.MakePointWithInches(1, 0, 0)).Should().BeTrue();
            points.Contains(Point.MakePointWithInches(4, 0, 0)).Should().BeTrue();
            points.Contains(Point.MakePointWithInches(4, 2, 0)).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_SortIntoClockWiseSegments()
        {
            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 4, 2));
            LineSegment segment2 = new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(3, 1, 0));
            LineSegment segment3 = new LineSegment(Point.MakePointWithInches(0, 4, 2), Point.MakePointWithInches(3, 4, 2));
            LineSegment segment4 = new LineSegment(Point.MakePointWithInches(3, 1, 0), Point.MakePointWithInches(3, 4, 2));

            List<LineSegment> lineSegments = new List<LineSegment>() { segment1, segment2, segment3, segment4 };

            List<LineSegment> sorted = (List<LineSegment>)lineSegments.SortSegments();

            (sorted[0] == segment1).Should().BeTrue();
            (sorted[1] == segment3).Should().BeTrue();
            (sorted[2] == new LineSegment(Point.MakePointWithInches(3, 4, 2), Point.MakePointWithInches(3, 1, 0))).Should().BeTrue();
            (sorted[3] == new LineSegment(Point.MakePointWithInches(3, 1, 0), Point.MakePointWithInches(0, 1, 0))).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_MakeIntoLineSegmentsThatMeet()
        {
            
            List<LineSegment> lineSegments =
                new List<Point> {
                    Point.MakePointWithInches(0.000,  0.000),
                    Point.MakePointWithInches(0.000,  0.250),
                    Point.MakePointWithInches(6.500,  3.500),
                    Point.MakePointWithInches(144.000,  3.500),
                    Point.MakePointWithInches(144.000,  0.000)
                }.MakeIntoLineSegmentsThatMeet();

            lineSegments.Count.Should().Be(5);


            
            
        }
    }
}