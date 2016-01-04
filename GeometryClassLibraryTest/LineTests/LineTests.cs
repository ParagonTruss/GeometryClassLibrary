using System;
using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Metric.CentimeterUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class LineTests
    {
        [Test()]
        public void Line_JSON()
        {
            Point testBasePoint = Point.MakePointWithInches(1, 0, 2);
            Line line = new Line(testBasePoint, Point.MakePointWithInches(2, 3, 1));

            var json = JsonConvert.SerializeObject(line);
            Line deserializedLine = JsonConvert.DeserializeObject<Line>(json);

            bool areEqual = (line == deserializedLine);
            areEqual.Should().BeTrue();
        }

        [Test()]
        public void Line_GetPointOnLine()
        {
            Point testBasePoint = Point.MakePointWithInches(1, 0, 2);

            Line testLine = new Line(testBasePoint, Point.MakePointWithInches(2, 3, 1));

            Point pointOnLine = Point.MakePointWithInches(3, 6, 0);

            testLine.GetPointAlongLine(new Distance(new Inch(), 6.62)).Should().Be(pointOnLine);         
        }

        [Test()]
        public void Line_Intersection_Inches()
        {
            Point basePointLine1 = Point.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = Point.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(-2, 1, 3));

            Point expectedResult = Point.MakePointWithInches(0, 2.5, 1.5);
            Point actualResult = line1.IntersectWithLine(line2);

            actualResult.Should().Be(expectedResult);  
        }

        [Test()]
        public void Line_Intersection_Millimeters()
        {
            Point basePointLine1 = Point.MakePointWithMillimeters(2, 1, 0);
            Point basePointLine2 = Point.MakePointWithMillimeters(2, 4, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithMillimeters(-2, 4, 3));
            Line line2 = new Line(basePointLine2, Point.MakePointWithMillimeters(-2, 1, 3));

            Point expectedResult = Point.MakePointWithMillimeters(0, 2.5, 1.5);
            Point actualResult = line1.IntersectWithLine(line2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular()
        {
            Point basePointLine1 = Point.MakePointWithInches(1, 2, 2);
            Point basePointLine2 = Point.MakePointWithInches(1, 1, 2);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 1, 2));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(1, 1, 1));

            Point expectedResult = Point.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.IntersectWithLine(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular2()
        {
            Point basePointLine1 = Point.MakePointWithInches(1, 1, 2);
            Point basePointLine2 = Point.MakePointWithInches(1, 1, 2);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 7, 2));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(1, 1, 6));

            Point expectedResult = Point.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.IntersectWithLine(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular3()
        {
            Point basePointLine1 = Point.MakePointWithInches(1, 2, 2);
            Point basePointLine2 = Point.MakePointWithInches(1, 1, 1);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 1, 2));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(1, 1, 2));

            Point expectedResult = Point.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.IntersectWithLine(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_Intersection_Origin()
        {
            Point basePointLine1 = Point.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = Point.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 0, 0));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(0, 0, 1));

            Point expectedResult = Point.MakePointWithInches(0, 0, 0);
            Point actualResult = line1.IntersectWithLine(line2);

            (expectedResult == actualResult).Should().BeTrue();  
        }

        [Test()]
        public void Line_WillTwoLinesOnTopOfOneAnotherIntersectTest()
        {

            Line line1 = new Line(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(10, 0, 0));
            Line line2 = new Line(Point.MakePointWithInches(3, 0, 0), Point.MakePointWithInches(6, 0, 0));
            Line line3 = new Line(Point.MakePointWithInches(3, 2, 0), Point.MakePointWithInches(-2, 5, 3));
            Line line4 = new Line(Point.MakePointWithInches(3, 5, 0), Point.MakePointWithInches(-2, 2, 3));
            Line line5 = new Line(Point.MakePointWithInches(0, 5), Point.MakePointWithInches(0, -3));
            Line line6 = new Line(Point.MakePointWithInches(0, 2), Point.MakePointWithInches(0, -1));

            bool resultT1 = line1.IntersectsLine(line2);
            bool resultT2 = line3.IntersectsLine(line4);
            bool resultT3 = line5.IntersectsLine(line6);
            bool resultF1 = line1.IntersectsLine(line3);

            resultT1.Should().BeTrue();
            resultT2.Should().BeTrue();
            resultT3.Should().BeTrue();
            resultF1.Should().BeFalse();
        }

        [Test()]
        public void Line_SmallestAngleBetween_Inches()
        {
            Point basePointLine1 = Point.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = Point.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(-2, 1, 3));

            Angle expectedResult = new Angle(new Radian(), 1.080839);
            Angle actualResult = line1.SmallestAngleBetween(line2);
            
            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Line_SmallestAngleBetween_Millimeters()
        {
            Point basePointLine1 = Point.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = Point.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(-2, 1, 3));

            Angle expectedResult = new Angle(new Radian(), 1.080839);
            Angle actualResult = line1.SmallestAngleBetween(line2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Line_SmallestAngleBetween_IntersectAtOrigin()
        {
            Point basePointLine1 = Point.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = Point.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 1, 1));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(0, 0, 1));

            double angleBetweenLinesInRadians = Math.Acos(1 / Math.Sqrt(3));
            Angle angleBetweenLines = new Angle(new Radian(), angleBetweenLinesInRadians);

            line1.SmallestAngleBetween(line2).Should().Be(angleBetweenLines);
        }

        [Test()]
        public void Line_SmallestAngleBetween_Perpendicular()
        {
            Point basePointLine1 = Point.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = Point.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, Point.MakePointWithInches(1, 0, 0));
            Line line2 = new Line(basePointLine2, Point.MakePointWithInches(0, 0, 1));

            double angleBetweenLinesInRadians = Math.Acos(0);
            Angle angleBetweenLines = new Angle(new Radian(), angleBetweenLinesInRadians);

            line1.SmallestAngleBetween(line2).Should().Be(angleBetweenLines);

        }

        [Test()]
        public void Line_IsCoplanarWith()
        {
            Line line1 = new Line(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 1, 0));
            Line line2 = new Line(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(1, 1, 0));
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 0), Point.MakePointWithInches(1, 0, 0));
            Line line4 = new Line(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(3, 0, 4));

            line1.IsCoplanarWith(line2).Should().BeTrue();
            line3.IsCoplanarWith(line4).Should().BeFalse();
            line1.IsCoplanarWith(line3).Should().BeTrue();
            line4.IsCoplanarWith(line2).Should().BeFalse();
        }

        [Test()]
        public void Line_IsParallelTo()
        {
            Line line1 = new LineSegment(Point.MakePointWithInches(0,0), Point.MakePointWithInches(5,0));
            Line line2 = new LineSegment(Point.MakePointWithInches(-3,-3), Point.MakePointWithInches(8,-3));
            Line line3 = new LineSegment(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 1));

            Line line4 = new LineSegment(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(0, 5));
            Line line5 = new LineSegment(Point.MakePointWithInches(-3, -3), Point.MakePointWithInches(-3, 8));
            Line line6 = new LineSegment(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(1,4));
             
            line1.IsParallelTo(line2).Should().BeTrue();
            line2.IsParallelTo(line3).Should().BeTrue();

            line4.IsParallelTo(line5).Should().BeTrue();
            line5.IsParallelTo(line6).Should().BeTrue();

            line1.IsParallelTo(line4).Should().BeFalse();
            line5.IsParallelTo(line3).Should().BeFalse();
        }

        [Test()]
        public void Line_IsPerpendicularTo()
        {
            Line line1 = new LineSegment(Point.Origin, new Direction(new Angle(new Degree(), 45)), new Distance(new Inch(), 1));
            Line line2 = new LineSegment(Point.Origin, new Direction(new Angle(new Degree(), 135)), new Distance(new Inch(), 1));
            Line line3 = new LineSegment(Point.MakePointWithInches(2,-3,1), new Direction(new Angle(new Degree(), 45), Angle.ZeroAngle), new Distance(new Inch(), 1));

            Line line4 = new LineSegment(Point.MakePointWithInches(3, 5, 7));
            Line line5 = new LineSegment(Point.MakePointWithInches(1, -2, 1));
            Line line6 = new LineSegment(Point.MakePointWithInches(1, 2, -3), Point.MakePointWithInches(5, 1, -4)); //4, -1, -1

            line1.IsPerpendicularTo(line2).Should().BeTrue();
            line1.IsPerpendicularTo(line3).Should().BeTrue();
            line2.IsPerpendicularTo(line3).Should().BeTrue();
            line3.IsPerpendicularTo(line1).Should().BeTrue(); //check its symmetric

            line4.IsPerpendicularTo(line5).Should().BeTrue();
            line4.IsPerpendicularTo(line6).Should().BeTrue();
            line5.IsPerpendicularTo(line6).Should().BeFalse();
            line6.IsPerpendicularTo(line4).Should().BeTrue(); //check its symmetric

            line1.IsPerpendicularTo(line4).Should().BeFalse();
            
        }

        [Test()]
        public void Line_Rotate_AboutZAxis()
        {
            Point basePointLine1 = Point.MakePointWithInches(2, 1, 0);
            Point otherPointLine1 = Point.MakePointWithInches(3, 3, 3);

            Line line1 = new Line(basePointLine1, otherPointLine1);
            Line axisLine = new Line(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 0, 1));

            Angle rotationAngle = new Angle(new Degree(), 199);

            Line actualResult = line1.Rotate(new Rotation(axisLine, rotationAngle));

            Point expectedResultBasePoint = Point.MakePointWithInches(-1.5654689967414768, -1.5966548845136304, 0.0);
            Direction expectedDirection = new Direction(Point.MakePointWithInches(-0.29438226668500322,-2.2166053056557904,3.0));
            Line expectedResult = new Line(expectedDirection, expectedResultBasePoint);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_Rotate_AboutReferencePoint()
        {
            Point start = Point.MakePointWithInches(0, 1, 3);
            Point end = Point.MakePointWithInches(0, 1, 5);

            Line toRotate = new Line(start, end);
            Line startPointYAxis = new Line(start, Point.MakePointWithInches(0, 2, 3)); //relative y axis

            Angle rotationAngle = new Angle(new Degree(), 90);

            Line afterRotate = toRotate.Rotate(new Rotation(startPointYAxis, rotationAngle));

            Point expectedStart = new Point(start);
            Point expectedEnd = Point.MakePointWithInches(2, 1, 3);
            Line expectedResult = new Line(expectedStart, expectedEnd);

            (expectedResult == afterRotate).Should().BeTrue();
        }

        [Test()]
        public void Line_Translate()
        {
            Line line1 = new Line(Point.MakePointWithInches(1, 2, 3), Point.MakePointWithInches(-3, -2, 0));

            //Direction testDirection = new Direction(Point.MakePointWithInches(-1, 5, 4));
            //Distance testDisplacement = new Distance(new Inch(), 12.9614814);
            Point testDisplacement = Point.MakePointWithInches(-2, 10, 8);

            Line actualLine1 = line1.Translate((testDisplacement));

            Line expectedLine1 = new Line(Point.MakePointWithInches(-1, 12, 11), Point.MakePointWithInches(-5, 8, 8));

            (actualLine1 == expectedLine1).Should().BeTrue();
        }

        [Test()]
        public void Line_DoesIntersectLine()
        {
            Line line1 = new Line(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(0, 5));
            Line line2 = new Line(Point.MakePointWithInches(5, 0), Point.MakePointWithInches(-3, 0));
            Line line3 = new Line(Point.MakePointWithInches(3, 3, 0), Point.MakePointWithInches(-3, -3, 2));
            Line line4 = new Line(Point.MakePointWithInches(3, -3, 0), Point.MakePointWithInches(-3, 3, 2));

            bool resultT1 = line1.IntersectsLine(line2);
            bool resultF1 = line2.IntersectsLine(line3);
            bool resultT2 = line3.IntersectsLine(line4);

            resultT1.Should().BeTrue();
            resultF1.Should().BeFalse();
            resultT2.Should().BeTrue();
        }

        [Test()]
        public void Line_DoesIntersectLineSegment()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 0, 0), Point.MakePointWithInches(-3, 0, 0));
            LineSegment segment1 = new LineSegment(Point.MakePointWithInches(3, -3, 0), Point.MakePointWithInches(2, 0, 0));

            bool resultT1 = line1.DoesIntersect(segment1);
        }


        //[Test()]
        //public void Line_PlaneThroughLineInDirectionOf_ZAxis()
        //{
        //    Line test1 = new Line(Point.MakePointWithMillimeters(2, 1, -1), Point.MakePointWithMillimeters(-1, 5, 0));
        //    Line test2 = new Line(Point.MakePointWithMillimeters(2, 1, -1),  Point.MakePointWithMillimeters(-1, 5, 2));

        //    Plane result1 = test1.PlaneThroughLineInDirectionOf(Enums.Axis.Z);
        //    Plane result2 = test2.PlaneThroughLineInDirectionOf(Enums.Axis.Z);

        //    Plane expectedPlane = new Plane(Point.MakePointWithMillimeters(2, 1, 6), Point.MakePointWithMillimeters(2, 1, -1), Point.MakePointWithMillimeters(-1, 5, -23));

        //    result1.Should().Be(result2);
        //    result1.Should().Be(expectedPlane);
        //}

        [Test()]
        public void Line_XZIntercept()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            Point intercept1 = line1.XZIntercept();
            Point intercept2 = line2.XZIntercept();
            Point intercept3 = line3.XZIntercept();
            Point intercept4 = line4.XZIntercept();
            Point intercept5 = line5.XZIntercept();

            intercept1.Should().Be(Point.MakePointWithInches(-1, 0, 1));
            intercept2.Should().Be(Point.MakePointWithInches(6, 0, 0));
            intercept3.Should().Be(Point.MakePointWithInches(0, 0, 6));
            intercept4.Should().Be(Point.MakePointWithInches(4, 0, 3));
            intercept5.Should().Be(null);
        }

        [Test()]
        public void Line_YZIntercept()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 0, 1, -3
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 0, 1.5, -0.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at 0, -1, -1
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            Point intercept1 = line1.YZIntercept();
            Point intercept2 = line2.YZIntercept();
            Point intercept3 = line3.YZIntercept();
            Point intercept4 = line4.YZIntercept();
            Point intercept5 = line5.YZIntercept();

            intercept1.Should().Be(Point.MakePointWithInches(0, 1, -3));
            intercept2.Should().Be(Point.MakePointWithInches(0, 1.5, -.5));
            intercept3.Should().Be(Point.MakePointWithInches(0, 0, 6));
            intercept4.Should().Be(Point.MakePointWithInches(0, -1, -1));
            intercept5.Should().Be(null);
        }

        [Test]
        public void Line_XYIntercept()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 1), Point.MakePointWithInches(5, 3, 2)); //intersects at 1, 1, 0
            Line line2 = new Line(Point.MakePointWithInches(6, 0, -2), Point.MakePointWithInches(2, 3, -1)); //intersects at -2, 6, 0
            Line line3 = new Line(Point.MakePointWithInches(3, 3, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at -2,-2,0
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4, 15, 0
            Line line5 = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 1, 2)); //doesnt intersect

            Point intercept1 = line1.XYIntercept();
            Point intercept2 = line2.XYIntercept();
            Point intercept3 = line3.XYIntercept();
            Point intercept4 = line4.XYIntercept();
            Point intercept5 = line5.XYIntercept();

            intercept1.Should().Be(Point.MakePointWithInches(1, 1, 0));
            intercept2.Should().Be(Point.MakePointWithInches(-2, 6, 0));
            intercept3.Should().Be(Point.MakePointWithInches(-2, -2, 0));
            intercept4.Should().Be(Point.MakePointWithInches(4, 15, 0));
            intercept5.Should().Be(null);
        }

        [Test()]
        public void Line_XInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3

            Distance intercept1 = line1.XInterceptIn2D();
            Distance intercept2 = line2.XInterceptIn2D();
            Distance intercept3 = line3.XInterceptIn2D();
            Distance intercept4 = line4.XInterceptIn2D();

            intercept1.Should().Be(new Distance(new Inch(), -1));
            intercept2.Should().Be(new Distance(new Inch(), 6));
            intercept3.Should().Be(new Distance(new Inch(), 0));
            intercept4.Should().Be(new Distance(new Inch(), 4));
        }

        [Test()]
        public void Line_XInterceptIn2D_LineDoesNotIntersectXAxis()
        {            
            Line line = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            //should throw an exception
            Distance intercept = line.XInterceptIn2D();
            (intercept == null).Should().BeTrue();
        }

        [Test()]
        public void Line_YInterceptIn2D()
        {
            Line line1 = new Line(Point.MakePointWithInches(2, 2, 2), Point.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(Point.MakePointWithInches(2, 1, 1), Point.MakePointWithInches(3, 2, 2)); //intersects at -1

            Distance intercept1 = line1.YInterceptIn2D();
            Distance intercept2 = line2.YInterceptIn2D();
            Distance intercept3 = line3.YInterceptIn2D();
            Distance intercept4 = line4.YInterceptIn2D();

            intercept1.Should().Be(new Distance(new Inch(), 1));
            intercept2.Should().Be(new Distance(new Inch(), 1.5));
            intercept3.Should().Be(new Distance(new Inch(), 0));
            intercept4.Should().Be(new Distance(new Inch(), -1));
        }

        [Test()]
        public void Line_YInterceptIn2D_LineDoesNotIntersectYAxis()
        {
            Line line = new Line(Point.MakePointWithInches(4, 2, 2), Point.MakePointWithInches(4, 2, 1)); //doesnt intersect

            //should throw an exception
            Distance intercept5 = line.YInterceptIn2D();
            (intercept5 == null).Should().BeTrue();
        }

        [Test()]
        public void Line_Sort()
        {
            Line line1 = new Line(Point.MakePointWithInches(3, 2, 5), Point.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(Point.MakePointWithInches(1, 1, 5), Point.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(Point.MakePointWithInches(4, 10, 1), Point.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3

            List<Line> lines = new List<Line> { line2, line3, line4, line1 };


            lines.Sort();

            lines[0].Should().Be(line1);
            lines[1].Should().Be(line3);
            lines[2].Should().Be(line4);
            lines[3].Should().Be(line2);
        }

        [Test()]
        public void Line_ProjectOntoPlane()
        {
            Line toProject = new Line(Point.MakePointWithInches(2, 2, 1));
            Plane projectOnto = new Plane(Line.XAxis, Line.YAxis);

            Line result = toProject.ProjectOntoPlane(projectOnto);

            Line expected = new Line(Point.MakePointWithInches(2, 2, 0));

            result.Should().Be(expected);
        }

		[Test()]
        public void Line_DoesIntersectPolyhedron()
        {
            //make a polyhedron
            Point bottom1 = Point.MakePointWithInches(0, 0, 0);
            Point bottom2 = Point.MakePointWithInches(4, 0, 0);
            Point bottom3 = Point.MakePointWithInches(4, 12, 0);
            Point bottom4 = Point.MakePointWithInches(0, 12, 0);

            Point top1 = Point.MakePointWithInches(0, 0, 2);
            Point top2 = Point.MakePointWithInches(4, 0, 2);
            Point top3 = Point.MakePointWithInches(4, 12, 2);
            Point top4 = Point.MakePointWithInches(0, 12, 2);

            List<Polygon> faces = new List<Polygon>();
            Polygon bottomFace = new Polygon(new List<Point> {bottom1, bottom2, bottom3, bottom4});
            Polygon topFace = new Polygon(new List<Point> { top1, top2, top3, top4 });
            Polygon frontFace = new Polygon(new List<Point> { bottom1, bottom2, top2, top1 });
            Polygon rightFace = new Polygon(new List<Point> { bottom2, bottom3, top3, top2 });
            Polygon backFace = new Polygon(new List<Point> { bottom3, bottom4, top4, top3 });
            Polygon leftFace = new Polygon(new List<Point> { bottom4, bottom1, top1, top4 });
            
            faces.Add(bottomFace);
            faces.Add(topFace);
            faces.Add(frontFace);
            faces.Add(rightFace);
            faces.Add(backFace);
            faces.Add(leftFace);
            
            Polyhedron testPolyhedron = new Polyhedron(faces);

            //now make some lines that will intersect it
            Line intersecting1 = new Line(Point.MakePointWithInches(2, 0, 1), Point.MakePointWithInches(2, 1, 1));
            Line intersecting2 = new Line(Point.MakePointWithInches(2, 0, .5), Point.MakePointWithInches(5, 12, 1));
            Line intersectingAlongSide = new Line(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 1, 0));
            Line noIntersect = new Line(Point.MakePointWithInches(5, 0, 0), Point.MakePointWithInches(5, 1, 0));

            // Temporary
            frontFace.DoesIntersect(intersecting1).Should().BeTrue();
            backFace.DoesIntersect(intersecting1).Should().BeTrue();
            // Temporary


            intersecting1.DoesIntersect(testPolyhedron).Should().BeTrue();
            intersecting2.DoesIntersect(testPolyhedron).Should().BeTrue();
            intersectingAlongSide.DoesIntersect(testPolyhedron).Should().BeTrue();
            noIntersect.DoesIntersect(testPolyhedron).Should().BeFalse();
        }

        [Test()]
        public void Line_Contains()
        {
            Line line = new Line(Direction.Right, Point.MakePointWithInches(1, 2, 3));
            Point notThere = null;
            Point offLine = Point.MakePointWithInches(0, 0, 3);
            Point onLine = Point.MakePointWithInches(0, 2, 3);

            
            (line.Contains(notThere)).Should().BeFalse();
            (line.Contains(offLine)).Should().BeFalse();
            (line.Contains(onLine)).Should().BeTrue();
        }

        [Test()]
        public void Line_Contains_PrecisionCheck()
        {
            Point point1 = Point.MakePointWithInches(1, 1, 2);
            Point point2 = Point.MakePointWithInches(-2.5, 73, 3.5);
            Line testLine = new Line(point1, point2);

            testLine.Contains(point1).Should().BeTrue();
            testLine.Contains(point2).Should().BeTrue();
        }

        [Test()]
        public void Line_UnitVector()
        {
            Line testLine = new Line(Point.MakePointWithInches(2, 3, 1), Point.MakePointWithInches(5, -7, 0));
            Vector unitVector = testLine.UnitVector(new Inch());

            unitVector.Magnitude.Should().Be(new Distance(new Centimeter(), 2.54));
            unitVector.Direction.Should().Be(testLine.Direction);
            unitVector.BasePoint.Should().Be(testLine.BasePoint);
        }
    }
}
