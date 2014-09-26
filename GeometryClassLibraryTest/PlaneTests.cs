using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class PlaneTests
    {
        [Test()]
        public void Plane_ContainsLine()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 3, 0), PointGenerator.MakePointWithInches(4, -2, 0));
            Line testLine = new Line(PointGenerator.MakePointWithInches(2, 0, 0), PointGenerator.MakePointWithInches(0, -1, 0));
            Line testFalseLine = new Line(PointGenerator.MakePointWithInches(1, 2, -2), PointGenerator.MakePointWithInches(2, 4, 3));

            bool result = testPlane.Contains(testLine);
            bool result2 = testPlane.Contains(testFalseLine);

            result.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_ContainsLineOnOrigin()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 3, 0), PointGenerator.MakePointWithInches(-3, -1, 0));
            Line testLine = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 1, 0));

            bool result = testPlane.Contains(testLine);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPolygon()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(1, 0, 0), PointGenerator.MakePointWithInches(1, -3, 4), PointGenerator.MakePointWithInches(1, 2, 2));
            Polygon testRegion = new Polygon(new List<Point>
                {
                    PointGenerator.MakePointWithInches(1,1,1),
                    PointGenerator.MakePointWithInches(1,2,1),
                    PointGenerator.MakePointWithInches(1,2,2),
                    PointGenerator.MakePointWithInches(1,1,2)
                });

            bool result = testPlane.Contains(testRegion);

            result.Should().BeTrue();
        }

        [Test()]
        public void Plane_ContainsPoint()
        {
            Plane testPlane = new Plane(PointGenerator.MakePointWithInches(0,1,0), PointGenerator.MakePointWithInches(2,1,-3), PointGenerator.MakePointWithInches(-4,1,-1));
            Point testPoint1 = PointGenerator.MakePointWithInches(0, 1, 5);
            Point testPoint2 = PointGenerator.MakePointWithInches(2, 2, 2);

            bool result1 = testPlane.Contains(testPoint1);
            bool result2 = testPlane.Contains(testPoint2);

            result1.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Test()]
        public void Plane_RotateTest()
        {
            Point testBasePoint = PointGenerator.MakePointWithInches(1, 1, -1);            
            Vector testNormalVector = new Vector(PointGenerator.MakePointWithInches(0, 2, 3), PointGenerator.MakePointWithInches(-3, -2, 0));

            Plane testPlane = new Plane(testBasePoint, testNormalVector);

            Line rotationAxis = new Line(PointGenerator.MakePointWithInches(1, -1, -1), new Vector(PointGenerator.MakePointWithInches(1, 1, 1)));
            Angle rotationAngle = new Angle(AngleType.Degree, 212);

            Plane actualResult = testPlane.Rotate(rotationAxis, rotationAngle);

            Point expectedPoint = PointGenerator.MakePointWithInches(2.8439301238119032, -1.4640641282085687, -0.37986599560333495);
            Vector expectedVector = new Vector(PointGenerator.MakePointWithInches(5.23819525861547, 1.681697053112619, -1.91989231172809), PointGenerator.MakePointWithInches(1.3162301967095191, -1.0862708827830958, -5.2299593139264218));
            Plane expectedResult = new Plane(expectedPoint, expectedVector);

            (actualResult == expectedResult).Should().BeTrue();
        }

        [Test()]
        public void Plane_PointOnSameSideAs()
        {
            Point testPoint = PointGenerator.MakePointWithInches(1, 3, -1);
            Point testPoint2 = PointGenerator.MakePointWithInches(-1, -2, 5);
            Point testPoint3 = PointGenerator.MakePointWithInches(0, 1, 0);
            Point referencePoint = PointGenerator.MakePointWithInches(1, 2, 1);
            Point referencePoint2 = PointGenerator.MakePointWithInches(0, 2, 1);
            Vector testNormalVector = new Vector(PointGenerator.MakePointWithInches(1, 0, 0));

            Plane testPlane = new Plane(PointGenerator.MakePointWithMillimeters(0,0,0), testNormalVector);

            testPlane.PointIsOnSameSideAs(testPoint, referencePoint).Should().BeTrue();
            testPlane.PointIsOnSameSideAs(testPoint2, referencePoint).Should().BeFalse();
            testPlane.PointIsOnSameSideAs(testPoint3, referencePoint).Should().BeFalse();
            testPlane.PointIsOnSameSideAs(testPoint, referencePoint2).Should().BeFalse();


            Point testPointOffOrigin = PointGenerator.MakePointWithInches(5, 4, 0);
            Point testPointOffOrigin2 = PointGenerator.MakePointWithInches(-2, -2, -1);
            Point testPointOffOrigin3 = PointGenerator.MakePointWithInches(1, -4, 2);
            Point referencePointOffOrigin = PointGenerator.MakePointWithInches(1, 2, 3);
            Vector testNormalVectorOffOrigin = new Vector(PointGenerator.MakePointWithInches(-1, 2, 1));

            Plane testPlaneOffOrigin = new Plane(PointGenerator.MakePointWithMillimeters(1, -4, 2), testNormalVectorOffOrigin);

            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin, referencePointOffOrigin).Should().BeTrue();
            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin2, referencePointOffOrigin).Should().BeFalse();
            testPlaneOffOrigin.PointIsOnSameSideAs(testPointOffOrigin3, referencePointOffOrigin).Should().BeFalse();

        }
    }
}
