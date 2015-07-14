using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnitClassLibrary;
using GeometryClassLibrary;
using GeometryStubLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PolyhedronTests
    {
        [Test()]
        public void Polyhedron_ShiftXY()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees towards x
            Shift ninetyShift = new Shift(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, -90)), PointGenerator.MakePointWithInches(8, 0));
            Polyhedron result = polyhedron.Shift(ninetyShift);

            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0), PointGenerator.MakePointWithInches(16, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0), PointGenerator.MakePointWithInches(8, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(16, 0), PointGenerator.MakePointWithInches(16, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, -4), PointGenerator.MakePointWithInches(16, -4))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_ShiftYZ()
        {
            Polyhedron polyhedron = new TestRectangularBox2();


            //rotate 90 degrees towards z
            Shift nintyShift = new Shift(new Rotation(Line.XAxis, new Angle(AngleType.Degree, 90)));
            Polyhedron result = polyhedron.Shift(nintyShift);

            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(4, 0, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 8), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_MultiShiftReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();


            //rotate 90 degrees towards z
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Rotation zRotation = new Rotation(Line.ZAxis, zAngle);
            Angle xAngle = new Angle(AngleType.Degree, 90); //This is the X axis
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Shift ninetyShift = new Shift(new List<Rotation>() { zRotation, xRotation });
            Polyhedron shifted = polyhedron.Shift(ninetyShift);

            //undo the previous shift
            Polyhedron s = new Polyhedron(shifted.Shift(ninetyShift.Negate()));

            s.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Shift_RotationOnly()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);

            Shift ninetyShift = new Shift(xRotation);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(4, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 8), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 8), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, -3, 0), PointGenerator.MakePointWithInches(4, -3, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, -3, 0), PointGenerator.MakePointWithInches(4, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, -3, 8), PointGenerator.MakePointWithInches(0, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, -3, 8), PointGenerator.MakePointWithInches(0, -3, 0))).Should().BeTrue(); //from y axis to z axis

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, -3, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, -3, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 8), PointGenerator.MakePointWithInches(4, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 8), PointGenerator.MakePointWithInches(0, -3, 8))).Should().BeTrue(); //from y axis to z axis


        }
        
        [Test()]
        public void Polyhedron_Shift_TranslationOnly()
        {
            Polyhedron polyhedron = new TestRectangularBox2();


            //Move 5 in. in z direction
            Point displacementPoint = PointGenerator.MakePointWithInches(0, 0, 5);
            //Angle rotationAngle = new Angle(); //No rotation, just displacement
            Shift shift = new Shift(displacementPoint);

            //Move 3 in. in y direction
            Point displacementPoint2 = PointGenerator.MakePointWithInches(0, 3, 0);
            //Angle rotationAngle2 = new Angle(); //No rotation, just displacement
            Shift shift2 = new Shift(displacementPoint2);

            Polyhedron s1 = polyhedron.Shift(shift);
            Polyhedron s2 = s1.Shift(shift2);

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(0, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(8, 3, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 7, 5), PointGenerator.MakePointWithInches(0, 3, 9)));
        }

        [Test()]
        public void Polyhedron_Shift_RotateAndTranslate()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(1, -2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(5, -2, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(5, -2, 5), PointGenerator.MakePointWithInches(5, -2, 13))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(5, -2, 13), PointGenerator.MakePointWithInches(1, -2, 13))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 13), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_Shift_RotateAndTranslate_ThenReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 63);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(0, 0, 1);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Negate());

            s2.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Shift_RotateNotThroughOriginAndTranslate()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, -90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(-1, 2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(8, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, -1, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, -1, 5), PointGenerator.MakePointWithInches(0, -1, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, -1, 5), PointGenerator.MakePointWithInches(0, 3, 5))).Should().BeTrue();

        }

        [Test()]
        public void Polyhedron_Shift_RotateNotThroughOriginAndTranslate_ThenReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = PointGenerator.MakePointWithInches(1, 3, -4);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Negate());

            s2.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Slice_Across()
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
        public void Polyhedron_Slice_Diagonal()
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
            //results.Contains(ExpectedPolyhedron1).Should().BeTrue();
            results.Contains(ExpectedPolyhedron2).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_Slice_Multiple()
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
        public void Polyhedron_SliceThroughOppositeEdges()
        {
            Point bottomPoint1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point bottomPoint2 = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomPoint3 = PointGenerator.MakePointWithInches(4, 12, 0);
            Point bottomPoint4 = PointGenerator.MakePointWithInches(4, 0, 0);

            Point topPoint1 = PointGenerator.MakePointWithInches(0, 0, 2);
            Point topPoint2 = PointGenerator.MakePointWithInches(0, 12, 2);
            Point topPoint3 = PointGenerator.MakePointWithInches(4, 12, 2);
            Point topPoint4 = PointGenerator.MakePointWithInches(4, 0, 2);

            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomPoint1, bottomPoint2, bottomPoint3, bottomPoint4 }));
            faces.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4}));
            faces.Add(new Polygon(new List<Point> { bottomPoint1, topPoint1, topPoint2, bottomPoint2}));
            faces.Add(new Polygon(new List<Point> { bottomPoint2, topPoint2, topPoint3, bottomPoint3 }));
            faces.Add(new Polygon(new List<Point> { bottomPoint3, topPoint3, topPoint4, bottomPoint4 }));
            faces.Add(new Polygon(new List<Point> { bottomPoint4, topPoint4, topPoint1, bottomPoint1 }));
            Polyhedron testPolyhedron = new Polyhedron(faces);

            Plane slicingPlane = new Plane(bottomPoint1, bottomPoint4, topPoint2);
            
            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);


            List<Polygon> polygons1 = new List<Polygon>();
            polygons1.Add(new Polygon(new List<Point> { bottomPoint1, bottomPoint2, bottomPoint3, bottomPoint4 }));
            polygons1.Add(new Polygon(new List<Point> { bottomPoint2, bottomPoint3, topPoint3, topPoint2 }));
            polygons1.Add(new Polygon(new List<Point> { bottomPoint4, bottomPoint1, topPoint2, topPoint3 }));
            polygons1.Add(new Polygon(new List<Point> { bottomPoint1, bottomPoint2, topPoint2 }));
            polygons1.Add(new Polygon(new List<Point> { bottomPoint3, bottomPoint4, topPoint3 }));
            Polyhedron expected1 = new Polyhedron(polygons1);

            List<Polygon> polygons2 = new List<Polygon>();
            polygons2.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4 }));
            polygons2.Add(new Polygon(new List<Point> { topPoint4, topPoint1, bottomPoint1, bottomPoint4 }));
            polygons2.Add(new Polygon(new List<Point> { bottomPoint4, bottomPoint1, topPoint2, topPoint3 }));
            polygons2.Add(new Polygon(new List<Point> { topPoint1, topPoint2, bottomPoint1 }));
            polygons2.Add(new Polygon(new List<Point> { topPoint3, topPoint4, bottomPoint4 }));
            Polyhedron expected2 = new Polyhedron(polygons2);
            
            //now test to see if we got what we expect
            results.Count.Should().Be(2);
            results.Contains(expected1).Should().BeTrue();
            results.Contains(expected2).Should().BeTrue();
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
        public void Polyhedron_Vertices()
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

            List<Point> results = testPolyhedron.Vertices;

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
        public void Polyhedron_Volume_Tetrahedron()
        {
            TestTetrahedron testTetrahedron = new TestTetrahedron();
            Volume volume = ((Polyhedron)testTetrahedron).Volume;
            volume.Should().Be(testTetrahedron.Volume);
        }

        [Test()]
        public void Polyhedron_Volume_RectangularBox()
        {
            TestRectangularBox1 testBox = new TestRectangularBox1();
            Volume volume = ((Polyhedron) testBox).Volume;
            volume.Should().Be(testBox.Volume);
        }

        [Test()]
        public void Polyhedron_Volume_ConcavePentagonalPrism()
        {
            TestConcavePentagonalPrism testSolid = new TestConcavePentagonalPrism();
            Volume volume = ((Polyhedron) testSolid).Volume;
            volume.Should().Be(testSolid.Volume);
        }

        [Test()]
        public void Polyhedron_Centroid_Tetrahedron()
        {
            TestTetrahedron testTetrahedron = new TestTetrahedron();
            Point centroid = ((Polyhedron) testTetrahedron).Centroid;
            centroid.Should().Be(testTetrahedron.Centroid);
        }

        [Test()]
        public void Polyhedron_Centroid_RectangularBox()
        {
            TestRectangularBox1 testBox = new TestRectangularBox1();
            Point centroid = ((Polyhedron) testBox).Centroid;
            centroid.Should().Be(testBox.Centroid);
        }

        [Test()]
        public void Polyhedron_Centroid_ConcavePentagonalPrism()
        {
            TestConcavePentagonalPrism testSolid = new TestConcavePentagonalPrism();
            Point centroid = ((Polyhedron) testSolid).Centroid;
            centroid.Should().Be(testSolid.Centroid);
        }

        [Test()]
        public void Polyhedron_IsConvex_ConcavePentagonalPrism()
        {
            Polyhedron testSolid = new TestConcavePentagonalPrism();

            testSolid.IsConvex.Should().BeFalse();
        }

        [Test()]
        public void Polyhedron_IsConvex_RectangularBox()
        {
            Polyhedron testBox = new TestRectangularBox1();

            testBox.IsConvex.Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_IsConvex_Tetrahedron()
        {
            Polyhedron testTetrahedron = new TestTetrahedron();

            testTetrahedron.IsConvex.Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_IsConvex_Decahedron()
        {
            Polyhedron testDecahedron = new ConcaveDecahedron();

            testDecahedron.IsConvex.Should().BeFalse();
        }
    }
}
