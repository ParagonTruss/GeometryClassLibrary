﻿using System;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.AreaUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class ArcTests
    {
       // [Test()]
        //public void Arc_Constructors()
        //{
        //    //make our default arc
        //    Point basePoint = Point.MakePointWithInches(0, 1, 3);
        //    Point endPoint = Point.MakePointWithInches(-2, 0, 5);
        //    Direction directionAtStart = new Direction(Point.MakePointWithInches(-1, 2, -2));

        //    Arc noCalculationConstructed = new Arc(basePoint, endPoint, directionAtStart);

        //    //now make it some other ways and make sure we end up with the same thing

        //    //using the center point and the containing plane     
        //    Vector directionVector = new Vector(basePoint, directionAtStart, new Distance(new Inch(), 5));
        //    Point thirdPlanePoint = directionVector.EndPoint;
        //    Plane containingPlane = new Plane(basePoint, thirdPlanePoint, endPoint);

        //    Arc testArc1 = new Arc(basePoint, noCalculationConstructed.CentralAngle, noCalculationConstructed.CenterPoint, containingPlane);
        //    (testArc1 == noCalculationConstructed).Should().BeTrue();

        //    //with the center point and the central angle
        //    Arc testArc2 = new Arc(basePoint, noCalculationConstructed.CentralAngle, noCalculationConstructed.CenterPoint, directionAtStart);
        //    (testArc2 == noCalculationConstructed).Should().BeTrue();

        //    //last contructor using radius and containing plane
        //    //Arc testArc3 = new Arc(basePoint, endPoint, noCalculationConstructed.ArcRadius, false, containingPlane);
        //    //Arc testArc4 = new Arc(basePoint, endPoint, noCalculationConstructed.ArcRadius, true, containingPlane);
        //    //(testArc3 == noCalculationConstructed).Should().BeTrue();
        //}

        [Test()]
        public void Arc_Properties_Quarter_Circle()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(3, 3, 4.24264); //sqr(3^2 + 3^2)
            Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

            Arc quarterArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = quarterArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(new Inch(), 4.24264 * Math.PI / 2);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = quarterArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(new SquareInch(), (Math.PI / 2) / 2 * (18)); //Pi/2 = 90 degrees, 18 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = quarterArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentLength = new Area(new SquareInch(), 18 / 2 * (Math.PI / 2 - Math.Sin(Math.PI / 2))); //Pi/2 = 90 degrees, 18 = r^2
            (arcSegmentArea == expectedSegmentLength).Should().BeTrue();

            //test the central angle
            Angle centralAngle = quarterArc.CentralAngle;
            (centralAngle == Angle.RightAngle).Should().BeTrue();

            //test the center point
            Point centerPoint = quarterArc.CenterPoint;
            (centerPoint == Point.MakePointWithInches(0, 0, 4.24264)).Should().BeTrue();//sqr(3^2 + 3^2)

            //test the radius
            Distance radius = quarterArc.RadiusOfCurvature;
            (radius == new Distance(new Inch(), 4.24264)).Should().BeTrue();//sqr(3^2 + 3^2)

            //test the straight line direction (same as direction)
            Direction straightDirection = quarterArc.StraightLineDirection;
            (straightDirection == new Direction(Angle.RightAngle / 2, Angle.RightAngle / 2)).Should().BeTrue();
        }

        [Test()]
        public void Arc_Properties_Half_Circle()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(0, 0, 6);
            Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

            Arc halfArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = halfArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(new Inch(), 3 * Math.PI);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = halfArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(new SquareInch(), (Math.PI) *4.5 ); //Pi = 180 degrees, 9 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = halfArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentArea = new Area(new SquareInch(), 9 * 0.5 * (Math.PI)); //Pi = 180 degrees, 9 = r^2
            (arcSegmentArea == expectedSegmentArea).Should().BeTrue();

            //test the central angle
            Angle centralAngle = halfArc.CentralAngle;
            (centralAngle == Angle.StraightAngle).Should().BeTrue();

            //test the center point
            Point centerPoint = halfArc.CenterPoint;
            (centerPoint == Point.MakePointWithInches(0, 0, 3)).Should().BeTrue();

            //test the radius
            Distance radius = halfArc.RadiusOfCurvature;
            (radius == new Distance(new Inch(), 3)).Should().BeTrue();

            //test the straight line direction (same as direction)
            Direction straightDirection = halfArc.StraightLineDirection;
            (straightDirection == new Direction(Angle.ZeroAngle, Angle.ZeroAngle)).Should().BeTrue();
        }

        [Test()]
        public void Arc_Properties_Three_Quarters_Circle()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(-3, -3, 4.24264);
            Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

            Arc threeQuartersArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = threeQuartersArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(new Inch(), 4.24264 * Math.PI * 3 / 2);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = threeQuartersArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(new SquareInch(), (Math.PI * 3 / 2) / 2 * 18); //Pi = 180 degrees, 18 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = threeQuartersArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentArea = new Area(new SquareInch(), 18 * 0.5 * (Math.PI * 3 / 2 - Math.Sin(Math.PI * 3 / 2))); //Pi = 180 degrees, 18 = r^2
            (arcSegmentArea == expectedSegmentArea).Should().BeTrue();

            //test the central angle
            Angle centralAngle = threeQuartersArc.CentralAngle;
            (centralAngle == new Angle(new Degree(), 270)).Should().BeTrue();

            //test the center point
            Point centerPoint = threeQuartersArc.CenterPoint;
            (centerPoint == Point.MakePointWithInches(0, 0, 4.24264)).Should().BeTrue();

            //test the radius
            Distance radius = threeQuartersArc.RadiusOfCurvature;
            (radius == new Distance(new Inch(), 4.24264)).Should().BeTrue();

            //test the straight line direction (same as direction)
            Direction straightDirection = threeQuartersArc.StraightLineDirection;
            (straightDirection == new Direction(new Angle(new Degree(), 225), Angle.RightAngle / 2)).Should().BeTrue();
        }

        [Test()]
        public void Arc_Translate()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(-3, 2, 1);
            Direction directionAtStart = new Direction(Point.MakePointWithInches(-1, 1, 0.5));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Arc results = testArc.Translate(Point.MakePointWithInches(-1, 2, .5));

            Arc expected = new Arc(Point.MakePointWithInches(-1, 2, 0.5), Point.MakePointWithInches(-3 - 1, 2 + 2, 1 + .5), directionAtStart);

            (results == expected).Should().BeTrue();
        }

        [Test()]
        public void Arc_Rotate()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(3, 3, 4.24264);
            Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Arc results = testArc.Rotate(new Rotation(new Line(new Direction(Point.MakePointWithInches(1, -1, 0)), Point.MakePointWithInches(0, 0, 4.24264)), Angle.RightAngle));

            Arc expected = new Arc(endPoint, Point.MakePointWithInches(0, 0, 4.24264 * 2), new Direction(Point.MakePointWithInches(0, 0, 1)));

            (results == expected).Should().BeTrue();

            //try another rotation
            Arc results2 = testArc.Rotate(new Rotation(new Line(new Direction(Point.MakePointWithInches(0, 0, 1)), Point.MakePointWithInches(1.5, 1.5, 0)), Angle.StraightAngle));

            Arc expected2 = new Arc(Point.MakePointWithInches(3, 3, 0), Point.MakePointWithInches(0, 0, 4.24264), new Direction(Point.MakePointWithInches(-1, -1, 0)));

            (results2 == expected2).Should().BeTrue();
        }

        [Test()]
        public void Arc_Shift()
        {
            //make our default arc
            Point basePoint = Point.Origin;
            Point endPoint = Point.MakePointWithInches(3, 3, 4.24264);
            Direction directionAtStart = new Direction(Point.MakePointWithInches(1, 1, 0));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Shift testShift = new Shift(new Rotation(Line.ZAxis, Angle.RightAngle / 2), Point.MakePointWithInches(-3, 0.25, -2));
            Arc results = testArc.Shift(testShift);

            Arc expected = new Arc(Point.MakePointWithInches(-3, 0.25, -2), Point.MakePointWithInches(0 - 3, 4.24264 + 0.25, 4.24264 - 2), Direction.Up);

            (results == expected).Should().BeTrue();
        }
    }
}
