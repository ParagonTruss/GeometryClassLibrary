using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class LineListExtensionTests
    {
        [Test]
        public void LineList_AreAllCoplanar()
        {
            List<LineSegment> testList1 = new List<Point>
                {
                    Point.MakePointWithInches(1,1,1),
                    Point.MakePointWithInches(1,2,1),
                    Point.MakePointWithInches(1,2,2),
                    Point.MakePointWithInches(1,1,2)
                }
                .MakeIntoLineSegmentsThatMeet();

            testList1.AreAllCoplanar().Should().BeTrue();

            List<LineSegment> testList2 = new List<LineSegment>();
            testList2.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 1, 0)));
            testList2.Add(new LineSegment(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(0, 1, 1)));
            testList2.Add(new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, 1, 0)));

            //Pairwise these segments are coplanar
            bool bool2 = testList2.AreAllCoplanar();
            bool2.Should().BeFalse();

            testList2.Add(new LineSegment(Point.MakePointWithInches(3, 2, 1), Point.MakePointWithInches(5, -11, 6)));
            bool bool3 = testList2.AreAllCoplanar();
            bool3.Should().BeFalse();

            //null case; should be vacuously true
            (new List<LineSegment>()).AreAllCoplanar().Should().BeTrue();
        }

        [Test()]
        public void LineList_AreAllParallel()
        {
            List<LineSegment> horizontalList = new List<LineSegment>
                {
                    new LineSegment(Point.MakePointWithInches(0,0), Point.MakePointWithInches(5,0)),
                    new LineSegment(Point.MakePointWithInches(-3,-3), Point.MakePointWithInches(8,-3)),
                    new LineSegment(Point.MakePointWithInches(1,1), Point.MakePointWithInches(4,1))
                };

            horizontalList.AreAllParallel().Should().BeTrue();

            horizontalList.Add(new LineSegment(Point.MakePointWithInches(0, 1), Point.MakePointWithInches(5, 0)));

            horizontalList.AreAllParallel().Should().BeFalse();

            //null case; should be vacuously true
            (new List<LineSegment>()).AreAllParallel().Should().BeTrue();
        }

        [Test()]
        public void LineList_SmallestXInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithSmallestXInterceptIn2D();

            result.Should().Be(line1);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithSmallestXInterceptIn2D();

            result2.Should().Be(line1);
        }

        [Test()]
        public void LineList_LargestXInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithLargestXInterceptIn2D();

            result.Should().Be(line2);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithLargestXInterceptIn2D();

            result2.Should().Be(line2);
        }

        [Test()]
        public void LineList_LineWithXInterceptIn2DClosestTo()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithXInterceptIn2DClosestTo(new Distance(DistanceType.Inch, 3));

            result.Should().Be(line4);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithXInterceptIn2DClosestTo(new Distance(DistanceType.Inch, 0));

            result2.Should().Be(line3);
        }

        [Test()]
        public void LineList_LineWithXInterceptIn2DFarthestFrom()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithXInterceptIn2DFarthestFrom(new Distance(DistanceType.Inch, 3));

            result.Should().Be(line1);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithXInterceptIn2DFarthestFrom(new Distance(DistanceType.Inch, 0));

            result2.Should().Be(line2);
        }

        [Test()]
        public void LineList_SmallestYInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line2, line4, line1, line5, line3 };

            Line result = lines.LineWithSmallestYInterceptIn2D();

            result.Should().Be(line4);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithSmallestYInterceptIn2D();

            result2.Should().Be(line4);
        }

        [Test()]
        public void LineList_LargestYInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithLargestYInterceptIn2D();

            result.Should().Be(line2);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithLargestYInterceptIn2D();

            result2.Should().Be(line2);
        }

        [Test()]
        public void LineList_LineWithYInterceptIn2DClosestTo()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithYInterceptIn2DClosestTo(new Distance(DistanceType.Inch, 1.3));

            result.Should().Be(line2);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithYInterceptIn2DClosestTo(new Distance(DistanceType.Inch, -0.25));

            result2.Should().Be(line3);
        }

        [Test()]
        public void LineList_LineWithYInterceptIn2DFarthestFrom()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithYInterceptIn2DFarthestFrom(new Distance(DistanceType.Inch, 1.3));

            result.Should().Be(line4);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithYInterceptIn2DFarthestFrom(new Distance(DistanceType.Inch, -0.25));

            result2.Should().Be(line2);
        }

        [Test()]
        public void LineList_OnlyLinesParallelTo()
        {
            Line referenceLine = new Line(Point.MakePointWithInches(0, 2, 1), Point.MakePointWithInches(2, -1, 0));

            Line parallel1 = new Line(Point.MakePointWithInches(1, 2, 1), Point.MakePointWithInches(3, -1, 0));
            Line parallel2 = new Line(Point.MakePointWithInches(2, 0.5, .5), Point.MakePointWithInches(3, -1, 0));
            Line parallel3 = new Line(Point.MakePointWithInches(-1, -1, -2), Point.MakePointWithInches(1, -4, -3));
            Line parallel4 = new Line(Point.MakePointWithInches(6, -2, 0), Point.MakePointWithInches(2, 4, 2));
            Line notParallel1 = new Line(Point.MakePointWithInches(0, 2, 1), Point.MakePointWithInches(3, -1, 0));
            Line notParallel2 = new Line(Point.MakePointWithInches(-2, 1, 1), Point.MakePointWithInches(0, 1, 0));
            Line notParallel3 = new Line(Point.MakePointWithInches(0.75, 1.5, 0.5), Point.MakePointWithInches(-0.25, 2.25, 0.75));

            List<Line> lines = new List<Line>() { parallel1, notParallel1, parallel2, parallel3, notParallel2, parallel4, notParallel3, notParallel1 };

            List<Line> resultsParallel = lines.OnlyLinesParallelTo(referenceLine);

            List<Line> expectedParallel = new List<Line>() { parallel1, parallel2, parallel3, parallel4 };

            resultsParallel.Should().Contain(expectedParallel);
        }
    }
}
