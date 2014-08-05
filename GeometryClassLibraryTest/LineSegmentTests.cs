﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;
namespace ClearspanTypeLibrary.Tests
{
    [TestClass()]
    public class LineSegmentTests
    {
        [TestMethod()]
        public void LineSegment_ConstructorTest()
        {
            //Fixture fixture = new Fixture();
            //fixture.Customize<Dimension>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //var mem = fixture.Create<Dimension>();

            //fixture.Customize<Member>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            ////fixture.Customize<Line>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //fixture.Customize<Point>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //var memb = fixture.Create<Member>();
            Assert.AreEqual("","");
        }

        [TestMethod()]
        public void LineSegment_IntersectionOriginTest()
        {
            LineSegment verticalLine = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment flatLine = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(5, 0));

            verticalLine.Intersection(flatLine).Should().Be(PointGenerator.MakePointWithInches(0, 0));
        }

        [TestMethod()]
        public void LineSegment_SegmentIntersectionTest()
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 1, 1));
            LineSegment line2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 1), PointGenerator.MakePointWithInches(1, 1, 0));
            LineSegment line3 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(3, -4, 2));
            LineSegment line4 = new LineSegment(PointGenerator.MakePointWithInches(0, -4, 0), PointGenerator.MakePointWithInches(3, 0, 2));

            Point intersectT1 = line1.Intersection(line2);
            Point intersectT2 = line3.Intersection(line4);
            Point intersectF1 = line1.Intersection(line4);

            intersectT1.Should().Be(PointGenerator.MakePointWithInches(.5, .5, .5));
            intersectT2.Should().Be(PointGenerator.MakePointWithInches(1.5, -2, 1));
            intersectF1.Should().BeNull();
        }

        [TestMethod()]
        public void LineSegment_LineIntersectionTest() 
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 1, 1));
            Line line2 = new Line(PointGenerator.MakePointWithInches(0, 0, 1), PointGenerator.MakePointWithInches(1, 1, 0));
            LineSegment line3 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-5, -5, 0));
            Line line4 = new Line(PointGenerator.MakePointWithInches(5, -4, 0), PointGenerator.MakePointWithInches(-5, 6, 0));

            Point intersectT1 = line1.Intersection(line2);
            Point intersectF1 = line3.Intersection(line4);

            intersectT1.Should().Be(PointGenerator.MakePointWithInches(.5, .5, .5));
            intersectF1.Should().BeNull();
        }

        [TestMethod()]
        public void LineSegment_MidpointTest()
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(2, 2, 2));

            line1.MidPoint.Should().Be(PointGenerator.MakePointWithInches(1, 1, 1));
        }

        [TestMethod()]
        public void LineSegment_HypotheticalIntersectionTest()
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-5, -5, 0));
            LineSegment line2 = new LineSegment(PointGenerator.MakePointWithInches(5, -4, 0), PointGenerator.MakePointWithInches(-5, 6, 0));

            Point intersectT1 = line1.HypotheticalIntersection(line2);

            intersectT1.Should().Be(PointGenerator.MakePointWithInches(.5, .5, 0));
        }

        [TestMethod()]
        public void LineSegment_LineSegmentOverlappingEquality()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 5), PointGenerator.MakePointWithInches(0, 0));

            segment1.Should().Be(segment2);
        }

        [TestMethod()]
        public void LineSegment_EqualityOperator()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));

            segment1.Should().Be(segment2);
        }

        [TestMethod()]
        public void LineSegment_InequalityOperator()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));

            segment1.Should().Be(segment2);
        }

        [TestMethod()]
        public void LineSegment_3dRotateTest_Orthogonal()
        {
            LineSegment originalSegment = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 5, 0));
            Line axis = new Line(PointGenerator.MakePointWithInches(0,0,0), PointGenerator.MakePointWithInches(1, 0, 0));
            Angle toRotate = new Angle(AngleType.Degree, 180);

            LineSegment actualSegment = originalSegment.Rotate(axis, toRotate);
            LineSegment expectedSegment = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, -5, 0));
            (actualSegment == expectedSegment).Should().BeTrue();
        }

        [TestMethod()]
        public void LineSegment_3dRotateTest()
        {

            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3));

            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, -1, -1), new Vector(PointGenerator.MakePointWithInches(1, 1, 1)));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            LineSegment actualSegment1 = segment1.Rotate(rotationAxis, rotationAngle);
            LineSegment actualSegment2 = segment2.Rotate(rotationAxis, rotationAngle);

            LineSegment expectedSegment1 = new LineSegment(PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), PointGenerator.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            LineSegment expectedSegment2 = new LineSegment(PointGenerator.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495), PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
            (actualSegment2 == expectedSegment2).Should().BeTrue();


        }

        [TestMethod()]
        public void LineSegment_TranslateTest()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithMillimeters(1, 2, 3), PointGenerator.MakePointWithMillimeters(-3, -2, 0));

            Vector testDirectionVector = new Vector(PointGenerator.MakePointWithMillimeters(-1, 5, 4));
            Dimension testDisplacement = new Dimension(DimensionType.Millimeter, 12.9614814);

            LineSegment actualSegment1 = segment1.Translate(testDirectionVector, testDisplacement);

            LineSegment expectedSegment1 = new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 12, 11), PointGenerator.MakePointWithMillimeters(-5, 8, 8));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
        }

        [TestMethod()]
        public void LineSegment_Contains()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(5, 0));
        }
    }
}
