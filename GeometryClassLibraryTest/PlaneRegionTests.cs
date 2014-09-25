using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PlaneRegionTests
    {
      

        [Test()]
        public void PlaneRegion_RotateAndRoundTest_Orthogonal()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            PlaneRegion testPlaneRegion = new PlaneRegion(lineSegments);

            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0)); //This is the X axis
            Angle rotationAngle = new Angle(AngleType.Degree, 90);

            PlaneRegion actualPlaneRegion = testPlaneRegion.Rotate(rotationAxis, rotationAngle);

            List<LineSegment> expectedLineSegments = new List<LineSegment>();
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4)));
            expectedLineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0)));
            PlaneRegion expectedPlaneRegion = new PlaneRegion(expectedLineSegments);

            (actualPlaneRegion == expectedPlaneRegion).Should().BeTrue();
        }

        [Test()]
        public void PlaneRegion_TranslateTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, -5, -4), PointGenerator.MakePointWithInches(0, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(-2, 10, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-2, 10, 8), PointGenerator.MakePointWithInches(1, -5, -4)));
            PlaneRegion testPlaneRegion = new PlaneRegion(lineSegments);

            Vector testDirectionVector = new Vector(PointGenerator.MakePointWithMillimeters(-1, 5, 4));
            Dimension testDisplacement = new Dimension(DimensionType.Millimeter, Math.Sqrt(42));

            PlaneRegion actualPlaneRegion = testPlaneRegion.Translate(testDirectionVector, testDisplacement);

            actualPlaneRegion.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(-1, 5, 4)));
            actualPlaneRegion.Contains(new LineSegment(PointGenerator.MakePointWithInches(-1, 5, 4), PointGenerator.MakePointWithInches(-2, 10, 8)));
            actualPlaneRegion.Contains(new LineSegment(PointGenerator.MakePointWithInches(-2, 10, 8), PointGenerator.MakePointWithInches(-3, 15, 12)));
            actualPlaneRegion.Contains(new LineSegment(PointGenerator.MakePointWithInches(-3, 15, 12), PointGenerator.MakePointWithInches(0, 0, 0)));
        }


        [Test()]
        public void PlaneRegion_Copy()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-4, 2, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 5, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            PlaneRegion planeCopy = new PlaneRegion(testPlaneRegion);

            //make sure it copied correctly 
            foreach (LineSegment line in testPlaneRegion.PlaneBoundaries)
            {
                planeCopy.PlaneBoundaries.Contains(line).Should().BeTrue();
            }
            (planeCopy.BasePoint == testPlaneRegion.BasePoint).Should().BeTrue();
            (planeCopy.NormalVector == testPlaneRegion.NormalVector).Should().BeTrue();

            //now make sure the copy is independent by shifting it and then testing again
            planeCopy = planeCopy.Shift(new Shift(new Vector(PointGenerator.MakePointWithMillimeters(1, 4, -2)), new Rotation(Line.XAxis, new Angle(AngleType.Degree, 45))));

            foreach (LineSegment line in testPlaneRegion.PlaneBoundaries)
            {
                planeCopy.PlaneBoundaries.Contains(line).Should().BeFalse();
            }
            (planeCopy.BasePoint == testPlaneRegion.BasePoint).Should().BeFalse();
            (planeCopy.NormalVector == testPlaneRegion.NormalVector).Should().BeFalse();
        }

        [Test()]
        public void PlaneRegion_ContainsPoint()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-1, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(-4, 2, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-4, 2, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-1, 5, 0), PointGenerator.MakePointWithMillimeters(-5, 5, 0)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            Point test = PointGenerator.MakePointWithMillimeters(-2, 2, 0);
            Point test2 = PointGenerator.MakePointWithMillimeters(-2, 2, 1);

            testPlaneRegion.Contains(test).Should().BeTrue();
            testPlaneRegion.Contains(test2).Should().BeFalse();

            //make sure the sides are not included
            Point sideTest = PointGenerator.MakePointWithMillimeters(0, 0, 0);
            testPlaneRegion.Contains(sideTest).Should().BeFalse();

            //make sure the PlaneRegion contains the centroid
            testPlaneRegion.Contains(testPlaneRegion.Centroid()).Should().BeTrue();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 2, 3), PointGenerator.MakePointWithMillimeters(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(-3, -2, 0), PointGenerator.MakePointWithMillimeters(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, -1), PointGenerator.MakePointWithMillimeters(0, 2, 3)));
            PlaneRegion testPlaneRegion2 = new PlaneRegion(lineSegments);

            //make sure the PlaneRegion contains the centroid
            Point center = testPlaneRegion2.Centroid();
            testPlaneRegion2.Contains(center).Should().BeTrue();

            Point test3 = center.Shift(new Shift(new Vector(PointGenerator.MakePointWithMillimeters(0.1, 0, 0))));
            testPlaneRegion2.Contains(test3).Should().BeFalse();

        }


        [Test()]
        public void PlaneRegion_OverlappingPlaneRegionOneEnclosedInOtherTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 3), PointGenerator.MakePointWithMillimeters(4, 4, 3)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 4), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            PlaneRegion testPlaneRegion2 = new PlaneRegion(lineSegments);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            PlaneRegion intersect1 = testPlaneRegion.OverlappingPlaneRegion(testPlaneRegion2);
            PlaneRegion intersect2 = testPlaneRegion2.OverlappingPlaneRegion(testPlaneRegion);
            intersect1.Equals(intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            intersect1.Equals(testPlaneRegion).Should().BeTrue();
        }

        [Test()]
        public void PlaneRegion_OverlappingPlaneRegionSharedSidesTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 2)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 2), PointGenerator.MakePointWithMillimeters(4, 1, 2)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 2)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 2), PointGenerator.MakePointWithMillimeters(4, 12, 2)));
            PlaneRegion testPlaneRegion2 = new PlaneRegion(lineSegments);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            PlaneRegion intersect1 = testPlaneRegion.OverlappingPlaneRegion(testPlaneRegion2);
            PlaneRegion intersect2 = testPlaneRegion2.OverlappingPlaneRegion(testPlaneRegion);

            intersect1.Equals(intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            intersect1.Equals(testPlaneRegion).Should().BeTrue();
        }

        [Test()]
        public void PlaneRegion_OverlappingPlaneRegionIntersectingBoundriesTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 5)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 3), PointGenerator.MakePointWithMillimeters(4, 4, 5)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 12, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 0), PointGenerator.MakePointWithMillimeters(4, 0, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 12, 0), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 0, 4), PointGenerator.MakePointWithMillimeters(4, 12, 4)));
            PlaneRegion testPlaneRegion2 = new PlaneRegion(lineSegments);

            List<LineSegment> expectedBounds = new List<LineSegment>();
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 4, 1)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 1), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 1), PointGenerator.MakePointWithMillimeters(4, 4, 4)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3.5, 4), PointGenerator.MakePointWithMillimeters(4, 3, 3)));
            expectedBounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 4, 4), PointGenerator.MakePointWithMillimeters(4, 3.5, 4)));
            PlaneRegion expected = new PlaneRegion(expectedBounds);

            //check to see if it is what we were expecting
            PlaneRegion intersect = testPlaneRegion2.OverlappingPlaneRegion(testPlaneRegion);
            intersect.Equals(expected).Should().BeTrue();

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            PlaneRegion intersect1 = testPlaneRegion.OverlappingPlaneRegion(testPlaneRegion2);
            PlaneRegion intersect2 = testPlaneRegion2.OverlappingPlaneRegion(testPlaneRegion);
            intersect1.Equals(intersect2).Should().BeTrue();
        }

        [Test()]
        public void PlaneRegion_OverlappingPlaneRegion()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 0), PointGenerator.MakePointWithMillimeters(4, 3, 0)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -1, 0), PointGenerator.MakePointWithMillimeters(1, -1, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -1, 0), PointGenerator.MakePointWithMillimeters(3, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, -1, 0), PointGenerator.MakePointWithMillimeters(4, 5, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 5, 0), PointGenerator.MakePointWithMillimeters(4, 5, 0)));
            PlaneRegion testPlaneRegion2 = new PlaneRegion(lineSegments);

            List<LineSegment> expectedBound = new List<LineSegment>();
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 0), PointGenerator.MakePointWithMillimeters(2, 1, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 1, 0), PointGenerator.MakePointWithMillimeters(3, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 3, 0), PointGenerator.MakePointWithMillimeters(2, 3, 0)));
            expectedBound.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 3, 0), PointGenerator.MakePointWithMillimeters(1, 1, 0)));
            PlaneRegion expected = new PlaneRegion(expectedBound);

            //check to see if its what we expected
            PlaneRegion intersect = testPlaneRegion.OverlappingPlaneRegion(testPlaneRegion2);
            intersect.Equals(expected).Should().BeTrue();

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            PlaneRegion intersect1 = testPlaneRegion.OverlappingPlaneRegion(testPlaneRegion2);
            PlaneRegion intersect2 = testPlaneRegion2.OverlappingPlaneRegion(testPlaneRegion);

            intersect1.Equals(intersect2).Should().BeTrue();
        }


        [Test()]
        public void PlaneRegion_SliceOnLineTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(4, 1, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 4), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            PlaneRegion testPlaneRegion = new PlaneRegion(bounds);

            Line slicingLine = new Line(new Point(), PointGenerator.MakePointWithMillimeters(1, 1, 1));

            List<PlaneRegion> results = testPlaneRegion.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(0, 3, 0)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 1, 0), PointGenerator.MakePointWithMillimeters(1, 1, 1)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 0), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            expected1Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            PlaneRegion expected1 = new PlaneRegion(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(3, 3, 3)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 1, 1), PointGenerator.MakePointWithMillimeters(4, 1, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(3, 3, 3), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            expected2Bounds.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 1, 4), PointGenerator.MakePointWithMillimeters(4, 3, 4)));
            PlaneRegion expected2 = new PlaneRegion(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(new Point(), PointGenerator.MakePointWithMillimeters(1, 1, 0.9));
            List<PlaneRegion> results2 = testPlaneRegion.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPlaneRegion).Should().BeTrue();
        }
    }
}
