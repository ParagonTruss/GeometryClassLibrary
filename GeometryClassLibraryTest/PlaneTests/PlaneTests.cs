using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class PlaneTests
    {
        [Test()]
        public void Plane_ContainsLine()
        {
            Plane testPlane = new Plane(Point.Origin, Point.MakePointWithInches(1, 3, 0), Point.MakePointWithInches(4, -2, 0));
            Line testLine = new Line(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(0, -1, 0));
            Line testFalseLine = new Line(Point.MakePointWithInches(1, 2, -2), Point.MakePointWithInches(2, 4, 3));

            bool result = testPlane.Contains(testLine);
            bool result2 = testPlane.Contains(testFalseLine);

            result.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_ContainsLineOnOrigin()
        {
            Plane testPlane = new Plane(Point.Origin, Point.MakePointWithInches(1, 3, 0), Point.MakePointWithInches(-3, -1, 0));
            Line testLine = new Line(Point.Origin, Point.MakePointWithInches(1, 1, 0));

            bool result = testPlane.Contains(testLine);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPolygon()
        {
            Plane testPlane = new Plane(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, -3, 4), Point.MakePointWithInches(1, 2, 2));
            Polygon testRegion = new Polygon(new List<Point>
                {
                    Point.MakePointWithInches(1,1,1),
                    Point.MakePointWithInches(1,2,1),
                    Point.MakePointWithInches(1,2,2),
                    Point.MakePointWithInches(1,1,2)
                });

            bool result = testPlane.Contains(testRegion);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPoint()
        {
            Plane testPlane = new Plane(Point.MakePointWithInches(0,1,0), Point.MakePointWithInches(2,1,-3), Point.MakePointWithInches(-4,1,-1));
            Point testPoint1 = Point.MakePointWithInches(0, 1, 5);
            Point testPoint2 = Point.MakePointWithInches(2, 2, 2);

            bool result1 = testPlane.Contains(testPoint1);
            bool result2 = testPlane.Contains(testPoint2);

            result1.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_RotateTest()
        {
            Point testBasePoint = Point.MakePointWithInches(1, 1, -1);            
            Vector testNormalVector = new Vector(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0));

            Plane testPlane = new Plane(testNormalVector.Direction, testBasePoint);

            Line rotationAxis = new Line(new Direction(Point.MakePointWithInches(1, 1, 1)), Point.MakePointWithInches(1, -1, -1));
            Angle rotationAngle = new Angle(new Degree(), 212);

            Plane actualResult = testPlane.Rotate(new Rotation(rotationAxis, rotationAngle));

            Point expectedPoint = Point.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495);
            Vector expectedVector = new Vector(Point.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), Point.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            Plane expectedResult = new Plane(expectedVector.Direction, expectedPoint);

            bool test = expectedPoint == actualResult.BasePoint;
            bool test2 = expectedVector == actualResult.NormalVector;

            (actualResult == expectedResult).Should().BeTrue();
        }

        [Test()]
        public void Plane_PointOnSameSideAs()
        {
            Point testPoint = Point.MakePointWithInches(1, 3, -1);
            Point testPoint2 = Point.MakePointWithInches(-1, -2, 5);
            Point testPoint3 = Point.MakePointWithInches(0, 1, 0);
            Point referencePoint = Point.MakePointWithInches(1, 2, 1);
            Point referencePoint2 = Point.MakePointWithInches(0, 2, 1);
            Vector testNormalVector = new Vector(Point.MakePointWithInches(1, 0, 0));

            Plane testPlane = new Plane(testNormalVector);

            testPlane.PointIsOnSameSideAs(testPoint, referencePoint).Should().BeTrue(); //test one on the same side
            testPlane.PointIsOnSameSideAs(testPoint2, referencePoint).Should().BeFalse(); //test one on the opposite side
            testPlane.PointIsOnSameSideAs(testPoint3, referencePoint).Should().BeFalse(); //test one on the plane
            testPlane.PointIsOnSameSideAs(testPoint, referencePoint2).Should().BeFalse(); //test a reference point on the plane


            Point testPointOffOrigin = Point.MakePointWithInches(5, 4, 0);
            Point testPointOffOrigin2 = Point.MakePointWithInches(5, -2, 0);
            Point referencePointOffOrigin = Point.MakePointWithInches(1, 2, 3);
            Point planeBase = Point.MakePointWithInches(1, -4, 2);
            Vector testNormalVectorOffOrigin = new Vector(Point.MakePointWithInches(-1, 2, 1));

            Plane testPlaneOffOrigin = new Plane(testNormalVectorOffOrigin.Direction, planeBase);

            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin, referencePointOffOrigin).Should().BeTrue();
            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin2, referencePointOffOrigin).Should().BeFalse();
            testPlaneOffOrigin.PointIsOnSameSideAs(planeBase, referencePointOffOrigin).Should().BeFalse();

        }

        [Test()]
        public void Plane_IntersectionWithPlane()
        {
            Plane testPlane1 = new Plane(new Direction(Point.MakePointWithInches(2, -1, 1)), Point.MakePointWithInches(2, 1, 2));
            Plane testPlane2 = new Plane(new Direction(Point.MakePointWithInches(1, 1, -1)), Point.MakePointWithInches(1, 3, 3));

            Line test12Intersect = testPlane1.IntersectWithPlane(testPlane2);
            Line test21Intersect = testPlane2.IntersectWithPlane(testPlane1);

            Line expectedLine = new Line(new Direction(Point.MakePointWithInches(0, 3, 3)), Point.MakePointWithInches(2, -1, 0));

            Assert.IsTrue(test12Intersect.Equals(test21Intersect));
            Assert.IsTrue(test21Intersect.Equals(expectedLine));
        }

        [Test()]
        public void Plane_IntersectionWithPlane_IdenticalPlane()
        {
            Plane testPlane = new Plane(new Direction(2, -1, 1), Point.MakePointWithInches(2, 1, 2));

            Line found = (testPlane.IntersectWithPlane(testPlane));
            Line expected = new Line(new Direction(-1, 0, 2), Point.MakePointWithInches(2, 1, 2));

            testPlane.Contains(found).Should().BeTrue();
            (found == expected).Should().BeTrue();
        }

        [Test()]
        public void Plane_IntersectionWithPlane_ZeroCases()
        {
            //try all the zero cases
            Point testPoint = Point.Origin;
            Point testPointX = Point.MakePointWithInches(10, 0, 0);
            Point testPointY = Point.MakePointWithInches(0, 10, 0);
            Point testPointZ = Point.MakePointWithInches(0, 0, 10);

            Plane testPlaneXY = new Plane(testPoint, testPointX, testPointY);
            Plane testPlaneXZ = new Plane(testPoint, testPointX, testPointZ);
            Plane testPlaneYZ = new Plane(testPoint, testPointY, testPointZ);

            Line testXYXZIntersect = testPlaneXZ.IntersectWithPlane(testPlaneXY);
            Line testXZXYIntersect = testPlaneXY.IntersectWithPlane(testPlaneXZ);
            testXYXZIntersect.Should().Be(Line.XAxis);
            testXZXYIntersect.Should().Be(Line.XAxis);

            Line testXYYZIntersect = testPlaneXY.IntersectWithPlane(testPlaneYZ);
            Line testYZXYIntersect = testPlaneYZ.IntersectWithPlane(testPlaneXY);
            testXYYZIntersect.Should().Be(Line.YAxis);
            testYZXYIntersect.Should().Be(Line.YAxis);

            Line testXZYZIntersect = testPlaneXZ.IntersectWithPlane(testPlaneYZ);
            Line testYZXZIntersect = testPlaneYZ.IntersectWithPlane(testPlaneXZ);
            testXZYZIntersect.Should().Be(Line.ZAxis);
            testYZXZIntersect.Should().Be(Line.ZAxis);

            Line testXYXY = testPlaneXY.IntersectWithPlane(testPlaneXY);
            testXYXY.Should().Be(Line.YAxis);
        }

        [Test()]
        public void Plane_ParallelLineTest()
        {
            Line plane1Line1 = new Line(Point.MakePointWithInches(1, 1, 0));
            Line plane1Line2 = new Line(Point.MakePointWithInches(2, 0, -1), Point.MakePointWithInches(3, 1, -1));
            Plane testPlane1 = new Plane(plane1Line1, plane1Line2);

            Line plane2Line1 = new Line(Point.MakePointWithInches(-3, 2, -2));
            Line plane2Line2 = new Line(Point.MakePointWithInches(1, -2, -1), Point.MakePointWithInches(-2, 0, -3));
            Plane testPlane2 = new Plane(plane2Line1, plane2Line2);

            Line parallel1 = new Line(Point.MakePointWithInches(1, -2, 1), Point.MakePointWithInches(2, -1, 1));
            Line parallel2 = new Line(Point.MakePointWithInches(-6, 4, -4));

            testPlane1.IsParallelTo(parallel1).Should().BeTrue();
            testPlane1.IsParallelTo(parallel2).Should().BeFalse();

            testPlane2.IsParallelTo(parallel1).Should().BeFalse();
            testPlane2.IsParallelTo(parallel2).Should().BeTrue();
        }

        [Test()]
        public void Plane_PerpendicularLineTest()
        {
            Plane testPlane1 = new Plane(new Direction(Point.MakePointWithInches(2, -1, 1)), Point.MakePointWithInches(2, 1, 2));
            Plane testPlane2 = new Plane(new Direction(Point.MakePointWithInches(1, 1, -1)), Point.MakePointWithInches(1, 3, 3));

            Line perpindicular1 = new Line(Point.MakePointWithInches(2, -1, 1));
            Line perpindicular2 = new Line(Point.MakePointWithInches(3, 1, -3), Point.MakePointWithInches(4, 2, -4));

            testPlane1.IsPerpendicularTo(perpindicular1).Should().BeTrue();
            testPlane1.IsPerpendicularTo(perpindicular2).Should().BeFalse();

            testPlane2.IsPerpendicularTo(perpindicular1).Should().BeFalse();
            testPlane2.IsPerpendicularTo(perpindicular2).Should().BeTrue();
        }

        [Test()]
        public void Plane_IntersectLine()
        {
            Plane testPlane1 = new Plane(new Direction(2, -1, 1), Point.MakePointWithInches(2, -1, 1));
            Plane testPlane2 = new Plane(new Direction(1, 2, -1), Point.MakePointWithInches(2, -1, 1));

            Line perpendicular1 = new Line(Point.MakePointWithInches(2, -1, 1));
            Line perpendicular2 = new Line(Point.MakePointWithInches(3, 1, -3), Point.MakePointWithInches(4, 3, -4)); //1, 2, -1

            Point intersection11 = testPlane1.IntersectWithLine(perpendicular1);
            Point intersection12 = testPlane1.IntersectWithLine(perpendicular2);
            Point intersection21 = testPlane2.IntersectWithLine(perpendicular1);
            Point intersection22 = testPlane2.IntersectWithLine(perpendicular2);

            intersection11.Should().Be(Point.MakePointWithInches(2, -1, 1));
            intersection21.Should().Be(Point.MakePointWithInches(2, -1, 1));
            intersection12.Should().Be(Point.MakePointWithInches(-1, -7, 1));
            intersection22.Should().Be(Point.MakePointWithInches(1.5, -2, -1.5));
        }

        [Test()]
        public void Plane_Intersect_Line_Segment()
        {
            Plane testPlane = new Plane(new Direction(2, -1, 1), Point.MakePointWithInches(2, -1, 1));

            var linesegment=new LineSegment(Point.MakePointWithInches(2, -1, 1));
            testPlane.Intersects(linesegment).Should().BeTrue();
            
        }
        [Test()]
        public void Plane_Does_Not_Intersect_Parallel_Line_Segment()
        {        
            var linesegment = new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(2, 1, 2));
            Plane.XZ.Intersects(linesegment).Should().BeFalse();

        }
        [Test()]
        public void Plane_IntersectLineOnPlane()
        {
            Plane testPlane = new Plane(Direction.Out);
            Line lineOnPlane = new Line(Point.MakePointWithInches(2, 1, 0));

            testPlane.IntersectWithLine(lineOnPlane).Should().Be(Point.Origin);
        }

        [Test()]
        public void Plane_IntersectLine_PrecisionCheck()
        {
            Point point1 = Point.MakePointWithInches(-2.5, 73, 3.5);
            Point point2 = Point.MakePointWithInches(1, 1, 2);
            Point point3 = Point.MakePointWithInches(0, 0, 2);
            
            Plane testPlane = new Plane(Direction.Out, point3);
            Line line = new Line(point1, point2);

            Point actualIntersection = testPlane.IntersectWithLine(line);
            Point expectedIntersection = point2;

            (actualIntersection == expectedIntersection).Should().BeTrue();
        }

        [Test()]
        public void Plane_Lies_Along_Polygon_Without_Cutting()
        {
            Point point1 = Point.MakePointWithInches(0, 0);
            Point point2 = Point.MakePointWithInches(1, 0);
            Point point3 = Point.MakePointWithInches(1, 1);
            Point point4 = Point.MakePointWithInches(0, 1);

            var polyList=new List<Point>();
            polyList.Add(point1);
            polyList.Add(point2);
            polyList.Add(point3);
            polyList.Add(point4);

            var poly=new Polygon(polyList);
            var cutsAcross = Plane.XZ.CutsAcross(poly);
            cutsAcross.Should().BeFalse();

        }


        [Test()]
        public void Plane_Cuts_Across_Polygon()
        {
            var point1 = Point.MakePointWithInches(0, -1);
            var point2 = Point.MakePointWithInches(1, -1);
            var point3 = Point.MakePointWithInches(1, 0);
            var point4 = Point.MakePointWithInches(0.5, 0);
            var point5 = Point.MakePointWithInches(0, 1);

            var polyList = new List<Point>();
            polyList.Add(point1);
            polyList.Add(point2);
            polyList.Add(point3);
            polyList.Add(point4);
            polyList.Add(point5);

            var poly = new Polygon(polyList);
            var cutsAcross = Plane.XZ.CutsAcross(poly);
            cutsAcross.Should().BeTrue();

        }
    }
}
