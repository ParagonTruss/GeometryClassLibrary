﻿using System;
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
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Rotation zRotation = new Rotation(Line.Z_AXIS, zAngle);
            Angle xAngle = new Angle(AngleType.Degree, 90); //This is the X axis
            Rotation xRotation = new Rotation(Line.X_AXIS, xAngle);
            Shift ninetyShift = new Shift(displacementVector, new List<Rotation>() {zRotation, xRotation});
            Solid shifted = Solid.Shift(ninetyShift);

            //undo the previous shift
            Solid s = new Solid(shifted.Shift(ninetyShift.Negate()));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


        }

        [TestMethod()]
        public void Solid_ShiftTest_RotationOnly()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.X_AXIS, xAngle);
            //Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

        }

        [TestMethod()]
        public void Solid_ShiftTest_TranslationOnly()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //Move 5 in. in z direction
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 5));
            //Angle rotationAngle = new Angle(); //No rotation, just displacement
            Shift shift = new Shift(displacementVector);

            //Move 3 in. in y direction
            Vector displacementVector2 = new Vector(PointGenerator.MakePointWithInches(0, 3, 0));
            //Angle rotationAngle2 = new Angle(); //No rotation, just displacement
            Shift shift2 = new Shift(displacementVector2);

            Solid s1 = Solid.Shift(shift);
            Solid s2 = s1.Shift(shift2);

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(0, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(8, 3, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, 7, 5)));
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 7, 5), PointGenerator.MakePointWithInches(0, 3, 9)));
        }

        [TestMethod()]
        public void Solid_ShiftTest_RotateAndTranslate()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.X_AXIS, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches( 1, -2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue(); //from y axis to z axis
        
        }

        [TestMethod()]
        public void Solid_ShiftTest_RotateAndTranslate_ThenReturnToOriginal()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.X_AXIS, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 1));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            Solid s2 = s.Shift(ninetyShift.Negate());

            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue(); //no change
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis
        }

        [TestMethod()]
        public void Solid_ShiftTest_OffAxisRotateAndTranslate()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(new Line(PointGenerator.MakePointWithInches(0 ,0, 0), PointGenerator.MakePointWithInches(1, 1, 0)), xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(1, -2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue(); //from y axis to z axis

        }





    }
}
