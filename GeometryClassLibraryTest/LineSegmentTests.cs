using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;
namespace ClearspanTypeLibrary.Tests
{
    [TestFixture()]
    public class LineSegmentTests
    {
        [Test()]
        public void LineSegment_ConstructorTest()
        {
            //Fixture fixture = new Fixture();
            //fixture.Customize<Distance>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //var mem = fixture.Create<Distance>();

            //fixture.Customize<Member>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            ////fixture.Customize<Line>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //fixture.Customize<Point>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
            //var memb = fixture.Create<Member>();
            Assert.AreEqual("", "");
        }

        [Test()]
        public void LineSegment_IntersectionOriginTest()
        {
            LineSegment verticalLine = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment flatLine = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(5, 0));

            verticalLine.Intersection(flatLine).Should().Be(PointGenerator.MakePointWithInches(0, 0));
        }

        [Test()]
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

        [Test()]
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

        [Test()]
        public void LineSegment_MidpointTest()
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(2, 2, 2));

            line1.MidPoint.Should().Be(PointGenerator.MakePointWithInches(1, 1, 1));
        }

        [Test()]
        public void LineSegment_HypotheticalIntersectionTest()
        {
            LineSegment line1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-5, -5, 0));
            LineSegment line2 = new LineSegment(PointGenerator.MakePointWithInches(5, -4, 0), PointGenerator.MakePointWithInches(-5, 6, 0));

            Point intersectT1 = line1.HypotheticalIntersection(line2);

            intersectT1.Should().Be(PointGenerator.MakePointWithInches(.5, .5, 0));
        }

        [Test()]
        public void LineSegment_LineSegmentOverlappingEquality()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 5), PointGenerator.MakePointWithInches(0, 0));

            segment1.Should().Be(segment2);
        }

        [Test()]
        public void LineSegment_EqualityOperator()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));

            segment1.Should().Be(segment2);
        }

        [Test()]
        public void LineSegment_InequalityOperator()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));

            segment1.Should().Be(segment2);
        }

        [Test()]
        public void LineSegment_3dRotateTest_Orthogonal()
        {
            LineSegment originalSegment = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 5, 0));
            Line axis = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0));
            Angle toRotate = new Angle(AngleType.Degree, 180);

            LineSegment actualSegment = originalSegment.Rotate(new Rotation(axis, toRotate));
            LineSegment expectedSegment = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, -5, 0));
            (actualSegment == expectedSegment).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_3dRotateTest()
        {

            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));
            LineSegment segment2 = new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3));

            Line rotationAxis = new Line(new Direction(PointGenerator.MakePointWithInches(1, 1, 1)), PointGenerator.MakePointWithInches(1, -1, -1));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);
            Rotation toRotate = new Rotation(rotationAxis, rotationAngle);

            LineSegment actualSegment1 = segment1.Rotate(toRotate);
            LineSegment actualSegment2 = segment2.Rotate(toRotate);

            LineSegment expectedSegment1 = new LineSegment(PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), PointGenerator.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            LineSegment expectedSegment2 = new LineSegment(PointGenerator.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495), PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
            (actualSegment2 == expectedSegment2).Should().BeTrue();


        }

        [Test()]
        public void LineSegment_TranslateTest()
        {
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(1, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));

            //Direction testDirection = new Direction(PointGenerator.MakePointWithInches(-1, 5, 4));
            //Distance testDisplacement = new Distance(DistanceType.Inch, 12.9614814);
            Point testDisplacement = PointGenerator.MakePointWithInches(-2, 10, 8);

            LineSegment actualSegment1 = segment1.Translate(testDisplacement);

            LineSegment expectedSegment1 = new LineSegment(PointGenerator.MakePointWithInches(-1, 12, 11), PointGenerator.MakePointWithInches(-5, 8, 8));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Contains()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(5, 0));
        }

        [Test()]
        public void LineSegment_ProjectOntoLine2DThroughOrigin()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(2, 5));
            Line projectOnto = new Line(PointGenerator.MakePointWithInches(2, 1));
            LineSegment result = testSegment.ProjectOntoLine(projectOnto);

            LineSegment expected = new LineSegment(PointGenerator.MakePointWithInches(3.6, 1.8));

            //make sure the result is actually along the projected direction
            result.Direction.Should().Be(projectOnto.Direction);


            //make sure the expected and result are along the same line
            result.Direction.Should().Be(expected.Direction);

            //now check the actual projected line
            result.BasePoint.Should().Be(expected.BasePoint);
            result.EndPoint.Should().Be(expected.EndPoint);
        }

        [Test()]
        public void LineSegment_ProjectOntoLine3DNotThroughOrigin()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(2, 0, 4), PointGenerator.MakePointWithInches(0, 2, 1));
            Line projectOnto = new Line(PointGenerator.MakePointWithInches(5, 1, 2), PointGenerator.MakePointWithInches(1, 4, 0));
            LineSegment result = testSegment.ProjectOntoLine(projectOnto);

            LineSegment expected = new LineSegment(PointGenerator.MakePointWithInches(5 - 0.689655, 1 + 0.517242, 2 - 0.344828), PointGenerator.MakePointWithInches(5 - 3.448276, 1 + 2.586207, 2 - 1.724138));

            //make sure the result is actually along the right line
            result.Direction.Should().Be(projectOnto.Direction);

            //make sure the expected and result are along the same line
            result.Direction.Should().Be(expected.Direction);

            result.BasePoint.Should().Be(expected.BasePoint);
            result.EndPoint.Should().Be(expected.EndPoint);
        }

        [Test()]
        public void LineSegment_SliceWithPoint()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(2, 0, 4), PointGenerator.MakePointWithInches(0, 2, 1));
            Point midPoint = PointGenerator.MakePointWithInches(1, 1, 2.5);
            Point notOnLine = PointGenerator.MakePointWithInches(1, 1, 2);
            Point onLine = PointGenerator.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751);

            List<LineSegment> expectedwithMidPoint = new List<LineSegment>();
            expectedwithMidPoint.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 4), PointGenerator.MakePointWithInches(1, 1, 2.5)));
            expectedwithMidPoint.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, 2.5), PointGenerator.MakePointWithInches(0, 2, 1)));

            List<LineSegment> splitMidPoint = testSegment.Slice(midPoint);

            //its the midpoint so the lengths should be the same and the order is ambiguos
            (splitMidPoint[0].Length == splitMidPoint[1].Length).Should().BeTrue();

            //now check of the segments are the same
            (splitMidPoint.Count == expectedwithMidPoint.Count).Should().BeTrue();
            foreach (LineSegment line in expectedwithMidPoint)
            {
                splitMidPoint.Contains(line).Should().BeTrue();
            }

            //now check to make sure it works right for a point not on the line
            List<LineSegment> offPointResult = testSegment.Slice(notOnLine);

            //now check of the segments are the same
            (offPointResult.Count == 1).Should().BeTrue();
            (offPointResult[0] == testSegment).Should().BeTrue();

            //now try a more generic point
            List<LineSegment> expectedOnLine = new List<LineSegment>();
            expectedOnLine.Add(new LineSegment(PointGenerator.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751), PointGenerator.MakePointWithInches(0, 2, 1)));
            //thiss second one is the unitvector so it is only 1 in length and is smaller so it should go second
            expectedOnLine.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 4), PointGenerator.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751)));

            List<LineSegment> splitOnLine = testSegment.Slice(onLine);
            //now check of the segments are the same
            (splitOnLine.Count == expectedOnLine.Count).Should().BeTrue();
            for (int i = 0; i < splitOnLine.Count; i++)
            {
                (splitOnLine[i] == expectedOnLine[i]).Should().BeTrue();
            }
        }

        [Test()]
        public void LineSegment_ReverseTest()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(2, 0, 4), PointGenerator.MakePointWithInches(0, 2, 1));
            LineSegment result = testSegment.Reverse();
            result.BasePoint.Should().Be(testSegment.EndPoint);
            result.Length.Should().Be(testSegment.Length);
            Direction expectedDirection = new Direction(PointGenerator.MakePointWithInches(0, 2, 1), PointGenerator.MakePointWithInches(2, 0, 4));
            result.Direction.Should().Be(expectedDirection);
        }
    }
}