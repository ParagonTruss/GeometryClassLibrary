using System;
using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class PolygonTests
    {

        [Test]
        public void Polygon_Extrude_Colinear_Points()
        {
            var extrusion = 3.5*Inches*Direction.Out;
            var polygon = Polygon.CreateInXYPlane(Inches, -2.22044604925031E-16, 16.5, 1.5, 16.5, 3, 16.5, 3, 1.5, 1.5,
                1.5, 0, 1.5);

            var solid = polygon.Extrude(extrusion);

            solid.Volume.Should().Be(new Volume(new CubicInch(), 3*15*3.5));
        }
      
        
        [Test()]
        public void Polygon_Extrude()
        {
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 4, 0);
            Point bottomRightPoint = Point.MakePointWithInches(8, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(8, 4, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, -4);
            Point backtopleftpoint = Point.MakePointWithInches(0, 4, -4);
            Point backbottomrightpoint = Point.MakePointWithInches(8, 0, -4);
            Point backtoprightpoint = Point.MakePointWithInches(8, 4, -4);

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


            Polyhedron extrudedResult = frontRegion.Extrude(new Vector(Point.MakePointWithInches(0, 0, -4)));
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
            lineSegments.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 0, 0), Point.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 4, 0), Point.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 4, 0), Point.Origin));
            Polygon testPolygon = new Polygon(lineSegments);

            Line rotationAxis = new Line(Point.MakePointWithInches(1, 0, 0)); //This is the X axis
            Angle rotationAngle = Angle.RightAngle;

            Polygon actualPolygon = testPolygon.Rotate(new Rotation(rotationAxis, rotationAngle));

            List<LineSegment> expectedLineSegments = new List<LineSegment>();
            expectedLineSegments.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(8, 0, 0)));
            expectedLineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 0, 0), Point.MakePointWithInches(8, 0, 4)));
            expectedLineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 0, 4), Point.MakePointWithInches(0, 0, 4)));
            expectedLineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 4), Point.Origin));
            Polygon expectedPolygon = new Polygon(expectedLineSegments);

            (actualPolygon == expectedPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_RotateTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon = new Polygon(lineSegments);

            Line rotationAxis = new Line(new Direction(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(1, -1, -1)));
            Angle rotationAngle = new Angle(new Degree(), 212);

            Polygon actualPolygon = testPolygon.Rotate(new Rotation(rotationAxis, rotationAngle));

            List<LineSegment> expectedLineSegments = new List<LineSegment>();
            actualPolygon.Contains(new LineSegment(Point.MakePointWithInches(5.238195, 1.6816970, -1.919892), Point.MakePointWithInches(1.31623019, -1.08627088, -5.229959)));
            actualPolygon.Contains(new LineSegment(Point.MakePointWithInches(1.3162301, -1.0862708, -5.229959), Point.MakePointWithInches(2.843930, -1.46406412, -0.379865)));
            actualPolygon.Contains(new LineSegment(Point.MakePointWithInches(2.8439301, -1.4640641, -0.379865), Point.MakePointWithInches(5.238195, 1.681697053, -1.9198923)));
        }

        [Test()]
        public void Polygon_TranslateTest()
        {

            Point point1 = Point.MakePointWithInches(1, -5, -4);
            Point point2 = Point.MakePointWithInches(1, 0, 0);
            Point point3 = Point.Origin;
            Point point4 = Point.MakePointWithInches(0, -5, -4);
            List<Point> vertices = new List<Point>() { point1, point2, point3, point4 };
            Polygon testPolygon = new Polygon(vertices);

            Point testDisplacement = Point.MakePointWithInches(-1, 5, 4);

            Polygon actualPolygon = testPolygon.Translate(testDisplacement);

            Point point5 = Point.Origin;
            Point point6 = Point.MakePointWithInches(0, 5, 4);
            Point point7 = Point.MakePointWithInches(-1, 5, 4);
            Point point8 = Point.MakePointWithInches(-1, 0, 0);
            List<Point> expectedVertices = new List<Point>() { point5, point6, point7, point8 };
            Polygon expectedPolygon = new Polygon(expectedVertices);

            actualPolygon.Should().Be(expectedPolygon);
        }

        [Test()]
        public void Polygon_Copy()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-4, 2, 0), Point.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 0), Point.MakePointWithInches(-5, 5, 0)));
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
            planeCopy = planeCopy.Shift(new Shift(new Rotation(Line.XAxis, Angle.RightAngle / 2), Point.MakePointWithInches(1, 4, -2)));

            foreach (LineSegment line in testPolygon.LineSegments)
            {
                planeCopy.LineSegments.Contains(line).Should().BeFalse();
            }
            (planeCopy.BasePoint == testPolygon.BasePoint).Should().BeFalse();
            (planeCopy.NormalVector == testPolygon.NormalVector).Should().BeFalse();
        }

        [Test()]
        public void Polygon_Contains_ContainsOnInside_Touches()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-4, 2, 0), Point.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 0), Point.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point insidePlane1 = Point.MakePointWithInches(-2, 2, 0);
            Point insidePlane2 = Point.MakePointWithInches(-2, 2, 1);

            Point center1 = testPolygon.CenterPoint;

            //make sure the sides are not included
            Point sideTest = Point.Origin;


            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            //make sure the PlaneRegion contains the CenterPoint
            Point center2 = testPolygon2.CenterPoint;

            Point notOnPlane = center2.Shift(new Shift(Point.MakePointWithInches(.5, 0, 0)));

            //Points on the plane not boundaries (true for exclusive and inclusive, false for touching)
            testPolygon.ContainsOnInside(insidePlane1).Should().BeTrue();
            testPolygon.Contains(insidePlane1).Should().BeTrue();
            testPolygon.Touches(insidePlane1).Should().BeFalse();

            testPolygon.ContainsOnInside(insidePlane2).Should().BeFalse();
            testPolygon.Contains(insidePlane2).Should().BeFalse();
            testPolygon.Touches(insidePlane2).Should().BeFalse();

            //make sure the PlaneRegion contains the CenterPoint (true for exclusive and inclusive, false for touching)
            testPolygon.ContainsOnInside(center1).Should().BeTrue();
            testPolygon.Contains(center1).Should().BeTrue();
            testPolygon.Touches(center1).Should().BeFalse();

            testPolygon2.ContainsOnInside(center2).Should().BeTrue();
            testPolygon2.Contains(center2).Should().BeTrue();
            testPolygon2.Touches(center2).Should().BeFalse();

            //check the side point (true for inclusive and touches, false for exclusive)
            testPolygon.ContainsOnInside(sideTest).Should().BeFalse();
            testPolygon.Contains(sideTest).Should().BeTrue();
            testPolygon.Touches(sideTest).Should().BeTrue();

            //not on plane (false for all)
            testPolygon2.ContainsOnInside(notOnPlane).Should().BeFalse();
            testPolygon2.Contains(notOnPlane).Should().BeFalse();
            testPolygon2.Touches(notOnPlane).Should().BeFalse();
        }

        [Test()]
        public void Polygon_ContainsPolygon()
        {
            Point pentagonPoint1 = Point.MakePointWithInches(0, 0);
            Point pentagonPoint2 = Point.MakePointWithInches(4, 0);
            Point pentagonPoint3 = Point.MakePointWithInches(2, 2);
            Point pentagonPoint4 = Point.MakePointWithInches(4, 4);
            Point pentagonPoint5 = Point.MakePointWithInches(0, 4);
            Polygon concavePentagon =
                new Polygon(new List<Point>()
                {
                    pentagonPoint1,
                    pentagonPoint2,
                    pentagonPoint3,
                    pentagonPoint4,
                    pentagonPoint5
                });

            Point rectanglePoint1 = Point.MakePointWithInches(1, 1);
            Point rectanglePoint2 = Point.MakePointWithInches(2.5, 1);
            Point rectanglePoint3 = Point.MakePointWithInches(2.5, 3);
            Point rectanglePoint4 = Point.MakePointWithInches(1, 3);
            Polygon rectangle =
                new Polygon(new List<Point>() {rectanglePoint1, rectanglePoint2, rectanglePoint3, rectanglePoint4});

            Point squarePoint1 = Point.MakePointWithInches(1, 1);
            Point squarePoint2 = Point.MakePointWithInches(2, 1);
            Point squarePoint3 = Point.MakePointWithInches(2, 2);
            Point squarePoint4 = Point.MakePointWithInches(1, 2);
            Polygon square = new Polygon(new List<Point>() {squarePoint1, squarePoint2, squarePoint3, squarePoint4});

            concavePentagon.Contains(rectangle).Should().BeFalse();
            rectangle.Contains(concavePentagon).Should().BeFalse();

            concavePentagon.Contains(square).Should().BeTrue();
            square.Contains(concavePentagon).Should().BeFalse();

            rectangle.Contains(square).Should().BeTrue();
            square.Contains(rectangle).Should().BeFalse();
        }
        [Test()]
        public void Polygon_ContainsPoint()
        {
            Point pentagonPoint1 = Point.MakePointWithInches(0, 0);
            Point pentagonPoint2 = Point.MakePointWithInches(4, 0);
            Point pentagonPoint3 = Point.MakePointWithInches(2, 2);
            Point pentagonPoint4 = Point.MakePointWithInches(4, 4);
            Point pentagonPoint5 = Point.MakePointWithInches(0, 4);
            Polygon concavePentagon =
                new Polygon(new List<Point>()
                {
                    pentagonPoint1,
                    pentagonPoint2,
                    pentagonPoint3,
                    pentagonPoint4,
                    pentagonPoint5
                });

            Point rectanglePoint1 = Point.MakePointWithInches(1, 1);
            Point rectanglePoint2 = Point.MakePointWithInches(2.5, 1);
            Point rectanglePoint3 = Point.MakePointWithInches(2.5, 3);
            Point rectanglePoint4 = Point.MakePointWithInches(1, 3);
            Polygon rectangle =
                new Polygon(new List<Point>() { rectanglePoint1, rectanglePoint2, rectanglePoint3, rectanglePoint4 });

            Point squarePoint1 = Point.MakePointWithInches(1, 1);
            Point squarePoint2 = Point.MakePointWithInches(2, 1);
            Point squarePoint3 = Point.MakePointWithInches(2, 2);
            Point squarePoint4 = Point.MakePointWithInches(1, 2);
            Polygon square = new Polygon(new List<Point>() { squarePoint1, squarePoint2, squarePoint3, squarePoint4 });

            Point notInPlane = Point.MakePointWithInches(1, 1, 1);
            Point distantPoint = Point.MakePointWithInches(20, 20);
            Point outsidePentagon = Point.MakePointWithInches(3, 2);
            Point onRectangleSegment = Point.MakePointWithInches(1, 1.5);

            concavePentagon.Contains(squarePoint3).Should().BeTrue();
            concavePentagon.Contains(notInPlane).Should().BeFalse();
            concavePentagon.Contains(rectanglePoint1).Should().BeTrue();
            concavePentagon.Contains(distantPoint).Should().BeFalse();
            concavePentagon.Contains(outsidePentagon).Should().BeFalse();

            square.Contains(rectanglePoint1).Should().BeTrue();
            square.Contains(rectanglePoint4).Should().BeFalse();
            square.Contains(pentagonPoint1).Should().BeFalse();
            square.Contains(distantPoint).Should().BeFalse();
            square.Contains(onRectangleSegment).Should().BeTrue();

            rectangle.Contains(rectanglePoint1).Should().BeTrue();
            rectangle.Contains(squarePoint2).Should().BeTrue();
            rectangle.Contains(pentagonPoint1).Should().BeFalse();
            rectangle.Contains(distantPoint).Should().BeFalse();
            rectangle.Contains(onRectangleSegment).Should().BeTrue();

        }
        [Test]
        public void Polygon_ContainsPolygon_Concave()
        {
            var birdsMouth = Polygon.CreateInXYPlane(Inches,
                0, 0,
                4, 0,
                2, 2,
                4, 4,
                0, 4);

            var innerSquare = Polygon.CreateInXYPlane(Inches,
                1, 1,
                3, 1,
                3, 3,
                1, 3);

            birdsMouth.Contains(innerSquare).Should().BeFalse();
        }

        [Test()]
        public void Polygon_NormalLine()
        {
            Point point1 = Point.MakePointWithInches(0, 1, 0);
            Point point2 = Point.MakePointWithInches(0, 3, 0);
            Point point3 = Point.MakePointWithInches(4, 3, 0);
            Point point4 = Point.MakePointWithInches(4, 1, 0);
            List<Point> vertices1 = new List<Point>(){ point1, point2, point3, point4 };
            Polygon upsideDownSquare = new Polygon(vertices1);

            Line normal1 = upsideDownSquare.NormalLine;
            (normal1.Direction == new Direction(0, 0, -1)).Should().BeTrue();
            (new Plane(upsideDownSquare).Contains(normal1.BasePoint)).Should().BeTrue();


            Point topPoint1 = Point.MakePointWithInches(0, 0, 5);
            Point topPoint2 = Point.MakePointWithInches(1, 1, 5);
            Point topPoint3 = Point.MakePointWithInches(2, 0, 5);
            Point topPoint4 = Point.MakePointWithInches(2, 2, 5);
            Point topPoint5 = Point.MakePointWithInches(0, 2, 5);

            Polygon concavePentagon = new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }, false);

            Line normal2 = concavePentagon.NormalLine;
            (normal2.Direction == new Direction(0,0,1)).Should().BeTrue();


            List<LineSegment> bounds3 = new List<LineSegment>();
            bounds3.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(3, 2, 1)));
            bounds3.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds3.Add(new LineSegment(Point.MakePointWithInches(3, 2, 1), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon3 = new Polygon(bounds3);

            Line normal3 = testPolygon3.NormalLine;
            (normal3.Direction == new Direction(-1, 2, 1)).Should().BeTrue();
            (new Plane(testPolygon3).Contains(normal3.BasePoint)).Should().BeTrue();   
        }

        [Test()]
        public void Polygon_IsConvex_Quadrilateral()
        {
            Polygon concaveQuadrilateral = new ConcaveQuadrilateral();

            concaveQuadrilateral.IsConvex.Should().BeFalse();
        }

        [Test()]
        public void Polygon_IsConvex_ConcavePentagon()
        {
            Polygon concavePentagon = new ConcavePentagon();

            concavePentagon.IsConvex.Should().BeFalse();
        }

        [Test()]
        public void Polygon_IsConvex_Triangle()
        {
            List<LineSegment> lineSegments = new List<LineSegment>
            {
                new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)),
                new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)),
                new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3))
            };
            Polygon triangle = new Polygon(lineSegments);

            triangle.IsConvex.Should().BeTrue();
        }

        [Test()]
        public void Polygon_AreaOfRectangle()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon1 = new Polygon(bounds);

            //check to see if its what we expected
            Area testArea1 = testPolygon1.Area;
            testArea1.Should().Be(new Area(new SquareInch(), 8));
        }

        [Test()]
        public void Polygon_AreaOf3DTriangle()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            Area area = testPolygon2.Area;
            Area expected2 = new Area(new SquareInch(), 10.52);

            (area == expected2).Should().BeTrue();

        }
        [Test()]
        public void Polygon_AreaOfConcaveQuadrilateral()
        {
            Polygon testPolygon = new ConcaveQuadrilateral();

            //check to see if its what we expected
            Area testArea2 = testPolygon.Area;
            testArea2.Should().Be(new Area(new SquareInch(), 3));
        }

        [Test()]
        public void Polygon_AreaOfConcavePentagon()
        {
            Polygon concavePentagon = new ConcavePentagon();
            Area area = concavePentagon.Area;
            Area expected = new Area(new SquareInch(), 3);
            (area == expected).Should().BeTrue();
        }

        [Test()]
        public void Polygon_AreaOfSquareInXZPlane()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(4, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 0, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 0, 4), Point.MakePointWithInches(0, 0, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 4), Point.Origin));
            Polygon testPolygon1 = new Polygon(bounds);

            //check to see if its what we expected
            Area testArea1 = testPolygon1.Area;
            testArea1.Should().Be(new Area(new SquareInch(), 16));
        }

        [Test()]
        public void Polygon_AreaOfSquareInYZPlane()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 4, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 4, 0), Point.MakePointWithInches(0, 4, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 4, 4), Point.MakePointWithInches(0, 0, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 4), Point.Origin));
            Polygon testPolygon1 = new Polygon(bounds);

            //check to see if its what we expected
            Area testArea1 = testPolygon1.Area;
            testArea1.Should().Be(new Area(new SquareInch(), 16));
        }

        [Test()]
        public void Polygon_Centroid()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-4, 2, 0), Point.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 0), Point.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Point center = testPolygon.Centroid;
            Point expected = Point.MakePointWithInches(-2.33, 3, 0);

            center.Should().Be(expected);

            //make sure the centroid is in the region
            testPolygon.Contains(center).Should().BeTrue();
            //Note: The Centroid is always contained within the region of a convex polygon.
            //However, for concave polygons this is not the case....

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon2 = new Polygon(lineSegments);

            Point center2 = testPolygon2.Centroid;
            Point expected2 = Point.MakePointWithInches(-0.6666667, 0.33333333, 0.66666667);

            center2.Should().Be(expected2);

            //make sure the centroid is in the region
            testPolygon2.Contains(center2).Should().BeTrue();
        }
        [Test()]
        public void Polygon_SliceOnLineTest()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 4)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 4), Point.MakePointWithInches(4, 3, 4)));
            Polygon testPolygon = new Polygon(bounds);

            Line slicingLine = new Line(Point.Origin, Point.MakePointWithInches(1, 1, 1));

            List<Polygon> results = testPolygon.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(1, 1, 1)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(3, 3, 3)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(3, 3, 3)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(3, 3, 3)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(1, 1, 1), Point.MakePointWithInches(4, 1, 4)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(3, 3, 3), Point.MakePointWithInches(4, 3, 4)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 4), Point.MakePointWithInches(4, 3, 4)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(Point.Origin, Point.MakePointWithInches(1, 1, 0.9));
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
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(2, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.MakePointWithInches(6, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(6, 0, 0), Point.Origin));
            bounds.Add(new LineSegment(Point.MakePointWithInches(6, 1, 0), Point.MakePointWithInches(6, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Line slicingLine = new Line(Point.MakePointWithInches(6, 5, 0), Point.MakePointWithInches(2, 1, 0));

            List<Polygon> results = testPolygon.Slice(slicingLine);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(2, 1, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.MakePointWithInches(6, 1, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(1, 0, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(6, 1, 0), Point.MakePointWithInches(6, 0, 0)));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(1, 0, 0)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(2, 1, 0)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.Origin));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected2).Should().BeTrue();
            results.Contains(expected1).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(Point.Origin, Point.MakePointWithInches(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SliceACaseThatDidntWorkBefore()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 3.5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3.5, 0), Point.MakePointWithInches(240, 3.5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 3.5, 0), Point.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.Origin));
            Polygon testPolygon = new Polygon(bounds);

            Plane slicingPlane = new Plane(new Direction(new Angle(new Radian(), 5.6548667765)), Point.MakePointWithInches(122.8315595, 169.313137732, 0));

            List<Polygon> results = testPolygon.Slice(slicingPlane);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 0.25, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0.25, 0), Point.MakePointWithInches(2.36126321602, 3.5, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(2.36126321602, 3.5, 0), Point.MakePointWithInches(240, 3.5, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(240, 3.5, 0), Point.MakePointWithInches(240, 0, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.Origin));
            Polygon expected1 = new Polygon(expected1Bounds);

            List<LineSegment> expected2Bounds = new List<LineSegment>();
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0.25, 0), Point.MakePointWithInches(0, 3.5, 0)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3.5, 0), Point.MakePointWithInches(2.36126321602, 3.5, 0)));
            expected2Bounds.Add(new LineSegment(Point.MakePointWithInches(2.36126321602, 3.5, 0), Point.MakePointWithInches(0, 0.25, 0)));
            Polygon expected2 = new Polygon(expected2Bounds);


            results.Contains(expected1).Should().BeTrue();
            results.Contains(expected2).Should().BeTrue();

            //now make sure it handles no intersection well
            Line notIntersecting = new Line(Point.Origin, Point.MakePointWithInches(1, 1, 0.9));
            List<Polygon> results2 = testPolygon.Slice(notIntersecting);

            //should only return the original plane
            results2.Count.Should().Be(1);
            (results2[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_SliceAnotherCaseThatDidntWorkBefore()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 0, 1.5)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 1.5), Point.MakePointWithInches(240, 0, 1.5)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 1.5), Point.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.Origin));
            Polygon testPolygon = new Polygon(bounds);

            Plane slicingPlane = new Plane(new Direction(new Angle(new Radian(), 5.6548667765)), Point.MakePointWithInches(122.8315595, 169.313137732, 0));

            List<Polygon> results = testPolygon.Slice(slicingPlane);

            //create the expected planes to compare to
            List<LineSegment> expected1Bounds = new List<LineSegment>();
            expected1Bounds.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(240, 0, 0)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.MakePointWithInches(240, 0, 1.5)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 1.5), Point.MakePointWithInches(0, 0, 1.5)));
            expected1Bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 1.5), Point.Origin));
            Polygon expected1 = new Polygon(expected1Bounds);

            //should only return the original plane
            results.Count.Should().Be(1);
            (results[0] == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_DoesContainPointAlongSides()
        {
            //think messes up due to percision error
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            Polygon testPolygon = new Polygon(lineSegments);

            Point pointOn = Point.MakePointWithInches(-1.5, 0, 1.5);
            Point anotherPointOn = Point.MakePointWithInches(.5, 1.5, 1);
            Point pointNotOn = Point.MakePointWithInches(-2, 0, 1.5);

            bool resultOn = testPolygon.DoesContainPointAlongSides(pointOn);
            bool resultAnotherOn = testPolygon.DoesContainPointAlongSides(anotherPointOn);
            bool resultNotOn = testPolygon.DoesContainPointAlongSides(pointNotOn);

            resultOn.Should().BeTrue();
            resultAnotherOn.Should().BeTrue();
            resultNotOn.Should().BeFalse();
        }

        //[Test()]
        //public void Polygon_FindVertexNotOnTheGivenPlane()
        //{
        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
        //    lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 2, 0)));
        //    lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.Origin));
        //    Polygon testPolygon = new Polygon(lineSegments);

        //    Plane borderPlane = new Plane(Point.Origin, Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(0, 0, 1));
        //    Plane containingPlane = new Plane(Point.Origin, Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(1, 0, 0));

        //    Point results1 = testPolygon.FindVertexNotOnTheGivenPlane(borderPlane);
        //    Point results2 = testPolygon.FindVertexNotOnTheGivenPlane(containingPlane);

        //    (results1 == Point.MakePointWithInches(-3, 2, 0)).Should().BeTrue();
        //    (results2 == null).Should().BeTrue();
        //}

        [Test()]
        public void Polygon_DoesShareOrContainSide()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.Origin));
            Polygon testPolygon = new Polygon(lineSegments);

            List<LineSegment> lineSegments2 = new List<LineSegment>();
            lineSegments2.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-7, 1, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-7, 1, 0), Point.Origin));
            Polygon testExactSide = new Polygon(lineSegments2);

            List<LineSegment> lineSegments3 = new List<LineSegment>();
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(1, -2, 0), Point.MakePointWithInches(0, 3, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(1, -2, 0), Point.MakePointWithInches(0, 1, 0)));
            Polygon testOverlappingSide = new Polygon(lineSegments3);

            List<LineSegment> lineSegments4 = new List<LineSegment>();
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(1, 1, 0), Point.MakePointWithInches(-1, 3, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(2, -3, 0), Point.MakePointWithInches(-1, 3, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(2, -3, 0), Point.MakePointWithInches(1, 1, 0)));
            Polygon testIntersecting = new Polygon(lineSegments4);

            bool resultsExact = testPolygon.DoesShareOrContainSide(testExactSide);
            bool resultsOverlapping = testPolygon.DoesShareOrContainSide(testOverlappingSide);
            bool resultsIntersecting = testPolygon.DoesShareOrContainSide(testIntersecting);

            resultsExact.Should().BeTrue();
            resultsOverlapping.Should().BeFalse();
            resultsIntersecting.Should().BeFalse();
        }

        [Test()]
        public void Polygon_IntersectWithLine()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.Origin));
            Polygon testPolygon = new Polygon(lineSegments);

            Line testIntersect = new Line(Point.MakePointWithInches(-1.5, 1, 0), Point.MakePointWithInches(-3, -2, 1));
            Line testIntersectSide = new Line(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(2, -1, 1));
            Line noIntersect = new Line(Point.MakePointWithInches(-4, 1, 0), Point.MakePointWithInches(-5, -2, 1));

            Point intersect = testPolygon.IntersectWithLine(testIntersect);
            Point intersectSide = testPolygon.IntersectWithLine(testIntersectSide);
            Point none = testPolygon.IntersectWithLine(noIntersect);

            (intersect == Point.MakePointWithInches(-1.5, 1, 0)).Should().BeTrue();
            (intersectSide == Point.MakePointWithInches(0, 1, 0)).Should().BeTrue();
            (none == null).Should().BeTrue();
        }

        [Test()]
        public void Polygon_DoesIntersectNotCoplanar()
        {
            Point front1 = Point.Origin;
            Point front2 = Point.MakePointWithInches(4, 0, 0);
            Point front3 = Point.MakePointWithInches(4, 0, 2);
            Point front4 = Point.MakePointWithInches(0, 0, 2);

            Point back1 = Point.MakePointWithInches(0, 12, 0);
            Point back2 = Point.MakePointWithInches(4, 12, 0);
            Point back3 = Point.MakePointWithInches(4, 12, 2);
            Point back4 = Point.MakePointWithInches(0, 12, 2);

            Polygon frontFace = new Polygon(new List<Point> { front1, front2, front3, front4 });
            Polygon backFace = new Polygon(new List<Point> { back1, back2, back3, back4 });

            Line intersecting1 = new Line(Point.MakePointWithInches(2, 0, 1), Point.MakePointWithInches(2, 1, 1));
            Line intersecting2 = new Line(Point.MakePointWithInches(2, 0, .5), Point.MakePointWithInches(5, 12, 1));
            Line intersectingAlongSide = new Line(Point.Origin, Point.MakePointWithInches(0, 1, 0));
            Line noIntersect = new Line(Point.MakePointWithInches(5, 0, 0), Point.MakePointWithInches(5, 1, 0));


            frontFace.DoesIntersect(intersecting1).Should().BeTrue();
            backFace.DoesIntersect(intersecting1).Should().BeTrue();

        }

        [Test()]
        public void Polygon_DoesContainLineSegment()
        {
            Point point1 = Point.MakePointWithInches(0, 0);
            Point point2 = Point.MakePointWithInches(2, 0);
            Point point3 = Point.MakePointWithInches(2, 1);
            Point point4 = Point.MakePointWithInches(1, 1);
            Point point5 = Point.MakePointWithInches(1, 2);
            Point point6 = Point.MakePointWithInches(2, 2);
            Point point7 = Point.MakePointWithInches(2, 3);
            Point point8 = Point.MakePointWithInches(0, 3);
            List<Point> vertices = new List<Point>(){point1, point2, point3, point4, point5, point6, point7, point8};
            Polygon cShape = new Polygon(vertices);

            Point basePoint1 = Point.MakePointWithInches(1, 0);
            Point endPoint1 = Point.MakePointWithInches(1, 3);
            LineSegment segmentTouchesInsideEdge = new LineSegment(basePoint1, endPoint1);

            Point basePoint2 = Point.MakePointWithInches(0, 0);
            Point endPoint2 = Point.MakePointWithInches(2, 2);
            LineSegment segmentCutsThroughVertices = new LineSegment(basePoint2, endPoint2);

            Point basePoint3 = Point.MakePointWithInches(1.5, 0);
            Point endPoint3 = Point.MakePointWithInches(.5, 4);
            LineSegment segmentCutsThroughVertexAndEdge = new LineSegment(basePoint3, endPoint3);

            Point basePoint4 = Point.MakePointWithInches(1.5, 0);
            Point endPoint4 = Point.MakePointWithInches(1.5, 3);
            LineSegment segmentCutsThroughEdges = new LineSegment(basePoint4, endPoint4);

            Point basePoint5 = Point.MakePointWithInches(0.5, 0);
            Point endPoint5 = Point.MakePointWithInches(0.5, 3);
            LineSegment segmentIsChord = new LineSegment(basePoint5, endPoint5);


            cShape.Contains(segmentTouchesInsideEdge).Should().BeTrue();
            cShape.Contains(segmentCutsThroughVertices).Should().BeFalse();
            cShape.Contains(segmentCutsThroughVertexAndEdge).Should().BeFalse();
            cShape.Contains(segmentIsChord).Should().BeTrue();
        }

        [Test()]
        public void Polygon_Rectangle_Constructor()
        {
            LineSegment testSegment = new LineSegment(new Vector(Point.Origin, Direction.Left, new Distance(1, Inches)));
            testSegment = testSegment.Rotate(new Rotation(Line.ZAxis, new Angle(33, Degrees)));
            Rectangle myRectangle = new Rectangle(testSegment, new Distance(1, Inches));

            Area myArea = myRectangle.Area;
            (myArea == new Area(new SquareInch(), 1)).Should().BeTrue();
            myRectangle.IsRectangle().Should().BeTrue();

        }

        [Test]
        public void Polygon_RemovePolygon()
        {
            var rectangle1 = Polygon.Rectangle(new Distance(3, Inches), new Distance(1, Inches));
            var rectangle2 = Polygon.Rectangle(new Distance(1, Inches), new Distance(3, Inches), Point.MakePointWithInches(1, -1));
            var polyList = new List<Polygon>();
            polyList.Add(rectangle2);
            var results = rectangle1.RemoveOverlappingPolygons(polyList);

            var expected1 = Polygon.Square(new Distance(1, Inches));
            var expected2 = Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(2, 0));
            (results.Count).Should().Be(2);
            (results[0] == expected1).Should().BeTrue();
            (results[1] == expected2).Should().BeTrue();
        }

        [Test]
        public void Polygon_RemovePolygon_SharedEdges()
        {
            var rectangle = Polygon.Rectangle(new Distance(3, Inches), new Distance(1, Inches));
            var square = Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(1, 0));
            var polyToRemove = new List<Polygon>();
            polyToRemove.Add(square);
            var results = rectangle.RemoveOverlappingPolygons(polyToRemove);

            var expected1 = Polygon.Square(new Distance(1, Inches));
            var expected2 = Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(2, 0));
            (results.Contains(expected1)).Should().BeTrue();
            (results.Contains(expected2)).Should().BeTrue();
        }
        [Test]
        public void Polygon_RemovePolygons_SharedEdgesAndEndpoint()
        {
            var rectangle = Polygon.Rectangle(new Distance(3, Inches), new Distance(1, Inches));
            var square = Polygon.Square(new Distance(1, Inches));
            var polyToRemove = new List<Polygon>();
            polyToRemove.Add(square);
            var results = rectangle.RemoveOverlappingPolygons(polyToRemove);

            var expected = Polygon.Rectangle(new Distance(2, Inches), new Distance(1, Inches), Point.MakePointWithInches(1, 0));
            (results.Contains(expected)).Should().BeTrue();
            
        }
        [Test]
        public void Polygon_RemovePolygons_VM_6b()
        {
            //this will be a test of the top plate from VM_6b
            var point1=new Point(41*Distance.Inches, 25.874*Distance.Inches);
            var point2 = new Point(45 * Distance.Inches, 25.874 * Distance.Inches);
            var point3 = new Point(45 * Distance.Inches, 29.874 * Distance.Inches);
            var point4 = new Point(41 * Distance.Inches, 29.874 * Distance.Inches);

            var point5 = new Point(41.25 * Distance.Inches, 25.874 * Distance.Inches);
            var point6 = new Point(44.75 * Distance.Inches, 25.874 * Distance.Inches);
            var point7 = new Point(44.75 * Distance.Inches, 26.562 * Distance.Inches);
            var point8 = new Point(43 * Distance.Inches, 27.875 * Distance.Inches);
            var point9 = new Point(41.25 * Distance.Inches, 26.562 * Distance.Inches);

            var point10 = new Point(41 * Distance.Inches, 26.375 * Distance.Inches);
            var point11 = new Point(45 * Distance.Inches, 26.375 * Distance.Inches);
            var point12 = new Point(43 * Distance.Inches, 29.874 * Distance.Inches);

            var platePoints =new List<Point>();
            platePoints.Add(point1);
            platePoints.Add(point2);
            platePoints.Add(point3);
            platePoints.Add(point4);

            var area1Points = new List<Point>();
            area1Points.Add(point5);
            area1Points.Add(point6);
            area1Points.Add(point7);
            area1Points.Add(point8);
            area1Points.Add(point9);

            var area2Points = new List<Point>();
            area2Points.Add(point10);
            area2Points.Add(point8);
            area2Points.Add(point12);
            area2Points.Add(point4);

            var area3Points = new List<Point>();
            area3Points.Add(point8);
            area3Points.Add(point11);
            area3Points.Add(point3);
            area3Points.Add(point12);
            var area1 = new Polygon(area1Points);
            var area2 =new Polygon(area2Points);
            var area3 = new Polygon(area3Points);
            var plate = new Polygon(platePoints);
            var polygons = new List<Polygon>();
            var plates = new List<Polygon>();
            polygons.Add(area2);
            polygons.Add(area3);
            polygons.Add(area1);
           
            plates.Add(plate);
            var results = plate.RemoveOverlappingPolygons(polygons);
            results.Count.Should().Be(2);

            var leftover1points = new List<Point>();
            leftover1points.Add(point2);
            leftover1points.Add(point11);
            leftover1points.Add(point7);
            leftover1points.Add(point6);

            var leftover1 = new Polygon(leftover1points);
            leftover1.Should().Be(results[0]);

            var leftover2points = new List<Point>();
            leftover2points.Add(point1);
            leftover2points.Add(point5);
            leftover2points.Add(point9);
            leftover2points.Add(point10);

            var leftover2 =new Polygon(leftover2points);
            leftover2.Should().Be(results[1]);

           

        }

        [Test]
        public void Polygon_RemovePolygons_CutThroughCorners()
        {
            var square = Polygon.Square(new Distance(4, Inches));
            var rectangle = Polygon.Rectangle(new Distance(4 * Math.Sqrt(2), Inches), new Distance(1, Inches));
            rectangle = rectangle.Rotate(new Rotation(new Angle(45, Degrees)));
            rectangle = rectangle.Shift(square.CenterPoint - rectangle.CenterPoint);
            var polyToRemove = new List<Polygon>();
            polyToRemove.Add(rectangle);
            var results = square.RemoveOverlappingPolygons(polyToRemove);
           // var results = square.RemoveRegion(rectangle);

            results.Count.Should().Be(2);
            var expected1 = Polygon.Triangle(new Vector(Direction.Right, new Distance(3.29289, Inches)), new Vector(Direction.Down, new Distance(3.29289, Inches)), Point.MakePointWithInches(0, 4));
            var expected2 = Polygon.Triangle(new Vector(Direction.Left, new Distance(3.29289, Inches)), new Vector(Direction.Up, new Distance(3.29289, Inches)), Point.MakePointWithInches(4, 0));

            (results.Contains(expected1)).Should().BeTrue();
            (results.Contains(expected2)).Should().BeTrue();

        }
        [Test]
        public void Polygon_RemovePolygons_SquareRemoveDiamond()
        {
            var square1 = Polygon.Square(new Distance(4, Inches));
            var square2 = Polygon.Square(new Distance(4 / Math.Sqrt(2), Inches));
            //var diamond = square2.Rotate(new Rotation(RightAngle / 2));
            var point1=new Point(Distance.Inches, 2.0, 0);
            var point2= new Point(Distance.Inches, 4.0, 2.0);
            var point3= new Point(Distance.Inches, 2.0, 4.0);
            var point4 = new Point(Distance.Inches, 0, 2.0);
            var diamondList = new List<Point>();
            diamondList.Add(point1);
            diamondList.Add(point2);
            diamondList.Add(point3);
            diamondList.Add(point4);
           // diamond = diamond.Translate(square1.CenterPoint - diamond.CenterPoint);
           var diamond=new Polygon(diamondList);
            var polyToRemove = new List<Polygon>();
            polyToRemove.Add(diamond);
            var results = square1.RemoveOverlappingPolygons(polyToRemove);
            //var results = square1.RemoveRegion(diamond);

            results.Count.Should().Be(4);

            Distance twoInches = new Distance(2, Inches);
            var expected1 = Polygon.Triangle(new Vector(Direction.Right, twoInches), new Vector(Direction.Up, twoInches));
            var expected2 = Polygon.Triangle(new Vector(Direction.Right, twoInches), new Vector(Direction.Down, twoInches), Point.MakePointWithInches(0, 4));
            var expected3 = Polygon.Triangle(new Vector(Direction.Left, twoInches), new Vector(Direction.Down, twoInches), Point.MakePointWithInches(4, 4));
            var expected4 = Polygon.Triangle(new Vector(Direction.Left, twoInches), new Vector(Direction.Up, twoInches), Point.MakePointWithInches(4, 0));

            (results.Contains(expected1)).Should().BeTrue();
            (results.Contains(expected2)).Should().BeTrue();
            (results.Contains(expected3)).Should().BeTrue();
            (results.Contains(expected4)).Should().BeTrue();
        }

        [Test]
        public void Polygon_RemovePolygon_TouchingVertices()
        {
            // The shapes touch at the upper right vertex.
            // There's no overlapping region, 
            // so the results should be just one square or the other

            var square1 = Polygon.Square(3 * new Distance(1, Inches));
            var square2 = Polygon.Square(3 * new Distance(1, Inches));
            square2 = square2.Rotate(new Rotation(65 * new Angle(1, Degrees)));
            square2 = square2.Translate(Point.MakePointWithInches(3, 3));

            var polyToRemove1 = new List<Polygon>();
            polyToRemove1.Add(square1);
            

            var polyToRemove2 = new List<Polygon>();
            polyToRemove2.Add(square2);

            var results1 = square1.RemoveOverlappingPolygons(polyToRemove2);
            var results2 = square2.RemoveOverlappingPolygons(polyToRemove1);
            var results3 = square1.RemoveOverlappingPolygons(polyToRemove1);

            (results1.Count == 1).Should().BeTrue();
            (results2.Count == 1).Should().BeTrue();

            (results1[0] == square1).Should().BeTrue();
            (results2[0] == square2).Should().BeTrue();

            (results3.Count == 0).Should().BeTrue();
        }


        [Test]
        public void Polygon_OverlappingPolygons()
        {
            var initialRectangle = Polygon.Rectangle(2 * new Distance(1, Inches), 3 * new Distance(1, Inches));
            var square = Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(1, 1));
            var squareList = new List<Polygon>();
            squareList.Add(square);
            var cShape = initialRectangle.RemoveOverlappingPolygons(squareList)[0];
            var rectangle = Polygon.Rectangle(new Distance(1, Inches), 3 * new Distance(1, Inches), Point.MakePointWithInches(1, 0));

            var results = cShape.OverlappingPolygons(rectangle);

            results.Count.Should().Be(2);
            results.Contains(Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(1, 2))).Should().BeTrue();
            results.Contains(Polygon.Square(new Distance(1, Inches), Point.MakePointWithInches(1, 0))).Should().BeTrue();

        }


        [Test]
        public void Polygon_OverlappingPolygons_NoOverlap()
        {
            var rectangle1 = Polygon.Rectangle(new Distance(3, Inches), new Distance(2, Inches));
            var rectangle2 = rectangle1.Translate(Point.MakePointWithInches(3, 0));

            var results = rectangle1.OverlappingPolygons(rectangle2);

            results.Count.Should().Be(0);
        }

        [Test]
        public void Contains_Piece_Of_Segment_Test()
        {
            var point1 = Point.MakePointWithInches(460.10509398461357, 22.280925985117292);
            var point2 = Point.MakePointWithInches(452.59860725706864, 23.532015269343788);

            var point3 = Point.MakePointWithInches(459.83140878334285, 20.805720582794379);
            var point4 = Point.MakePointWithInches(455.8858333001466, 21.463317804808426);
            var point5 = Point.MakePointWithInches(456.3790312166571, 24.422499417205621);
            var point6 = Point.MakePointWithInches(460.32460669985335, 23.764902195191574);

            LineSegment testLineSegment=new LineSegment(point1, point2);
            List<Point> polygonPoints=new List<Point>();
            polygonPoints.Add(point3);
            polygonPoints.Add(point4);
            polygonPoints.Add(point5);
            polygonPoints.Add(point6);

            Polygon poly=new Polygon(polygonPoints);

            var test = poly.ContainedPieceOfSegment(testLineSegment);
           
            Math.Round(test.Length.ValueInInches, 1).ShouldBeEquivalentTo(4.0);
        }

        [Test]
        public void Contains_Piece_Of_Segment_Test2()
        {
            var point1 = Point.MakePointWithInches(288, 48.9);
            var point2 = Point.MakePointWithInches(194.06, 1.75);

            var point3 = Point.MakePointWithInches(195.8, 5.5);
            var point4 = Point.MakePointWithInches(286.25, 52.65);
            var point5 = Point.MakePointWithInches(286.25, 48.7);
            var point6 = Point.MakePointWithInches(199.6, 3.5);
            var point7 = Point.MakePointWithInches(195.8, 3.5);

            LineSegment testLineSegment = new LineSegment(point1, point2);
            List<Point> polygonPoints = new List<Point>();
            polygonPoints.Add(point3);
            polygonPoints.Add(point4);
            polygonPoints.Add(point5);
            polygonPoints.Add(point6);
            polygonPoints.Add(point7);

            Polygon poly = new Polygon(polygonPoints);

            var test = poly.ContainedPieceOfSegment(testLineSegment);

            Math.Round(test.Length.ValueInInches, 2).ShouldBeEquivalentTo(60.77);
        }
    }
}