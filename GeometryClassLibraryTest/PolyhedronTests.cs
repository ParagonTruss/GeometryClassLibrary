using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using NUnit.Framework;
using UnitClassLibrary;
using GeometryClassLibrary;
namespace ClearspanTypeLibrary.Tests
{
    [TestFixture()]
    public class PolyhedronTests
    {
        [Test()]
        public void Polyhedron_MakeCoplanarLineSegmentsIntoPolygons()
        {
            List<LineSegment> lineSegments =
                new List<Point> {
                    PointGenerator.MakePointWithInches(0.000,  0.000),
                    PointGenerator.MakePointWithInches(0.000,  0.250),
                    PointGenerator.MakePointWithInches(6.500,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  3.500),
                    PointGenerator.MakePointWithInches(144.000,  0.000)
                }.MakeIntoLineSegmentsThatMeet();

            List<Polygon> polygons = lineSegments.MakeCoplanarLineSegmentsIntoPolygons();

            Polyhedron testPoly = new Polyhedron(polygons);

            testPoly.LineSegments.Should().BeEquivalentTo(lineSegments);
        }

        [Test()]
        public void Polyhedron_ShiftXYTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0), PointGenerator.MakePointWithInches(4, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 8), PointGenerator.MakePointWithInches(0, 8)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees towards x
            Shift ninetyShift = new Shift(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, -90)), PointGenerator.MakePointWithInches(8, 0));
            Polyhedron result = polyhedron.Shift(ninetyShift);

            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0), PointGenerator.MakePointWithInches(16, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0), PointGenerator.MakePointWithInches(8, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(16, 0), PointGenerator.MakePointWithInches(16, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, -4), PointGenerator.MakePointWithInches(16, -4))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_ShiftYZTest()
        {

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0), PointGenerator.MakePointWithInches(4, 8)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 8), PointGenerator.MakePointWithInches(0, 8)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees towards z
            Shift nintyShift = new Shift(new Rotation(Line.XAxis, new Angle(AngleType.Degree, 90)));
            Polyhedron result = polyhedron.Shift(nintyShift);

            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(4, 0, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 8), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_MultiShiftReturnToOriginalTest()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees towards z
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Rotation zRotation = new Rotation(Line.ZAxis, zAngle);
            Angle xAngle = new Angle(AngleType.Degree, 90); //This is the X axis
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Shift ninetyShift = new Shift(new List<Rotation>() { zRotation, xRotation });
            Polyhedron shifted = polyhedron.Shift(ninetyShift);

            //undo the previous shift
            Polyhedron s = new Polyhedron(shifted.Shift(ninetyShift.Negate()));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


        }

        [Test()]
        public void Polyhedron_ShiftTest_RotationOnly()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);

            Shift ninetyShift = new Shift(xRotation);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

        }

        [Test()]
        public void Polyhedron_ShiftTest_TranslationOnly()
        {
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //Move 5 in. in z direction
            Point displacementPoint = PointGenerator.MakePointWithInches(0, 0, 5);
            //Angle rotationAngle = new Angle(); //No rotation, just displacement
            Shift shift = new Shift(displacementPoint);

            //Move 3 in. in y direction
            Point displacementPoint2 = PointGenerator.MakePointWithInches(0, 3, 0);
            //Angle rotationAngle2 = new Angle(); //No rotation, just displacement
            Shift shift2 = new Shift(displacementPoint2);

            Polyhedron s1 = Polyhedron.Shift(shift);
            Polyhedron s2 = s1.Shift(shift2);

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(0, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(8, 3, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 7, 5), PointGenerator.MakePointWithInches(0, 3, 9)));
        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateAndTranslate()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(1, -2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue(); //from y axis to z axis

        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateAndTranslate_ThenReturnToOriginal()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 63);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(0, 0, 1);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Negate());

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, -90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(-1, 2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(4, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 5), PointGenerator.MakePointWithInches(4, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, -5, 5), PointGenerator.MakePointWithInches(0, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, -5, 5), PointGenerator.MakePointWithInches(0, 3, 5))).Should().BeTrue();

        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate_ThenReturnToOriginal()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(1, 3, -4);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Negate());

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_SimpleSlice()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 0, 0)), PointGenerator.MakePointWithInches(1, 0, 0));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedBottom = PointGenerator.MakePointWithInches(1, 0, 0);
            Point slicedTop = PointGenerator.MakePointWithInches(1, 12, 0);
            Point slicedBottomBack = PointGenerator.MakePointWithInches(1, 0, 2);
            Point slicedTopBack = PointGenerator.MakePointWithInches(1, 12, 2);

            List<Polygon> ExpectedPlanes1 = new List<Polygon>();
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, topRightPoint, bottomRightPoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottomBack, slicedTopBack, backtoprightpoint, backbottomrightpoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedTop, topRightPoint, backtoprightpoint, slicedTopBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, bottomRightPoint, backbottomrightpoint, slicedBottomBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, slicedTopBack, slicedBottomBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron ExpectedPolyhedron1 = new Polyhedron(ExpectedPlanes1);

            List<Polygon> ExpectedPlanes2 = new List<Polygon>();
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, slicedTop, slicedBottom }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, slicedTopBack, slicedBottomBack }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeftPoint, slicedTop, slicedTopBack, backtopleftpoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, slicedBottom, slicedBottomBack, backbasepoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, slicedTopBack, slicedBottomBack }));
            Polyhedron ExpectedPolyhedron2 = new Polyhedron(ExpectedPlanes2);

            //now test to see if we got what we expect
            results.Count.Should().Be(2);
            results.Contains(ExpectedPolyhedron1).Should().BeTrue();
            results.Contains(ExpectedPolyhedron2).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_DiagonalSlice()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 1, 0)), PointGenerator.MakePointWithInches(1, 0, 0));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedBottom = PointGenerator.MakePointWithInches(1, 0, 0);
            Point slicedTop = PointGenerator.MakePointWithInches(0, 1, 0);
            Point slicedBottomBack = PointGenerator.MakePointWithInches(1, 0, 2);
            Point slicedTopBack = PointGenerator.MakePointWithInches(0, 1, 2);

            List<Polygon> ExpectedPlanes1 = new List<Polygon>();
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, topLeftPoint, topRightPoint, bottomRightPoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottomBack, slicedTopBack, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, bottomRightPoint, backbottomrightpoint, slicedBottomBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedTop, topLeftPoint, backtopleftpoint, slicedTopBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, slicedTopBack, slicedBottomBack }));
            Polyhedron ExpectedPolyhedron1 = new Polyhedron(ExpectedPlanes1);

            List<Polygon> ExpectedPlanes2 = new List<Polygon>();
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, slicedTop, slicedBottom }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { backbasepoint, slicedTopBack, slicedBottomBack }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, slicedBottom, slicedBottomBack, backbasepoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { basePoint, slicedTop, slicedTopBack, backbasepoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { slicedBottom, slicedTop, slicedTopBack, slicedBottomBack }));
            Polyhedron ExpectedPolyhedron2 = new Polyhedron(ExpectedPlanes2);

            //now test to see if we got what we expect
            results.Count.Should().Be(2);
            results.Contains(ExpectedPolyhedron1).Should().BeTrue();
            results.Contains(ExpectedPolyhedron2).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_MultiSlice()
        {
            Point bottomLeft = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeft = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRight = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRight = PointGenerator.MakePointWithInches(4, 12, 0);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(0, 0, 2);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 12, 2);
            Point bottomRightBack = PointGenerator.MakePointWithInches(4, 0, 2);
            Point topRightBack = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            planes.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            planes.Add(new Polygon(new List<Point> { topLeft, topRight, topRightBack, topLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, bottomRight, bottomRightBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topLeftBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomRight, topRight, topRightBack, bottomRightBack }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            //make our slices
            Plane slicingPlane1 = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 0, 0)), PointGenerator.MakePointWithInches(1, 0, 0));
            Plane slicingPlane2 = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 1, 0)), PointGenerator.MakePointWithInches(2, 0, 0));
            List<Plane> multiSlices = new List<Plane> { slicingPlane1, slicingPlane2 };

            List<Polyhedron> results = testPolyhedron.Slice(multiSlices);

            //make our results
            //from first slice
            Point sliced1Bottom = PointGenerator.MakePointWithInches(1, 0, 0);
            Point sliced1Top = PointGenerator.MakePointWithInches(1, 12, 0);
            Point sliced1BottomBack = PointGenerator.MakePointWithInches(1, 0, 2);
            Point sliced1TopBack = PointGenerator.MakePointWithInches(1, 12, 2);

            //from seceond slice
            Point sliced2Bottom = PointGenerator.MakePointWithInches(2, 0, 0);
            Point sliced2Top = PointGenerator.MakePointWithInches(0, 2, 0);
            Point sliced2BottomBack = PointGenerator.MakePointWithInches(2, 0, 2);
            Point sliced2TopBack = PointGenerator.MakePointWithInches(0, 2, 2);

            //from where the two slice lines intersect
            Point sliced12 = PointGenerator.MakePointWithInches(1, 1, 0);
            Point sliced12Back = PointGenerator.MakePointWithInches(1, 1, 2);

            //largest piece
            List<Polygon> ExpectedPlanes1 = new List<Polygon>();
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced2Bottom, bottomRight, topRight, sliced1Top, sliced12 }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced2BottomBack, bottomRightBack, topRightBack, sliced1TopBack, sliced12Back }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced1Top, sliced1TopBack, topRightBack, topRight }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced2Bottom, sliced2BottomBack, bottomRightBack, bottomRight }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced2Bottom, sliced2BottomBack, sliced12Back, sliced12 }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { sliced12, sliced12Back, sliced1TopBack, sliced1Top }));
            Polyhedron ExpectedPolyhedron1 = new Polyhedron(ExpectedPlanes1);

            //second largest
            List<Polygon> ExpectedPlanes2 = new List<Polygon>();
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeft, sliced1Top, sliced12, sliced2Top }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeftBack, sliced1TopBack, sliced12Back, sliced2TopBack }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeft, topLeftBack, sliced1TopBack, sliced1Top }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { sliced1Top, sliced1TopBack, sliced12Back, sliced12 }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { sliced12, sliced12Back, sliced2TopBack, sliced2Top }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { sliced2Top, sliced2TopBack, topLeftBack, topLeft }));
            Polyhedron ExpectedPolyhedron2 = new Polyhedron(ExpectedPlanes2);

            //third largest
            List<Polygon> ExpectedPlanes3 = new List<Polygon>();
            ExpectedPlanes3.Add(new Polygon(new List<Point> { sliced2Top, sliced12, sliced1Bottom, bottomLeft }));
            ExpectedPlanes3.Add(new Polygon(new List<Point> { sliced2TopBack, sliced12Back, sliced1BottomBack, bottomLeftBack }));
            ExpectedPlanes3.Add(new Polygon(new List<Point> { sliced2Top, sliced2TopBack, sliced12Back, sliced12 }));
            ExpectedPlanes3.Add(new Polygon(new List<Point> { sliced12, sliced12Back, sliced1BottomBack, sliced1Bottom }));
            ExpectedPlanes3.Add(new Polygon(new List<Point> { sliced1Bottom, sliced1BottomBack, bottomLeftBack, bottomLeft }));
            ExpectedPlanes3.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, sliced2TopBack, sliced2Top }));
            Polyhedron ExpectedPolyhedron3 = new Polyhedron(ExpectedPlanes3);

            //smallest triangle piece
            List<Polygon> ExpectedPlanes4 = new List<Polygon>();
            ExpectedPlanes4.Add(new Polygon(new List<Point> { sliced12, sliced1Bottom, sliced2Bottom }));
            ExpectedPlanes4.Add(new Polygon(new List<Point> { sliced12Back, sliced1BottomBack, sliced2BottomBack }));
            ExpectedPlanes4.Add(new Polygon(new List<Point> { sliced12, sliced12Back, sliced1BottomBack, sliced1Bottom }));
            ExpectedPlanes4.Add(new Polygon(new List<Point> { sliced1Bottom, sliced1BottomBack, sliced2BottomBack, sliced2Bottom }));
            ExpectedPlanes4.Add(new Polygon(new List<Point> { sliced2Bottom, sliced2BottomBack, sliced12Back, sliced12 }));
            Polyhedron ExpectedPolyhedron4 = new Polyhedron(ExpectedPlanes4);

            //now test to see if we got what we expect
            results.Count.Should().Be(4);
            results.Contains(ExpectedPolyhedron1).Should().BeTrue();
            results.Contains(ExpectedPolyhedron2).Should().BeTrue();
            results.Contains(ExpectedPolyhedron3).Should().BeTrue();
            results.Contains(ExpectedPolyhedron4).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_SliceAtVertex()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(PointGenerator.MakePointWithInches(4, 12, 0), PointGenerator.MakePointWithInches(0, 8, 0), PointGenerator.MakePointWithInches(4, 12, 2));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedPoint = PointGenerator.MakePointWithInches(0, 8, 0);
            Point slicedPointBack = PointGenerator.MakePointWithInches(0, 8, 2);

            List<Polygon> ExpectedPlanes1 = new List<Polygon>();
            ExpectedPlanes1.Add(new Polygon(new List<Point> { basePoint, slicedPoint, topRightPoint, bottomRightPoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { backbasepoint, slicedPointBack, backtoprightpoint, backbottomrightpoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { slicedPoint, topRightPoint, backtoprightpoint, slicedPointBack }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { basePoint, slicedPoint, slicedPointBack, backbasepoint }));
            ExpectedPlanes1.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron ExpectedPolyhedron1 = new Polyhedron(ExpectedPlanes1);

            List<Polygon> ExpectedPlanes2 = new List<Polygon>();
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, slicedPoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { backtopleftpoint, backtoprightpoint, slicedPointBack }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { backtopleftpoint, slicedPointBack, slicedPoint, topLeftPoint }));
            ExpectedPlanes2.Add(new Polygon(new List<Point> { slicedPoint, slicedPointBack, backtoprightpoint, topRightPoint }));
            Polyhedron ExpectedPolyhedron2 = new Polyhedron(ExpectedPlanes2);

            //now test to see if we got what we expect
            results.Count.Should().Be(2);
            results.Contains(ExpectedPolyhedron1).Should().BeTrue();
            results.Contains(ExpectedPolyhedron2).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_SliceOnSide()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(PointGenerator.MakePointWithInches(4, 12, 0), PointGenerator.MakePointWithInches(0, 12, 0), PointGenerator.MakePointWithInches(4, 12, 2));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Polyhedron expected = new Polyhedron(planes);

            //now test to see if we got what we expect
            results.Count.Should().Be(1);
            results.Contains(expected).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_DoesContainPointAlongSides()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Point pointOn = PointGenerator.MakePointWithInches(0, 4, 0);
            Point anotherPointOn = PointGenerator.MakePointWithInches(2, 0, 0);
            Point pointNotOn = PointGenerator.MakePointWithInches(1, 4, 0);

            bool resultOn = testPolyhedron.DoesContainPointAlongSides(pointOn);
            bool resultAnotherOn = testPolyhedron.DoesContainPointAlongSides(anotherPointOn);
            bool resultNotOn = testPolyhedron.DoesContainPointAlongSides(pointNotOn);

            resultOn.Should().BeTrue();
            resultAnotherOn.Should().BeTrue();
            resultNotOn.Should().BeFalse();
        }

        [Test()]
        public void Polyhedron_Verticies()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            List<Point> results = testPolyhedron.Verticies;

            //now test to see if we got what we expect
            results.Count.Should().Be(8);

            List<Point> expected = new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint, backbasepoint, 
                backtopleftpoint, backtoprightpoint, backbottomrightpoint };

            foreach (var point in expected)
            {
                results.Should().Contain(point);
            }
        }

        [Test()]
        public void Polyhedron_DoesShareOrContainSide()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(4, 4, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 4, 0), PointGenerator.MakePointWithInches(4, 0, 0)));
            bounds.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polygon testFlatSide = new Polygon(bounds);

            List<LineSegment> bounds2 = new List<LineSegment>();
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 2, 0), PointGenerator.MakePointWithInches(4, 6, 0)));
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 2, 0), PointGenerator.MakePointWithInches(6, 5, 0)));
            bounds2.Add(new LineSegment(PointGenerator.MakePointWithInches(4, 6, 0), PointGenerator.MakePointWithInches(6, 5, 0)));
            Polygon testTouches = new Polygon(bounds2);

            List<LineSegment> bounds3 = new List<LineSegment>();
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, -1, 0), PointGenerator.MakePointWithInches(-1, 3, 0)));
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, -1, 0), PointGenerator.MakePointWithInches(-1, 5, 0)));
            bounds3.Add(new LineSegment(PointGenerator.MakePointWithInches(-1, 3, 0), PointGenerator.MakePointWithInches(-1, 5, 0)));
            Polygon testNone = new Polygon(bounds3);

            bool resultFlat = testPolyhedron.DoesShareOrContainSide(testFlatSide);
            bool resultTouches = testPolyhedron.DoesShareOrContainSide(testTouches);
            bool resultNone = testPolyhedron.DoesShareOrContainSide(testNone);

            resultFlat.Should().BeTrue();
            resultTouches.Should().BeTrue();
            resultNone.Should().BeFalse();
        }

        [Test()]
        public void Polyhedron_OverlappingPolygon()
        {
            //make our first polyhedron
            Point bottomLeft = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeft = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRight = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRight = PointGenerator.MakePointWithInches(4, 12, 0);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(0, 0, 2);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 12, 2);
            Point bottomRightBack = PointGenerator.MakePointWithInches(4, 0, 2);
            Point topRightBack = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            planes.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            planes.Add(new Polygon(new List<Point> { topLeft, topRight, topRightBack, topLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, bottomRight, bottomRightBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topLeftBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomRight, topRight, topRightBack, bottomRightBack }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            //and a second one that intersects both sides only partially
            Point bottomLeft2 = PointGenerator.MakePointWithInches(6, 3, 1);
            Point topLeft2 = PointGenerator.MakePointWithInches(6, 6, 1);
            Point bottomRight2 = PointGenerator.MakePointWithInches(4, 3, 1);
            Point topRight2 = PointGenerator.MakePointWithInches(4, 6, 1);

            Point bottomLeftBack2 = PointGenerator.MakePointWithInches(6, 3, 5);
            Point topLeftBack2 = PointGenerator.MakePointWithInches(6, 6, 5);
            Point bottomRightBack2 = PointGenerator.MakePointWithInches(4, 3, 5);
            Point topRightBack2 = PointGenerator.MakePointWithInches(4, 6, 5);

            List<Polygon> planes2 = new List<Polygon>();
            planes2.Add(new Polygon(new List<Point> { bottomLeft2, topLeft2, topRight2, bottomRight2 }));
            planes2.Add(new Polygon(new List<Point> { bottomLeftBack2, topLeftBack2, topRightBack2, bottomRightBack2 }));
            planes2.Add(new Polygon(new List<Point> { topLeft2, topRight2, topRightBack2, topLeftBack2 }));
            planes2.Add(new Polygon(new List<Point> { bottomLeft2, bottomRight2, bottomRightBack2, bottomLeftBack2 }));
            planes2.Add(new Polygon(new List<Point> { bottomLeft2, topLeft2, topLeftBack2, bottomLeftBack2 }));
            planes2.Add(new Polygon(new List<Point> { bottomRight2, topRight2, topRightBack2, bottomRightBack2 }));
            Polyhedron testPolyhedron2 = new Polyhedron(planes2);

            Point intersect12Bottom = PointGenerator.MakePointWithInches(4, 3, 2);
            Point intersect12Top = PointGenerator.MakePointWithInches(4, 6, 2);
            Polygon expected12overlap = new Polygon(new List<Point>() { bottomRight2, topRight2, intersect12Top, intersect12Bottom });

            Polygon overlap12 = testPolyhedron.OverlappingPolygon(testPolyhedron2);
            (overlap12 == expected12overlap).Should().BeTrue();

            //now make a thrid one that shares and end with the first and doesnt overlapp the second
            Point bottomLeft3 = PointGenerator.MakePointWithInches(0, 12, 0);
            Point topLeft3 = PointGenerator.MakePointWithInches(0, 18, 0);
            Point bottomRight3 = PointGenerator.MakePointWithInches(4, 12, 0);
            Point topRight3 = PointGenerator.MakePointWithInches(4, 18, 0);

            Point bottomLeftBack3 = PointGenerator.MakePointWithInches(0, 12, 2);
            Point topLeftBack3 = PointGenerator.MakePointWithInches(0, 18, 2);
            Point bottomRightBack3 = PointGenerator.MakePointWithInches(4, 12, 2);
            Point topRightBack3 = PointGenerator.MakePointWithInches(4, 18, 2);

            Polygon expected13Overlap = new Polygon(new List<Point> { bottomLeft3, bottomRight3, bottomRightBack3, bottomLeftBack3 });
            List<Polygon> planes3 = new List<Polygon>();
            planes3.Add(new Polygon(new List<Point> { bottomLeft3, topLeft3, topRight3, bottomRight3 }));
            planes3.Add(new Polygon(new List<Point> { bottomLeftBack3, topLeftBack3, topRightBack3, bottomRightBack3 }));
            planes3.Add(new Polygon(new List<Point> { topLeft3, topRight3, topRightBack3, topLeftBack3 }));
            planes3.Add(expected13Overlap);
            planes3.Add(new Polygon(new List<Point> { bottomLeft3, topLeft3, topLeftBack3, bottomLeftBack3 }));
            planes3.Add(new Polygon(new List<Point> { bottomRight3, topRight3, topRightBack3, bottomRightBack3 }));
            Polyhedron testPolyhedron3 = new Polyhedron(planes3);

            Polygon overlap13 = testPolyhedron.OverlappingPolygon(testPolyhedron3);
            (overlap13 == expected13Overlap).Should().BeTrue();

            Polygon overlap23 = testPolyhedron2.OverlappingPolygon(testPolyhedron3);
            (overlap23 == null).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_AllIntersectingPoints()
        {
            //make a polyhedron
            Point bottomLeft = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeft = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRight = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRight = PointGenerator.MakePointWithInches(4, 12, 0);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(0, 0, 2);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 12, 2);
            Point bottomRightBack = PointGenerator.MakePointWithInches(4, 0, 2);
            Point topRightBack = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            planes.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            planes.Add(new Polygon(new List<Point> { topLeft, topRight, topRightBack, topLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, bottomRight, bottomRightBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topLeftBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomRight, topRight, topRightBack, bottomRightBack }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            //now make some lines that will intersect it
            Line intersecting1 = new Line(PointGenerator.MakePointWithInches(2, 0, 1), PointGenerator.MakePointWithInches(2, 1, 1));
            Line intersecting2 = new Line(PointGenerator.MakePointWithInches(1, 0, .5), PointGenerator.MakePointWithInches(5, 12, 1.5));
            Line intersectingAlongSide = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 1, 0));
            Line noIntersect = new Line(PointGenerator.MakePointWithInches(5, 0, 0), PointGenerator.MakePointWithInches(5, 1, 0));

            List<Point> points1 = testPolyhedron.FindAllIntersectionPoints(intersecting1);
            List<Point> points2 = testPolyhedron.FindAllIntersectionPoints(intersecting2);
            List<Point> pointsAlongSide = testPolyhedron.FindAllIntersectionPoints(intersectingAlongSide);
            List<Point> pointsNone = testPolyhedron.FindAllIntersectionPoints(noIntersect);

            (points1.Count == 2).Should().BeTrue();
            points1.Contains(PointGenerator.MakePointWithInches(2, 0, 1)).Should().BeTrue();
            points1.Contains(PointGenerator.MakePointWithInches(2, 12, 1)).Should().BeTrue();

            (points2.Count == 2).Should().BeTrue();
            points2.Contains(PointGenerator.MakePointWithInches(1, 0, 0.5)).Should().BeTrue();
            points2.Contains(PointGenerator.MakePointWithInches(4, 9, 1.25)).Should().BeTrue();

            (pointsAlongSide.Count == 2).Should().BeTrue();
            pointsAlongSide.Contains(PointGenerator.MakePointWithInches(0, 0, 0)).Should().BeTrue();
            pointsAlongSide.Contains(PointGenerator.MakePointWithInches(0, 12, 0)).Should().BeTrue();

            (pointsNone.Count == 0).Should().BeTrue();
        }
    }
}
