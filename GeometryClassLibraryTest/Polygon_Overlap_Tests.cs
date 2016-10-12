using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using System.Linq;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class Polygon_Overlap_Tests
    {
        [Test()]
        public void Polygon_OverlappingPolygonOneEnclosedInOtherTest()
        {
            var testPolygon = Polygon.Rectangle(Inches, 
                3, 1, 
                4, 3);

            var testPolygon2 = Polygon.Rectangle(Inches, 0, 0, 12, 4);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);
            intersect1.Equals(intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            intersect1.Equals(testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygonSharedSidesTest()
        {
            var testPolygon = Polygon.Rectangle(Inches,
               0, 0,
               1, 2);

            var testPolygon2 = Polygon.Rectangle(Inches, 
                0, 0, 
                12, 2);

            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);

            (intersect1 == intersect2).Should().BeTrue();

            //the intersection should simply be the smaller plane in this case
            (intersect1 == testPolygon).Should().BeTrue();
        }

        [Test()]
        public void Polygon_OverlappingPolygonIntersectingBoundariesTest()
        {
            var testPolygon = Polygon.CreateInXYPlane(Inches,
             3, 1,
             4, 1,
             4, 5,
             3, 3);

            var testPolygon2 = Polygon.Rectangle(Inches,
                0, 0,
                12, 4);

            var expected = Polygon.CreateInXYPlane(Inches,
                4, 1, 
                4, 4, 
                3.5, 4, 
                3, 3, 
                3, 1);

            //It shouldnt matter which one we use to clip (unless one is concave) so try it both ways
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);
            intersect1.Should().Be(expected);
            intersect1.Should().Be(intersect2);
        }

        [Test()]
        public void Polygon_OverlappingPolygon()
        {
            var testPolygon = Polygon.Rectangle(Inches,
                0, 1,
                4, 3);

            var testPolygon2 = Polygon.CreateInXYPlane(Inches,
                0, -1, 
                3, 5, 
                4, 5, 
                1, -1);

            var expected = Polygon.CreateInXYPlane(Inches, 
                2, 1, 
                3, 3, 
                2, 3, 
                1, 1);
           
            //check to see if its what we expected
            Polygon intersect1 = testPolygon.OverlappingPolygon(testPolygon2);
            Polygon intersect2 = testPolygon2.OverlappingPolygon(testPolygon);
            bool overlapAsExpected1 = (intersect1 == expected);
            bool overlapAsExpected2 = (intersect2 == expected);
            Assert.IsTrue(overlapAsExpected1);
            Assert.IsTrue(overlapAsExpected2);
        }

        [Test()]
        public void Polygon_OverlappingPolygon_ErrorCase_1()
        {
            var polygon1 = Polygon.Rectangle(Inches, 30, 1.5, 0, 0);
            var polygon2 = Polygon.Rectangle(Inches, 3.5, 0, 7, 3.5);

            var result = polygon1.OverlappingPolygon(polygon2);

            var expected = Polygon.Rectangle(Inches, 3.5, 0, 7, 1.5);

            (result == expected).Should().BeTrue();
        }
        [Test()]
        public void Polygon_OverlappingPolygon_ErrorCase2()
        {
            var plate = Polygon.CreateInXYPlane(Inches,
                20, 14,
                16, 14,
                16, 18,
                20, 18);
            var member = Polygon.CreateInXYPlane(Inches,
                214.5, 18,
                214.5, 16.5,
                0, 16.5,
                0, 18);

            var overlap = plate.OverlappingPolygon(member);

            overlap.IsNull().Should().BeFalse();

            var expected = Polygon.Rectangle(Inches, 16, 16.5, 20, 18);


            (overlap == expected).Should().BeTrue();
        }
        //Automatically generated at 8/4/2016 10:36:47 AM
        [Test]
        public void OverlappingPolygon_ErrorCase_CD319ADE()
        {
            //5x3 Plate centered at :(22.5, 19) in, Angle: 46.87°
            var plate = Polygon.CreateInXYPlane(Inches, 25.3038481173636, 19.7990217360978, 21.8855612413269, 16.1500061382802, 19.6961518826364, 18.2009782639022, 23.1144387586731, 21.8499938617198);

            //W2
            var member = Polygon.CreateInXYPlane(Inches, 56.93, 3.5, 22.5, 17.118, 22.5, 19, 24.143, 20.232, 61.688, 5.382, 61.688, 3.5);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);

            var expected = Polygon.CreateInXYPlane(Inches, 22.713, 17.034, 25.285, 19.78, 24.143, 20.232, 22.5, 19, 22.5, 17.118);
            actual.Should().Be(expected);
        }

        //Automatically generated at 8/4/2016 10:36:47 AM
        [Test]
        public void OverlappingPolygon_ErrorCase_CF09090A()
        {
            //4x3 Plate centered at :(62.04, 3.85) in, Angle: 0°
            var plate = Polygon.CreateInXYPlane(Inches, 64.0410533905933, 2.35355339059327, 60.0410533905933, 2.35355339059327, 60.0410533905933, 5.35355339059327, 64.0410533905933, 5.35355339059327);

            //W2
            var member = Polygon.CreateInXYPlane(Inches, 56.93, 3.5, 22.5, 17.118, 22.5, 19, 24.143, 20.232, 61.688, 5.382, 61.688, 3.5);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);

            var expected = Polygon.CreateInXYPlane(Inches, 61.688, 3.5, 61.688, 5.353, 60.041, 5.353, 60.041, 3.5);
            actual.Should().Be(expected);
        }

        //Automatically generated at 8/4/2016 10:36:47 AM
        [Test]
        public void OverlappingPolygon_ErrorCase_CC8639FA()
        {
            //4x3 Plate centered at :(61.33, 3.85) in, Angle: 0°
            var plate = Polygon.CreateInXYPlane(Inches, 63.3339466094067, 2.35355339059327, 59.3339466094067, 2.35355339059327, 59.3339466094067, 5.35355339059327, 63.3339466094067, 5.35355339059327);

            //W2
            var member = Polygon.CreateInXYPlane(Inches, 56.93, 3.5, 22.5, 17.118, 22.5, 19, 24.143, 20.232, 61.688, 5.382, 61.688, 3.5);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);

            var expected = Polygon.CreateInXYPlane(Inches, 61.688, 3.5, 61.688, 5.353, 59.333, 5.353, 59.333, 3.5);
            actual.Should().Be(expected);
        }

        //Automatically generated at 8/5/2016 3:43:56 PM
        [Test]
        public void OverlappingPolygon_ErrorCase_CD8A2BBF()
        {
            //4x3 Plate centered at :(8.08, 3.5) in, Angle: 36.87°
            var plate = Polygon.CreateInXYPlane(Inches, 10.58333, 3.49999972801679, 7.38332973889612, 1.10000007615531, 5.58333000000002, 3.50000027198321, 8.78333026110388, 5.89999992384469);

            //B1
            var member = Polygon.CreateInXYPlane(Inches, 214.833, 3.5, 219.5, 0, 0.5, 0, 5.167, 3.5);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);

        }


        //Automatically generated at 8/5/2016 3:43:56 PM
        [Test]
        public void OverlappingPolygon_ErrorCase_CCABCAF4()
        {
            //4x3 Plate centered at :(211.92, 3.5) in, Angle: 323.13°
            var plate = Polygon.CreateInXYPlane(Inches, 212.616670261099, 1.10000007615396, 209.41667, 3.49999972802163, 211.216669738901, 5.89999992384604, 214.41667, 3.50000027197837);

            //B1
            var member = Polygon.CreateInXYPlane(Inches, 214.833, 3.5, 219.5, 0, 0.5, 0, 5.167, 3.5);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);
        }

        //Automatically generated at 8/5/2016 3:43:56 PM
        [Test]
        public void OverlappingPolygon_ErrorCase_CCAACAC6()
        {
            //4x3 Plate centered at :(211.92, 3.5) in, Angle: 323.13°
            var plate = Polygon.CreateInXYPlane(Inches, 212.616670261099, 1.10000007615396, 209.41667, 3.49999972802163, 211.216669738901, 5.89999992384604, 214.41667, 3.50000027197837);

            //T2
            var member = Polygon.CreateInXYPlane(Inches, 209, 3.49999999999997, 110, 77.75, 110, 82.125, 214.833, 3.49999999999999);

            var actual = plate.OverlappingPolygon(member);
            actual.Should().NotBe(null);
        }
        private static string Coordinates(Polygon polygon)
        {
            return string.Join(", ", polygon.Vertices
                 .SelectMany(point => new[] { point.X.ValueInInches, point.Y.ValueInInches })
                 .Select(d => d.ToString()));
        }

        private string PolygonString(Polygon polygon)
        {
            return $"Polygon.CreateInXYPlane(Distance.Inches, {Coordinates(polygon)})";
        }
    }
}
