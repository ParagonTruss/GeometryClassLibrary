using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class CoordinateSystemTests
    {
        [Test]
        public void CoordinateSystem_JSON()
        {
            CoordinateSystem coordinateSystem = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 90));

            var json = JsonConvert.SerializeObject(coordinateSystem);
            CoordinateSystem deserializedCoordinateSystem = JsonConvert.DeserializeObject<CoordinateSystem>(json);

            bool areEqual = (coordinateSystem == deserializedCoordinateSystem);
            areEqual.Should().BeTrue();
        }

        [Test]
        public void CoordinateSystem_PlaneAndVectorConstuctor()
        {
            CoordinateSystem expected = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 90));

            Vector xVector = new Vector(Point.MakePointWithInches(-.5, -.707106781, -.5));
            Vector yVector = new Vector(Point.MakePointWithInches(.707106781, 0, -.707106781));
            Vector zVector = new Vector(Point.MakePointWithInches(.5, -.707106781, .5));

            //make sure our vectors are right
            xVector.IsPerpendicularTo(yVector).Should().BeTrue();
            yVector.IsPerpendicularTo(zVector).Should().BeTrue();
            zVector.IsPerpendicularTo(xVector).Should().BeTrue();

            Plane xyPlane = new Plane(xVector, yVector);
            CoordinateSystem results = new CoordinateSystem(xyPlane, xVector, Enums.Axis.X, Enums.AxisPlanes.XYPlane);

            results.AreDirectionsEquivalent(expected).Should().BeTrue();

        }

        [Test()]
        public void CoordinateSystem_RotationMatrix()
        {
            Point origin = Point.MakePointWithInches(0, 0, 0);
            Angle angleX = new Angle(AngleType.Degree, -46.775);
            Angle angleY = new Angle(AngleType.Degree, 23.6);
            Angle angleZ = new Angle(AngleType.Degree, 213);
            CoordinateSystem testSystem = new CoordinateSystem(origin, angleX, angleY, angleZ);

            Matrix test1 = Matrix.RotationMatrixAboutZ(angleZ) * Matrix.RotationMatrixAboutY(angleY) * Matrix.RotationMatrixAboutX(angleX);
            List<Angle> results1 = test1.GetAnglesOutOfRotationMatrixForXYZRotationOrder();
            Matrix test2 = Matrix.RotationMatrixAboutX(angleX) * Matrix.RotationMatrixAboutY(angleY) * Matrix.RotationMatrixAboutZ(angleZ);
            List<Angle> results2 = test2.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            Matrix testMatrix = testSystem.RotationMatrixFromThisToWorld();
            List<Angle> results = testMatrix.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            List<Rotation> resultRotations = new List<Rotation>();
            resultRotations.Add(new Rotation(Line.XAxis, results[0]));
            resultRotations.Add(new Rotation(Line.YAxis, results[1]));
            resultRotations.Add(new Rotation(Line.ZAxis, results[2]));
            Shift resultShift = new Shift(resultRotations, origin);

            List<Rotation> resultRotations2 = new List<Rotation>();
            resultRotations2.Add(new Rotation(Line.ZAxis, results[2]));
            resultRotations2.Add(new Rotation(Line.YAxis, results[1]));
            resultRotations2.Add(new Rotation(Line.XAxis, results[0]));
            Shift resultShift2 = new Shift(resultRotations2, origin);

            Point testPoint = Point.MakePointWithInches(3, -1, -20);
            Point expectedPoint = testPoint.Shift(testSystem.ShiftToThisFrom());
            Point expectedPoint2 = testPoint.Shift(testSystem.ShiftFromThisTo());
            Point resultPoint = testPoint.Shift(resultShift);
            Point resultPoint2 = testPoint.Shift(resultShift.Negate());
            Point resultPoint3 = testPoint.Shift(resultShift2);


            (results[0] == angleX).Should().BeTrue();
            (results[1] == angleY).Should().BeTrue();
            (results[2] == angleZ).Should().BeTrue();
        }

        [Test]
        public void CoordinateSystem_AreDirectionsEquivalent()
        {
            CoordinateSystem same = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, -45));
            CoordinateSystem same2 = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, -45));

            CoordinateSystem equivalent = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 135));

            same.AreDirectionsEquivalent(same2).Should().BeTrue();
            same.AreDirectionsEquivalent(equivalent).Should().BeTrue();

            CoordinateSystem other = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem equavalentToOther = new CoordinateSystem(Point.Origin, new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45));

            //the two 'others' should be equavlanet
            other.AreDirectionsEquivalent(equavalentToOther).Should().BeTrue();

            //these two sets of systems should not be equavalent
            same.AreDirectionsEquivalent(other).Should().BeFalse();
        }

        [Test]
        public void CoordinateSystem_Shift_FromAndTo()
        {
            CoordinateSystem test = new CoordinateSystem(Point.MakePointWithInches(1, -2, -3), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 23.6), new Angle(AngleType.Degree, 243));

            Shift resultFrom = test.ShiftFromThisTo();
            Shift resultTo = test.ShiftToThisFrom();

            List<Rotation> expectedRotations = new List<Rotation>();
            expectedRotations.Add(new Rotation(Line.XAxis, new Angle(AngleType.Degree, -45)));
            expectedRotations.Add(new Rotation(Line.YAxis, new Angle(AngleType.Degree, 23.6)));
            expectedRotations.Add(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 243)));
            Shift expected = new Shift(expectedRotations, Point.MakePointWithInches(1, -2, -3));
            resultFrom.Should().Be(expected);

            //it ends up being what we inputted because it shift reflects the movement of the objects so it is by definition the negative of the 
            //coordinate system. Therfore, whenever we want to revert it we are taking a "double negative" in implementation, giving us the origina
            resultTo.Should().Be(expected.Negate());
        }

        [Test]
        public void CoordinateSystem_Shift_UndoShift()
        {
            CoordinateSystem system = new CoordinateSystem(Point.MakePointWithInches(-1, 2, 4), new Angle(AngleType.Degree, 123), new Angle(AngleType.Degree, -22), new Angle(AngleType.Degree, 78));

            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);

            Polygon shifted = testPolygon.Shift(system.ShiftToThisFrom());
            Polygon shifted2 = shifted.Shift(system.ShiftFromThisTo());

            testPolygon.Should().Be(shifted2);
        }

        [Test]
        public void CoordinateSystem_FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem()
        {
            //test one that really is more just an origin test
            CoordinateSystem testCurrent = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(Point.MakePointWithInches(1, -2, 1), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));

            CoordinateSystem basedOnWorld1 = testRelativeToCurrent.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent);

            Point expectedOrigin = Point.MakePointWithInches(3, 3, 4);
            Angle xExpected = new Angle(AngleType.Degree, 0);
            Angle yExpected = new Angle(AngleType.Degree, 0);
            Angle zExpected = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined1 = new CoordinateSystem(expectedOrigin, xExpected, yExpected, zExpected);

            //this just helps with debugging and is redundent
            Point testPointWorld1 = Point.MakePointWithInches(3 - 2.12132034356, 3 - 0.70710678118, 7);
            Point resultPoint1 = testPointWorld1.Shift(basedOnWorld1.ShiftToThisFrom()); //1,2,3
            Point expectedPoint1 = testPointWorld1.Shift(expectedCombined1.ShiftToThisFrom()); //1,2,3

            (basedOnWorld1 == expectedCombined1).Should().BeTrue();


            //test that negates the coordinate system (this works in both intrinsic and extrensic scenarios
            CoordinateSystem testCurrent2 = new CoordinateSystem(Point.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent2 = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld2 = testRelativeToCurrent2.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent2);

            CoordinateSystem expectedCombined2 = new CoordinateSystem(Point.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());

            //this just helps with debugging and is redundent
            Point testPointWorld2 = Point.MakePointWithInches(3, -2, 3);
            Point resultPoint2 = testPointWorld2.Shift(basedOnWorld2.ShiftToThisFrom()); //1,2,3
            Point expectedPoint2 = testPointWorld2.Shift(expectedCombined2.ShiftToThisFrom()); //1,2,3

            basedOnWorld2.Should().Be(expectedCombined2);

            //test one that onlt works when using the current coords axes for the second one and not the worlds
            CoordinateSystem testCurrent3 = new CoordinateSystem(Point.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem testRelativeToCurrent3 = new CoordinateSystem(Point.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 45), new Angle(), new Angle());

            CoordinateSystem basedOnWorld3 = testRelativeToCurrent3.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent3);

            Point expectedOrigin3 = Point.MakePointWithInches(0, 0, 0); //1, 3, -2
            Angle xExpected3 = new Angle(AngleType.Degree, 90);
            Angle yExpected3 = new Angle(AngleType.Degree, 90);
            Angle zExpected3 = new Angle(AngleType.Degree, -45);
            CoordinateSystem expectedCombined3 = new CoordinateSystem(expectedOrigin3, xExpected3, yExpected3, zExpected3);

            Matrix e = expectedCombined3.RotationMatrixFromThisToWorld();
            List<Angle> ea = e.GetAnglesOutOfRotationMatrixForXYZRotationOrder();
            Matrix r = basedOnWorld3.RotationMatrixFromThisToWorld();
            List<Angle> ra = r.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            (basedOnWorld3 == expectedCombined3).Should().BeTrue();

            //now try another test just for a more robust case
            CoordinateSystem testCurrent4 = new CoordinateSystem(Point.MakePointWithInches(1, 1, -1), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));
            CoordinateSystem testRelativeToCurrent4 = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld4 = testRelativeToCurrent4.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent4);

            Point expectedOrigin4 = Point.MakePointWithInches(1, 3, -2); //1, 3, -2
            Angle xExpected4 = new Angle(AngleType.Degree, 0);
            Angle yExpected4 = new Angle(AngleType.Degree, 90);
            Angle zExpected4 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined4 = new CoordinateSystem(expectedOrigin4, xExpected4, yExpected4, zExpected4);

            //this just helps with debugging and is redundent
            Point testPointWorld4 = Point.MakePointWithInches(1 - 3.53553390611, 3 + 0.70710678032, -3);
            Point resultPoint4 = testPointWorld4.Shift(basedOnWorld4.ShiftToThisFrom()); //1,2,3
            Point expectedPoint4 = testPointWorld4.Shift(expectedCombined4.ShiftToThisFrom()); //1,2,3

            (basedOnWorld4 == expectedCombined4).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_TwoSystemsEqualExpectedCombinedSystemShift()
        {
            Point expectedPoint = Point.MakePointWithInches(1, 2, 3);

            //try our first one
            CoordinateSystem testCurrent = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(Point.MakePointWithInches(1, -2, 1), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));

            Point expectedOrigin1 = Point.MakePointWithInches(3, 3, 4);
            Angle xExpected1 = new Angle(AngleType.Degree, 0);
            Angle yExpected1 = new Angle(AngleType.Degree, 0);
            Angle zExpected1 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined1 = new CoordinateSystem(expectedOrigin1, xExpected1, yExpected1, zExpected1);

            Point testPointWorld1 = Point.MakePointWithInches(3 - 2.12132034356, 3 - 0.70710678118, 7);
            Point point1InCurrent = testPointWorld1.Shift(testCurrent.ShiftToThisFrom());
            Point point1DoubleShifted = point1InCurrent.Shift(testRelativeToCurrent.ShiftToThisFrom());
            Point point1Combined = testPointWorld1.Shift(expectedCombined1.ShiftToThisFrom());

            (expectedPoint == point1DoubleShifted).Should().BeTrue();
            (expectedPoint == point1Combined).Should().BeTrue();

            //now another one
            CoordinateSystem testCurrent2 = new CoordinateSystem(Point.MakePointWithInches(1, 1, -1), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));
            CoordinateSystem testRelativeToCurrent2 = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, -90), new Angle());

            Point expectedOrigin2 = Point.MakePointWithInches(1, 3, -2); //1, 3, -2
            Angle xExpected2 = new Angle(AngleType.Degree, 0);
            Angle yExpected2 = new Angle(AngleType.Degree, 90);
            Angle zExpected2 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined2 = new CoordinateSystem(expectedOrigin2, xExpected2, yExpected2, zExpected2);


            Point testPointWorld2 = Point.MakePointWithInches(1 - 3.53553390611, 3 + 0.70710678032, -3);
            Point point2InCurrent = testPointWorld2.Shift(testCurrent2.ShiftToThisFrom());
            Point point2DoubleShifted = point2InCurrent.Shift(testRelativeToCurrent2.ShiftToThisFrom());
            Point point2Combined = testPointWorld2.Shift(expectedCombined2.ShiftToThisFrom());

            (expectedPoint == point2DoubleShifted).Should().BeTrue();
            (expectedPoint == point2Combined).Should().BeTrue();

            //and one last one that negates out the anles
            CoordinateSystem testCurrent3 = new CoordinateSystem(Point.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent3 = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem expectedCombined3 = new CoordinateSystem(Point.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());

            Point testPointWorld3 = Point.MakePointWithInches(3, -2, 3);
            Point point3InCurrent = testPointWorld3.Shift(testCurrent3.ShiftToThisFrom());
            Point point3DoubleShifted = point3InCurrent.Shift(testRelativeToCurrent3.ShiftToThisFrom());
            Point point3Combined = testPointWorld3.Shift(expectedCombined3.ShiftToThisFrom());

            (expectedPoint == point3DoubleShifted).Should().BeTrue();
            (expectedPoint == point3Combined).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_Shift_SingleAxisRotation()
        {
            CoordinateSystem testSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 0));
            Shift testShift1 = new Shift(new Rotation(new Line(Line.YAxis.Direction, testSystem1.TranslationToOrigin), new Angle(AngleType.Degree, 90)), Point.MakePointWithInches(-1, 1, 2));
            CoordinateSystem results1 = testSystem1.Shift(testShift1);
            CoordinateSystem expectedSystem1 = new CoordinateSystem(Point.MakePointWithInches(0, 3, 5), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 135), new Angle(AngleType.Degree, 0));
            (results1 == expectedSystem1).Should().BeTrue();

            CoordinateSystem testSystem2 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -90));
            Shift testShift2 = new Shift(new Rotation(new Line(Line.XAxis.Direction, testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, 45)), Point.MakePointWithInches(2, -1, -1));
            CoordinateSystem results2 = testSystem2.Shift(testShift2);
            CoordinateSystem expectedSystem2 = new CoordinateSystem(Point.MakePointWithInches(3, 1, 2), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, -90));
            (results2 == expectedSystem2).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_Shift_MultipleAxisRotations()
        {
            CoordinateSystem testSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 30), new Angle(AngleType.Degree, 0));

            List<Rotation> testRotations1 = new List<Rotation>();
            testRotations1.Add(new Rotation(new Line(Line.ZAxis.Direction, testSystem1.TranslationToOrigin), new Angle(AngleType.Degree, -90)));
            testRotations1.Add(new Rotation(new Line(Line.XAxis.Direction, testSystem1.TranslationToOrigin), new Angle(AngleType.Degree, 30)));
            Shift testShift1 = new Shift(testRotations1, Point.MakePointWithInches(0, -2, 5));

            CoordinateSystem results1 = testSystem1.Shift(testShift1);
            CoordinateSystem expectedSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 0, 8), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 60), new Angle(AngleType.Degree, -90));
            (results1 == expectedSystem1).Should().BeTrue();

            //now try another
            CoordinateSystem testSystem2 = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -30));

            List<Rotation> testRotations2 = new List<Rotation>();
            testRotations2.Add(new Rotation(new Line(Line.ZAxis.Direction, testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, -15)));
            testRotations2.Add(new Rotation(new Line(Line.XAxis.Direction, testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, 90)));
            testRotations2.Add(new Rotation(new Line(Line.ZAxis.Direction, testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, 90)));
            testRotations2.Add(new Rotation(new Line(Line.XAxis.Direction, testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, -90)));
            Shift testShift2 = new Shift(testRotations2, Point.MakePointWithInches(2, 0, -1));

            CoordinateSystem results2 = testSystem2.Shift(testShift2);
            CoordinateSystem expectedSystem2 = new CoordinateSystem(Point.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 0));
            (results2 == expectedSystem2).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_Shift_NonAxisRotation()
        {
            CoordinateSystem testSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 90));
            Shift testShift1 = new Shift(new Rotation(new Line(new Direction(new Angle(AngleType.Degree, 45)), testSystem1.TranslationToOrigin), new Angle(AngleType.Degree, 90)), Point.MakePointWithInches(-1, 1, 2));
            CoordinateSystem results1 = testSystem1.Shift(testShift1);
            CoordinateSystem expectedSystem1 = new CoordinateSystem(Point.MakePointWithInches(0, 3, 5), new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 45));
            (results1 == expectedSystem1).Should().BeTrue();

            //now try another
            CoordinateSystem testSystem2 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -90));
            Shift testShift2 = new Shift(new Rotation(new Line(new Direction(new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, 45)), testSystem2.TranslationToOrigin), new Angle(AngleType.Degree, -90)), Point.MakePointWithInches(2, -1, -1));
            CoordinateSystem results2 = testSystem2.Shift(testShift2);
            CoordinateSystem expectedSystem2 = new CoordinateSystem(Point.MakePointWithInches(3, 1, 2), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 180));
            (results2 == expectedSystem2).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_Shift_NotThroughOrigin()
        {
            CoordinateSystem testSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 0));
            Shift testShift1 = new Shift(new Rotation(new Line(new Direction(Line.ZAxis.Direction), Point.MakePointWithInches(1, 4, 0)), new Angle(AngleType.Degree, 180)));
            CoordinateSystem results1 = testSystem1.Shift(testShift1);
            CoordinateSystem expectedSystem1 = new CoordinateSystem(Point.MakePointWithInches(1, 6, 3), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 180));
            (results1 == expectedSystem1).Should().BeTrue();

            //now try another
            CoordinateSystem testSystem2 = new CoordinateSystem(Point.MakePointWithInches(-1, 0, 2), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 0), new Angle(AngleType.Degree, 45));
            Shift testShift2 = new Shift(new Rotation(new Line(new Direction(Line.YAxis.Direction), Point.MakePointWithInches(0, 0, 2)), new Angle(AngleType.Degree, 90)), Point.MakePointWithInches(2, -1, -1));
            CoordinateSystem results2 = testSystem2.Shift(testShift2);
            CoordinateSystem expectedSystem2 = new CoordinateSystem(Point.MakePointWithInches(2, -1, 2), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 90));
            (results2 == expectedSystem2).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_RelativeShift()
        {
            //a nice simple test
            CoordinateSystem testOriginalCoords = new CoordinateSystem(CoordinateSystem.WorldCoordinateSystem);
            CoordinateSystem testCurrentSystem = new CoordinateSystem(Point.MakePointWithInches(1, 0, -1), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));

            //now shift it a bit
            CoordinateSystem shifted = testOriginalCoords.RelativeShift(new Shift(Point.MakePointWithInches(2, 0, 1)), testCurrentSystem);
            (shifted == new CoordinateSystem(Point.MakePointWithInches(1.4, 1.4, 1))).Should().BeTrue();

            //now do one with more complicated shifts
            CoordinateSystem secondTest = new CoordinateSystem(Point.MakePointWithInches(2, -3, 3), new Angle(AngleType.Degree, 90), new Angle(), new Angle());
            CoordinateSystem testCurrentSystem2 = new CoordinateSystem(Point.MakePointWithInches(1, 0, -1), new Angle(), new Angle(), new Angle(AngleType.Degree, 90));

            Shift secondShift = new Shift(new List<Rotation>() { new Rotation(Line.XAxis, new Angle(AngleType.Degree, 90)) }, Point.MakePointWithInches(2, -2, -2));
            CoordinateSystem shifted2 = secondTest.RelativeShift(secondShift, testCurrentSystem2);

            CoordinateSystem expected2 = new CoordinateSystem(Point.MakePointWithInches(7, -1, -4), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 90), new Angle());
            (shifted2 == expected2).Should().BeTrue();
        }

        //These test show how coordinate systems could be used in higher level libraries and also test to make sure that they work how they are intended to for these cases
        //Note: not all the examples are nesecarily pratical uses cases for CoordianteSystems, but are added for completeness of testing. The more pratical applications will be noted as so
        //Note: this is not the only way to use coordinate systems, but this is the way the were intendend to be used when creating them. While other ways may work, this shows the
        //      way in mind that they would be used when creating them.
        //Implentaion Note: It is important in this case to keep track of the current coordinates an object is in since the legoBlocks do not store their "home" geomtry as well as their
        //      current geometry. Two "safer" ways than keeping track of the current coordinates externally would be to eaither store two geometies, one geometry based in the world coordinates 
        //      and another for the current views geometry along with its "home" coordinates based on the world system or to store the current system internally to the object 
        //      as well as its "home" system relative to the world system along with its geometry in the current systems view this way you can figure out where the geometry is based on the two 
        //      systems being stored. Each has its advantages and disadvantages. The first, if you cahnge the geometry, you would need to change it twice, but it would involve less shifting and 
        //      garuntee the geometry's realtion to the world would never be lost. The second would required more shifts and more relative ones, but be better if you change the geometry often
        //      since you only need to changeit once. However, if the current system is changed and the geometry is not shifted, it would lose how it should be positioned in the world system

        [Test]
        public void CoordinateSystem_Demo_CoordinateSystemShifting()
        {
            //This test demonstrated how you could shift between one coordinate system view and another for an object that stores its "home" coordinateSystem based on the world system
            //and how the geometry of the Systems would need to be shifted
            CoordinateSystem LegoSetSystem = new CoordinateSystem(Point.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CURRENT_COORDINATE_SYSTEM = LegoSetSystem;

            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(Point.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(Point.MakePointWithInches(-1, 3, -5), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));

            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block2SystemRelativeToLegoSet);

            //make sure the blocks were made right
            (block1.Geometry == _makeExpectedBlock1InCurrent()).Should().BeTrue();
            (block2.Geometry == _makeExpectedBlock2InCurrent()).Should().BeTrue();

            //check their locations relative to the origin
            CoordinateSystem block1ExpectedCoords = new CoordinateSystem(Point.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());
            CoordinateSystem block2ExpectedCoords = new CoordinateSystem(Point.MakePointWithInches(-3, -3, 3), new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 90), new Angle());

            block1.BlockSystem.Should().Be(block1ExpectedCoords);
            block2.BlockSystem.Should().Be(block2ExpectedCoords);

            //now that we know the blocks are made correctly, lets test shifting them to see if that works correctly too
            //Note: many of these shifts "cheat" and do not use the block in the current coordinate system.
            //      The ones that are realistic in how and when the would be used are noted as so in the comments 
            //      before their blocks, but are the ones that use "block1.geometry.shift(" (or the smae for block2)
            //      and use "CURRENT_COORDINATE_SYSTEM" as one of the systems in the shift

            //create our expected polyhedrons for the block in different coordinates here
            Polyhedron blockInLocal = _makeExpectedBlockInLocal();
            Polyhedron block1InCurrent = _makeExpectedBlock1InCurrent();
            Polyhedron block2InCurrent = _makeExpectedBlock2InCurrent();
            Polyhedron block1InWorld = _makeExpectedBlock1InWorld();
            Polyhedron block2InWorld = _makeExpectedBlock2InWorld();

            //try going from the lego systems to world system
            //Note: this models how you can shift using coordinate systems in actual pratice
            Polyhedron block1FromCurrentToWorld = block1.Geometry.Shift(CURRENT_COORDINATE_SYSTEM.ShiftFromThisTo());
            Polyhedron block2FromCurrentToWorld = block2.Geometry.Shift(CURRENT_COORDINATE_SYSTEM.ShiftFromThisTo());
            (block1FromCurrentToWorld == block1InWorld).Should().BeTrue();
            (block2FromCurrentToWorld == block2InWorld).Should().BeTrue();

            //now try switching to world Coordinates
            Polyhedron block1FromLocalToWorld = blockInLocal.Shift(block1.BlockSystem.ShiftFromThisTo());
            Polyhedron block2FromLocalToWorld = blockInLocal.Shift(block2.BlockSystem.ShiftFromThisTo());
            (block1FromLocalToWorld == block1InWorld).Should().BeTrue();
            (block2FromLocalToWorld == block2InWorld).Should().BeTrue();

            //try going from local coordinates to current Coords
            Polyhedron block1FromLocalToCurrent = blockInLocal.Shift(block1.BlockSystem.ShiftFromThisTo(CURRENT_COORDINATE_SYSTEM));
            Polyhedron block2FromLocalToCurrent = blockInLocal.Shift(block2.BlockSystem.ShiftFromThisTo(CURRENT_COORDINATE_SYSTEM));
            (block1FromLocalToCurrent == block1InCurrent).Should().BeTrue();
            (block2FromLocalToCurrent == block2InCurrent).Should().BeTrue();

            //now try some shiftTo ones

            //shift to the current from the world
            Polyhedron block1ToCurrentFromWorld = block1InWorld.Shift(CURRENT_COORDINATE_SYSTEM.ShiftToThisFrom());
            Polyhedron block2ToCurrentFromWorld = block2InWorld.Shift(CURRENT_COORDINATE_SYSTEM.ShiftToThisFrom());
            (block1ToCurrentFromWorld == block1InCurrent).Should().BeTrue();
            (block2ToCurrentFromWorld == block2InCurrent).Should().BeTrue();

            //to the local from the world
            Polyhedron block1ToLocalFromWorld = block1InWorld.Shift(block1.BlockSystem.ShiftToThisFrom());
            Polyhedron block2ToLocalFromWorld = block2InWorld.Shift(block2.BlockSystem.ShiftToThisFrom());
            (block1ToLocalFromWorld == blockInLocal).Should().BeTrue();
            (block2ToLocalFromWorld == blockInLocal).Should().BeTrue();

            //note: these last two exhibit how these coordinate systems would likely be used in a real context as well
            //note: WorldCoords.ShiftToThisFrom(CURRENT_COORDS) is equivalent to CURRENT_COORDS.ShiftFromThisTo(WorldCoords)
            //to the world from the current
            Polyhedron block1ToWorldFromCurrent = block1.Geometry.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            Polyhedron block2ToWorldFromCurrent = block2.Geometry.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            (block1ToWorldFromCurrent == block1InWorld).Should().BeTrue();
            (block2ToWorldFromCurrent == block2InWorld).Should().BeTrue();

            //to the local from the current
            Polyhedron block1ToLocalFromCurrent = block1.Geometry.Shift(block1.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            Polyhedron block2ToLocalFromCurrent = block2.Geometry.Shift(block2.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            (block1ToLocalFromCurrent == blockInLocal).Should().BeTrue();
            (block2ToLocalFromCurrent == blockInLocal).Should().BeTrue();

            //this shows the follwoing relationship is true
            //note: WorldCoords.ShiftToThisFrom(CURRENT_COORDS) is equivalent to CURRENT_COORDS.ShiftFromThisTo(WorldCoords)
            (block2ToWorldFromCurrent == block2FromCurrentToWorld).Should().BeTrue();
        }

        [Test]
        public void CoordinateSystem_Demo_ShiftingCoordinateSystems()
        {
            //This test demonstrates how you would shift a coordinate system and the difference between a Shift and a RelativeShift as well as giving examples of when each would be used.

            //set up pur lego set and our lego blocks
            LegoSet testSet = new LegoSet();
            CoordinateSystem LegoSetSystem = new CoordinateSystem(Point.MakePointWithInches(3, -2, -2), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));
            testSet.SetSystem = LegoSetSystem;

            CURRENT_COORDINATE_SYSTEM = new CoordinateSystem(LegoSetSystem);

            //make block1 on top of block 2
            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(Point.MakePointWithInches(-2, 1, 2), new Angle(), new Angle(), new Angle(AngleType.Degree, -90));
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(Point.MakePointWithInches(-2, 1, 1), new Angle(), new Angle(), new Angle(AngleType.Degree, -90));
            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block2SystemRelativeToLegoSet);

            testSet.Blocks = new List<LegoBlock>() { block1, block2 };

            //Show how RelativeShift works
            //RelativeShift should be used when the shift is created for the current system and not for the world system and if you want to shif the object in the current system
            //lets say we want to rotate the block2 -90 degrees z in the sets system(CURRENT_SYSTEM) and its origin point
            Shift shiftBlocks90 = new Shift(new Rotation(new Line(Line.ZAxis.Direction, Point.MakePointWithInches(-2, 1, 1)), new Angle(AngleType.Degree, -90)));

            //shift only the ones we want to in the list of blocks
            block2.Geometry = block2.Geometry.Shift(shiftBlocks90);
            block2.BlockSystem = block2.BlockSystem.RelativeShift(shiftBlocks90, CURRENT_COORDINATE_SYSTEM);

            //show its where we want relatvie to the set
            (block2.Geometry == _makeExpectedBlock2Shifted90InSetSystem()).Should().BeTrue();

            //now check that the coordinate system places it where it should be by shifting the block to its home coords
            Polyhedron shiftedBlock2ToHome = block2.Geometry.Shift(block2.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM)); //this shows the coordinate systme was shifted right
            (shiftedBlock2ToHome == _makeExpectedBlockInLocal()).Should().BeTrue();

            //now we desicded we want block2 on top of block1 and have it so the 2 length dimensions overlap(move in block1s local y)
            //This is how we would do that (note: we dont edit the coordinate systems when changing current coordinates)
            block1.Geometry = block1.Geometry.Shift(block1.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            block2.Geometry = block2.Geometry.Shift(block1.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            CURRENT_COORDINATE_SYSTEM = block1.BlockSystem;

            //now make the shift in the block1 coordinates to apply to block 2
            Shift shiftBlock2OnTop = new Shift(Point.MakePointWithInches(0, 2, 2));
            block2.Geometry = block2.Geometry.Shift(shiftBlock2OnTop);
            block2.BlockSystem = block2.BlockSystem.RelativeShift(shiftBlock2OnTop, CURRENT_COORDINATE_SYSTEM);

            //show its where we want in the the set system
            (block2.Geometry.Shift(CURRENT_COORDINATE_SYSTEM.ShiftFromThisTo(LegoSetSystem)) == _makeExpectedBlock2ShiftedOnBlock1InSetSystem()).Should().BeTrue();

            //now check its geometry in local again
            shiftedBlock2ToHome = block2.Geometry.Shift(block2.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM)); //this shows the coordinate systme was shifted right
            (shiftedBlock2ToHome == _makeExpectedBlockInLocal()).Should().BeTrue();


            //Now what if we want to move the whole set 2 in the x and rotate it so its at 45 degees z around the setSystem origin?
            //note: this is the same effect as RealtiveShift(shift, WorldCoordinateSystem)

            //Switch the coordinate systems to world
            //note: we dont edit the coordinate systems when changing current coordinates
            block1.Geometry = block1.Geometry.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            block2.Geometry = block2.Geometry.Shift(CoordinateSystem.WorldCoordinateSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM));
            CURRENT_COORDINATE_SYSTEM = CoordinateSystem.WorldCoordinateSystem;

            //These Shifts will only work correctly when they are in the CURRENT_COORDINATE_SYSTEM
            //(presumabley in the legoSet.Shift() function)
            Shift shiftWholeSet = new Shift(new Rotation(new Line(Line.ZAxis.Direction, testSet.SetSystem.TranslationToOrigin), new Angle(AngleType.Degree, 45)), Point.MakePointWithInches(2, 0, 0));
            testSet.SetSystem = testSet.SetSystem.Shift(shiftWholeSet);

            //check the coords because we dont hav a geomtry for this - its a collection
            (testSet.SetSystem == new CoordinateSystem(Point.MakePointWithInches(5, -2, -2), new Angle(), new Angle(), new Angle(AngleType.Degree, 90))).Should().BeTrue();

            //shift the pieces in the collection (also presumably in the LegoSet.Shift() method)
            block1.Geometry = block1.Geometry.Shift(shiftWholeSet);
            block1.BlockSystem = block1.BlockSystem.Shift(shiftWholeSet);
            block2.Geometry = block2.Geometry.Shift(shiftWholeSet);
            block2.BlockSystem = block2.BlockSystem.Shift(shiftWholeSet);

            //show its where we want in the world
            Polyhedron block1InWorld = block1.Geometry.Shift(CURRENT_COORDINATE_SYSTEM.ShiftFromThisTo());
            Polyhedron block2InWorld = block2.Geometry.Shift(CURRENT_COORDINATE_SYSTEM.ShiftFromThisTo());
            (block1InWorld == _makeExpectedBlock1ShiftedInWorld()).Should().BeTrue();
            (block2InWorld == _makeExpectedBlock2ShiftedInWorld()).Should().BeTrue();

            //now check that the coordinate system places it where it should be by shifting the blocks to their home coords
            Polyhedron shiftedBlock1ToHome = block1.Geometry.Shift(block1.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM)); //this shows the coordinate systme was shifted right
            (shiftedBlock1ToHome == _makeExpectedBlockInLocal()).Should().BeTrue();
            shiftedBlock2ToHome = block2.Geometry.Shift(block2.BlockSystem.ShiftToThisFrom(CURRENT_COORDINATE_SYSTEM)); //this shows the coordinate systme was shifted right
            (shiftedBlock2ToHome == _makeExpectedBlockInLocal()).Should().BeTrue();

        }

        //these methods are for checking and demonstrating how coordinate systems can/should be used
        #region Coordinate System Switching Helper Stuff

        public CoordinateSystem CURRENT_COORDINATE_SYSTEM;

        private class LegoBlock
        {
            public Polyhedron Geometry;
            public CoordinateSystem BlockSystem;
        }

        private class LegoSet
        {
            public List<LegoBlock> Blocks = new List<LegoBlock>();
            public CoordinateSystem SetSystem = new CoordinateSystem();
        }

        private LegoBlock createBlockInGivenCoordinateSystem(Distance xDimension, Distance yDimension, Distance zDimension, CoordinateSystem blocksCoordinateSystem)
        {
            LegoBlock toReturn = new LegoBlock();

            //make our geometry
            Point basePoint = Point.Origin;
            Point topLeftPoint = new Point(new Distance(), yDimension, new Distance());
            Point bottomRightPoint = new Point(xDimension, new Distance(), new Distance());
            Point topRightPoint = new Point(xDimension, yDimension, new Distance());

            Point backBasePoint = new Point(new Distance(), new Distance(), zDimension);
            Point backTopLeftPoint = new Point(new Distance(), yDimension, zDimension);
            Point backBottomRightPoint = new Point(xDimension, new Distance(), zDimension);
            Point backTopRightPoint = new Point(xDimension, yDimension, zDimension);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backBasePoint, backTopLeftPoint, backTopRightPoint, backBottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backTopRightPoint, backTopLeftPoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backBottomRightPoint, backBasePoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backTopLeftPoint, backBasePoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backTopRightPoint, backBottomRightPoint }));

            toReturn.Geometry = new Polyhedron(planes);

            //now figure out is base coordinate system relative to the world and store it
            toReturn.BlockSystem = blocksCoordinateSystem.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(CURRENT_COORDINATE_SYSTEM);

            //now move it so it is in ouur current coordinate system
            toReturn.Geometry = toReturn.Geometry.Shift(blocksCoordinateSystem.ShiftFromThisTo());

            //then return it
            return toReturn;
        }

        public Polyhedron _makeExpectedBlockInLocal()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(0, 0, 1);
            Point topLeft = Point.MakePointWithInches(0, 2, 1);
            Point topRight = Point.MakePointWithInches(4, 2, 1);
            Point bottomRight = Point.MakePointWithInches(4, 0, 1);

            Point bottomLeftBack = Point.MakePointWithInches(0, 0, 0);
            Point topLeftBack = Point.MakePointWithInches(0, 2, 0);
            Point topRightBack = Point.MakePointWithInches(4, 2, 0);
            Point bottomRightBack = Point.MakePointWithInches(4, 0, 0);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock1InWorld()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(2, -4, 1);
            Point topLeft = Point.MakePointWithInches(2, -2, 1);
            Point topRight = Point.MakePointWithInches(6, -2, 1);
            Point bottomRight = Point.MakePointWithInches(6, -4, 1);

            Point bottomLeftBack = Point.MakePointWithInches(2, -4, 0);
            Point topLeftBack = Point.MakePointWithInches(2, -2, 0);
            Point topRightBack = Point.MakePointWithInches(6, -2, 0);
            Point bottomRightBack = Point.MakePointWithInches(6, -4, 0);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock2InWorld()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(-3, -3, 3);
            Point topLeft = Point.MakePointWithInches(-3, -5, 3);
            Point topRight = Point.MakePointWithInches(-3, -5, -1);
            Point bottomRight = Point.MakePointWithInches(-3, -3, -1);

            Point bottomLeftBack = Point.MakePointWithInches(-4, -3, 3);
            Point topLeftBack = Point.MakePointWithInches(-4, -5, 3);
            Point topRightBack = Point.MakePointWithInches(-4, -5, -1);
            Point bottomRightBack = Point.MakePointWithInches(-4, -3, -1);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock1InCurrent()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(-2, 0, 0);
            Point topLeft = Point.MakePointWithInches(0, 0, 0);
            Point topRight = Point.MakePointWithInches(0, 0, 4);
            Point bottomRight = Point.MakePointWithInches(-2, 0, 4);

            Point bottomLeftBack = Point.MakePointWithInches(-2, 1, 0);
            Point topLeftBack = Point.MakePointWithInches(0, 1, 0);
            Point topRightBack = Point.MakePointWithInches(0, 1, 4);
            Point bottomRightBack = Point.MakePointWithInches(-2, 1, 4);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock2InCurrent()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(-1, 3, -5);
            Point topLeft = Point.MakePointWithInches(-3, 3, -5);
            Point topRight = Point.MakePointWithInches(-3, -1, -5);
            Point bottomRight = Point.MakePointWithInches(-1, -1, -5);

            Point bottomLeftBack = Point.MakePointWithInches(-1, 3, -6);
            Point topLeftBack = Point.MakePointWithInches(-3, 3, -6);
            Point topRightBack = Point.MakePointWithInches(-3, -1, -6);
            Point bottomRightBack = Point.MakePointWithInches(-1, -1, -6);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock2Shifted90InSetSystem()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(-2, 1, 2);
            Point topLeft = Point.MakePointWithInches(-2, -1, 2);
            Point topRight = Point.MakePointWithInches(-6, -1, 2);
            Point bottomRight = Point.MakePointWithInches(-6, 1, 2);

            Point bottomLeftBack = Point.MakePointWithInches(-2, 1, 1);
            Point topLeftBack = Point.MakePointWithInches(-2, -1, 1);
            Point topRightBack = Point.MakePointWithInches(-6, -1, 1);
            Point bottomRightBack = Point.MakePointWithInches(-6, 1, 1);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock2ShiftedOnBlock1InSetSystem()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(0, 1, 4);
            Point topLeft = Point.MakePointWithInches(0, -1, 4);
            Point topRight = Point.MakePointWithInches(-4, -1, 4);
            Point bottomRight = Point.MakePointWithInches(-4, 1, 4);

            Point bottomLeftBack = Point.MakePointWithInches(0, 1, 3);
            Point topLeftBack = Point.MakePointWithInches(0, -1, 3);
            Point topRightBack = Point.MakePointWithInches(-4, -1, 3);
            Point bottomRightBack = Point.MakePointWithInches(-4, 1, 3);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock2ShiftedInWorld()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(4, -2, 2);
            Point topLeft = Point.MakePointWithInches(4, -6, 2);
            Point topRight = Point.MakePointWithInches(6, -6, 2);
            Point bottomRight = Point.MakePointWithInches(6, -2, 2);

            Point bottomLeftBack = Point.MakePointWithInches(4, -2, 1);
            Point topLeftBack = Point.MakePointWithInches(4, -6, 1);
            Point topRightBack = Point.MakePointWithInches(6, -6, 1);
            Point bottomRightBack = Point.MakePointWithInches(6, -2, 1);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        public Polyhedron _makeExpectedBlock1ShiftedInWorld()
        {
            //make the points
            Point bottomLeft = Point.MakePointWithInches(4, -4, 1);
            Point topLeft = Point.MakePointWithInches(4, -2, 1);
            Point topRight = Point.MakePointWithInches(8, -2, 1);
            Point bottomRight = Point.MakePointWithInches(8, -4, 1);

            Point bottomLeftBack = Point.MakePointWithInches(4, -4, 0);
            Point topLeftBack = Point.MakePointWithInches(4, -2, 0);
            Point topRightBack = Point.MakePointWithInches(8, -2, 0);
            Point bottomRightBack = Point.MakePointWithInches(8, -4, 0);

            //make the faces
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { bottomLeft, topLeft, topRight, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomLeftBack, topLeftBack, topRightBack, bottomRightBack }));
            faces.Add(new Polygon(new List<Point> { bottomLeft, bottomLeftBack, topLeftBack, topLeft }));
            faces.Add(new Polygon(new List<Point> { topLeft, topLeftBack, topRightBack, topRight }));
            faces.Add(new Polygon(new List<Point> { topRight, topRightBack, bottomRightBack, bottomRight }));
            faces.Add(new Polygon(new List<Point> { bottomRight, bottomRightBack, bottomLeftBack, bottomLeft }));

            //make the Polyhedron
            return new Polyhedron(faces);
        }

        #endregion
    }
}
