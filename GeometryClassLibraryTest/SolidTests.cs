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
            Angle xzAngle = new Angle(AngleType.Degree, 90); //This is the X axis
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Shift ninetyShift = new Shift(displacementVector, zAngle, xzAngle);
            Solid.Shift(ninetyShift);

            //undo the previous shift
            Solid s = new Solid (Solid.Shift(ninetyShift.Negate()));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 4, 0))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 4, 0), PointGenerator.MakePointWithInches(0, 4, 0))).Should().BeTrue();


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
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Vector displacementVector = new Vector(); //No displacement, just rotation
            Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, zAngle, xzAngle);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(8, 0, 0))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 0), PointGenerator.MakePointWithInches(8, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 0, 4), PointGenerator.MakePointWithInches(0, 0, 4))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 0, 4), PointGenerator.MakePointWithInches(0, 0, 0))).Should().BeTrue(); //from y axis to z axis

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

            Solid s = new Solid (Solid.Shift(ninetyShift2));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 3, 5), PointGenerator.MakePointWithInches(0, 7, 5)));
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(0, 7, 5), PointGenerator.MakePointWithInches(8, 3, 5)));
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 3, 5), PointGenerator.MakePointWithInches(8, 7, 5)));
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(8, 7, 5), PointGenerator.MakePointWithInches(0, 3, 9)));
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
            Angle zAngle = new Angle(AngleType.Degree, 90);
            Vector displacementVector = new Vector(PointGenerator.MakePointWithInches( 1, -2, 5)); //No displacement, just rotation
            Angle xzAngle = new Angle(AngleType.Degree, 0);
            Shift ninetyShift = new Shift(displacementVector, zAngle, xzAngle);

            Solid s = new Solid(Solid.Shift(ninetyShift));

            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(9, -2, 5))).Should().BeTrue(); //no change
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 5), PointGenerator.MakePointWithInches(9, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(9, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue();
            s.LineSegments.Contains(new LineSegment(PointGenerator.MakePointWithInches(1, -2, 9), PointGenerator.MakePointWithInches(1, -2, 9))).Should().BeTrue(); //from y axis to z axis
        
        }


    }
}
