using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitClassLibrary;
using GeometryClassLibrary;
namespace ClearspanTypeLibrary.Tests
{
    [TestClass()]
    public class SolidTests
    {
        //[TestMethod()]
        //public void Solid_ShiftXYTest()
        //{
        //    Solid Solid = new Solid();

        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(0, 4)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0), PointGenerator.MakePointWithInches(8, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0), PointGenerator.MakePointWithInches(8, 4)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4), PointGenerator.MakePointWithInches(0, 4)));
        //    Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

        //    //rotate 90 degrees towards x
        //    Shift ninetyShift = new Shift(new Vector(), new Angle(AngleType.Degree, -90));
        //    Solid.Shift(ninetyShift, PointGenerator.MakePointWithInches(8, 0));

        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 8), PointGenerator.MakePointWithInches(8, 12))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(12, 0), PointGenerator.MakePointWithInches(8, 12))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(12, 0), PointGenerator.MakePointWithInches(8, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 8), PointGenerator.MakePointWithInches(8, 0))).Should().BeTrue();
        //}

        //[TestMethod()]
        //public void Solid_ShiftYZTest()
        //{
        //    Solid Solid = new Solid();

        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

        //    //rotate 90 degrees towards z
        //    Shift nintyShift = new Shift(new Vector(), new Angle(), new Angle(AngleType.Degree, -90));
        //    Solid.Shift(nintyShift, PointGenerator.MakePointWithInches(8, 0, 0));

        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //}

        //[TestMethod()]
        //public void Solid_ReturnToOriginalTest()
        //{
        //    Solid Solid = new Solid();

        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

        //    //rotate 90 degrees towards z
        //    Shift nintyzShift = new Shift(new Vector(), new Angle(), new Angle(AngleType.Degree, -90));
        //    //rotate 90 degrees towards x
        //    Shift nintyxShift = new Shift(new Vector(), new Angle(AngleType.Degree, -90), new Angle());
        //    Solid.Shift(nintyzShift, PointGenerator.MakePointWithInches(8, 0, 0));
        //    Solid.Shift(nintyxShift, PointGenerator.MakePointWithInches(8, 0, 0));

        //    Solid.ShiftBackToHomeCoordinateSystem();

        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


        //}

        [TestMethod()]
        public void Solid_ReturnToOriginalTest()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees towards z
            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0)); //This is the X axis
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle rotationAngle = new Angle(AngleType.Degree, 90);
            Shift ninetyShift = new Shift(displacementVector, rotationAngle, rotationAxis);
            Solid.Shift(ninetyShift);

            //undo the previous shift
            Solid.Shift(ninetyShift.Negate());

            //rotate 90 degrees towards x
            rotationAxis = new Line(PointGenerator.MakePointWithInches(0, 1, 0)); //This is the Y axis
            displacementVector = new Vector(); //No displacement, just rotation
            rotationAngle = new Angle(AngleType.Degree, 90);
            ninetyShift = new Shift(displacementVector, rotationAngle, rotationAxis);
            Solid.Shift(ninetyShift);

            //undo the previous shift
            Solid.Shift(ninetyShift.Negate());

            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


        }

        [TestMethod()]
        public void Solid_ShiftTest_Orthogonal_RotationOnly()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, 0, 0)); //This is the X axis
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle rotationAngle = new Angle(AngleType.Degree, 90);
            Shift ninetyShift = new Shift(displacementVector, rotationAngle, rotationAxis);

            Solid.Shift(ninetyShift);

            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue(); //no change
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue(); 
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

        }

        [TestMethod()]
        public void Solid_ShiftTest_Orthogonal_TranslationOnly()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 0, 4)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //Move 5 in. in z direction
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 5));
            Angle rotationAngle = new Angle(); //No rotation, just displacement
            Shift ninetyShift = new Shift(displacementVector, rotationAngle, rotationAngle);

            //Move 3 in. in y direction
            Vector displacementVector2 = new Vector(PointGenerator.MakePointWithInches(0, 3, 0));
            Angle rotationAngle2 = new Angle(); //No rotation, just displacement
            Shift ninetyShift2 = new Shift(displacementVector2, rotationAngle2, rotationAngle2);

            Solid.Shift(ninetyShift2);

            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(0, 7, 5)));
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 7, 5), PointGenerator.MakePointWithInches(8, 3, 5)));
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, 7, 5)));
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 7, 5), PointGenerator.MakePointWithInches(0, 3, 9)));
        }

        [TestMethod()]
        public void Solid_ShiftTest_RotateAndTranslate()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(-3, -2, 0), PointGenerator.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(0, 2, 3)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, -1, -1), new Vector(PointGenerator.MakePointWithInches(1, 1, 1)));
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(-10, 4, 6)); 
            Angle rotationAngle = new Angle(AngleType.Degree, 212);
            Shift ninetyShift = new Shift(displacementVector, rotationAngle, rotationAxis);

            Solid.Shift(ninetyShift);

            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(5.2382, 1.6817, -1.9199), PointGenerator.MakePointWithInches(1.3162,-1.0863,-5.23))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1.3162, -1.0863, -5.23), PointGenerator.MakePointWithInches(2.8439,-1.4641,-0.3799))).Should().BeTrue();
            Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(2.8439, -1.4641, -0.3799), PointGenerator.MakePointWithInches(5.2382, 1.6817, -1.9199))).Should().BeTrue();
        }


    }
}
