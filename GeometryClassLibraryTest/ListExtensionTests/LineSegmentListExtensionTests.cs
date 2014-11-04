using System;
using GeometryClassLibraryTests;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture]
    public class LineSegmentListExtensionTests
    {

        [Test()]
        public void LineSegmentList_DoFormClosedRegionTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));

            lineSegments.DoFormClosedRegion().Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_GetPoints()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));

            List<Point> verticies = lineSegments.GetAllPoints();

            verticies.Count.Should().Be(3);
            verticies.Contains(PointGenerator.MakePointWithInches(0, 2, 3)).Should().BeTrue();
            verticies.Contains(PointGenerator.MakePointWithInches(-3, -2, 0)).Should().BeTrue();
            verticies.Contains(PointGenerator.MakePointWithInches(1, 1, -1)).Should().BeTrue();

            List<LineSegment> nonClosed = new List<LineSegment>();
            nonClosed.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0)));
            nonClosed.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 2, 0)));

            List<Point> points = nonClosed.GetAllPoints();

            points.Count.Should().Be(4);
            points.Contains(PointGenerator.MakePointWithInches(0, 0, 0)).Should().BeTrue();
            points.Contains(PointGenerator.MakePointWithInches(1, 0, 0)).Should().BeTrue();
            points.Contains(PointGenerator.MakePointWithInches(4, 0, 0)).Should().BeTrue();
            points.Contains(PointGenerator.MakePointWithInches(4, 2, 0)).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_SortClockwiseTest()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 4, 2));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(3, 1, 0));
            LineSegment segment3 = new LineSegment(PointGenerator.MakePointWithInches(0, 4, 2), PointGenerator.MakePointWithInches(3, 4, 2));
            LineSegment segment4 = new LineSegment(PointGenerator.MakePointWithInches(3, 1, 0), PointGenerator.MakePointWithInches(3, 4, 2));

            List<LineSegment> lineSegments = new List<LineSegment>() { segment1, segment2, segment3, segment4 };

            List<LineSegment> sorted = (List<LineSegment>)lineSegments.SortIntoClockWiseSegments();

            (sorted[0] == segment1).Should().BeTrue();
            (sorted[1] == segment3).Should().BeTrue();
            (sorted[2] == new LineSegment(PointGenerator.MakePointWithInches(3, 4, 2), PointGenerator.MakePointWithInches(3, 1, 0))).Should().BeTrue();
            (sorted[3] == new LineSegment(PointGenerator.MakePointWithInches(3, 1, 0), PointGenerator.MakePointWithInches(0, 1, 0))).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_MakeIntoLineSegmentsThatMeet()
        {
            
            List<LineSegment> lineSegments =
                new List<Point> {
                    PointGenerator.MakePointWithInches(0.000,  0.000),
                    PointGenerator.MakePointWithInches(0.000,  0.250),
                    PointGenerator.MakePointWithInches(6.500,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  0.000)
                }.MakeIntoLineSegmentsThatMeet();

            lineSegments.Count.Should().Be(5);


            
            
        }
    }
}