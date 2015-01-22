using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PolygonTests
    {
        [Test()]
        public void Polygon_ExtrudePolygon()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 4, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(8, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(8, 4, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, -4);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 4, -4);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(8, 0, -4);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(8, 4, -4);

            LineSegment left = new LineSegment(basePoint, topLeftPoint);
            LineSegment right = new LineSegment(bottomRightPoint, topRightPoint);
            LineSegment top = new LineSegment(topLeftPoint, topRightPoint);
            LineSegment bottom = new LineSegment(basePoint, bottomRightPoint);

            LineSegment backLeft = new LineSegment(backbasepoint, backtopleftpoint);
            LineSegment backRight = new LineSegment(backbottomrightpoint, backtoprightpoint);
            LineSegment backTop = new LineSegment(backtopleftpoint, backtoprightpoint);
            LineSegment backBottom = new LineSegment(backbasepoint, backbottomrightpoint);

            LineSegment topleftConnector = new LineSegment(topLeftPoint, backtopleftpoint);
            LineSegment toprightConnector = new LineSegment(topRightPoint, backtoprightpoint);
            LineSegment baseConnector = new LineSegment(basePoint, backbasepoint);
            LineSegment bottomRightConnector = new LineSegment(bottomRightPoint, backbottomrightpoint);

            Polygon frontRegion = new Polygon(new List<LineSegment> { left, top, bottom, right });
            Polygon backRegion = new Polygon(new List<LineSegment> { backLeft, backRight, backTop, backBottom });
            Polygon topRegion = new Polygon(new List<LineSegment> { top, backTop, topleftConnector, toprightConnector });
            Polygon bottomRegion = new Polygon(new List<LineSegment> { bottom, backBottom, baseConnector, bottomRightConnector });
            Polygon leftRegion = new Polygon(new List<LineSegment> { left, backLeft, baseConnector, topleftConnector });
            Polygon rightRegion = new Polygon(new List<LineSegment> { right, backRight, toprightConnector, bottomRightConnector });


            Polyhedron extrudedResult = frontRegion.ExtrudeAsPolyhedron(new Vector(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, -4)));
            extrudedResult.Polygons.Contains(frontRegion).Should().BeTrue();
            extrudedResult.Polygons.Contains(backRegion).Should().BeTrue();
            extrudedResult.Polygons.Contains(topRegion).Should().BeTrue();
            extrudedResult.Polygons.Contains(bottomRegion).Should().BeTrue();
            extrudedResult.Polygons.Contains(leftRegion).Should().BeTrue();
            extrudedResult.Polygons.Contains(rightRegion).Should().BeTrue();
        }

        [Test()]
        public void Polygon_RotateAndRoundTest_Orthogonal()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(lineSegments);

            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0)); //This is the X axis
            Angle rotationAngle = new Angle(AngleType.Degree, 90);

            Polygon actualPolygon = testPolygon.Rotate(new Rotation(rotationAxis, rotationAngle));

            List<LineSegment> expectedLineSegments = new List<LineSegment>();
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon expectedPolygon = new Polygon(expectedLineSegments);

            (actualPolygon == expectedPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_RotateTest()
        {
            //think messes up due to percision error
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon = new Polygon(lineSegments);

            Line rotationAxis = new Line(new Direction(PointGenerator.MakePointWithInches(1, 1, 1), PointGenerator.MakePointWithInches(1, -1, -1)));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            Polygon actualPolygon = testPolygon.Rotate(new Rotation(rotationAxis, rotationAngle));

            List<LineSegment> expectedLineSegments = new List<LineSegment>();
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(5.238195, 1.6816970, -1.919892), PointGenerator.MakePointWithInches(1.31623019, -1.08627088, -5.229959)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(1.3162301, -1.0862708, -5.229959), PointGenerator.MakePointWithInches(2.843930, -1.46406412, -0.379865)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(2.8439301, -1.4640641, -0.379865), PointGenerator.MakePointWithInches(5.238195, 1.681697053, -1.9198923)));
        }

        [Test()]
        public void Polygon_TranslateTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -5, -4), PointGenerator.MakePointWithInches(0, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(-2, 10, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-2, 10, 8), PointGenerator.MakePointWithInches(1, -5, -4)));
            Polygon testPolygon = new Polygon(lineSegments);

            //Direction testDirection = new Direction(PointGenerator.MakePointWithInches(-1, 5, 4));
            //Distance testDisplacement = new Distance(DistanceType.Inch, Math.Sqrt(42));
            Point testDiplacement = PointGenerator.MakePointWithInches(-1, 5, 4);

            Polygon actualPolygon = testPolygon.Translate(testDiplacement);

            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 4)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(-2, 10, 8)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-2, 10, 8), PointGenerator.MakePointWithInches(-3, 15, 12)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-3, 15, 12), PointGenerator.MakePointWithInches(0, 0, 0)));
        }

        [Test()]
        public void Polygon_Centorid()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-4, 2, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point center = testPolygon.Centroid();
            Point expected = PointGenerator.MakePointWithInches(-2.5, 3, 0);

            center.Should().Be(expected);

            //make sure the centroid is in the region
            testPolygon.Contains(center).Should().BeTrue();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            Point center2 = testPolygon2.Centroid();
            Point expected2 = PointGenerator.MakePointWithInches(-0.6666667, 0.33333333, 0.66666667);

            center2.Should().Be(expected2);

            //make sure the centroid is in the region
            testPolygon2.Contains(center2).Should().BeTrue();
        }

        [Test()]
        public void Polygon_Copy()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-4, 2, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon planeCopy = new Polygon(testPolygon);

            //make sure it copied correctly 
            foreach (LineSegment line in testPolygon.LineSegments)
            {
                planeCopy.LineSegments.Contains(line).Should().BeTrue();
            }
            (planeCopy.BasePoint == testPolygon.BasePoint).Should().BeTrue();
            (planeCopy.NormalVector == testPolygon.NormalVector).Should().BeTrue();

            //now make sure the copy is independent by shifting it and then testing again
            planeCopy = planeCopy.Shift(new Shift(new Rotation(Line.XAxis, new Angle(AngleType.Degree, 45)), PointGenerator.MakePointWithInches(1, 4, -2)));

            foreach (LineSegment line in testPolygon.LineSegments)
            {
                planeCopy.LineSegments.Contains(line).Should().BeFalse();
            }
            (planeCopy.BasePoint == testPolygon.BasePoint).Should().BeFalse();
            (planeCopy.NormalVector == testPolygon.NormalVector).Should().BeFalse();
        }

        [Test()]
        public void Polygon_ContainsExclusiveInclusiveAndTouchingPoint()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-4, 2, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 0), PointGenerator.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point insidePlane1 = PointGenerator.MakePointWithInches(-2, 2, 0);
            Point insidePlane2 = PointGenerator.MakePointWithInches(-2, 2, 1);

            Point center1 = testPolygon.Centroid();

            //make sure the sides are not included
            Point sideTest = PointGenerator.MakePointWithInches(0, 0, 0);


            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //make sure the PlaneRegion contains the centroid
            Point center2 = testPolygon2.Centroid();

            Point notOnPlane = center2.Shift(new Shift(PointGenerator.MakePointWithInches(.5, 0, 0)));

            //Points on the plane not boundaries (true for exclusive and inclusive, false for touching)
            testPolygon.ContainsExclusive(insidePlane1).Should().BeTrue();
            testPolygon.ContainsInclusive(insidePlane1).Should().BeTrue();
            testPolygon.Touches(insidePlane1).Should().BeFalse();

            testPolygon.ContainsExclusive(insidePlane2).Should().BeFalse();
            testPolygon.ContainsInclusive(insidePlane2).Should().BeFalse();
            testPolygon.Touches(insidePlane2).Should().BeFalse();

            //make sure the PlaneRegion contains the centroid (true for exclusive and inclusive, false for touching)
            testPolygon.ContainsExclusive(center1).Should().BeTrue();
            testPolygon.ContainsInclusive(center1).Should().BeTrue();
            testPolygon.Touches(center1).Should().BeFalse();

            testPolygon2.ContainsExclusive(center2).Should().BeTrue();
            testPolygon2.ContainsInclusive(center2).Should().BeTrue();
            testPolygon2.Touches(center2).Should().BeFalse();

            //check the side point (true for inclusive and touches, false for exclusive)
            testPolygon.ContainsExclusive(sideTest).Should().BeFalse();
            testPolygon.ContainsInclusive(sideTest).Should().BeTrue();
            testPolygon.Touches(sideTest).Should().BeTrue();

            //not on plane (false for all)
            testPolygon2.ContainsExclusive(notOnPlane).Should().BeFalse();
            testPolygon2.ContainsInclusive(notOnPlane).Should().BeFalse();
            testPolygon2.Touches(notOnPlane).Should().BeFalse();
        }


        [Test()]
        public void Polygon_OverlappingPolygonOneEnclosedInOtherTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 4, 1), PointGenerator.MakePointWithInches(4, 4, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 3), PointGenerator.MakePointWithInches(4, 4, 3)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 12, 0), PointGenerator.MakePointWithInches(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 4), PointGenerator.MakePointWithInches(4, 12, 4)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);
            intersect1.Equals(intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            intersect1.Equals(testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygonSharedSidesTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 1, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 2), PointGenerator.MakePointWithInches(4, 1, 2)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 12, 0), PointGenerator.MakePointWithInches(4, 12, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 2), PointGenerator.MakePointWithInches(4, 12, 2)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);

            (intersect1 == intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            (intersect1 == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygonIntersectingBoundriesTest()
        {
            //changed to inches because mm were gave results smaller than what we are considering equivalent
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 4, 1), PointGenerator.MakePointWithInches(4, 4, 5)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 3), PointGenerator.MakePointWithInches(4, 4, 5)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 12, 0), PointGenerator.MakePointWithInches(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 4), PointGenerator.MakePointWithInches(4, 12, 4)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 4, 1)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 1), PointGenerator.MakePointWithInches(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 4, 1), PointGenerator.MakePointWithInches(4, 4, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 3.5, 4), PointGenerator.MakePointWithInches(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 4, 4), PointGenerator.MakePointWithInches(4, 3.5, 4)));
            Polygon expected = new Polygon(expectedBounds);

            //check to see if it is what we were expecting
            Polygon intersect = testPolygon2.OverlappingPolygon(testPolygon);
            intersect.Equals(expected).Should().BeTrue();

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);
            intersect1.Equals(intersect2).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygon()
        {
            //changed to inches because mm were gave results smaller than what we are considering equivalent
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, -1, 0), PointGenerator.MakePointWithInches(1, -1, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, -1, 0), PointGenerator.MakePointWithInches(3, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -1, 0), PointGenerator.MakePointWithInches(4, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 5, 0), PointGenerator.MakePointWithInches(4, 5, 0)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            List<LineSegment> expectedBound = new List<LineSegment>();
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(3, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 3, 0), PointGenerator.MakePointWithInches(2, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 3, 0), PointGenerator.MakePointWithInches(1, 1, 0)));
            Polygon expected = new Polygon(expectedBound);

            //check to see if its what we expected
            Polygon intersect = testPolygon.OverlappingPolygon(testPolygon2);
            intersect.Equals(expected).Should().BeTrue();

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);

            intersect1.Equals(intersect2).Should().BeTrue();
        }

        [Test()]
        public void Polygon_AreaTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 0), PointGenerator.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            //check to see if its what we expected
            Area testArea = testPolygon.Area;
            testArea.Should().Be(new Area(AreaType.InchesSquared, 8));
        }

        [Test()]
        public void Polygon_SliceOnLineTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(4, 1, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(4, 3, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 4), PointGenerator.MakePointWithInches(4, 3, 4)));
            Polygon testPolygon = new Polygon(bounds);

            Line slicingLine = new Line(new Point(), PointGenerator.MakePointWithInches(1, 1, 1));

            List<Polygon> results = testPolygon.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(0, 3, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 1, 0), PointGenerator.MakePointWithInches(1, 1, 1)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 0), PointGenerator.MakePointWithInches(3, 3, 3)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, 1), PointGenerator.MakePointWithInches(3, 3, 3)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, 1), PointGenerator.MakePointWithInches(3, 3, 3)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, 1), PointGenerator.MakePointWithInches(4, 1, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 3, 3), PointGenerator.MakePointWithInches(4, 3, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 1, 4), PointGenerator.MakePointWithInches(4, 3, 4)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(new Point(), PointGenerator.MakePointWithInches(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SliceWithLineTest()
        {
            //changed to inches because mm were gave results smaller than what we are considering equivalent
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(6, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 1, 0), PointGenerator.MakePointWithInches(6, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Line slicingLine = new Line(PointGenerator.MakePointWithInches(6, 5, 0), PointGenerator.MakePointWithInches(2, 1, 0));

            List<Polygon> results = testPolygon.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(6, 1, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 1, 0), PointGenerator.MakePointWithInches(6, 0, 0)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(new Point(), PointGenerator.MakePointWithInches(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SliceACaseThatDidntWorkBefore()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 3.5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3.5, 0), PointGenerator.MakePointWithInches(240, 3.5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 3.5, 0), PointGenerator.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Plane slicingPlane = new Plane(new Direction(new Angle(AngleType.Radian, 5.6548667765)), PointGenerator.MakePointWithInches(122.8315595, 169.313137732, 0));

            List<Polygon> results = testPolygon.Slice(slicingPlane);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0.25, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0.25, 0), PointGenerator.MakePointWithInches(2.36126321602, 3.5, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2.36126321602, 3.5, 0), PointGenerator.MakePointWithInches(240, 3.5, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 3.5, 0), PointGenerator.MakePointWithInches(240, 0, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0.25, 0), PointGenerator.MakePointWithInches(0, 3.5, 0)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 3.5, 0), PointGenerator.MakePointWithInches(2.36126321602, 3.5, 0)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(2.36126321602, 3.5, 0), PointGenerator.MakePointWithInches(0, 0.25, 0)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected1).Should().BeTrue();
            results.Contains(expected2).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(new Point(), PointGenerator.MakePointWithInches(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SliceAnotherCaseThatDidntWorkBefore()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 1.5)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 1.5), PointGenerator.MakePointWithInches(240, 0, 1.5)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 1.5), PointGenerator.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Plane slicingPlane = new Plane(new Direction(new Angle(AngleType.Radian, 5.6548667765)), PointGenerator.MakePointWithInches(122.8315595, 169.313137732, 0));

            List<Polygon> results = testPolygon.Slice(slicingPlane);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(240, 0, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 0), PointGenerator.MakePointWithInches(240, 0, 1.5)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(240, 0, 1.5), PointGenerator.MakePointWithInches(0, 0, 1.5)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 1.5), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon expected1 = new Polygon(expected1Bounds);

            //should only return the original plane
            results.Count.Should().Be(1);
            (results[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SharedPointNotOnThisPolygonsBoundary()
        {
            List<LineSegment> bounds1 = new List<LineSegment>();
            bounds1.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(3, 0, 0)));
            bounds1.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 2, 0)));
            bounds1.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 0, 0), PointGenerator.MakePointWithInches(3, 2, 0)));
            bounds1.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 0), PointGenerator.MakePointWithInches(3, 2, 0)));
            Polygon testPolygon1 = new Polygon(bounds1);

            List<LineSegment> bounds2 = new List<LineSegment>();
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(3.5, 0, 0)));
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(3.5, 0, 0), PointGenerator.MakePointWithInches(3.5, 1, 0)));
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(3.5, 1, 0)));
            Polygon testPolygon2 = new Polygon(bounds2);

            //check for generic overlapping
            Point result12 = testPolygon1.SharedPointNotOnThisPolygonsBoundary(testPolygon2);
            (result12 != null).Should().BeTrue();
            testPolygon1.ContainsExclusive(result12).Should().BeTrue();


            List<LineSegment> bounds3 = new List<LineSegment>();
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(5, 0, 0)));
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(2, 1, 0)));
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(5, 0, 0), PointGenerator.MakePointWithInches(5, 1, 0)));
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 0), PointGenerator.MakePointWithInches(5, 1, 0)));
            Polygon testPolygon3 = new Polygon(bounds3);

            //check when one contains the other
            Point result13 = testPolygon1.SharedPointNotOnThisPolygonsBoundary(testPolygon3);
            (result13 != null).Should().BeTrue();
            testPolygon1.ContainsExclusive(result13).Should().BeTrue();


            List<LineSegment> bounds4 = new List<LineSegment>();
            bounds4.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 0, 0), PointGenerator.MakePointWithInches(5, 0, 0)));
            bounds4.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 0, 0), PointGenerator.MakePointWithInches(3, 1, 0)));
            bounds4.Add(new LineSegment(PointGenerator.MakePointWithInches(5, 0, 0), PointGenerator.MakePointWithInches(5, 1, 0)));
            bounds4.Add(new LineSegment(PointGenerator.MakePointWithInches(3, 1, 0), PointGenerator.MakePointWithInches(5, 1, 0)));
            Polygon testPolygon4 = new Polygon(bounds4);

            //check when they touch sides but dont overlap
            Point result14 = testPolygon1.SharedPointNotOnThisPolygonsBoundary(testPolygon4);
            (result14 != null).Should().BeFalse();


            List<LineSegment> bounds5 = new List<LineSegment>();
            bounds5.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 0, 0), PointGenerator.MakePointWithInches(9, 0, 0)));
            bounds5.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 0, 0), PointGenerator.MakePointWithInches(-1, .5, 0)));
            bounds5.Add(new LineSegment(PointGenerator.MakePointWithInches(9, 0, 0), PointGenerator.MakePointWithInches(9, .5, 0)));
            bounds5.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, .5, 0), PointGenerator.MakePointWithInches(9, .5, 0)));
            Polygon testPolygon5 = new Polygon(bounds5);

            //check for overlapping when both centroids arent contained
            Point result15 = testPolygon1.SharedPointNotOnThisPolygonsBoundary(testPolygon5);
            (result15 != null).Should().BeTrue();
            testPolygon1.ContainsExclusive(result15).Should().BeTrue();


            //one more becasue it was causing errors before in program implementing this
            List<LineSegment> bounds6 = new List<LineSegment>();
            bounds6.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 6, 2), PointGenerator.MakePointWithInches(6, 5, 2)));
            bounds6.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 6, 2), PointGenerator.MakePointWithInches(12, 0, 2)));
            bounds6.Add(new LineSegment(PointGenerator.MakePointWithInches(6, 5, 2), PointGenerator.MakePointWithInches(10, 1, 2)));
            bounds6.Add(new LineSegment(PointGenerator.MakePointWithInches(10, 1, 2), PointGenerator.MakePointWithInches(12, 0, 2)));
            Polygon testPolygon6 = new Polygon(bounds6);

            List<LineSegment> bounds7 = new List<LineSegment>();
            bounds7.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 2), PointGenerator.MakePointWithInches(2, 1, 2)));
            bounds7.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 2), PointGenerator.MakePointWithInches(12, 0, 2)));
            bounds7.Add(new LineSegment(PointGenerator.MakePointWithInches(2, 1, 2), PointGenerator.MakePointWithInches(10, 1, 2)));
            bounds7.Add(new LineSegment(PointGenerator.MakePointWithInches(10, 1, 2), PointGenerator.MakePointWithInches(12, 0, 2)));
            Polygon testPolygon7 = new Polygon(bounds7);

            Point result67 = testPolygon6.SharedPointNotOnThisPolygonsBoundary(testPolygon7);
            (result67 != null).Should().BeFalse();
        }

        [Test()]
        public void Polygon_DoesContainPointAlongSides()
        {
            //think messes up due to percision error
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon = new Polygon(lineSegments);

            Point pointOn = PointGenerator.MakePointWithInches(-1.5, 0, 1.5);
            Point anotherPointOn = PointGenerator.MakePointWithInches(.5, 1.5, 1);
            Point pointNotOn = PointGenerator.MakePointWithInches(-2, 0, 1.5);

            bool resultOn = testPolygon.DoesContainPointAlongSides(pointOn);
            bool resultAnotherOn = testPolygon.DoesContainPointAlongSides(anotherPointOn);
            bool resultNotOn = testPolygon.DoesContainPointAlongSides(pointNotOn);

            resultOn.Should().BeTrue();
            resultAnotherOn.Should().BeTrue();
            resultNotOn.Should().BeFalse();
        }

        [Test()]
        public void Polygon_FindVertexNotOnTheGivenPlane()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, 2, 0), PointGenerator.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, 2, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(lineSegments);

            Plane borderPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 2, 0), PointGenerator.MakePointWithInches(0, 0, 1));
            Plane containingPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 2, 0), PointGenerator.MakePointWithInches(1, 0, 0));

            Point results1 = testPolygon.FindVertexNotOnTheGivenPlane(borderPlane);
            Point results2 = testPolygon.FindVertexNotOnTheGivenPlane(containingPlane);

            (results1 == PointGenerator.MakePointWithInches(-3, 2, 0)).Should().BeTrue();
            (results2 == null).Should().BeTrue();
        }
    }
}
