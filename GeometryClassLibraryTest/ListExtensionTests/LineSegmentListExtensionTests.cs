using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using static GeometryClassLibrary.Point;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class LineSegmentListExtensionTests
    {

        [Test()]
        public void LineSegmentList_DoFormClosedRegionTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(MakePointWithInches(0, 2, 3), MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(MakePointWithInches(-3, -2, 0), MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(MakePointWithInches(1, 1, -1), MakePointWithInches(0, 2, 3)));

            lineSegments.DoFormClosedRegion().Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_GetPoints()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(MakePointWithInches(0, 2, 3), MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(MakePointWithInches(-3, -2, 0), MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(MakePointWithInches(1, 1, -1), MakePointWithInches(0, 2, 3)));

            List<Point> vertices = lineSegments.GetAllPoints();

            vertices.Count.Should().Be(3);
            vertices.Contains(MakePointWithInches(0, 2, 3)).Should().BeTrue();
            vertices.Contains(MakePointWithInches(-3, -2, 0)).Should().BeTrue();
            vertices.Contains(MakePointWithInches(1, 1, -1)).Should().BeTrue();

            List<LineSegment> nonClosed = new List<LineSegment>();
            nonClosed.Add(new LineSegment(MakePointWithInches(0, 0, 0), MakePointWithInches(1, 0, 0)));
            nonClosed.Add(new LineSegment(MakePointWithInches(4, 0, 0), MakePointWithInches(4, 2, 0)));

            List<Point> points = nonClosed.GetAllPoints();

            points.Count.Should().Be(4);
            points.Contains(MakePointWithInches(0, 0, 0)).Should().BeTrue();
            points.Contains(MakePointWithInches(1, 0, 0)).Should().BeTrue();
            points.Contains(MakePointWithInches(4, 0, 0)).Should().BeTrue();
            points.Contains(MakePointWithInches(4, 2, 0)).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_SortSegments()
        {
            LineSegment segment1 = new LineSegment(MakePointWithInches(0, 1, 0), MakePointWithInches(0, 4, 2));
            LineSegment segment2 = new LineSegment(MakePointWithInches(0, 1, 0), MakePointWithInches(3, 1, 0));
            LineSegment segment3 = new LineSegment(MakePointWithInches(0, 4, 2), MakePointWithInches(3, 4, 2));
            LineSegment segment4 = new LineSegment(MakePointWithInches(3, 1, 0), MakePointWithInches(3, 4, 2));

            List<LineSegment> lineSegments = new List<LineSegment>() { segment1, segment2, segment3, segment4 };

            List<LineSegment> sorted = lineSegments.FixSegmentOrientation();

            (sorted[0] == segment1).Should().BeTrue();
            (sorted[1] == segment3).Should().BeTrue();
            (sorted[2] == new LineSegment(MakePointWithInches(3, 4, 2), MakePointWithInches(3, 1, 0))).Should().BeTrue();
            (sorted[3] == new LineSegment(MakePointWithInches(3, 1, 0), MakePointWithInches(0, 1, 0))).Should().BeTrue();
        }

        [Test()]
        public void LineSegmentList_MakeIntoLineSegmentsThatMeet()
        {
            
            List<LineSegment> lineSegments =
                new List<Point> {
                    MakePointWithInches(0.000,  0.000),
                    MakePointWithInches(0.000,  0.250),
                    MakePointWithInches(6.500,  3.500),
                    MakePointWithInches(144.000,  3.500),
                    MakePointWithInches(144.000,  0.000)
                }.MakeIntoLineSegmentsThatMeet();

            lineSegments.Count.Should().Be(5);


            
            
        }
    }
}