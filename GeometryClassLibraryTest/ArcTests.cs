using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class ArcTests
    {
        [Test()]
        public void Arc_Constructors()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 1, 3);
            Point endPoint = PointGenerator.MakePointWithInches(-2, 0, 5);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(-1, 2, -2));

            Arc noCalculationConstructed = new Arc(basePoint, endPoint, directionAtStart);

            //now make it some other ways and make sure we end up with the same thing

            //using the center point and the containing plane     
            Vector directionVector = new Vector(basePoint, directionAtStart, new Distance(DistanceType.Inch, 5));
            Point thirdPlanePoint = directionVector.EndPoint;
            Plane containingPlane = new Plane(basePoint, thirdPlanePoint, endPoint);

            Arc testArc1 = new Arc(basePoint, noCalculationConstructed.CentralAngle, noCalculationConstructed.CenterPoint, containingPlane);
            (testArc1 == noCalculationConstructed).Should().BeTrue();

            //with the center point and the central angle
            Arc testArc2 = new Arc(basePoint, noCalculationConstructed.CentralAngle, noCalculationConstructed.CenterPoint, directionAtStart);
            (testArc2 == noCalculationConstructed).Should().BeTrue();

            //last contructor using radius and containing plane
            //Arc testArc3 = new Arc(basePoint, endPoint, noCalculationConstructed.ArcRadius, false, containingPlane);
            //Arc testArc4 = new Arc(basePoint, endPoint, noCalculationConstructed.ArcRadius, true, containingPlane);
            //(testArc3 == noCalculationConstructed).Should().BeTrue();
        }

        [Test()]
        public void Arc_PropertiesTests_Quarter_Circle()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(3, 3, 4.24264); //sqr(3^2 + 3^2)
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(1, 1, 0));

            Arc quarterArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = quarterArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(DistanceType.Inch, 4.24264 * Math.PI / 2);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = quarterArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(AreaType.InchesSquared, (Math.PI / 2) / 2 * (18)); //Pi/2 = 90 degrees, 18 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = quarterArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentLength = new Area(AreaType.InchesSquared, 18 / 2 * (Math.PI / 2 - Math.Sin(Math.PI / 2))); //Pi/2 = 90 degrees, 18 = r^2
            (arcSegmentArea == expectedSegmentLength).Should().BeTrue();

            //test the central angle
            Angle centralAngle = quarterArc.CentralAngle;
            (centralAngle == new Angle(AngleType.Degree, 90)).Should().BeTrue();

            //test the center point
            Point centerPoint = quarterArc.CenterPoint;
            (centerPoint == PointGenerator.MakePointWithInches(0, 0, 4.24264)).Should().BeTrue();//sqr(3^2 + 3^2)

            //test the radius
            Distance radius = quarterArc.Radius;
            (radius == new Distance(DistanceType.Inch, 4.24264)).Should().BeTrue();//sqr(3^2 + 3^2)

            //test the straight line direction (same as direction)
            Direction straightDirection = quarterArc.StraightLineDirection;
            (straightDirection == new Direction(new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45))).Should().BeTrue();
        }

        [Test()]
        public void Arc_PropertiesTests_Half_Circle()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(0, 0, 6);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(1, 1, 0));

            Arc halfArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = halfArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(DistanceType.Inch, 3 * Math.PI);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = halfArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(AreaType.InchesSquared, (Math.PI) / 2 * 9); //Pi = 180 degrees, 9 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = halfArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentArea = new Area(AreaType.InchesSquared, 9 * 0.5 * (Math.PI - Math.Sin(Math.PI))); //Pi = 180 degrees, 9 = r^2
            (arcSegmentArea == expectedSegmentArea).Should().BeTrue();

            //test the central angle
            Angle centralAngle = halfArc.CentralAngle;
            (centralAngle == new Angle(AngleType.Degree, 180)).Should().BeTrue();

            //test the center point
            Point centerPoint = halfArc.CenterPoint;
            (centerPoint == PointGenerator.MakePointWithInches(0, 0, 3)).Should().BeTrue();

            //test the radius
            Distance radius = halfArc.Radius;
            (radius == new Distance(DistanceType.Inch, 3)).Should().BeTrue();

            //test the straight line direction (same as direction)
            Direction straightDirection = halfArc.StraightLineDirection;
            (straightDirection == new Direction(new Angle(), new Angle())).Should().BeTrue();
        }

        [Test()]
        public void Arc_PropertiesTests_Three_Quarters_Circle()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(-3, -3, 4.24264);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(1, 1, 0));

            Arc threeQuartersArc = new Arc(basePoint, endPoint, directionAtStart);

            //test arc length
            Distance arcLength = threeQuartersArc.ArcLength;
            //s = r(theta)
            Distance expectedArclength = new Distance(DistanceType.Inch, 4.24264 * Math.PI * 3 / 2);
            (arcLength == expectedArclength).Should().BeTrue();

            //test arc area
            Area arcArea = threeQuartersArc.SectorArea;
            //a = (theta)/2 * r^2
            Area expectedArea = new Area(AreaType.InchesSquared, (Math.PI * 3 / 2) / 2 * 18); //Pi = 180 degrees, 18 = r^2
            (arcArea == expectedArea).Should().BeTrue();

            //test the arcSegmentArea
            Area arcSegmentArea = threeQuartersArc.SegmentArea;
            //a = r^2 / 2 * (theta - sin(theta))
            Area expectedSegmentArea = new Area(AreaType.InchesSquared, 18 * 0.5 * (Math.PI * 3 / 2 - Math.Sin(Math.PI * 3 / 2))); //Pi = 180 degrees, 18 = r^2
            (arcSegmentArea == expectedSegmentArea).Should().BeTrue();

            //test the central angle
            Angle centralAngle = threeQuartersArc.CentralAngle;
            (centralAngle == new Angle(AngleType.Degree, 270)).Should().BeTrue();

            //test the center point
            Point centerPoint = threeQuartersArc.CenterPoint;
            (centerPoint == PointGenerator.MakePointWithInches(0, 0, 4.24264)).Should().BeTrue();

            //test the radius
            Distance radius = threeQuartersArc.Radius;
            (radius == new Distance(DistanceType.Inch, 4.24264)).Should().BeTrue();

            //test the straight line direction (same as direction)
            Direction straightDirection = threeQuartersArc.StraightLineDirection;
            (straightDirection == new Direction(new Angle(AngleType.Degree, 225), new Angle(AngleType.Degree, 45))).Should().BeTrue();
        }

        [Test()]
        public void Arc_TranslateTest()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(-3, 2, 1);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(-1, 1, 0.5));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Arc results = testArc.Translate( new Translation(new Translation(PointGenerator.MakePointWithInches(-1, 2, .5))));

            Arc expected = new Arc(PointGenerator.MakePointWithInches(-1, 2, 0.5), PointGenerator.MakePointWithInches(-3 - 1, 2 + 2, 1 + .5), directionAtStart);

            (results == expected).Should().BeTrue();
        }

        [Test()]
        public void Arc_Rotate()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(3, 3, 4.24264);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(1, 1, 0));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Arc results = testArc.Rotate(new Rotation(new Line(new Direction(PointGenerator.MakePointWithInches(1, -1, 0)), PointGenerator.MakePointWithInches(0, 0, 4.24264)), new Angle(AngleType.Degree, 90)));

            Arc expected = new Arc(endPoint, PointGenerator.MakePointWithInches(0, 0, 4.24264 * 2), new Direction(PointGenerator.MakePointWithInches(0, 0, 1)));

            (results == expected).Should().BeTrue();

            //try another rotation
            Arc results2 = testArc.Rotate(new Rotation(new Line(new Direction(PointGenerator.MakePointWithInches(0, 0, 1)), PointGenerator.MakePointWithInches(1.5, 1.5, 0)), new Angle(AngleType.Degree, 180)));

            Arc expected2 = new Arc(PointGenerator.MakePointWithInches(3, 3, 0), PointGenerator.MakePointWithInches(0, 0, 4.24264), new Direction(PointGenerator.MakePointWithInches(-1, -1, 0)));

            (results2 == expected2).Should().BeTrue();
        }

        [Test()]
        public void Arc_ShiftTest()
        {
            //make our default arc
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point endPoint = PointGenerator.MakePointWithInches(3, 3, 4.24264);
            Direction directionAtStart = new Direction(PointGenerator.MakePointWithInches(1, 1, 0));

            Arc testArc = new Arc(basePoint, endPoint, directionAtStart);

            Shift testShift = new Shift(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 45)), PointGenerator.MakePointWithInches(-3, 0.25, -2));
            Arc results = testArc.Shift(testShift);

            Arc expected = new Arc(PointGenerator.MakePointWithInches(-3, 0.25, -2), PointGenerator.MakePointWithInches(0 - 3, 4.24264 + 0.25, 4.24264 - 2), new Direction(PointGenerator.MakePointWithInches(0, 1, 0)));

            (results == expected).Should().BeTrue();
        }
    }
}
