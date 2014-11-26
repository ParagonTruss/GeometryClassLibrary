﻿using System;
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
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithSmallestXInterceptIn2D();

            result.Should().Be(line1);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithSmallestXInterceptIn2D();

            result2.Should().Be(line1);
        }

        [Test()]
        public void LineListExtension_LargestXInterceptIn2D()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, 7)); //intersects at -1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithLargestXInterceptIn2D();

            result.Should().Be(line2);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithLargestXInterceptIn2D();

            result2.Should().Be(line2);
        }

        [Test()]
        public void LineListExtension_SmallestYInterceptIn2D()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(2, 2, 2), PointGenerator.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(PointGenerator.MakePointWithInches(2, 1, 1), PointGenerator.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line2, line4, line1, line5, line3 };

            Line result = lines.LineWithSmallestYInterceptIn2D();

            result.Should().Be(line4);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithSmallestYInterceptIn2D();

            result2.Should().Be(line4);
        }

        [Test()]
        public void LineListExtension_LargestYInterceptIn2D()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(2, 2, 2), PointGenerator.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(PointGenerator.MakePointWithInches(2, 1, 1), PointGenerator.MakePointWithInches(3, 2, 2)); //intersects at -1
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line4, line2, line1, line5, line3 };

            Line result = lines.LineWithLargestYInterceptIn2D();

            result.Should().Be(line2);

            //check it can handle a null intersect in the first spot
            List<Line> lines2 = new List<Line> { line5, line2, line1, line4, line3 };

            Line result2 = lines2.LineWithLargestYInterceptIn2D();

            result2.Should().Be(line2);
        }

    }
}