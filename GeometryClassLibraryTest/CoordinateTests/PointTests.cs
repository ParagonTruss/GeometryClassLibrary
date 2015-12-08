using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Metric.MillimeterUnit;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class PointTests
    {
        /// <summary>
        /// This test is to make sure that the z property is still created even if there are only 2 Distances passed in
        /// </summary>
        [Test()]
        public void Point_2DConstructorTest()
        {
            Distance xDistance = new Distance(new Millimeter(), 5);
            Distance yDistance = new Distance(new Millimeter(), 10);

            Point p = new Point(xDistance, yDistance);

            p.X.Should().Be(new Distance(new Millimeter(), 5));
            p.Y.Should().Be(new Distance(new Millimeter(), 10));
            p.Z.Should().Be(new Distance(new Millimeter(), 0));
        }

        [Test()]
        public void Point_3DConstructorTest()
        {
            Distance xDistance = new Distance(new Millimeter(), 0);
            Distance yDistance = new Distance(new Millimeter(), 0);
            Distance zDistance = new Distance(new Millimeter(), 0);

            Point p = new Point(xDistance, yDistance, zDistance);

            p.X.Should().Be(new Distance(new Millimeter(), 0));
            p.Y.Should().Be(new Distance(new Millimeter(), 0));
            p.Z.Should().Be(new Distance(new Millimeter(), 0));
        }

        [Test()]
        public void Point_JSON()
        {
            Point point = Point.MakePointWithInches(1, 2, 2);

            var json = JsonConvert.SerializeObject(point);
            Point deserializedPoint = JsonConvert.DeserializeObject<Point>(json);

            bool areEqual = (point == deserializedPoint);
            areEqual.Should().BeTrue();
        }

        [Test()]
        public void Point_PlusOperatorStandardTest()
        {
            Distance xDistance1 = new Distance(new Millimeter(), 5);
            Distance yDistance1 = new Distance(new Millimeter(), 5);
            Distance zDistance1 = new Distance(new Millimeter(), 5);

            Distance xDistance2 = new Distance(new Millimeter(), 5);
            Distance yDistance2 = new Distance(new Millimeter(), 5);
            Distance zDistance2 = new Distance(new Millimeter(), 5);

            Distance xDistance3 = new Distance(new Millimeter(), -10);
            Distance yDistance3 = new Distance(new Millimeter(), -10);
            Distance zDistance3 = new Distance(new Millimeter(), -10);

            Point p1 = new Point(xDistance1, yDistance1, zDistance1);
            Point p2 = new Point(xDistance2, yDistance2, zDistance2);
            Point p3 = new Point(xDistance3, yDistance3, zDistance3);

            Point cumulativePoint = (p1 + p2 + p3);
            cumulativePoint.X.Should().Be(new Distance(new Millimeter(), 0));
            cumulativePoint.Y.Should().Be(new Distance(new Millimeter(), 0));
            cumulativePoint.Z.Should().Be(new Distance(new Millimeter(), 0));
        }

        [Test()]
        public void Point_MinusOperatorStandardTest()
        {
            Distance xDistance1 = new Distance(new Millimeter(), 5);
            Distance yDistance1 = new Distance(new Millimeter(), 5);
            Distance zDistance1 = new Distance(new Millimeter(), 5);

            Distance xDistance2 = new Distance(new Millimeter(), 5);
            Distance yDistance2 = new Distance(new Millimeter(), 5);
            Distance zDistance2 = new Distance(new Millimeter(), 5);

            Distance xDistance3 = new Distance(new Millimeter(), -10);
            Distance yDistance3 = new Distance(new Millimeter(), -10);
            Distance zDistance3 = new Distance(new Millimeter(), -10);

            Point p1 = new Point(xDistance1, yDistance1, zDistance1);
            Point p2 = new Point(xDistance2, yDistance2, zDistance2);
            Point p3 = new Point(xDistance3, yDistance3, zDistance3);

            Point cumulativePoint = (p1 - p2 - p3);
            cumulativePoint.X.Should().Be(new Distance(new Millimeter(), 10));
            cumulativePoint.Y.Should().Be(new Distance(new Millimeter(), 10));
            cumulativePoint.Z.Should().Be(new Distance(new Millimeter(), 10));


        }

        [Test()]
        public void Point_IsOnLineStandardTest()
        {
            Point testBasePoint = Point.MakePointWithInches(1, 0, 2);
            Line testLine = new Line(testBasePoint, Point.MakePointWithInches(2, 3, 1));
            Point pointOnLine = Point.MakePointWithInches(3, 6, 0);

            Point testBase = Point.MakePointWithInches(0, 0);
            Line testLine2 = new Line(testBase, Point.MakePointWithInches(5, 0));
            Point pointOnLine2 = Point.MakePointWithInches(3, 0);

            bool result = pointOnLine.IsOnLine(testLine);
            bool result2 = pointOnLine2.IsOnLine(testLine2);
            bool result3 = testBasePoint.IsOnLine(testLine2);

            result.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeFalse();
        }

        [Test()]
        public void Point_IsOnLineWithComponentOfDirectionEqualToZeroTest()
        {
            Point testBasePoint = Point.MakePointWithInches(1, 0, 2);
            Direction testDirection = new Direction(Point.MakePointWithInches(0, 3, 1));

            Line testLine = new Line(testDirection, testBasePoint);

            Point pointOnLine = Point.MakePointWithInches(1, 6, 4);

            pointOnLine.IsOnLine(testLine).Should().BeTrue();
        }

        [Test()]
        public void Point_Rotate3DTest()
        {
            Point originPoint = Point.MakePointWithInches(1, 0, 0);
            Angle rotationAngle = new Angle(new Degree(), 90);

            Point newPoint = originPoint.Rotate3D(new Rotation(Line.ZAxis, rotationAngle));

            newPoint.Should().Be(Point.MakePointWithInches(0, 1, 0));
        }

        [Test()]
        public void Point_Rotate3DTest_AxisNotThroughOrigin()
        {
            Point pointToRotate = Point.MakePointWithInches(4, -2, 2);
            Line axis = new Line(new Direction(Point.MakePointWithInches(-1, -5, -3)), Point.MakePointWithInches(2, -2, -3));

            Angle rotationAngle = new Angle(new Degree(), 322);

            Point newPoint = pointToRotate.Rotate3D(new Rotation(axis, rotationAngle));

            newPoint.Should().Be(Point.MakePointWithInches(6.2806322893240427, -1.3811031899761135, 0.20829455351884096));
        }

        [Test()]
        public void Point_Rotate3DTest_AxisNotThroughOrigin_PointIsOrigin()
        {
            Point originPoint = Point.MakePointWithInches(0, 0, 0);
            Line axis = new Line(Point.MakePointWithInches(1, -1, 0), Point.MakePointWithInches(1, 1, 0));
            Angle rotationAngle = new Angle(new Degree(), 212);

            Point newPoint = originPoint.Rotate3D(new Rotation(axis, rotationAngle));

            newPoint.Should().Be(Point.MakePointWithInches(1.8480480961564261, 0, -0.52991926423320479));
        }

        [Test()]
        public void Point_IsOnLineSegment()
        {
            LineSegment testSegment = new LineSegment(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(5, 0));
            LineSegment testSegment2 = new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(-5, -5, 0));
            Point testPointT1 = Point.MakePointWithInches(3, 0);
            Point testPointT2 = Point.MakePointWithInches(0, 0);
            Point testPointT3 = Point.MakePointWithInches(5, 0);
            Point testPointF1 = Point.MakePointWithInches(6, 0);
            Point testPointF2 = Point.MakePointWithInches(-7, 4);
            Point testPointF3 = Point.MakePointWithInches(12.7, 12.7,0);

            bool resultT1 = testPointT1.IsOnLineSegment(testSegment);
            bool resultT2 = testPointT2.IsOnLineSegment(testSegment);
            bool resultT3 = testPointT3.IsOnLineSegment(testSegment);
            bool resultF1 = testPointF1.IsOnLineSegment(testSegment);
            bool resultF2 = testPointF2.IsOnLineSegment(testSegment);
            bool resultF3 = testPointF3.IsOnLineSegment(testSegment2);

            resultT1.Should().BeTrue();
            resultT2.Should().BeTrue();
            resultT3.Should().BeTrue();
            resultF1.Should().BeFalse();
            resultF2.Should().BeFalse();
            resultF3.Should().BeFalse();
        }

        [Test()]
        public void Point_MirrorAcrossTest_ZAxis()
        {
            Point pointToRotate = Point.MakePointWithInches(3, 1, 2);

            Line axisLine = new Line(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 0, 1));
            Point actualResult = pointToRotate.MirrorAcross(axisLine);

            Point expectedResult = Point.MakePointWithInches(-3, -1, 2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_MakePerpendicularLineSegmentTest()
        {
            Point destinationLineBasePoint = Point.MakePointWithInches(1, 0, 0);
            Line destinationLine = new Line(destinationLineBasePoint, Point.MakePointWithInches(1, 1, 0));

            Point testPoint = Point.MakePointWithInches(1, 0.5, 0.5);

            LineSegment actualResult = testPoint.MakePerpendicularLineSegment(destinationLine);

            LineSegment expectedResult = new LineSegment(testPoint, Point.MakePointWithInches(1, .5, 0));

            (actualResult == expectedResult).Should().BeTrue();
        }

        [Test()]
        public void Point_MakePerpendicularLineSegmentTest2()
        {
            Point destinationLineBasePoint = Point.MakePointWithInches(2,3,4);
            Line destinationLine = new Line(new Direction(Point.MakePointWithInches(6,4,-6)), destinationLineBasePoint);

            Point testPoint = Point.MakePointWithInches(0,0,0);

            LineSegment actualResult = testPoint.MakePerpendicularLineSegment(destinationLine);

            LineSegment expectedResult = new LineSegment(testPoint, Point.MakePointWithInches(2, 3, 4));

            actualResult.Should().Be(expectedResult);
        }


        [Test()]
        public void Point_TranslateTest()
        {
            Point pointToTranslate = Point.MakePointWithInches(1, 2, 3);
            //Direction directionToTranslate = new Direction(Point.MakePointWithInches(-1, 5, 4));
            //Distance displacementOfTranslation = new Distance(new Inch(), 12.9614814);
            Point testDisplacement = Point.MakePointWithInches(-2, 10, 8);

            Point actualResult = pointToTranslate.Translate(testDisplacement);

            Point expectedResult = Point.MakePointWithInches(-1, 12, 11);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_TranslateTest_OneComponent()
        {
            Point pointToTranslate = Point.MakePointWithInches(1,1,1);
            
            //Direction directionToTranslate = new Direction(Point.MakePointWithInches(1, 0, 0));
            //Distance displacementOfTranslation = new Distance(new Inch(), 4);
            Point testDisplacement = Point.MakePointWithInches(4, 0, 0);

            Point actualResult = pointToTranslate.Translate(testDisplacement);

            Point expectedResult = Point.MakePointWithInches(5,1,1);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_ShiftTest()
        {            
            Point point1 = Point.MakePointWithInches(1, 1, 0);

            Point displacementPoint = Point.MakePointWithInches(1, -1, 1);

            Angle angleAboutZAxis = new Angle(new Degree(), 45);
            Rotation zRotation = new Rotation(Line.ZAxis, angleAboutZAxis);
            Angle angleAboutXAxis = new Angle(new Degree(), 112);
            Rotation xRotation = new Rotation(Line.XAxis, angleAboutXAxis);
            Shift testShift = new Shift(new List<Rotation>() { zRotation, xRotation }, displacementPoint);

            Point actual1 = point1.Shift(testShift);

            Point expected1 = Point.MakePointWithInches(1 + 0, -1 + -0.5298, 1 + 1.3112);

            actual1.Should().Be(expected1);
        }

        [Test()]
        public void Point_ShiftTest_RotateOnly()
        {
            Point point1 = Point.MakePointWithInches(1, 1, 0);

            Angle angleAboutZAxis = new Angle(new Degree(), 45);
            Rotation zRotation = new Rotation(Line.ZAxis, angleAboutZAxis);
            Angle angleAboutXAxis = new Angle(new Degree(), 112);
            Rotation xRotation = new Rotation(Line.XAxis, angleAboutXAxis);
            Shift testShift = new Shift(new List<Rotation>() { zRotation, xRotation });

            Point actual1 = point1.Shift(testShift);

            Point expected1 = Point.MakePointWithInches(0, -0.52977372496316655, 1.3112359819417141);

            actual1.Should().Be(expected1);
        }

        [Test]
        public void Point_ProjectOntoPlane()
        {
            Point testPoint = Point.MakePointWithInches(3, -2, 5);

            var projection1 = testPoint.ProjectOntoPlane(Plane.XY);
            (projection1 == Point.MakePointWithInches(3, -2, 0)).Should().BeTrue();

            var projection2 = testPoint.ProjectOntoPlane(Plane.YZ);
            (projection2 == Point.MakePointWithInches(0, -2, 5)).Should().BeTrue();

            var projection3 = testPoint.ProjectOntoPlane(Plane.XZ);
            (projection3 == Point.MakePointWithInches(3, 0, 5)).Should().BeTrue();

            //Now for a harder case:
            var testPlane = new Plane(new Vector(Point.MakePointWithInches(7, -4, 1), Point.MakePointWithInches(-6, 5, 2)));
            var projection = testPoint.ProjectOntoPlane(testPlane);
            var expected = Point.MakePointWithInches(6.83, -4.65, 4.7);
          
            (projection == expected).Should().BeTrue();
        }
    }
}