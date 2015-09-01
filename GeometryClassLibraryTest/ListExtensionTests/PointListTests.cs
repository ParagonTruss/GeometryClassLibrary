using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class PointListTests
    {
        [Test]
        public void PointList_MakeIntoLineSegmentsThatMeetTest()
        {
            List<Point> myPoints = new List<Point> {
                    Point.MakePointWithInches(0.000,  0.000),
                    Point.MakePointWithInches(0.000,  0.250),
                    Point.MakePointWithInches(6.500,  3.500),
                    Point.MakePointWithInches(144.000,  3.500),
                    Point.MakePointWithInches(144.000,  0.000)
                };

            List<LineSegment> lines =  myPoints.MakeIntoLineSegmentsThatMeet();

            lines.Count.Should().Be(5);
        }
    }
}
