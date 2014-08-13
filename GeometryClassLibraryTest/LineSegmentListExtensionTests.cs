﻿using System;

using GeometryClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestClass]
    public class LineSegmentListExtensionTests
    {

        [TestMethod()]
        public void LineSegmentList_DoFormClosedRegionTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));

            lineSegments.DoFormClosedRegion().Should().BeTrue();
        }
    }
}