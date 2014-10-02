﻿using System;
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
    public class SolidTests
    {
        //[Test()]
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


        //[Test()]
        //public void Solid_ShiftYZTest()
        //{
        //    Solid Solid = new Solid();

        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

        //    Line rotationLine = new Line (PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0));

        //    //rotate 90 degrees towards z
        //    Shift nintyShift = new Shift(new Vector(), new Rotation(rotationLine, new Angle(AngleType.Degree, -90)));
        //    Solid.Shift(nintyShift);

        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //}


        //[Test()]
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
        //    Solid.Shift(nintyzShift);
        //    Solid.Shift(nintyxShift);

        //    Solid.ShiftBackToHomeCoordinateSystem();

        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
        //    Solid.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


        //}

       
        [Test()]
        public void Solid_ShiftTest_RotationOnly()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 4, 0), PointGenerator.MakePointWithMillimeters(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 4, 0), PointGenerator.MakePointWithMillimeters(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle xAngle = new Angle(AngleType.Degree, 90);
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            //Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 4), PointGenerator.MakePointWithMillimeters(0, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 4), PointGenerator.MakePointWithMillimeters(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

        }

        [Test()]
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

        [Test()]
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
            Rotation xRotation = new Rotation(Line.XAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches( 1, -2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 5), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 5))).Should().BeTrue(); //from y axis to z axis
        
        }

        //[Test()]
        //public void Solid_ShiftTest_RotateAndTranslate_ThenReturnToOriginal()
        //{
        //    Solid Solid = new Solid();

        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0)));
        //    lineSegments.Add(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0)));
        //    Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

        //    //rotate 90 degrees toward z
        //    Angle xAngle = new Angle(AngleType.Degree, 63);
        //    Rotation xRotation = new Rotation(Line.XAxis, xAngle);
        //    Vector displacementVector = new Vector(PointGenerator.MakePointWithInches(0, 0, 1));
        //    Shift ninetyShift = new Shift(displacementVector, xRotation);

        //    Solid s = new Solid(Solid.Shift(ninetyShift));

        //    Solid s2 = s.Shift(ninetyShift.Negate());

        //    s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
        //    s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
        //    s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
        //    s2.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 4, 0), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); 
        //}


        [Test()]
        public void Solid_ShiftTest_RotateNotThroughOriginAndTranslate()
        {
            Solid Solid = new Solid();

            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(8, 0, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 0, 0), PointGenerator.MakePointWithMillimeters(8, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(8, 4, 0), PointGenerator.MakePointWithMillimeters(0, 4, 0)));
            lineSegments.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 4, 0), PointGenerator.MakePointWithMillimeters(0, 0, 0)));
            Solid.PlaneRegions.Add(new PlaneRegion(lineSegments));

            //rotate 90 degrees toward z
            Angle xAngle = new Angle(AngleType.Degree, -90);
            Line testAxis = new Line(PointGenerator.MakePointWithMillimeters(1, 0, 0), PointGenerator.MakePointWithMillimeters(1, 0, 1));
            Rotation xRotation = new Rotation(testAxis, xAngle);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithMillimeters(-1, 2, 5));
            Shift ninetyShift = new Shift(displacementVector, xRotation);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, 3, 5), PointGenerator.MakePointWithMillimeters(4, 3, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(4, 3, 5), PointGenerator.MakePointWithMillimeters(4, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(4, -5, 5), PointGenerator.MakePointWithMillimeters(0, -5, 5))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(0, -5, 5), PointGenerator.MakePointWithMillimeters(0, 3, 5))).Should().BeTrue(); 

        }

    }
}
