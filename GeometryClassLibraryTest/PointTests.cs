using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PointTests
    {
        /// <summary>
        /// This test is to make sure that the z property is still created even if there are only 2 dimensions passed in
        /// </summary>
        [Test()]
        public void Point_2DConstructorTest()
        {
            Dimension xDimension = new Dimension(DimensionType.Millimeter, 5);
            Dimension yDimension = new Dimension(DimensionType.Millimeter, 10);

            Point p = new Point(xDimension, yDimension);

            p.X.Should().Be(new Dimension(DimensionType.Millimeter, 5));
            p.Y.Should().Be(new Dimension(DimensionType.Millimeter, 10));
            p.Z.Should().Be(new Dimension(DimensionType.Millimeter, 0));
        }

        [Test()]
        public void Point_3DConstructorTest()
        {
            Dimension xDimension = new Dimension(DimensionType.Millimeter, 0);
            Dimension yDimension = new Dimension(DimensionType.Millimeter, 0);
            Dimension zDimension = new Dimension(DimensionType.Millimeter, 0);

            Point p = new Point(xDimension, yDimension, zDimension);

            p.X.Should().Be(new Dimension(DimensionType.Millimeter, 0));
            p.Y.Should().Be(new Dimension(DimensionType.Millimeter, 0));
            p.Z.Should().Be(new Dimension(DimensionType.Millimeter, 0));
        }

        [Test()]
        public void Point_PlusOperatorStandardTest()
        {
            Dimension xDimension1 = new Dimension(DimensionType.Millimeter, 5);
            Dimension yDimension1 = new Dimension(DimensionType.Millimeter, 5);
            Dimension zDimension1 = new Dimension(DimensionType.Millimeter, 5);

            Dimension xDimension2 = new Dimension(DimensionType.Millimeter, 5);
            Dimension yDimension2 = new Dimension(DimensionType.Millimeter, 5);
            Dimension zDimension2 = new Dimension(DimensionType.Millimeter, 5);

            Dimension xDimension3 = new Dimension(DimensionType.Millimeter, -10);
            Dimension yDimension3 = new Dimension(DimensionType.Millimeter, -10);
            Dimension zDimension3 = new Dimension(DimensionType.Millimeter, -10);

            Point p1 = new Point(xDimension1, yDimension1, zDimension1);
            Point p2 = new Point(xDimension2, yDimension2, zDimension2);
            Point p3 = new Point(xDimension3, yDimension3, zDimension3);

            Point cumulativePoint = (p1 + p2 + p3);
            cumulativePoint.X.Should().Be(new Dimension(DimensionType.Millimeter, 0));
            cumulativePoint.Y.Should().Be(new Dimension(DimensionType.Millimeter, 0));
            cumulativePoint.Z.Should().Be(new Dimension(DimensionType.Millimeter, 0));
        }

        [Test()]
        public void Point_MinusOperatorStandardTest()
        {
            Dimension xDimension1 = new Dimension(DimensionType.Millimeter, 5);
            Dimension yDimension1 = new Dimension(DimensionType.Millimeter, 5);
            Dimension zDimension1 = new Dimension(DimensionType.Millimeter, 5);

            Dimension xDimension2 = new Dimension(DimensionType.Millimeter, 5);
            Dimension yDimension2 = new Dimension(DimensionType.Millimeter, 5);
            Dimension zDimension2 = new Dimension(DimensionType.Millimeter, 5);

            Dimension xDimension3 = new Dimension(DimensionType.Millimeter, -10);
            Dimension yDimension3 = new Dimension(DimensionType.Millimeter, -10);
            Dimension zDimension3 = new Dimension(DimensionType.Millimeter, -10);

            Point p1 = new Point(xDimension1, yDimension1, zDimension1);
            Point p2 = new Point(xDimension2, yDimension2, zDimension2);
            Point p3 = new Point(xDimension3, yDimension3, zDimension3);

            Point cumulativePoint = (p1 - p2 - p3);
            cumulativePoint.X.Should().Be(new Dimension(DimensionType.Millimeter, 10));
            cumulativePoint.Y.Should().Be(new Dimension(DimensionType.Millimeter, 10));
            cumulativePoint.Z.Should().Be(new Dimension(DimensionType.Millimeter, 10));


        }

        [Test()]
        public void Point_IsOnLineStandardTest()
        {
            Point testBasePoint = PointGenerator.MakePointWithInches(1, 0, 2);
            Line testLine = new Line(testBasePoint, PointGenerator.MakePointWithInches(2, 3, 1));
            Point pointOnLine = PointGenerator.MakePointWithInches(3, 6, 0);

            Point testBase = PointGenerator.MakePointWithInches(0, 0);
            Line testLine2 = new Line(testBase, PointGenerator.MakePointWithInches(5, 0));
            Point pointOnLine2 = PointGenerator.MakePointWithInches(3, 0);

            bool result = pointOnLine.IsOnLine(testLine);
            bool result2 = pointOnLine2.IsOnLine(testLine2);

            result.Should().BeTrue();
            result2.Should().BeTrue();
        }

        [Test()]
        public void Point_IsOnLineWithComponentOfDirectionEqualToZeroTest()
        {
            Point testBasePoint = PointGenerator.MakePointWithInches(1, 0, 2);
            Direction testDirection = new Direction(PointGenerator.MakePointWithInches(0, 3, 1));

            Line testLine = new Line(testDirection, testBasePoint);

            Point pointOnLine = PointGenerator.MakePointWithInches(1, 6, 4);

            pointOnLine.IsOnLine(testLine).Should().BeTrue();
        }

        [Test()]
        public void Point_Rotate3DTest()
        {
            Point originPoint = PointGenerator.MakePointWithInches(1, 0, 0);
            Angle rotationAngle = new Angle(AngleType.Degree, 90);

            Point newPoint = originPoint.Rotate3D(new Rotation(Line.ZAxis, rotationAngle));

            newPoint.Should().Be(PointGenerator.MakePointWithInches(0, 1, 0));
        }

        [Test()]
        public void Point_Rotate3DTest_AxisNotThroughOrigin()
        {
            Point pointToRotate = PointGenerator.MakePointWithInches(4, -2, 2);
            Line axis = new Line(new Direction(PointGenerator.MakePointWithInches(-1, -5, -3)), PointGenerator.MakePointWithInches(2, -2, -3));

            Angle rotationAngle = new Angle(AngleType.Degree, 322);

            Point newPoint = pointToRotate.Rotate3D(new Rotation(axis, rotationAngle));

            newPoint.Should().Be(PointGenerator.MakePointWithInches(6.2806322893240427, -1.3811031899761135, 0.20829455351884096));
        }

        [Test()]
        public void Point_Rotate3DTest_AxisNotThroughOrigin_PointIsOrigin()
        {
            Point originPoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Line axis = new Line(PointGenerator.MakePointWithInches(1, -1, 0), PointGenerator.MakePointWithInches(1, 1, 0));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            Point newPoint = originPoint.Rotate3D(new Rotation(axis, rotationAngle));

            newPoint.Should().Be(PointGenerator.MakePointWithInches(1.8480480961564261, 0, -0.52991926423320479));
        }

        [Test()]
        public void Point_IsOnLineSegment()
        {
            LineSegment testSegment = new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(5, 0));
            LineSegment testSegment2 = new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-5, -5, 0));
            Point testPointT1 = PointGenerator.MakePointWithInches(3, 0);
            Point testPointT2 = PointGenerator.MakePointWithInches(0, 0);
            Point testPointT3 = PointGenerator.MakePointWithInches(5, 0);
            Point testPointF1 = PointGenerator.MakePointWithInches(6, 0);
            Point testPointF2 = PointGenerator.MakePointWithInches(-7, 4);
            Point testPointF3 = PointGenerator.MakePointWithInches(12.7, 12.7,0);

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
            Point pointToRotate = PointGenerator.MakePointWithInches(3, 1, 2);

            Line axisLine = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 1));
            Point actualResult = pointToRotate.MirrorAcross(axisLine);

            Point expectedResult = PointGenerator.MakePointWithInches(-3, -1, 2);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_MakePerpendicularLineSegmentTest()
        {
            Point destinationLineBasePoint = PointGenerator.MakePointWithInches(1, 0, 0);
            Line destinationLine = new Line(destinationLineBasePoint, PointGenerator.MakePointWithInches(1, 1, 0));

            Point testPoint = PointGenerator.MakePointWithInches(1, 0.5, 0.5);

            LineSegment actualResult = testPoint.MakePerpendicularLineSegment(destinationLine);

            LineSegment expectedResult = new LineSegment(testPoint, PointGenerator.MakePointWithInches(1, .5, 0));

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_MakePerpendicularLineSegmentTest2()
        {
            Point destinationLineBasePoint = PointGenerator.MakePointWithInches(2,3,4);
            Line destinationLine = new Line(new Direction(PointGenerator.MakePointWithInches(6,4,-6)), destinationLineBasePoint);

            Point testPoint = PointGenerator.MakePointWithInches(0,0,0);

            LineSegment actualResult = testPoint.MakePerpendicularLineSegment(destinationLine);

            LineSegment expectedResult = new LineSegment(testPoint, PointGenerator.MakePointWithInches(2, 3, 4));

            actualResult.Should().Be(expectedResult);
        }


        [Test()]
        public void Point_TranslateTest()
        {
            Point pointToTranslate = PointGenerator.MakePointWithInches(1, 2, 3);
            //Direction directionToTranslate = new Direction(PointGenerator.MakePointWithInches(-1, 5, 4));
            //Dimension displacementOfTranslation = new Dimension(DimensionType.Inch, 12.9614814);
            Point testDisplacement = PointGenerator.MakePointWithInches(-2, 10, 8);

            Point actualResult = pointToTranslate.Translate(testDisplacement);

            Point expectedResult = PointGenerator.MakePointWithInches(-1, 12, 11);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_TranslateTest_OneComponent()
        {
            Point pointToTranslate = PointGenerator.MakePointWithInches(1,1,1);
            
            //Direction directionToTranslate = new Direction(PointGenerator.MakePointWithInches(1, 0, 0));
            //Dimension displacementOfTranslation = new Dimension(DimensionType.Inch, 4);
            Point testDisplacement = PointGenerator.MakePointWithInches(4, 0, 0);

            Point actualResult = pointToTranslate.Translate(testDisplacement);

            Point expectedResult = PointGenerator.MakePointWithInches(5,1,1);

            actualResult.Should().Be(expectedResult);
        }

        [Test()]
        public void Point_ShiftTest()
        {            
            Point point1 = PointGenerator.MakePointWithInches(1, 1, 0);

            Point displacementPoint = PointGenerator.MakePointWithInches(1, -1, 1);

            Angle angleAboutZAxis = new Angle(AngleType.Degree, 45);
            Rotation zRotation = new Rotation(Line.ZAxis, angleAboutZAxis);
            Angle angleAboutXAxis = new Angle(AngleType.Degree, 112);
            Rotation xRotation = new Rotation(Line.XAxis, angleAboutXAxis);
            Shift testShift = new Shift(new List<Rotation>() { zRotation, xRotation }, displacementPoint);

            Point actual1 = point1.Shift(testShift);

            Point expected1 = PointGenerator.MakePointWithInches(1 + 0, -1 + -0.5298, 1 + 1.3112);

            actual1.Should().Be(expected1);
        }

        [Test()]
        public void Point_ShiftTest_RotateOnly()
        {
            Point point1 = PointGenerator.MakePointWithInches(1, 1, 0);

            Angle angleAboutZAxis = new Angle(AngleType.Degree, 45);
            Rotation zRotation = new Rotation(Line.ZAxis, angleAboutZAxis);
            Angle angleAboutXAxis = new Angle(AngleType.Degree, 112);
            Rotation xRotation = new Rotation(Line.XAxis, angleAboutXAxis);
            Shift testShift = new Shift(new List<Rotation>() { zRotation, xRotation });

            Point actual1 = point1.Shift(testShift);

            Point expected1 = PointGenerator.MakePointWithInches(0, -0.52977372496316655, 1.3112359819417141);

            actual1.Should().Be(expected1);
        }

    }
}