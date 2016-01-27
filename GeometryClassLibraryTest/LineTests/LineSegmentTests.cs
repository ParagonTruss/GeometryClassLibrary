using System;
using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class LineSegmentTests
    {
        [Test()]
        [Ignore("JSON")]
        public void LineSegment_JSON()
        {
            LineSegment lineSegment = new LineSegment(Point.Origin, Point.MakePointWithInches(1, 1, 1));

            var json = JsonConvert.SerializeObject(lineSegment);
            LineSegment deserializedLineSegment = JsonConvert.DeserializeObject<LineSegment>(json);

            bool areEqual = (lineSegment == deserializedLineSegment);
            areEqual.Should().BeTrue();
        }

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
        public void LineSegment_GetHashCode_ShouldEqualVectorHashCode()
        {
            Point endPoint = Point.MakePointWithInches(1, 1, 1);

            LineSegment lineSegment = new LineSegment(endPoint);

            Vector vector = new Vector(lineSegment.BasePoint, lineSegment.EndPoint);

            lineSegment.GetHashCode().Should().Be(vector.GetHashCode());
        }

        [Test()]
        public void LineSegment_EqualityOperator_ShouldReturnTrue_IfBothNull()
        {
            LineSegment nullLineSegment1 = null;
            LineSegment nullLineSegment2 = null;

            (nullLineSegment1 == nullLineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_EqualityOperator_ShouldReturnFalse_IfOnlyFirstSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            (nullLineSegment == lineSegment).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_EqualityOperator_ShouldBeSameAsEquals()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment1 == lineSegment2).ShouldBeEquivalentTo(lineSegment1.Equals(lineSegment2));
        }

        [Test()]
        public void LineSegment_InequalityOperator_ShouldReturnFalse_IfBothNull()
        {
            LineSegment nullLineSegment1 = null;
            LineSegment nullLineSegment2 = null;

            (nullLineSegment1 != nullLineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_InequalityOperator_ShouldReturnTrue_IfOnlyFirstSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            (nullLineSegment != lineSegment).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_InequalityOperator_ShouldBeSameAsNotEquals()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment1 != lineSegment2).Should().Be(!lineSegment1.Equals(lineSegment2));
        }

        [Test()]
        public void LineSegment_GreaterThanOperator_ShouldThrowException_IfFirstSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            Func<bool> comparison = () => nullLineSegment > lineSegment;
            Action action = () => comparison();

            action.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_GreaterThanOperator_ShouldThrowException_IfSecondSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            Func<bool> comparison = () => lineSegment > nullLineSegment;
            Action action = () => comparison();

            action.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_GreaterThanOperator_ShouldReturnTrue_IfFirstSegmentHasGreaterMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 6));

            (lineSegment2 > lineSegment1).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_GreaterThanOperator_ShouldReturnFalse_IfFirstSegmentDoesNotHaveGreaterMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 6));

            (lineSegment1 > lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_GreaterThanOperator_ShouldReturnFalse_IfFirstHasEqualMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment1 > lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_LessThanOperator_ShouldThrowException_IfFirstSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            Func<bool> comparison = () => nullLineSegment < lineSegment;
            Action action = () => comparison();

            action.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_LessThanOperator_ShouldThrowException_IfSecondSegmentIsNull()
        {
            LineSegment nullLineSegment = null;
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));

            Func<bool> comparison = () => lineSegment < nullLineSegment;
            Action action = () => comparison();

            action.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_LessThanOperator_ShouldReturnTrue_IfFirstSegmentHasLessMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 6));

            (lineSegment1 < lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_LessThanOperator_ShouldReturnFalse_IfFirstSegmentDoesNotHaveLessMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 6));

            (lineSegment2 < lineSegment1).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_LessThanOperator_ShouldReturnFalse_IfFirstSegmentHasEqualMagnitude()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment2 < lineSegment1).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnFalse_IfObjectIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment nullLineSegment = null;

            lineSegment.Equals(nullLineSegment).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnFalse_IfObjectIsNotLineSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            Object nonLineSegment = new Object();

            lineSegment.Equals(nonLineSegment).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnTrue_IfOnlyBaseOrEndPointsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            lineSegment1.Equals(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnTrue_IfOnlyReverseBaseOrEndPointsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(5, 0));

            lineSegment1.Equals(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnFalse_IfNeitherBaseOrEndPointsAndReverseBaseOrEndPointsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(6, 1));

            lineSegment1.Equals(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Equals_ShouldReturnFalse_IfParameterIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment nullLineSegment = null;

            lineSegment.Equals(nullLineSegment).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_CompareTo_ShouldReturn0_IfSegmentLengthsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(5, 0));

            lineSegment1.CompareTo(lineSegment2).Should().Be(0);
        }

        [Test()]
        public void LineSegment_CompareTo_ShouldReturnLengthCompareTo_IfSegmentLengthsAreNotEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 6));

            lineSegment1.CompareTo(lineSegment2).Should().Be(lineSegment1.Length.CompareTo(lineSegment2.Length));
        }

        //In order to stay consistent with Object.CompareTo(), the instance should be considered greater than null
        [Test()]
        public void LineSegment_CompareTo_ShouldReturn1_IfParameterIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment nullLineSegment = null;

            lineSegment.CompareTo(nullLineSegment).Should().BeGreaterThan(0);
        }

        [Test()]
        public void LineSegment_ToString_ShouldReturnFormattedString()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));

            lineSegment1.ToString().Should().Be(string.Format("BasePoint= {0}, EndPoint= {1}, Length={2}", lineSegment1.BasePoint.ToString(), lineSegment1.EndPoint.ToString(), lineSegment1.Length.ToString()));
        }

        [Test()]
        public void LineSegment_IntersectWithLine_ShouldEqualLineIntersect()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Line line = new Line(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(1, 1, 0));

            Line lineSegmentToLine = new Line(lineSegment);

            lineSegment.IntersectWithLine(line).Should().Be(lineSegmentToLine.IntersectWithLine(line));
        }

        [Test()]
        public void LineSegment_IntersectWithLine_ShouldReturnNull_IfLinesDoNotIntersect()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 0, 1), Point.MakePointWithInches(1, 0, 2));
            Line line = new Line(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(0, 0, 2));

            lineSegment.IntersectWithLine(line).Should().BeNull();
        }

        [Test()]
        public void LineSegment_IntersectWithLine_ShouldReturnNull_IfLinesIntersectButNotOnSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));
            Line line = new Line(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(1, 1, 0));

            lineSegment.IntersectWithLine(line).Should().BeNull();
        }

        [Test()]
        public void LineSegment_IntersectWithLine_ShouldBeLineIntersection_IfLineSegmentEndPointIsOnLine()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Line line = new Line(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(0, 0, 2));

            Line lineSegmentToLine = new Line(lineSegment);

            lineSegment.IntersectWithLine(line).Should().Be(lineSegmentToLine.IntersectWithLine(line));
        }

        [Test()]
        public void LineSegment_IntersectWithLine_ShouldThrowNullPointerException_IfLineIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Line line = null;

            Action intersect = () => lineSegment.IntersectWithLine(line);
            intersect.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnBasePoint_IfBasePointIsBaseOrEndPointOfOtherSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(-1, -1, -1));

            lineSegment1.IntersectWithSegment(lineSegment2).Should().Be(lineSegment1.BasePoint);
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnEndPoint_IfEndPointIsBaseOrEndPointOfOtherSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(-1, -1, -1), Point.MakePointWithInches(1, 1, 1));

            lineSegment1.IntersectWithSegment(lineSegment2).Should().Be(lineSegment1.EndPoint);
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnNull_IfLinesFromLineSegmentsDoNotIntersect()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 0, 1), Point.MakePointWithInches(1, 1, 2));

            lineSegment1.IntersectWithSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnNull_IfLinesFromLineSegmentsIntersectButNotOnPassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(1, 2, 1));

            lineSegment1.IntersectWithSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnNull_IfLinesFromLineSegmentsIntersectButNotOnInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(1, 2, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(3, 3, 3));

            lineSegment1.IntersectWithSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_IntersectWithSegment_ShouldReturnLineIntersect()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(0, 1, 1));

            Line lineSegmentToLine2 = new Line(lineSegment2);

            lineSegment1.IntersectWithSegment(lineSegment2).Should().Be(lineSegment1.IntersectWithLine(lineSegmentToLine2));
        }

        [Test()]
        public void LineSegment_MidpointTest()
        {
            LineSegment line1 = new LineSegment(Point.Origin, Point.MakePointWithInches(2, 2, 2));
            Point expected = Point.MakePointWithInches(1, 1, 1);
            (line1.MidPoint == expected).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_HypotheticalIntersectionTest()
        {
            LineSegment line1 = new LineSegment(Point.Origin, Point.MakePointWithInches(-5, -5, 0));
            LineSegment line2 = new LineSegment(Point.MakePointWithInches(5, -4, 0), Point.MakePointWithInches(-5, 6, 0));

            Point intersectT1 = line1.HypotheticalIntersection(line2);

            intersectT1.Should().Be(Point.MakePointWithInches(.5, .5, 0));
        }

        [Test()]
        public void LineSegment_LineSegmentOverlappingEquality()
        {
            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(0, 5));
            LineSegment segment2 = new LineSegment(Point.MakePointWithInches(0, 5), Point.MakePointWithInches(0, 0));

            segment1.Should().Be(segment2);
        }

        [Test()]
        public void LineSegment_3dRotateTest_Orthogonal()
        {
            LineSegment originalSegment = new LineSegment(Point.Origin, Point.MakePointWithInches(0, 5, 0));
            Line axis = new Line(Point.Origin, Point.MakePointWithInches(1, 0, 0));
            Angle toRotate = Angle.StraightAngle;

            LineSegment actualSegment = originalSegment.Rotate(new Rotation(axis, toRotate));
            LineSegment expectedSegment = new LineSegment(Point.Origin, Point.MakePointWithInches(0, -5, 0));
            (actualSegment == expectedSegment).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_3dRotateTest()
        {

            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0));
            LineSegment segment2 = new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3));

            Line rotationAxis = new Line(new Direction(Point.MakePointWithInches(1, 1, 1)), Point.MakePointWithInches(1, -1, -1));
            Angle rotationAngle = new Angle(new Degree(), 212);

            LineSegment actualSegment1 = segment1.Rotate(new Rotation(rotationAxis, rotationAngle));
            LineSegment actualSegment2 = segment2.Rotate(new Rotation(rotationAxis, rotationAngle));

            LineSegment expectedSegment1 = new LineSegment(Point.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), Point.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            LineSegment expectedSegment2 = new LineSegment(Point.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495), Point.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
            (actualSegment2 == expectedSegment2).Should().BeTrue();


        }

        [Test()]
        public void LineSegment_TranslateTest()
        {
            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(1, 2, 3), Point.MakePointWithInches(-3, -2, 0));

            //Direction testDirection = new Direction(Point.MakePointWithInches(-1, 5, 4));
            //Distance testDisplacement = new Distance(new Inch(), 12.9614814);
            Point testDisplacement = Point.MakePointWithInches(-2, 10, 8);

            LineSegment actualSegment1 = segment1.Translate(testDisplacement);

            LineSegment expectedSegment1 = new LineSegment(Point.MakePointWithInches(-1, 12, 11), Point.MakePointWithInches(-5, 8, 8));

            (actualSegment1 == expectedSegment1).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Contains()
        {
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(5, 0));

            //ToDo: fill out this test.
        }

        [Test()]
        public void LineSegment_ProjectOntoLine2DThroughOrigin()
        {
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(2, 5));
            Line projectOnto = new Line(Point.MakePointWithInches(2, 1));
            LineSegment result = testSegment.ProjectOntoLine(projectOnto);

            LineSegment expected = new LineSegment(Point.MakePointWithInches(3.6, 1.8));

            result.Should().Be(expected);
        }

        [Test()]
        public void LineSegment_ProjectOntoLine3DNotThroughOrigin()
        {
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(2, 0, 4), Point.MakePointWithInches(0, 2, 1));
            Line projectOnto = new Line(Point.MakePointWithInches(5, 1, 2), Point.MakePointWithInches(0, 0, 4.25));
            LineSegment result = testSegment.ProjectOntoLine(projectOnto);

            Distance expectedMagnitude = new Distance(new Inch(), 0.22428);
            (result.Magnitude == expectedMagnitude).Should().BeTrue();
            (new Line(result) == projectOnto).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_SliceWithPoint()
        {
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(2, 0, 4), Point.MakePointWithInches(0, 2, 1));
            Point midPoint = Point.MakePointWithInches(1, 1, 2.5);
            Point notOnLine = Point.MakePointWithInches(1, 1, 2);
            Point onLine = Point.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751);

            List<LineSegment> expectedwithMidPoint = new List<LineSegment>();
            expectedwithMidPoint.Add(new LineSegment(Point.MakePointWithInches(2, 0, 4), Point.MakePointWithInches(1, 1, 2.5)));
            expectedwithMidPoint.Add(new LineSegment(Point.MakePointWithInches(1, 1, 2.5), Point.MakePointWithInches(0, 2, 1)));

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
            expectedOnLine.Add(new LineSegment(Point.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751), Point.MakePointWithInches(0, 2, 1)));
            //thiss second one is the unitvector so it is only 1 in length and is smaller so it should go second
            expectedOnLine.Add(new LineSegment(Point.MakePointWithInches(2, 0, 4), Point.MakePointWithInches(2 - 0.48507125007, 0 + 0.48507125007, 4 - 0.7276068751)));

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
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(2, 0, 4), Point.MakePointWithInches(0, 2, 1));
            LineSegment result = testSegment.Reverse();
            result.BasePoint.Should().Be(testSegment.EndPoint);
            result.Length.Should().Be(testSegment.Length);
            Direction expectedDirection = new Direction(Point.MakePointWithInches(0, 2, 1), Point.MakePointWithInches(2, 0, 4));
            result.Direction.Should().Be(expectedDirection);
        }

        [Test()]
        public void LineSegment_ContainsLineSegment()
        {
            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(2, 2, 4));
            LineSegment segment2 = new LineSegment(Point.MakePointWithInches(2, 2, 4), Point.Origin);
            LineSegment segment3 = new LineSegment(Point.MakePointWithInches(1, 1, 2), Point.Origin);

            (segment1.Contains(segment2)).Should().BeTrue();
            (segment2.Contains(segment1)).Should().BeTrue();
            (segment1.Contains(segment3)).Should().BeTrue();
        }
    }
}
