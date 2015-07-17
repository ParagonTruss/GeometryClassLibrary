using NUnit.Framework;
using FluentAssertions;
using GeometryClassLibrary;
using System.Collections.Generic;

namespace GeometryClassLibraryTests
{
    [TestFixture]
    public class PointListTests
    {
        [Test]
        public void PointList_MakeIntoLineSegmentsThatMeetTest()
        {
            List<Point> myPoints = new List<Point> {
                    PointGenerator.MakePointWithInches(0.000,  0.000),
                    PointGenerator.MakePointWithInches(0.000,  0.250),
                    PointGenerator.MakePointWithInches(6.500,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  0.000)
                };

            List<LineSegment> lines =  myPoints.MakeIntoLineSegmentsThatMeet();

            lines.Count.Should().Be(5);
        }
    }
}
