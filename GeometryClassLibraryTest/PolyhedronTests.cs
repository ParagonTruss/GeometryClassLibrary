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
            Shift ninetyShift = new Shift(new Vector(PointGenerator.MakePointWithInches(8, 0)), new Rotation(Line.ZAxis, new Angle(AngleType.Degree, -90)));
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
            Shift nintyShift = new Shift(new Vector(), new Rotation(Line.XAxis, new Angle(AngleType.Degree, 90)));
            Polyhedron result = polyhedron.Shift(nintyShift);

            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(4, 0, 0))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 0), PointGenerator.MakePointWithInches(4, 0, 8))).Should().BeTrue();
            result.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 0, 8), PointGenerator.MakePointWithInches(0, 0, 8))).Should().BeTrue();
        }

        [Test()]
        public void Polyhedron_ReturnToOriginalTest()
        {
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees towards z
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Rotation zRotation = new Rotation(Line.ZAxis, zAngle);
            Angle xAngle = new Angle(AngleType.Degree, 90); //This is the X axis
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Shift ninetyShift = new Shift(displacementVector, new List<Rotation>() {zRotation, xRotation});
            Polyhedron shifted = Polyhedron.Shift(ninetyShift);

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
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            //Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

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
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 5));
            //Angle rotationAngle = new Angle(); //No rotation, just displacement
            Shift shift = new Shift(displacementVector);

            //Move 3 in. in y direction
            Vector displacementVector2 = new Vector(PointGenerator.MakePointWithInches(0, 3, 0));
            //Angle rotationAngle2 = new Angle(); //No rotation, just displacement
            Shift shift2 = new Shift(displacementVector2);

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
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches( 1, -2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue(); //from y axis to z axis
        
        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateAndTranslate_ThenReturnToOriginal()
        {
            //messes up due to percision errors
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 63);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 1));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

            Polyhedron s2 = s.Shift(ninetyShift.Negate());

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); 
        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate()
        {
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, -90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(-1, 2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(4, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, 3, 5), PointGenerator.MakePointWithInches(4, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(4, -5, 5), PointGenerator.MakePointWithInches(0, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, -5, 5), PointGenerator.MakePointWithInches(0, 3, 5))).Should().BeTrue(); 

        }

        [Test()]
        public void Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate_ThenReturnToOriginal()
        {
            Polyhedron Polyhedron = new Polyhedron();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Line testAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 0));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

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

            Plane slicingPlane = new Plane(PointGenerator.MakePointWithInches(1, 0, 0), new Vector(PointGenerator.MakePointWithInches(1,0,0)));

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
    }
}
