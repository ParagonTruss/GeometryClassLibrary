using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PlaneTests
    {
        [Test()]
        public void Plane_ContainsLine()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 3, 0), PointGenerator.MakePointWithInches(4, -2, 0));
            Line testLine = new Line(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(0, -1, 0));
            Line testFalseLine = new Line(PointGenerator.MakePointWithInches(1, 2, -2), PointGenerator.MakePointWithInches(2, 4, 3));

            bool result = testPlane.Contains(testLine);
            bool result2 = testPlane.Contains(testFalseLine);

            result.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_ContainsLineOnOrigin()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 3, 0), PointGenerator.MakePointWithInches(-3, -1, 0));
            Line testLine = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 1, 0));

            bool result = testPlane.Contains(testLine);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPolygon()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, -3, 4), PointGenerator.MakePointWithInches(1, 2, 2));
            Polygon testRegion = new Polygon(new List<Point>
                {
                    PointGenerator.MakePointWithInches(1,1,1),
                    PointGenerator.MakePointWithInches(1,2,1),
                    PointGenerator.MakePointWithInches(1,2,2),
                    PointGenerator.MakePointWithInches(1,1,2)
                });

            bool result = testPlane.Contains(testRegion);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPoint()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0,1,0), PointGenerator.MakePointWithInches(2,1,-3), PointGenerator.MakePointWithInches(-4,1,-1));
            Point testPoint1 = PointGenerator.MakePointWithInches(0, 1, 5);
            Point testPoint2 = PointGenerator.MakePointWithInches(2, 2, 2);

            bool result1 = testPlane.Contains(testPoint1);
            bool result2 = testPlane.Contains(testPoint2);

            result1.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_RotateTest()
        {
            Point testBasePoint = PointGenerator.MakePointWithInches(1, 1, -1);            
            Vector testNormalVector = new Vector(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));

            Plane testPlane = new Plane(testNormalVector.Direction, testBasePoint);

            Line rotationAxis = new Line(new Direction(PointGenerator.MakePointWithInches(1, 1, 1)), PointGenerator.MakePointWithInches(1, -1, -1));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            Plane actualResult = testPlane.Rotate(rotationAxis, rotationAngle);

            Point expectedPoint = PointGenerator.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495);
            Vector expectedVector = new Vector(PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), PointGenerator.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            Plane expectedResult = new Plane(expectedVector.Direction, expectedPoint);

            bool test = expectedPoint == actualResult.BasePoint;
            bool test2 = expectedVector == actualResult.NormalVector;

            (actualResult == expectedResult).Should().BeTrue();
        }

        [Test()]
        public void Plane_PointOnSameSideAs()
        {
            Point testPoint = PointGenerator.MakePointWithInches(1, 3, -1);
            Point testPoint2 = PointGenerator.MakePointWithInches(-1, -2, 5);
            Point testPoint3 = PointGenerator.MakePointWithInches(0, 1, 0);
            Point referencePoint = PointGenerator.MakePointWithInches(1, 2, 1);
            Point referencePoint2 = PointGenerator.MakePointWithInches(0, 2, 1);
            Vector testNormalVector = new Vector(PointGenerator.MakePointWithInches(1, 0, 0));

            Plane testPlane = new Plane(testNormalVector.Direction, PointGenerator.MakePointWithInches(0,0,0));

            testPlane.PointIsOnSameSideAs(testPoint, referencePoint).Should().BeTrue(); //test one on the same side
            testPlane.PointIsOnSameSideAs(testPoint2, referencePoint).Should().BeFalse(); //test one on the opposite side
            testPlane.PointIsOnSameSideAs(testPoint3, referencePoint).Should().BeFalse(); //test one on the plane
            testPlane.PointIsOnSameSideAs(testPoint, referencePoint2).Should().BeFalse(); //test a reference point on the plane


            Point testPointOffOrigin = PointGenerator.MakePointWithInches(5, 4, 0);
            Point testPointOffOrigin2 = PointGenerator.MakePointWithInches(5, -2, 0);
            Point referencePointOffOrigin = PointGenerator.MakePointWithInches(1, 2, 3);
            Point planeBase = PointGenerator.MakePointWithInches(1, -4, 2);
            Vector testNormalVectorOffOrigin = new Vector(PointGenerator.MakePointWithInches(-1, 2, 1));

            Plane testPlaneOffOrigin = new Plane(testNormalVectorOffOrigin.Direction, planeBase);

            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin, referencePointOffOrigin).Should().BeTrue();
            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin2, referencePointOffOrigin).Should().BeFalse();
            testPlaneOffOrigin.PointIsOnSameSideAs(planeBase, referencePointOffOrigin).Should().BeFalse();

        }

        [Test()]
        public void Plane_IntersectionLineWithPlane()
        {
            //try all the zero cases
            Plane testPlane1 = new Plane(new Direction(PointGenerator.MakePointWithInches(2, -1, 1)), PointGenerator.MakePointWithInches(2, 1, 2));
            Plane testPlane2 = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 1, -1)), PointGenerator.MakePointWithInches(1, 3, 3));

            Line test12Intersect = testPlane1.Intersection(testPlane2);
            Line test21Intersect = testPlane2.Intersection(testPlane1);

            Line expectedLine = new Line(new Direction(PointGenerator.MakePointWithInches(0, 3, 3)), PointGenerator.MakePointWithInches(2, -1, 0));

            test12Intersect.Should().Be(test21Intersect);
            test21Intersect.Should().Be(expectedLine);
        }

        [Test()]
        public void Plane_IntersectionLineWithPlane_ZeroCases()
        {
            //try all the zero cases
            Point testPoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point testPointX = PointGenerator.MakePointWithInches(10, 0, 0);
            Point testPointY = PointGenerator.MakePointWithInches(0, 10, 0);
            Point testPointZ = PointGenerator.MakePointWithInches(0, 0, 10);

            Plane testPlaneXY = new Plane(testPoint, testPointX, testPointY);
            Plane testPlaneXZ = new Plane(testPoint, testPointX, testPointZ);
            Plane testPlaneYZ = new Plane(testPoint, testPointY, testPointZ);

            Line testXYXZIntersect = testPlaneXZ.Intersection(testPlaneXY);
            Line testXZXYIntersect = testPlaneXY.Intersection(testPlaneXZ);
            testXYXZIntersect.Should().Be(Line.XAxis);
            testXZXYIntersect.Should().Be(Line.XAxis);

            Line testXYYZIntersect = testPlaneXY.Intersection(testPlaneYZ);
            Line testYZXYIntersect = testPlaneYZ.Intersection(testPlaneXY);
            testXYYZIntersect.Should().Be(Line.YAxis);
            testYZXYIntersect.Should().Be(Line.YAxis);

            Line testXZYZIntersect = testPlaneXZ.Intersection(testPlaneYZ);
            Line testYZXZIntersect = testPlaneYZ.Intersection(testPlaneXZ);
            testXZYZIntersect.Should().Be(Line.ZAxis);
            testYZXZIntersect.Should().Be(Line.ZAxis);

            Line testXYXY = testPlaneXY.Intersection(testPlaneXY);
            testXYXY.Should().Be(null);
        }

        [Test()]
        public void Plane_ParallelLineTest()
        {
            Line plane1Line1 = new Line(PointGenerator.MakePointWithInches(1, 1, 0));
            Line plane1Line2 = new Line(PointGenerator.MakePointWithInches(2, 0, -1), PointGenerator.MakePointWithInches(3, 1, -1));
            Plane testPlane1 = new Plane(plane1Line1, plane1Line2);

            Line plane2Line1 = new Line(PointGenerator.MakePointWithInches(-3, 2, -2));
            Line plane2Line2 = new Line(PointGenerator.MakePointWithInches(1, -2, -1), PointGenerator.MakePointWithInches(-2, 0, -3));
            Plane testPlane2 = new Plane(plane2Line1, plane2Line2);

            Line parallel1 = new Line(PointGenerator.MakePointWithInches(1, -2, 1), PointGenerator.MakePointWithInches(2, -1, 1));
            Line parallel2 = new Line(PointGenerator.MakePointWithInches(-6, 4, -4));

            testPlane1.IsParallelTo(parallel1).Should().BeTrue();
            testPlane1.IsParallelTo(parallel2).Should().BeFalse();

            testPlane2.IsParallelTo(parallel1).Should().BeFalse();
            testPlane2.IsParallelTo(parallel2).Should().BeTrue();
        }

        [Test()]
        public void Plane_PrepindicularLineTest()
        {
            Plane testPlane1 = new Plane(new Direction(PointGenerator.MakePointWithInches(2, -1, 1)), PointGenerator.MakePointWithInches(2, 1, 2));
            Plane testPlane2 = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 1, -1)), PointGenerator.MakePointWithInches(1, 3, 3));

            Line perpindicular1 = new Line(PointGenerator.MakePointWithInches(2, -1, 1));
            Line perpindicular2 = new Line(PointGenerator.MakePointWithInches(3, 1, -3), PointGenerator.MakePointWithInches(4, 2, -4));

            testPlane1.IsPerpindicularTo(perpindicular1).Should().BeTrue();
            testPlane1.IsPerpindicularTo(perpindicular2).Should().BeFalse();

            testPlane2.IsPerpindicularTo(perpindicular1).Should().BeFalse();
            testPlane2.IsPerpindicularTo(perpindicular2).Should().BeTrue();
        }

        [Test()]
        public void Plane_IntersectLine()
        {
            Plane testPlane1 = new Plane(new Direction(PointGenerator.MakePointWithInches(2, -1, 1)), PointGenerator.MakePointWithInches(2, -1, 1));
            Plane testPlane2 = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 2, -1)), PointGenerator.MakePointWithInches(2, -1, 1));

            Line perpindicular1 = new Line(PointGenerator.MakePointWithInches(2, -1, 1));
            Line perpindicular2 = new Line(PointGenerator.MakePointWithInches(3, 1, -3), PointGenerator.MakePointWithInches(4, 3, -4)); //1, 2, -1

            Point intersection11 = testPlane1.Intersection(perpindicular1);
            Point intersection12 = testPlane1.Intersection(perpindicular2);
            Point intersection21 = testPlane2.Intersection(perpindicular1);
            Point intersection22 = testPlane2.Intersection(perpindicular2);

            intersection11.Should().Be(PointGenerator.MakePointWithInches(2, -1, 1));
            intersection21.Should().Be(PointGenerator.MakePointWithInches(2, -1, 1));
            intersection12.Should().Be(PointGenerator.MakePointWithInches(-1, -7, 1));
            intersection22.Should().Be(PointGenerator.MakePointWithInches(1.5, -2, -1.5));
        }
    }
}
