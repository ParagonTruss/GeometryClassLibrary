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

            LineSegment topleftConnector = new LineSegment(basePoint, topLeftPoint);
            LineSegment toprightConnector = new LineSegment(bottomRightPoint, topRightPoint);
            LineSegment baseConnector = new LineSegment(topLeftPoint, topRightPoint);
            LineSegment bottomRightConnector = new LineSegment(basePoint, bottomRightPoint);

            Polygon frontRegion = new Polygon(new List<LineSegment> { left, top, bottom, right });
            Polygon backRegion = new Polygon(new List<LineSegment> { backLeft, backRight, backTop, backBottom });
            Polygon topRegion = new Polygon(new List<LineSegment> { top, backTop, topleftConnector, toprightConnector });
            Polygon bottomRegion = new Polygon(new List<LineSegment> { bottom, backBottom, baseConnector, bottomRightConnector });
            Polygon leftRegion = new Polygon(new List<LineSegment> { left, backLeft, baseConnector, topleftConnector });
            Polygon rightRegion = new Polygon(new List<LineSegment> { right, backRight, toprightConnector, bottomRightConnector });


            Polyhedron extrudedResult = frontRegion.Extrude(new Dimension(DimensionType.Inch, 4));
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

            Polygon actualPolygon = testPolygon.Rotate(rotationAxis, rotationAngle);

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
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon = new Polygon(lineSegments);

            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, -1, -1), new Vector(PointGenerator.MakePointWithInches(1, 1, 1)));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            Polygon actualPolygon = testPolygon.Rotate(rotationAxis, rotationAngle);
            
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

            Vector testDirectionVector = new Vector(PointGenerator.MakePointWithMillimeters(-1, 5, 4));
            Dimension testDisplacement = new Dimension(DimensionType.Millimeter, Math.Sqrt(42));

            Polygon actualPolygon = testPolygon.Translate(testDirectionVector, testDisplacement);

            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 4)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(-2, 10, 8)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-2, 10, 8), PointGenerator.MakePointWithInches(-3, 15, 12)));
            actualPolygon.Contains(new LineSegment(PointGenerator.MakePointWithInches(-3, 15, 12), PointGenerator.MakePointWithInches(0, 0, 0)));
        }

        [Test()]
        public void Polygon_Centorid()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-4, 2, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 5, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point center = testPolygon.Centroid();
            Point expected = PointGenerator.MakePointWithMillimeters(-2.5, 3, 0);

            center.X.Millimeters.Should().BeApproximately(expected.X.Millimeters, 0.00001);
            center.Y.Millimeters.Should().BeApproximately(expected.Y.Millimeters, 0.00001);
            center.Z.Millimeters.Should().BeApproximately(expected.Z.Millimeters, 0.00001);

            //make sure the centroid is in the region
            testPolygon.Contains(center).Should().BeTrue();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 2, 3), PointGenerator.MakePointWithMillimeters(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-3, -2, 0), PointGenerator.MakePointWithMillimeters(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, -1), PointGenerator.MakePointWithMillimeters(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            Point center2 = testPolygon2.Centroid();
            Point expected2 = PointGenerator.MakePointWithMillimeters(-0.6666667, 0.33333333, 0.66666667);

            center2.X.Millimeters.Should().BeApproximately(expected2.X.Millimeters, 0.00001);
            center2.Y.Millimeters.Should().BeApproximately(expected2.Y.Millimeters, 0.00001);
            center2.Z.Millimeters.Should().BeApproximately(expected2.Z.Millimeters, 0.00001);

            //make sure the centroid is in the region
            testPolygon2.Contains(center2).Should().BeTrue();
        }

        [Test()]
        public void Polygon_Copy()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-4, 2, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 5, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon planeCopy = new Polygon(testPolygon);

            //make sure it copied correctly 
            foreach (LineSegment line in testPolygon.PlaneBoundaries)
            {
                planeCopy.PlaneBoundaries.Contains(line).Should().BeTrue();
            }
            (planeCopy.BasePoint == testPolygon.BasePoint).Should().BeTrue();
            (planeCopy.NormalVector == testPolygon.NormalVector).Should().BeTrue();

            //now make sure the copy is independent by shifting it and then testing again
            planeCopy = planeCopy.Shift(new Shift(new Vector(PointGenerator.MakePointWithMillimeters(1, 4, -2)), new Rotation(Line.XAxis, new Angle(AngleType.Degree, 45))));

            foreach (LineSegment line in testPolygon.PlaneBoundaries)
            {
                planeCopy.PlaneBoundaries.Contains(line).Should().BeFalse();
            }
            (planeCopy.BasePoint == testPolygon.BasePoint).Should().BeFalse();
            (planeCopy.NormalVector == testPolygon.NormalVector).Should().BeFalse();
        }

        [Test()]
        public void Polygon_ContainsPoint()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-4, 2, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 5, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point test = PointGenerator.MakePointWithMillimeters(-2, 2, 0);
            Point test2 = PointGenerator.MakePointWithMillimeters(-2, 2, 1);

            testPolygon.Contains(test).Should().BeTrue();
            testPolygon.Contains(test2).Should().BeFalse();

            //make sure the sides are not included
            Point sideTest = PointGenerator.MakePointWithMillimeters(0, 0, 0);
            testPolygon.Contains(sideTest).Should().BeFalse();

            //make sure the Polygon contains the centroid
            testPolygon.Contains(testPolygon.Centroid()).Should().BeTrue();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 2, 3), PointGenerator.MakePointWithMillimeters(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-3, -2, 0), PointGenerator.MakePointWithMillimeters(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, -1), PointGenerator.MakePointWithMillimeters(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //make sure the Polygon contains the centroid
            Point center = testPolygon2.Centroid();
            testPolygon2.Contains(center).Should().BeTrue();

            Point test3 = center.Shift(new Shift(new Vector(PointGenerator.MakePointWithMillimeters(0.1, 0, 0))));
            testPolygon2.Contains(test3).Should().BeFalse();

        }


        [Test()]
        public void Polygon_OverlappingPolygonOneEnclosedInOtherTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 3), PointGenerator.MakePointWithMillimeters(4, 4, 3)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 4), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
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
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 2), PointGenerator.MakePointWithMillimeters(4, 1, 2)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 2), PointGenerator.MakePointWithMillimeters(4, 12, 2)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);

            intersect1.Equals(intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            intersect1.Equals(testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygonIntersectingBoundriesTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 5)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 3), PointGenerator.MakePointWithMillimeters(4, 4, 5)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 4), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3.5, 4), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 4), PointGenerator.MakePointWithMillimeters(4, 3.5, 4)));
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
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -1, 0), PointGenerator.MakePointWithMillimeters(1, -1, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -1, 0), PointGenerator.MakePointWithMillimeters(3, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, -1, 0), PointGenerator.MakePointWithMillimeters(4, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 5, 0), PointGenerator.MakePointWithMillimeters(4, 5, 0)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            List<LineSegment> expectedBound = new List<LineSegment>();
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 0), PointGenerator.MakePointWithMillimeters(2, 1, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 1, 0), PointGenerator.MakePointWithMillimeters(3, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 3, 0), PointGenerator.MakePointWithMillimeters(2, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 3, 0), PointGenerator.MakePointWithMillimeters(1, 1, 0)));
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
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            //check to see if its what we expected
            Area testArea = testPolygon.Area;
            testArea.ShouldBeEquivalentTo(new Area(AreaType.MillimetersSquared, 8));
        }

        [Test()]
        public void Polygon_SliceOnLineTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 4), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            Polygon testPolygon = new Polygon(bounds);

            Line slicingLine = new Line(new Point(), PointGenerator.MakePointWithMillimeters(1, 1, 1));

            List<Polygon> results = testPolygon.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(1, 1, 1)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(4, 1, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 3, 3), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 4), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(new Point(), PointGenerator.MakePointWithMillimeters(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }
    }
}
