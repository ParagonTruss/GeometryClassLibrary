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
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 4, 0), PointGenerator.MakePointWithMillimeters(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 4, 0), PointGenerator.MakePointWithMillimeters(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            //Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 4), PointGenerator.MakePointWithMillimeters(0, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 4), PointGenerator.MakePointWithMillimeters(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

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
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 4, 0), PointGenerator.MakePointWithMillimeters(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 4, 0), PointGenerator.MakePointWithMillimeters(0, 0, 0)));
            Polyhedron.Polygons.Add(new Polygon(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, -90);
            Line testAxis = new Line(PointGenerator.MakePointWithMillimeters(1, 0, 0), PointGenerator.MakePointWithMillimeters(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithMillimeters(-1, 2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Polyhedron s = new Polyhedron(Polyhedron.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 5), PointGenerator.MakePointWithMillimeters(4, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 5), PointGenerator.MakePointWithMillimeters(4, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(4, -5, 5), PointGenerator.MakePointWithMillimeters(0, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -5, 5), PointGenerator.MakePointWithMillimeters(0, 3, 5))).Should().BeTrue(); 

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
    }
}
