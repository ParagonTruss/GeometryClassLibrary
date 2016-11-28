using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class PolyhedronTests
    {
        [Test()]
        public void Polyhedron_ShiftXY()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees towards x
            Shift ninetyShift = new Shift(new Rotation(Line.ZAxis, -1 * Angle.RightAngle), Point.MakePointWithInches(8, 0));
            Polyhedron result = polyhedron.Shift(ninetyShift);

            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, 0), Point.MakePointWithInches(16, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, 0), Point.MakePointWithInches(8, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(16, 0), Point.MakePointWithInches(16, -4))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, -4), Point.MakePointWithInches(16, -4))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_ShiftYZ()
        {
            Polyhedron polyhedron = new TestRectangularBox2();


            //rotate 90 degrees towards z
            Shift nintyShift = new Shift(new Rotation(Line.XAxis, Angle.RightAngle));
            Polyhedron result = polyhedron.Shift(nintyShift);

            result.LineSegments.Contains(new LineSegment(Point.Origin, Point.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.Origin, Point.MakePointWithInches(4, 0, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 8), Point.MakePointWithInches(0, 0, 8))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_MultiShiftReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();


            //rotate 90 degrees towards z
            Angle zAngle = Angle.RightAngle;
            Rotation zRotation = new Rotation(Line.ZAxis, zAngle);
            Angle xAngle = Angle.RightAngle; //This is the X axis
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Shift ninetyShift = new Shift(new List<Rotation>() { zRotation, xRotation });
            Polyhedron shifted = polyhedron.Shift(ninetyShift);

            //undo the previous shift
            Polyhedron s = new Polyhedron(shifted.Shift(ninetyShift.Inverse()));

            s.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Shift_RotationOnly()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = Angle.RightAngle;
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);

            Polyhedron s = polyhedron.Shift(xRotation);

            s.LineSegments.Contains(new LineSegment(Point.Origin, Point.MakePointWithInches(4, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 8), Point.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, 0, 8), Point.Origin)).Should().BeTrue(); //from y axis to z axis

            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, -3, 0), Point.MakePointWithInches(4, -3, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, -3, 0), Point.MakePointWithInches(4, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, -3, 8), Point.MakePointWithInches(0, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, -3, 8), Point.MakePointWithInches(0, -3, 0))).Should().BeTrue(); //from y axis to z axis

            s.LineSegments.Contains(new LineSegment(Point.Origin, Point.MakePointWithInches(0, -3, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, -3, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(4, 0, 8), Point.MakePointWithInches(4, -3, 8))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, 0, 8), Point.MakePointWithInches(0, -3, 8))).Should().BeTrue(); //from y axis to z axis


        }
        
        [Test()]
        public void Polyhedron_Shift_TranslationOnly()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //Move 5 in. in z direction
            Point displacementPoint = Point.MakePointWithInches(0, 0, 5);

            Point displacementPoint2 = Point.MakePointWithInches(0, 3, 0);

            Polyhedron s1 = polyhedron.Shift(displacementPoint);
            Polyhedron s2 = s1.Shift(displacementPoint2);


            s2.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, 3, 5), Point.MakePointWithInches(0, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, 3, 5), Point.MakePointWithInches(8, 3, 5)));
            s2.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, 3, 5), Point.MakePointWithInches(8, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, 7, 5), Point.MakePointWithInches(0, 3, 9)));
        }

        [Test()]
        public void Polyhedron_Shift_RotateAndTranslate()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = Angle.RightAngle;
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = Point.MakePointWithInches(1, -2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(1, -2, 5), Point.MakePointWithInches(5, -2, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(5, -2, 5), Point.MakePointWithInches(5, -2, 13))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(5, -2, 13), Point.MakePointWithInches(1, -2, 13))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(1, -2, 13), Point.MakePointWithInches(1, -2, 5))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_Shift_RotateAndTranslate_ThenReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(new Degree(), 63);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Point displacementPoint = Point.MakePointWithInches(0, 0, 1);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Inverse());

            s2.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Shift_RotateNotThroughOriginAndTranslate()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(new Degree(), -90);
            Line testAxis = new Line(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = Point.MakePointWithInches(-1, 2, 5);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, 3, 5), Point.MakePointWithInches(8, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, 3, 5), Point.MakePointWithInches(8, -1, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(8, -1, 5), Point.MakePointWithInches(0, -1, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(Point.MakePointWithInches(0, -1, 5), Point.MakePointWithInches(0, 3, 5))).Should().BeTrue();

        }

        [Test()]
        public void Polyhedron_Shift_RotateNotThroughOriginAndTranslate_ThenReturnToOriginal()
        {
            Polyhedron polyhedron = new TestRectangularBox2();

            //rotate 90 degrees toward z
            Angle xAngle = Angle.RightAngle;
            Line testAxis = new Line(Point.MakePointWithInches(1, 0, 0), Point.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Point displacementPoint = Point.MakePointWithInches(1, 3, -4);
            Shift ninetyShift = new Shift(xRotation, displacementPoint);

            Polyhedron s = new Polyhedron(polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Inverse());

            s2.Should().Be(polyhedron);
        }

        [Test()]
        public void Polyhedron_Slice_Across()
        {
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(new Direction(Point.MakePointWithInches(1, 0, 0)), Point.MakePointWithInches(1, 0, 0));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedBottom = Point.MakePointWithInches(1, 0, 0);
            Point slicedTop = Point.MakePointWithInches(1, 12, 0);
            Point slicedBottomBack = Point.MakePointWithInches(1, 0, 2);
            Point slicedTopBack = Point.MakePointWithInches(1, 12, 2);

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
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(new Direction(Point.MakePointWithInches(1, 1, 0)), Point.MakePointWithInches(1, 0, 0));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedBottom = Point.MakePointWithInches(1, 0, 0);
            Point slicedTop = Point.MakePointWithInches(0, 1, 0);
            Point slicedBottomBack = Point.MakePointWithInches(1, 0, 2);
            Point slicedTopBack = Point.MakePointWithInches(0, 1, 2);

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
            Point bottomLeft = Point.Origin;
            Point topLeft = Point.MakePointWithInches(0, 12, 0);
            Point bottomRight = Point.MakePointWithInches(4, 0, 0);
            Point topRight = Point.MakePointWithInches(4, 12, 0);

            Point bottomLeftBack = Point.MakePointWithInches(0, 0, 2);
            Point topLeftBack = Point.MakePointWithInches(0, 12, 2);
            Point bottomRightBack = Point.MakePointWithInches(4, 0, 2);
            Point topRightBack = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            planes.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            planes.Add(new Polygon(new List<Point> { topLeft, topRight, topRightBack, topLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, bottomRight, bottomRightBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topLeftBack, bottomLeftBack }));
            planes.Add(new Polygon(new List<Point> { bottomRight, topRight, topRightBack, bottomRightBack }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            //make our slices
            Plane slicingPlane1 = new Plane(new Direction(Point.MakePointWithInches(1, 0, 0)), Point.MakePointWithInches(1, 0, 0));
            Plane slicingPlane2 = new Plane(new Direction(Point.MakePointWithInches(1, 1, 0)), Point.MakePointWithInches(2, 0, 0));
            List<Plane> multiSlices = new List<Plane> { slicingPlane1, slicingPlane2 };

            List<Polyhedron> results = testPolyhedron.Slice(multiSlices);

            //make our results
            //from first slice
            Point sliced1Bottom = Point.MakePointWithInches(1, 0, 0);
            Point sliced1Top = Point.MakePointWithInches(1, 12, 0);
            Point sliced1BottomBack = Point.MakePointWithInches(1, 0, 2);
            Point sliced1TopBack = Point.MakePointWithInches(1, 12, 2);

            //from seceond slice
            Point sliced2Bottom = Point.MakePointWithInches(2, 0, 0);
            Point sliced2Top = Point.MakePointWithInches(0, 2, 0);
            Point sliced2BottomBack = Point.MakePointWithInches(2, 0, 2);
            Point sliced2TopBack = Point.MakePointWithInches(0, 2, 2);

            //from where the two slice lines intersect
            Point sliced12 = Point.MakePointWithInches(1, 1, 0);
            Point sliced12Back = Point.MakePointWithInches(1, 1, 2);

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
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Plane slicingPlane = new Plane(Point.MakePointWithInches(4, 12, 0), Point.MakePointWithInches(0, 8, 0), Point.MakePointWithInches(4, 12, 2));

            List<Polyhedron> results = testPolyhedron.Slice(slicingPlane);

            //make our results
            Point slicedPoint = Point.MakePointWithInches(0, 8, 0);
            Point slicedPointBack = Point.MakePointWithInches(0, 8, 2);

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
            Point bottomPoint1 = Point.Origin;
            Point bottomPoint2 = Point.MakePointWithInches(0, 12, 0);
            Point bottomPoint3 = Point.MakePointWithInches(4, 12, 0);
            Point bottomPoint4 = Point.MakePointWithInches(4, 0, 0);

            Point topPoint1 = Point.MakePointWithInches(0, 0, 2);
            Point topPoint2 = Point.MakePointWithInches(0, 12, 2);
            Point topPoint3 = Point.MakePointWithInches(4, 12, 2);
            Point topPoint4 = Point.MakePointWithInches(4, 0, 2);

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
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);

            Point pointOn = Point.MakePointWithInches(0, 4, 0);
            Point anotherPointOn = Point.MakePointWithInches(2, 0, 0);
            Point pointNotOn = Point.MakePointWithInches(1, 4, 0);

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
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

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


        [Test()]
        public void Convex_Polyhedron_Contains_Point()
        {
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);
            Point testPoint = Point.MakePointWithInches(1, 1, 1);
            testPolyhedron.Contains(testPoint).Should().BeTrue();
            Point distantPoint = Point.MakePointWithInches(20, 20, 20);
            testPolyhedron.Contains(distantPoint).Should().BeFalse();
            Point edgePoint = Point.MakePointWithInches(4, 12, 2);
            testPolyhedron.Contains(edgePoint).Should().BeTrue();
            Point closePoint = Point.MakePointWithInches(4.1, 12.1, 2.1);
            testPolyhedron.Contains(closePoint).Should().BeFalse();
        }
        [Test()]
        public void Concave_Polyhedron_Contains_Point()
        {
            Point basePoint1 = Point.MakePointWithInches(0, 0);
            Point basePoint2 = Point.MakePointWithInches(4, 0);
            Point basePoint3 = Point.MakePointWithInches(2, 2);
            Point basePoint4 = Point.MakePointWithInches(4, 4);
            Point basePoint5 = Point.MakePointWithInches(0, 4);
            Point topPoint1 = Point.MakePointWithInches(0, 0,4);
            Point topPoint2 = Point.MakePointWithInches(4, 0,4);
            Point topPoint3 = Point.MakePointWithInches(2, 2,4);
            Point topPoint4 = Point.MakePointWithInches(4, 4,4);
            Point topPoint5 = Point.MakePointWithInches(0, 4,4);
            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4}));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5}));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5}));
            Polyhedron testPolyhedron = new Polyhedron(planes);
            testPolyhedron.IsConvex.Should().BeFalse();
            Point testpoint = Point.MakePointWithInches(3, 1.1, 4);
            testPolyhedron.Contains(testpoint).Should().BeFalse();
            Point distantPoint = Point.MakePointWithInches(20, 20, 20);
            testPolyhedron.Contains(distantPoint).Should().BeFalse();
            Point edgePoint = Point.MakePointWithInches(4, 4, 4);
            testPolyhedron.Contains(edgePoint).Should().BeTrue();
            Point closePoint = Point.MakePointWithInches(4.1, 4.1, 4.1);
            testPolyhedron.Contains(closePoint).Should().BeFalse();
        }
        [Test()]
        public void Convex_Polyhedron_Contains_Polyhedron()
        {
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron outerPolyhedron = new Polyhedron(planes);
            basePoint = Point.Origin;
            topLeftPoint = Point.MakePointWithInches(0, 6, 0);
            bottomRightPoint = Point.MakePointWithInches(2, 0, 0);
            topRightPoint = Point.MakePointWithInches(2, 6, 0);

            backbasepoint = Point.MakePointWithInches(0, 0, 1);
            backtopleftpoint = Point.MakePointWithInches(0, 6, 1);
            backbottomrightpoint = Point.MakePointWithInches(2, 0, 1);
            backtoprightpoint = Point.MakePointWithInches(2, 6, 1);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron innerPolyhedron = new Polyhedron(planes);
           
            basePoint = Point.Origin;
            topLeftPoint = Point.MakePointWithInches(0, 24, 0);
            bottomRightPoint = Point.MakePointWithInches(2, 0, 0);
            topRightPoint = Point.MakePointWithInches(2, 24, 0);

            backbasepoint = Point.MakePointWithInches(0, 0, 1);
            backtopleftpoint = Point.MakePointWithInches(0, 24, 1);
            backbottomrightpoint = Point.MakePointWithInches(2, 0, 1);
            backtoprightpoint = Point.MakePointWithInches(2, 24, 1);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron largePolyhedron = new Polyhedron(planes);

            outerPolyhedron.Contains(innerPolyhedron).Should().BeTrue();
            outerPolyhedron.Contains(largePolyhedron).Should().BeFalse();
        }
        [Test()]
        public void Concave_Polyhedron_Contains_Polyhedron()
        {
            Point basePoint1 = Point.MakePointWithInches(0, 0);
            Point basePoint2 = Point.MakePointWithInches(4, 0);
            Point basePoint3 = Point.MakePointWithInches(2, 2);
            Point basePoint4 = Point.MakePointWithInches(4, 4);
            Point basePoint5 = Point.MakePointWithInches(0, 4);
            Point topPoint1 = Point.MakePointWithInches(0, 0, 4);
            Point topPoint2 = Point.MakePointWithInches(4, 0, 4);
            Point topPoint3 = Point.MakePointWithInches(2, 2, 4);
            Point topPoint4 = Point.MakePointWithInches(4, 4, 4);
            Point topPoint5 = Point.MakePointWithInches(0, 4, 4);
            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron outerPolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(2, 2);
            basePoint4 = Point.MakePointWithInches(4, 4);
            basePoint5 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 2);
            topPoint2 = Point.MakePointWithInches(4, 0, 2);
            topPoint3 = Point.MakePointWithInches(2, 2, 2);
            topPoint4 = Point.MakePointWithInches(4, 4, 2);
            topPoint5 = Point.MakePointWithInches(0, 4, 2);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron innerPolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(2, 2);
            basePoint4 = Point.MakePointWithInches(4, 4);
            basePoint5 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 6);
            topPoint2 = Point.MakePointWithInches(4, 0, 6);
            topPoint3 = Point.MakePointWithInches(2, 2, 6);
            topPoint4 = Point.MakePointWithInches(4, 4, 6);
            topPoint5 = Point.MakePointWithInches(0, 4, 6);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron largePolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(4, 4);
            basePoint4 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 4);
            topPoint2 = Point.MakePointWithInches(4, 0, 4);
            topPoint3 = Point.MakePointWithInches(4, 4, 4);
            topPoint4 = Point.MakePointWithInches(0, 4, 4);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint1, topPoint1, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4}));
            Polyhedron convexPolyhedron = new Polyhedron(planes);
            outerPolyhedron.Contains(innerPolyhedron).Should().BeTrue();
            outerPolyhedron.Contains(largePolyhedron).Should().BeFalse();
            outerPolyhedron.Contains(convexPolyhedron).Should().BeFalse();
        }

        [Test()]
        public void addPolygonToCorrectPolyhedronTest()
        {
            Point basePoint1 = Point.MakePointWithInches(0, 0);
            Point basePoint2 = Point.MakePointWithInches(4, 0);
            Point basePoint3 = Point.MakePointWithInches(2, 2);
            Point basePoint4 = Point.MakePointWithInches(4, 4);
            Point basePoint5 = Point.MakePointWithInches(0, 4);
            Point topPoint1 = Point.MakePointWithInches(0, 0, 4);
            Point topPoint2 = Point.MakePointWithInches(4, 0, 4);
            Point topPoint3 = Point.MakePointWithInches(2, 2, 4);
            Point topPoint4 = Point.MakePointWithInches(4, 4, 4);
            Point topPoint5 = Point.MakePointWithInches(0, 4, 4);
            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron outerPolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(2, 2);
            basePoint4 = Point.MakePointWithInches(4, 4);
            basePoint5 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 2);
            topPoint2 = Point.MakePointWithInches(4, 0, 2);
            topPoint3 = Point.MakePointWithInches(2, 2, 2);
            topPoint4 = Point.MakePointWithInches(4, 4, 2);
            topPoint5 = Point.MakePointWithInches(0, 4, 2);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron innerPolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(2, 2);
            basePoint4 = Point.MakePointWithInches(4, 4);
            basePoint5 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 6);
            topPoint2 = Point.MakePointWithInches(4, 0, 6);
            topPoint3 = Point.MakePointWithInches(2, 2, 6);
            topPoint4 = Point.MakePointWithInches(4, 4, 6);
            topPoint5 = Point.MakePointWithInches(0, 4, 6);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 }));
            Polyhedron largePolyhedron = new Polyhedron(planes);
            basePoint1 = Point.MakePointWithInches(0, 0);
            basePoint2 = Point.MakePointWithInches(4, 0);
            basePoint3 = Point.MakePointWithInches(4, 4);
            basePoint4 = Point.MakePointWithInches(0, 4);
            topPoint1 = Point.MakePointWithInches(0, 0, 4);
            topPoint2 = Point.MakePointWithInches(4, 0, 4);
            topPoint3 = Point.MakePointWithInches(4, 4, 4);
            topPoint4 = Point.MakePointWithInches(0, 4, 4);
            planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4 }));
            planes.Add(new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 }));
            planes.Add(new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 }));
            planes.Add(new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 }));
            planes.Add(new Polygon(new List<Point> { basePoint4, basePoint1, topPoint1, topPoint4 }));
            planes.Add(new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4 }));
            Polyhedron convexPolyhedron = new Polyhedron(planes);
            outerPolyhedron.Contains(innerPolyhedron).Should().BeTrue();
            outerPolyhedron.Contains(largePolyhedron).Should().BeFalse();
            outerPolyhedron.Contains(convexPolyhedron).Should().BeFalse();
        }
    }
}
