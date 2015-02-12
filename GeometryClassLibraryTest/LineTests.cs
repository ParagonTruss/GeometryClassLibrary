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
    public class LineTests
    {
        [Test()]
        public void Line_GetPointOnLineTest()
        {
            Point testBasePoint = PointGenerator.MakePointWithInches(1, 0, 2);

            Line testLine = new Line(testBasePoint, PointGenerator.MakePointWithInches(2, 3, 1));

            Point pointOnLine = PointGenerator.MakePointWithInches(3, 6, 0);

            testLine.GetPointOnLine(6.62).Should().Be(pointOnLine);         
        }

        [Test()]
        public void Line_IntersectionTest_Inches()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(-2, 1, 3));

            Point expectedResult = PointGenerator.MakePointWithInches(0, 2.5, 1.5);
            Point actualResult = line1.Intersection(line2);

            actualResult.Should().Be(expectedResult);  
        }

        [Test()]
        public void Line_IntersectionTest_Millimeters()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(-2, 1, 3));

            Point expectedResult = PointGenerator.MakePointWithInches(0, 2.5, 1.5);
            Point actualResult = line1.Intersection(line2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(1, 2, 2);
            Point basePointLine2 = PointGenerator.MakePointWithInches(1, 1, 2);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 1, 2));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(1, 1, 1));

            Point expectedResult = PointGenerator.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.Intersection(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular2()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(1, 1, 2);
            Point basePointLine2 = PointGenerator.MakePointWithInches(1, 1, 2);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 7, 2));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(1, 1, 6));

            Point expectedResult = PointGenerator.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.Intersection(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_IntersectionTest_Perpendicular3()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(1, 2, 2);
            Point basePointLine2 = PointGenerator.MakePointWithInches(1, 1, 1);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 1, 2));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(1, 1, 2));

            Point expectedResult = PointGenerator.MakePointWithInches(1, 1, 2);
            Point actualResult = line1.Intersection(line2);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void IntersectionTest_Origin()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 0, 0));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(0, 0, 1));

            Point expectedResult = PointGenerator.MakePointWithInches(0, 0, 0);
            Point actualResult = line1.Intersection(line2);

            (expectedResult == actualResult).Should().BeTrue();  
        }


        [Test()]
        public void Line_AngleBetweenIntersectingLineTest_Inches()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(-2, 1, 3));

            Angle expectedResult = new Angle(AngleType.Radian, 1.080839);
            Angle actualResult = line1.SmallestAngleBetweenIntersectingLine(line2);
            
            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Line_AngleBetweenIntersectingLineTest_Millimeters()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(2, 1, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(2, 4, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(-2, 4, 3));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(-2, 1, 3));

            Angle expectedResult = new Angle(AngleType.Radian, 1.080839);
            Angle actualResult = line1.SmallestAngleBetweenIntersectingLine(line2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void AngleBetweenIntersectingLineTest_IntersectAtOrigin()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 1, 1));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(0, 0, 1));

            double angleBetweenLinesInRadians = Math.Acos(1 / Math.Sqrt(3));
            Angle angleBetweenLines = new Angle(AngleType.Radian, angleBetweenLinesInRadians);

            line1.SmallestAngleBetweenIntersectingLine(line2).Should().Be(angleBetweenLines);
        }

        [Test()]
        public void Line_AngleBetweenIntersectingLinePerpendicularTest()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point basePointLine2 = PointGenerator.MakePointWithInches(0, 0, 0);

            Line line1 = new Line(basePointLine1, PointGenerator.MakePointWithInches(1, 0, 0));
            Line line2 = new Line(basePointLine2, PointGenerator.MakePointWithInches(0, 0, 1));

            double angleBetweenLinesInRadians = Math.Acos(0);
            Angle angleBetweenLines = new Angle(AngleType.Radian, angleBetweenLinesInRadians);

            line1.SmallestAngleBetweenIntersectingLine(line2).Should().Be(angleBetweenLines);

        }

        [Test()]
        public void Line_CoplanarTest()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 1, 0));
            Line line2 = new Line(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(1, 1, 0));
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 0), PointGenerator.MakePointWithInches(1, 0, 0));
            Line line4 = new Line(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(3, 0, 4));

            line1.IsCoplanarWith(line2).Should().BeTrue();
            line3.IsCoplanarWith(line4).Should().BeFalse();
            line1.IsCoplanarWith(line3).Should().BeTrue();
            line4.IsCoplanarWith(line2).Should().BeFalse();
        }

        [Test()]
        public void Line_ParallelTest()
        {
            Line line1 = new LineSegment(PointGenerator.MakePointWithInches(0,0), PointGenerator.MakePointWithInches(5,0));
            Line line2 = new LineSegment(PointGenerator.MakePointWithInches(-3,-3), PointGenerator.MakePointWithInches(8,-3));
            Line line3 = new LineSegment(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            Line line4 = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            Line line5 = new LineSegment(PointGenerator.MakePointWithInches(-3, -3), PointGenerator.MakePointWithInches(-3, 8));
            Line line6 = new LineSegment(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(1,4));
             
            line1.IsParallelTo(line2).Should().BeTrue();
            line2.IsParallelTo(line3).Should().BeTrue();

            line4.IsParallelTo(line5).Should().BeTrue();
            line5.IsParallelTo(line6).Should().BeTrue();

            line1.IsParallelTo(line4).Should().BeFalse();
            line5.IsParallelTo(line3).Should().BeFalse();
        }

        [Test()]
        public void Line_PerpindicularTest()
        {
            Line line1 = new LineSegment(new Point(), new Direction(new Angle(AngleType.Degree, 45)), new Distance(DistanceType.Inch, 1));
            Line line2 = new LineSegment(new Point(), new Direction(new Angle(AngleType.Degree, 135)), new Distance(DistanceType.Inch, 1));
            Line line3 = new LineSegment(PointGenerator.MakePointWithInches(2,-3,1), new Direction(new Angle(AngleType.Degree, 45), new Angle()), new Distance(DistanceType.Inch, 1));

            Line line4 = new LineSegment(PointGenerator.MakePointWithInches(3, 5, 7));
            Line line5 = new LineSegment(PointGenerator.MakePointWithInches(1, -2, 1));
            Line line6 = new LineSegment(PointGenerator.MakePointWithInches(1, 2, -3), PointGenerator.MakePointWithInches(5, 1, -4)); //4, -1, -1

            line1.IsPerpindicularTo(line2).Should().BeTrue();
            line1.IsPerpindicularTo(line3).Should().BeTrue();
            line2.IsPerpindicularTo(line3).Should().BeTrue();
            line3.IsPerpindicularTo(line1).Should().BeTrue(); //check its reversable

            line4.IsPerpindicularTo(line5).Should().BeTrue();
            line4.IsPerpindicularTo(line6).Should().BeTrue();
            line5.IsPerpindicularTo(line6).Should().BeFalse();
            line6.IsPerpindicularTo(line4).Should().BeTrue(); //check its reversable

            line1.IsPerpindicularTo(line4).Should().BeFalse();
            
        }

        [Test()]
        public void Line_RotateTest_AboutZAxis()
        {
            Point basePointLine1 = PointGenerator.MakePointWithInches(2, 1, 0);
            Point otherPointLine1 = PointGenerator.MakePointWithInches(3, 3, 3);

            Line line1 = new Line(basePointLine1, otherPointLine1);
            Line axisLine = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 1));

            Angle rotationAngle = new Angle(AngleType.Degree, 199);

            Line actualResult = line1.Rotate(new Rotation(axisLine, rotationAngle));

            Point expectedResultBasePoint = PointGenerator.MakePointWithInches(-1.5654689967414768, -1.5966548845136304, 0.0);
            Direction expectedDirection = new Direction(PointGenerator.MakePointWithInches(-0.29438226668500322,-2.2166053056557904,3.0));
            Line expectedResult = new Line(expectedDirection, expectedResultBasePoint);

            (expectedResult == actualResult).Should().BeTrue();
        }

        [Test()]
        public void Line_RotateTest_AboutReferencePoint()
        {
            Point start = PointGenerator.MakePointWithInches(0, 1, 3);
            Point end = PointGenerator.MakePointWithInches(0, 1, 5);

            Line toRotate = new Line(start, end);
            Line startPointYAxis = new Line(start, PointGenerator.MakePointWithInches(0, 2, 3)); //relative y axis

            Angle rotationAngle = new Angle(AngleType.Degree, 90);

            Line afterRotate = toRotate.Rotate(new Rotation(startPointYAxis, rotationAngle));

            Point expectedStart = new Point(start);
            Point expectedEnd = PointGenerator.MakePointWithInches(2, 1, 3);
            Line expectedResult = new Line(expectedStart, expectedEnd);

            (expectedResult == afterRotate).Should().BeTrue();
        }

        [Test()]
        public void Line_TranslateTest()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(1, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));

            //Direction testDirection = new Direction(PointGenerator.MakePointWithInches(-1, 5, 4));
            //Distance testDisplacement = new Distance(DistanceType.Inch, 12.9614814);
            Point testDisplacement = PointGenerator.MakePointWithInches(-2, 10, 8);

            Line actualLine1 = line1.Translate(testDisplacement);

            Line expectedLine1 = new Line(PointGenerator.MakePointWithInches(-1, 12, 11), PointGenerator.MakePointWithInches(-5, 8, 8));

            (actualLine1 == expectedLine1).Should().BeTrue();
        }

        [Test()]
        public void Line_DoesIntersectLineTest()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 5));
            Line line2 = new Line(PointGenerator.MakePointWithInches(5, 0), PointGenerator.MakePointWithInches(-3, 0));
            Line line3 = new Line(PointGenerator.MakePointWithInches(3, 3, 0), PointGenerator.MakePointWithInches(-3, -3, 2));
            Line line4 = new Line(PointGenerator.MakePointWithInches(3, -3, 0), PointGenerator.MakePointWithInches(-3, 3, 2));

            bool resultT1 = line1.DoesIntersect(line2);
            bool resultF1 = line2.DoesIntersect(line3);
            bool resultT2 = line3.DoesIntersect(line4);

            resultT1.Should().BeTrue();
            resultF1.Should().BeFalse();
            resultT2.Should().BeTrue();
        }

        [Test()]
        public void Line_DoesIntersectLineSegmentTest()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 0, 0), PointGenerator.MakePointWithInches(-3, 0, 0));
            LineSegment segment1 = new LineSegment(PointGenerator.MakePointWithInches(3, -3, 0), PointGenerator.MakePointWithInches(2, 0, 0));

            bool resultT1 = line1.DoesIntersect(segment1);
        }


        [Test()]
        public void Line_PlaneThroughLineInDirectionOf_ZAxis()
        {
            Line test1 = new Line(PointGenerator.MakePointWithMillimeters(2, 1, -1), PointGenerator.MakePointWithMillimeters(-1, 5, 0));
            Line test2 = new Line(PointGenerator.MakePointWithMillimeters(2, 1, -1),  PointGenerator.MakePointWithMillimeters(-1, 5, 2));

            Plane result1 = test1.PlaneThroughLineInDirectionOf(Enums.Axis.Z);
            Plane result2 = test2.PlaneThroughLineInDirectionOf(Enums.Axis.Z);

            Plane expectedPlane = new Plane(PointGenerator.MakePointWithMillimeters(2, 1, 6), PointGenerator.MakePointWithMillimeters(2, 1, -1), PointGenerator.MakePointWithMillimeters(-1, 5, -23));

            result1.Should().Be(result2);
            result1.Should().Be(expectedPlane);
        }

        [Test()]
        public void Line_XZIntercept()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            Point intercept1 = line1.XZIntercept;
            Point intercept2 = line2.XZIntercept;
            Point intercept3 = line3.XZIntercept;
            Point intercept4 = line4.XZIntercept;
            Point intercept5 = line5.XZIntercept;

            intercept1.Should().Be(PointGenerator.MakePointWithInches(-1, 0, 1));
            intercept2.Should().Be(PointGenerator.MakePointWithInches(6, 0, 0));
            intercept3.Should().Be(PointGenerator.MakePointWithInches(0, 0, 6));
            intercept4.Should().Be(PointGenerator.MakePointWithInches(4, 0, 3));
            intercept5.Should().Be(null);
        }

        [Test()]
        public void Line_YZIntercept()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(2, 2, 2), PointGenerator.MakePointWithInches(4, 3, 7)); //intersects at 0, 1, -3
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-6, 3, -1)); //intersects at 0, 1.5, -0.5
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(PointGenerator.MakePointWithInches(2, 1, 1), PointGenerator.MakePointWithInches(3, 2, 2)); //intersects at 0, -1, -1
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            Point intercept1 = line1.YZIntercept;
            Point intercept2 = line2.YZIntercept;
            Point intercept3 = line3.YZIntercept;
            Point intercept4 = line4.YZIntercept;
            Point intercept5 = line5.YZIntercept;

            intercept1.Should().Be(PointGenerator.MakePointWithInches(0, 1, -3));
            intercept2.Should().Be(PointGenerator.MakePointWithInches(0, 1.5, -.5));
            intercept3.Should().Be(PointGenerator.MakePointWithInches(0, 0, 6));
            intercept4.Should().Be(PointGenerator.MakePointWithInches(0, -1, -1));
            intercept5.Should().Be(null);
        }

        [Test()]
        public void Line_XYIntercept()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 1), PointGenerator.MakePointWithInches(5, 3, 2)); //intersects at 1, 1, 0
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, -2), PointGenerator.MakePointWithInches(2, 3, -1)); //intersects at -2, 6, 0
            Line line3 = new Line(PointGenerator.MakePointWithInches(3, 3, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at -2,-2,0
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4, 15, 0
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 1, 2)); //doesnt intersect

            Point intercept1 = line1.XYIntercept;
            Point intercept2 = line2.XYIntercept;
            Point intercept3 = line3.XYIntercept;
            Point intercept4 = line4.XYIntercept;
            Point intercept5 = line5.XYIntercept;

            intercept1.Should().Be(PointGenerator.MakePointWithInches(1, 1, 0));
            intercept2.Should().Be(PointGenerator.MakePointWithInches(-2, 6, 0));
            intercept3.Should().Be(PointGenerator.MakePointWithInches(-2, -2, 0));
            intercept4.Should().Be(PointGenerator.MakePointWithInches(4, 15, 0));
            intercept5.Should().Be(null);
        }

        [Test()]
        public void Line_XInterceptIn2D()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3

            Distance intercept1 = line1.XInterceptIn2D;
            Distance intercept2 = line2.XInterceptIn2D;
            Distance intercept3 = line3.XInterceptIn2D;
            Distance intercept4 = line4.XInterceptIn2D;

            intercept1.Should().Be(new Distance(DistanceType.Inch, -1));
            intercept2.Should().Be(new Distance(DistanceType.Inch, 6));
            intercept3.Should().Be(new Distance(DistanceType.Inch, 0));
            intercept4.Should().Be(new Distance(DistanceType.Inch, 4));
        }

        [Test()]
        [ExpectedException(typeof(Exception))]
        public void Line_XInterceptIn2D_LineDoesNotIntersectXAxis()
        {            
            Line line = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            //should throw an exception
            Distance intercept = line.XInterceptIn2D;
        }

        [Test()]
        public void Line_YInterceptIn2D()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(2, 2, 2), PointGenerator.MakePointWithInches(4, 3, 7)); //intersects at 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-6, 3, -1)); //intersects at 1.5
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0
            Line line4 = new Line(PointGenerator.MakePointWithInches(2, 1, 1), PointGenerator.MakePointWithInches(3, 2, 2)); //intersects at -1

            Distance intercept1 = line1.YInterceptIn2D;
            Distance intercept2 = line2.YInterceptIn2D;
            Distance intercept3 = line3.YInterceptIn2D;
            Distance intercept4 = line4.YInterceptIn2D;

            intercept1.Should().Be(new Distance(DistanceType.Inch, 1));
            intercept2.Should().Be(new Distance(DistanceType.Inch, 1.5));
            intercept3.Should().Be(new Distance(DistanceType.Inch, 0));
            intercept4.Should().Be(new Distance(DistanceType.Inch, -1));
        }

        [Test()]
        [ExpectedException(typeof(Exception))]
        public void Line_YInterceptIn2D_LineDoesNotIntersectYAxis()
        {
            Line line = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            //should throw an exception
            Distance intercept5 = line.YInterceptIn2D;
        }

        [Test()]
        public void Line_Sort()
        {
            Line line1 = new Line(PointGenerator.MakePointWithInches(3, 2, 5), PointGenerator.MakePointWithInches(5, 3, 7)); //intersects at -1, 0, 1
            Line line2 = new Line(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(-5, 3, -1)); //intersects at 6, 0, 0
            Line line3 = new Line(PointGenerator.MakePointWithInches(1, 1, 5), PointGenerator.MakePointWithInches(2, 2, 4)); //intersects at 0, 0, 6
            Line line4 = new Line(PointGenerator.MakePointWithInches(4, 10, 1), PointGenerator.MakePointWithInches(4, 5, 2)); //intersects at 4, 0, 3
            Line line5 = new Line(PointGenerator.MakePointWithInches(4, 2, 2), PointGenerator.MakePointWithInches(4, 2, 1)); //doesnt intersect

            List<Line> lines = new List<Line> { line2, line3, line5, line4, line1 };

            lines.Sort();

            lines[0].Should().Be(line1);
            lines[1].Should().Be(line3);
            lines[2].Should().Be(line4);
            lines[3].Should().Be(line2);
            lines[4].Should().Be(line5);
        }

        [Test()]
        public void Line_ProjectOntoPlane()
        {
            Line toProject = new Line(PointGenerator.MakePointWithInches(2, 2, 1));
            Plane projectOnto = new Plane(Line.XAxis, Line.YAxis);

            Line result = toProject.ProjectOntoPlane(projectOnto);

            Line expected = new Line(PointGenerator.MakePointWithInches(2, 2, 0));

            result.Should().Be(expected);
        }

        [Test()]
        public void Line_DoesIntersectPolyhedron()
        {
            //make a polyhedron
            Point bottomLeft = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeft = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRight = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRight = PointGenerator.MakePointWithInches(4, 12, 0);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(0, 0, 2);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 12, 2);
            Point bottomRightBack = PointGenerator.MakePointWithInches(4, 0, 2);
            Point topRightBack = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            planes.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            planes.Add(new Polygon(new List<Point> { topLeft, topRight, topRightBack, topLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, bottomRight, bottomRightBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topLeftBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomRight, topRight, topRightBack, bottomRightBack }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            //now make some lines that will intersect it
            Line intersecting1 = new Line(PointGenerator.MakePointWithInches(2, 0, 1), PointGenerator.MakePointWithInches(2, 1, 1));
            Line intersecting2 = new Line(PointGenerator.MakePointWithInches(2, 0, .5), PointGenerator.MakePointWithInches(5, 12, 1));
            Line intersectingAlongSide = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 1, 0));
            Line noIntersect = new Line(PointGenerator.MakePointWithInches(5, 0, 0), PointGenerator.MakePointWithInches(5, 1, 0));

            intersecting1.DoesIntersect(testPolyhedron).Should().BeTrue();
            intersecting2.DoesIntersect(testPolyhedron).Should().BeTrue();
            intersectingAlongSide.DoesIntersect(testPolyhedron).Should().BeTrue();
            noIntersect.DoesIntersect(testPolyhedron).Should().BeFalse();
        }
    }
}