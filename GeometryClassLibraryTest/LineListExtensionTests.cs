using System;
using NUnit.Framework;
using System.Collections.Generic;

using FluentAssertions;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture]
    public class LineListExtensionTests
    {
        [Test]
        public void LineListExtension_AreAllCoplanarTest()
        {
            List<LineSegment> testList = new List<Point>
                {
                    PointGenerator.MakePointWithInches(1,1,1),
                    PointGenerator.MakePointWithInches(1,2,1),
                    PointGenerator.MakePointWithInches(1,2,2),
                    PointGenerator.MakePointWithInches(1,1,2)
                }
                .MakeIntoLineSegmentsThatMeet();

            testList.AreAllCoplanar().Should().BeTrue();
        }

        [Test()]
        public void LineListExtension_AreAllParallelTest()
        {
            List<LineSegment> horizontalList = new List<LineSegment>
                {
                    new LineSegment(PointGenerator.MakePointWithInches(0,0), PointGenerator.MakePointWithInches(5,0)),
                    new LineSegment(PointGenerator.MakePointWithInches(-3,-3), PointGenerator.MakePointWithInches(8,-3)),
                    new LineSegment(PointGenerator.MakePointWithInches(1,1), PointGenerator.MakePointWithInches(4,1))
                };

            horizontalList.AreAllParallel().Should().BeTrue();
        }

        [Test()]
        public void LineListExtension_SmallestXInterceptIn2D()
        {
            Line smallest = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, -1)); //intersects at -1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 2, 5), PointGenerator.MakePointWithInches(1, 3, 4)); //intersects at 1
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(2, 2, 1)); //intersects at 0
            List<Line> lines = new List<Line> {line2, line4, smallest, line3};

            Line result = lines.LineWithSmallestXInterceptIn2D();

            result.Should().Be(smallest);
        }
    }
}
