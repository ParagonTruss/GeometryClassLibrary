using System;
using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AngleUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class LineSegmentTests
    {
        [Test()]
        public void LineSegment_GetLength_ShouldReturnVectorMagnitude()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 0, 2));

            lineSegment.Length.Should().Be(new Distance(2, Distance.Inches));
        }

        [Test()]
        public void LineSegment_GetMidpoint_ShouldReturnCenterPoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            lineSegment.MidPoint.Should().Be(Point.MakePointWithInches(1, 1, 1));
        }

        [Test()]
        public void LineSegment_GetEndPoints_ShouldListOfEndPoints()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            List<Point> pointList = new List<Point>();
            pointList.Add(Point.Origin);
            pointList.Add(Point.MakePointWithInches(1, 1, 1));

            lineSegment.EndPoints.Should().Equal(pointList);
        }

        [Test()]
        public void LineSegment_GetIsClosed_ShouldReturnFalse()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment.IsClosed.Should().BeFalse();
        }

        [Test()]
        public void LineSegment_GetInitialDirection_ShouldReturnDirection()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            Direction direction = new Direction(1, 1, 1);

            lineSegment.InitialDirection.Should().Be(direction);
        }

        [Test()]
        public void LineSegment_ConstructorPoint_ShouldCreateLineSegmentThroughOrigin()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment.Should().Be(new LineSegment(Point.Origin, Point.MakePointWithInches(1, 1, 1)));
        }

        [Test()]
        public void LineSegment_ConstructorPoint_ShouldThrowException_IfPointIsOrigin()
        {
            Point point = Point.Origin;

            Action construct = () => new LineSegment(point);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPoint_ShouldThrowException_IfPointIsNull()
        {
            Point point = null;

            Action construct = () => new LineSegment(point);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorVector_ShouldCreateLineSegmentFromVector()
        {
            LineSegment lineSegment = new LineSegment(new Vector(Point.MakePointWithInches(1, 1, 1)));

            lineSegment.Should().Be(new LineSegment(Point.Origin, Point.MakePointWithInches(1, 1, 1)));
        }

        [Test()]
        public void LineSegment_ConstructorVector_ShouldThrowException_IfVectorLengthIsZero()
        {
            Vector vector = new Vector(Point.Origin);

            Action construct = () => new LineSegment(vector);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorVector_ShouldThrowException_IfVectorIsNull()
        {
            Vector vector = null;

            Action construct = () => new LineSegment(vector);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointPoint_ShouldCreateLineSegmentFromPointToPoint()
        {
            LineSegment lineSegment = new LineSegment(Point.Origin, Point.MakePointWithInches(1, 1, 1));

            List<Point> pointList = new List<Point>();
            pointList.Add(Point.Origin);
            pointList.Add(Point.MakePointWithInches(1, 1, 1));

            lineSegment.EndPoints.Should().Equal(pointList);
        }

        [Test()]
        public void LineSegment_ConstructorPointPoint_ShouldThrowException_IfThePointsAreTheSame()
        {
            Action construct = () => new LineSegment(Point.Origin, Point.Origin);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointPoint_ShouldThrowException_IfFirstPointIsNull()
        {
            Point point = null;

            Action construct = () => new LineSegment(point, Point.Origin);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointPoint_ShouldThrowException_IfSecondPointIsNull()
        {
            Point point = null;

            Action construct = () => new LineSegment(Point.Origin, point);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointVector_ShouldCreateLineSegmentFromPointAndVector()
        {
            Vector vector = new Vector(Point.MakePointWithInches(1, 1, 1));

            LineSegment lineSegment = new LineSegment(Point.Origin, vector);

            lineSegment.Should().Be(new LineSegment(Point.MakePointWithInches(1, 1, 1)));
        }

        [Test()]
        public void LineSegment_ConstructorPointVector_ShouldThrowException_IfTheVectorHasZeroLength()
        {
            Vector vector = new Vector(Point.Origin);

            Action construct = () => new LineSegment(Point.Origin, vector);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointVector_ShouldCreateLineSegmentFromOrigin_IfPointIsNull()
        {
            Point point = null;
            Vector vector = new Vector(Point.MakePointWithInches(1, 1, 1));

            LineSegment lineSegment = new LineSegment(point, vector);

            lineSegment.Should().Be(new LineSegment(vector));
        }

        [Test()]
        public void LineSegment_ConstructorPointVector_ShouldThrowException_IfVectorIsNull()
        {
            Vector vector = null;

            Action construct = () => new LineSegment(Point.Origin, vector);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointDirectionDistance_ShouldCreateLineSegmentFromPointDirectionAndDistance()
        {
            Direction direction = new Direction(1, 0, 0);
            Distance distance = new Distance(1, Distance.Inches);

            LineSegment lineSegment = new LineSegment(Point.Origin, direction, distance);

            lineSegment.Should().Be(new LineSegment(Point.MakePointWithInches(1, 0, 0)));
        }

        [Test()]
        public void LineSegment_ConstructorPointDirectionDistance_ShouldThrowException_IfDistanceIsZero()
        {
            Direction direction = new Direction(1, 0, 0);
            Distance distance = Distance.ZeroDistance;

            Action construct = () => new LineSegment(Point.Origin, direction, distance);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointDirectionDistance_ShouldCreateLineSegmentFromOrigin_IfPointIsNull()
        {
            Point point = null;
            Direction direction = new Direction(1, 0, 0);
            Distance distance = new Distance(1, Distance.Inches);

            LineSegment lineSegment = new LineSegment(point, direction, distance);

            lineSegment.Should().Be(new LineSegment(Point.MakePointWithInches(1, 0, 0)));
        }

        [Test()]
        public void LineSegment_ConstructorPointDirectionDistance_ShouldThrowException_IfDirectionIsNull()
        {
            Direction direction = null;
            Distance distance = new Distance(1, Distance.Inches);

            Action construct = () => new LineSegment(Point.Origin, direction, distance);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorPointDirectionDistance_ShouldThrowException_IfDistanceIsNull()
        {
            Direction direction = new Direction(1, 0, 0);
            Distance distance = null;

            Action construct = () => new LineSegment(Point.Origin, direction, distance);
            construct.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ConstructorLineSegment_ShouldCreateCopy()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            LineSegment lineSegment2 = new LineSegment(lineSegment1);

            lineSegment2.Should().NotBeSameAs(lineSegment1);
            lineSegment2.Should().Be(lineSegment1);
        }

        [Test()]
        public void LineSegment_ConstructorLineSegment_ShouldThrowException_IfLineSegmentIsNull()
        {
            LineSegment lineSegment = null;

            Action construct = () => new LineSegment(lineSegment);
            construct.ShouldThrow<Exception>();
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
        public void LineSegment_EqualityOperator_ShouldReturnFalse_IfOnlySecondSegmentIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment nullLineSegment = null;

            (lineSegment == nullLineSegment).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_EqualityOperator_ShouldBeSameAsEquals()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment1 == lineSegment2).Should().Be(lineSegment1.Equals(lineSegment2));
        }

        [Test()]
        public void LineSegment_EqualityOperator_ShouldReturnFalse_IfSegmentsHaveDifferentEndPoints()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            (lineSegment1 == lineSegment2).Should().BeFalse();
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
        public void LineSegment_InequalityOperator_ShouldReturnTrue_IfOnlySecondSegmentIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment nullLineSegment = null;

            (lineSegment != nullLineSegment).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_InequalityOperator_ShouldBeSameAsNotEquals()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 5));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 5));

            (lineSegment1 != lineSegment2).Should().Be(!lineSegment1.Equals(lineSegment2));
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
        public void LineSegment_Equals_ShouldReturnFalse_IfOnlyOneEndPointIsTheSame()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.Equals(lineSegment2).Should().BeFalse();
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
        public void LineSegment_IntersectWithSegment_ShouldThrowException_IfPassedSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action intersect = () => lineSegment1.IntersectWithSegment(lineSegment2);
            intersect.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldReturnTrue_IfThisBasePointEqualsPassedEndPoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(0, 0, 0));

            lineSegment1.SharesABaseOrEndPointWith(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldReturnTrue_IfThisBasePointEqualsPassedBasePoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, 1, 1));

            lineSegment1.SharesABaseOrEndPointWith(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldReturnTrue_IfThisEndPointEqualsPassedEndPoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, 1, 1));

            lineSegment1.SharesABaseOrEndPointWith(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldReturnTrue_IfThisEndPointEqualsPassedBasePoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.SharesABaseOrEndPointWith(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldReturnFalse_IfNoBaseOrEndPointsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 2), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.SharesABaseOrEndPointWith(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_SharesABaseOrEndPointWith_ShouldThrowException_IfPassedLineSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action shares = () => lineSegment1.SharesABaseOrEndPointWith(lineSegment2);
            shares.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ContainsLineSegment_ShouldReturnFalse_IfDoesNotContainBasePoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(3, 3, 3));

            (lineSegment1.Contains(lineSegment2)).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsLineSegment_ShouldReturnFalse_IfDoesNotContainEndPoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            (lineSegment1.Contains(lineSegment2)).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsLineSegment_ShouldReturnTrue_IfContainsBaseAndEndPoint()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            (lineSegment1.Contains(lineSegment2)).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsLineSegment_ShouldThrowException_IfPassedLineSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = null;

            Action contains = () => lineSegment1.Contains(lineSegment2);
            contains.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_Slice_ShouldReturnListWithOriginalLineSegment_IfPointIsNotOnLineSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(0, 0, 1);

            List<LineSegment> lineSegmentList = new List<LineSegment>();
            lineSegmentList.Add(lineSegment);
                
            lineSegment.Slice(point).Should().BeEquivalentTo(lineSegmentList);
        }

        [Test()]
        public void LineSegment_Slice_ShouldReturnListWithSplicedLineSegments_IfPointIsOnLineSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(0.5, 0.5, 0.5);

            LineSegment splicedLineSegment1 = new LineSegment(Point.MakePointWithInches(0.5, 0.5, 0.5));
            LineSegment splicedLineSegment2 = new LineSegment(Point.MakePointWithInches(0.5, 0.5, 0.5), Point.MakePointWithInches(1, 1, 1));

            List<LineSegment> lineSegmentList = new List<LineSegment>();
            lineSegmentList.Add(splicedLineSegment1);
            lineSegmentList.Add(splicedLineSegment2);

            lineSegment.Slice(point).Should().BeEquivalentTo(lineSegmentList);
        }

        [Test()]
        public void LineSegment_Slice_ShouldReturnOriginalSegment_IfPointIsOnBasePointLineSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(0, 0, 0);

            List<LineSegment> lineSegmentList = new List<LineSegment>();
            lineSegmentList.Add(lineSegment);

            lineSegment.Slice(point).Should().BeEquivalentTo(lineSegmentList);
        }

        [Test()]
        public void LineSegment_Slice_ShouldThrowException_IfPointIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = null;

            Action slice = () => lineSegment.Slice(point);
            slice.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ProjectOntoLine_ShouldReturnNull_IfVectorLengthIsZero()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1));
            Line line = new Line(Point.MakePointWithInches(1, 0), Point.MakePointWithInches(0, 1));

            lineSegment.ProjectOntoLine(line).Should().BeNull();
        }
        [Test()]
        public void LineSegment_ProjectOntoLine_PracticalTest()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1.75, 33.16), Point.MakePointWithInches(91.4, 35));
            Line line = new Line(Point.MakePointWithInches(0, 0), new Direction(Point.Origin, Point.MakePointWithInches( -0.020854359399988368, 0.99978252419914615)));

            lineSegment.ProjectOntoLine(line).Should().BeNull();
        }

        [Test()]
        public void LineSegment_ProjectOntoLine_ShouldReturnVectorProjection_IfVectorLengthIsNotZero()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1));
            Line line = new Line(Point.MakePointWithInches(1, 0), Point.MakePointWithInches(2, 0));

            Vector vector = new Vector(lineSegment.BasePoint, lineSegment.EndPoint);

            lineSegment.ProjectOntoLine(line).Should().Be(new LineSegment(vector.ProjectOntoLine(line)));
        }

        [Test()]
        public void LineSegment_ProjectOntoLine_ShouldThrowException_IfLineIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1));
            Line line = null;

            Action project = () => lineSegment.ProjectOntoLine(line);
            project.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ProjectOntoPlane_ShouldReturnNull_IfVectorLengthIsZero()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 0, 0));
            Plane plane = new Plane(Direction.Right);

            lineSegment.ProjectOntoPlane(plane).Should().BeNull();
        }

        [Test()]
        public void LineSegment_ProjectOntoPlane_ShouldReturnVectorProjection_IfVectorLengthIsNotZero()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Plane plane = new Plane(Direction.Right);

            Vector vector = new Vector(lineSegment.BasePoint, lineSegment.EndPoint);

            lineSegment.ProjectOntoPlane(plane).Should().Be(new LineSegment(vector.ProjectOntoPlane(plane)));
        }

        [Test()]
        public void LineSegment_ProjectOntoPlane_ShouldThrowException_IfPlaneIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Plane plane = null;

            Action project = () => lineSegment.ProjectOntoPlane(plane);
            project.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_Reverse_ShouldReturnReversedLineSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment.Should().Be(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(0, 0, 0)));
        }

        [Test()]
        public void LineSegment_Rotate_ShouldRotateAsVector()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 0, 0));
            Rotation rotation = new Rotation(new Angle(45, Angle.Degrees));

            LineSegment lineSegment2 = lineSegment1.Rotate(rotation);

            lineSegment2.Should().Be(new LineSegment(Point.Origin, new Direction(rotation.RotationAngle), new Distance(Distance.Inches, 1)));
        }

        [Test()]
        public void LineSegment_Rotate_ShouldThrowException_IfRotationIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 0, 0));
            Rotation rotation = null;

            Action rotate = () => lineSegment.Rotate(rotation);
            rotate.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_Shift_ShouldShiftAsVector()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Shift shift1 = new Shift(new Vector(Point.MakePointWithInches(0, 1, 0)));
            Shift shift2 = new Shift(new Vector(Point.MakePointWithInches(2, 0, 3)));

            Shift shift3 = Shift.Compose(shift1, shift2);

            lineSegment.Shift(shift3).Should().Be(new LineSegment(Point.MakePointWithInches(2, 1, 3), Point.MakePointWithInches(3, 2, 4)));
        }

        [Test()]
        public void LineSegment_Shift_ShouldThrowException_IfShiftIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Shift shift = null;

            Action shiftAction = () => lineSegment.Shift(shift);
            shiftAction.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_Translate_ShouldTranslateAsVector()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Translation translation = new Translation(new Vector(Point.MakePointWithInches(0, 1, 0)));

            lineSegment.Translate(translation).Should().Be(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(1, 2, 1)));
        }

        [Test()]
        public void LineSegment_Translate_ShouldThrowException_IfTranslateIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Translation translation = null;

            Action translate = () => lineSegment.Shift(translation);
            translate.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyPassedBasePointIsOnInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(0, 2, 0));

            lineSegment1.OverlappingSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyPassedEndPointIsOnInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(1, 1, 1));

            lineSegment1.OverlappingSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyInstanceBasePointIsOnPassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(0, 2, 0));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            lineSegment1.OverlappingSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyInstanceEndPointIsOnPassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            lineSegment1.OverlappingSegment(lineSegment2).Should().BeNull();
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnIntersection_IfBothPassedPointsAreOnInsideOfInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.OverlappingSegment(lineSegment2).Should().Be(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2)));
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnIntersection_IfBothInstancePointsAreOnInsideOfPassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(3, 3, 3));

            lineSegment1.OverlappingSegment(lineSegment2).Should().Be(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2)));
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnIntersection_IfBothSegmentsAreTheSame()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(3, 3, 3));

            lineSegment1.OverlappingSegment(lineSegment2).Should().Be(new LineSegment(Point.MakePointWithInches(3, 3, 3)));
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnIntersection_IfPassedTwoEndPointsAreTheSame()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            lineSegment1.OverlappingSegment(lineSegment2).Should().Be(new LineSegment(Point.MakePointWithInches(2, 2, 2)));
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyOneEndPointInCommon_1()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.OverlappingSegment(lineSegment2).Should().BeNull();
        }
        
        [Test]
        public void LineSegment_OverlappingSegment_ShouldReturnNull_IfOnlyOneEndPointInCommon_2()
        {
            var segment1 = new LineSegment(new Point(Inches, 200.529, -1.616),
                                           new Point(Inches, 200.529, 0.01));
            var segment2 = new LineSegment(new Point(Inches, 206.375, 0),
                                           new Point(Inches, 200.542, 0));
            var overlap1 = segment1.OverlappingSegment(segment2);
            var overlap2 = segment2.OverlappingSegment(segment1);
            overlap1.Should().Be(null);
            overlap2.Should().Be(null);
        }

        [Test()]
        public void LineSegment_OverlappingSegment_ShouldThrowException_IfPassedSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action intersection = () => lineSegment1.OverlappingSegment(lineSegment2);
            intersection.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnTrue_IfSegmentsAreEqual()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment1.Overlaps(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnFalse_IfSegmentsAreNotParallel()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(0, 2, 0));

            lineSegment1.Overlaps(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnFalse_IfSegmentsAreParallelButNoEndPointsAreOnTheInsideOfTheOtherSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.Overlaps(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnTrue_IfSegmentsAreParallelAndAnInstnaceEndpointIsOnTheInsideOfThePassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(2, 2, 2));

            lineSegment1.Overlaps(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnTrue_IfSegmentsAreParallelAndAPassedEndpointIsOnTheInsideOfTheInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.Overlaps(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldReturnTrue_IfSegmentsAreParallelAndAnyEndpointsAreOnTheInsideOfTheOtherSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.Overlaps(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_Overlaps_ShouldThrowException_IfSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action overlaps = () => lineSegment1.Overlaps(lineSegment2);
            overlaps.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldReturnFalse_IfBasePointIsOnPlane()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Plane plane = new Plane(Direction.Right);

            lineSegment.DoesIntersectNotTouching(plane).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldReturnFalse_IfEndPointIsOnPlane()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.Origin);
            Plane plane = new Plane(Direction.Right);

            lineSegment.DoesIntersectNotTouching(plane).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldReturnFalse_IfBothEndPointsAreOnPlane()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 0, 0));
            Plane plane = new Plane(Direction.Right);

            lineSegment.DoesIntersectNotTouching(plane).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldReturnFalse_IfEndPointsAreOnTheSameSideOfThePlane()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));
            Plane plane = new Plane(Direction.Right);

            lineSegment.DoesIntersectNotTouching(plane).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldReturnTrue_IfEndPointsAreOnOppositeSidesOfThePlane()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(-1, -1, -1));
            Plane plane = new Plane(Direction.Right);

            lineSegment.DoesIntersectNotTouching(plane).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingPlane_ShouldThrowException_IfPlaneIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(-1, -1, -1));
            Plane plane = null;

            Action intersect = () => lineSegment.DoesIntersectNotTouching(plane);
            intersect.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldReturnFalse_IfSegmentsDoNotIntersect()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(2, 0, 0));

            lineSegment1.DoesIntersectNotTouching(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldReturnFalse_IfIntersectionPointIsOneOfTheEndPointsOfInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(-1, 0, 0), Point.MakePointWithInches(1, 0, 0));

            lineSegment1.DoesIntersectNotTouching(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldReturnFalse_IfIntersectionPointIsOneOfTheEndPointsOfPassedSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(-1, 0, 0), Point.MakePointWithInches(1, 0, 0));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment1.DoesIntersectNotTouching(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldReturnFalse_IfIntersectionPointIsOneOfTheEndPointOfBothSegments()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.DoesIntersectNotTouching(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldReturnTrue_IfIntersectionPointIsNotOnAnyEndPoints()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(2, 1, 2));

            lineSegment1.DoesIntersectNotTouching(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_DoesIntersectNotTouchingLineSegment_ShouldThrowException_IfPassedLineSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action intersect = () => lineSegment1.DoesIntersectNotTouching(lineSegment2);
            intersect.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_ContainsOnInsidePoint_ShouldReturnFalse_IfPointIsOneOfTheEndPointsOfTheSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(1, 1, 1);

            lineSegment.ContainsOnInside(point).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsOnInsidePoint_ShouldReturnFalse_IfPointIsBeforeSegmentBasePoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(-1, -1, -1);

            lineSegment.ContainsOnInside(point).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsOnInsidePoint_ShouldReturnFalse_IfPointIsAfterSegmentEndPoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(2, 2, 2);

            lineSegment.ContainsOnInside(point).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsOnInsidePoint_ShouldReturnTrue_IfPointIsOnInsideOfSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            Point point = Point.MakePointWithInches(1, 1, 1);

            lineSegment.ContainsOnInside(point).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsOnInsidePoint_ShouldThrowException_IfPointIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            Point point = null;

            Action contains = () => lineSegment.ContainsOnInside(point);
            contains.ShouldThrow<NullReferenceException>();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnThrowException_IfPointIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            Point point = null;

            Action contains = () => lineSegment.ContainsOnInside(point);
            contains.ShouldThrow<NullReferenceException>();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnTrue_IfPointIsBasePoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.Origin;
            lineSegment.Contains(point).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnTrue_IfPointIsEndPoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(1, 1, 1);
            lineSegment.Contains(point).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnFalse_IfPointIsBeforeSegmentBasePoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(-1, -1, -1);

            lineSegment.Contains(point).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnFalse_IfPointIsAfterSegmentEndPoint()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Point point = Point.MakePointWithInches(2, 2, 2);

            lineSegment.Contains(point).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsPoint_ShouldReturnTrue_IfPointIsOnSegment()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            Point point = Point.MakePointWithInches(1, 1, 1);

            lineSegment.Contains(point).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsOnInsideLineSegment_ShouldReturnFalse_IfBasePointOfPassedSegmentIsNotOnInsideOfInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1));

            lineSegment1.ContainsOnInside(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsOnInsideLineSegment_ShouldReturnFalse_IfEndPointOfPassedSegmentIsNotOnInsideOfInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(2, 2, 2));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.ContainsOnInside(lineSegment2).Should().BeFalse();
        }

        [Test()]
        public void LineSegment_ContainsOnInsideLineSegment_ShouldReturnTrue_IfBothEndPointsOfPassedAreOnInsideOfInstanceSegment()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(3, 3, 3));
            LineSegment lineSegment2 = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 2, 2));

            lineSegment1.ContainsOnInside(lineSegment2).Should().BeTrue();
        }

        [Test()]
        public void LineSegment_ContainsOnInsideLineSegment_ShouldThrowException_IfPassedLineSegmentIsNull()
        {
            LineSegment lineSegment1 = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            LineSegment lineSegment2 = null;

            Action contains = () => lineSegment1.ContainsOnInside(lineSegment2);
            contains.ShouldThrow<Exception>();
        }

        [Test()]
        public void LineSegment_HypotheticalIntersectionTest_ShouldReturnLineIntersection()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(-5, -5, 0));
            Line line = new Line(Point.MakePointWithInches(5, -4, 0), Point.MakePointWithInches(-5, 6, 0));

            lineSegment.HypotheticalIntersection(line).Should().Be(Point.MakePointWithInches(.5, .5, 0));
        }

        [Test()]
        public void LineSegment_HypotheticalIntersectionTest_ShouldThrowNullPointerException_IfLineIsNull()
        {
            LineSegment lineSegment = new LineSegment(Point.MakePointWithInches(1, 1, 1));
            Line line = null;

            Action intersect = () => lineSegment.HypotheticalIntersection(line);
            intersect.ShouldThrow<Exception>();
        }
    }
}