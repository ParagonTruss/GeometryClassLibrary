using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using FluentAssertions;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestClass]
    public class LineListExtensionTests
    {
        [TestMethod]
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

        [TestMethod()]
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
    }
}
