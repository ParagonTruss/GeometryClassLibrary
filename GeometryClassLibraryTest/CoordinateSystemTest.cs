using NUnit.Framework;
using GeometryClassLibrary;
using FluentAssertions;
using UnitClassLibrary;
using System.Collections.Generic;
using System;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class CoordinateSystemTest
    {
        [Test]
        public void CoordinateSystem_PlaneAndVectorConstuctorTests()
        {
            CoordinateSystem expected = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 90));

            Vector xVector = new Vector(PointGenerator.MakePointWithInches(-.5, -.707106781, -.5));
            Vector yVector = new Vector(PointGenerator.MakePointWithInches(.707106781, 0, -.707106781));
            Vector zVector = new Vector(PointGenerator.MakePointWithInches(.5, -.707106781, .5));

            //make sure our vectors are right
            xVector.IsPerpindicularTo(yVector).Should().BeTrue();
            yVector.IsPerpindicularTo(zVector).Should().BeTrue();
            zVector.IsPerpindicularTo(xVector).Should().BeTrue();

            Plane xyPlane = new Plane(xVector, yVector);
            CoordinateSystem results = new CoordinateSystem(xyPlane, xVector, Enums.Axis.X, Enums.AxisPlanes.XYPlane);

            results.AreDirectionsEquivalent(expected).Should().BeTrue();

        }

        [Test()]
        public void CoordinateSystem_RotationMatrix()
        {
            Point origin = PointGenerator.MakePointWithInches(0, 0, 0);
            Angle angleX = new Angle(AngleType.Degree, -46.775);
            Angle angleY = new Angle(AngleType.Degree, 23.6);
            Angle angleZ = new Angle(AngleType.Degree, 213);
            CoordinateSystem testSystem = new CoordinateSystem(origin, angleX, angleY, angleZ);

            Matrix test1 = Matrix.RotationMatrixAboutZ(angleZ) * Matrix.RotationMatrixAboutY(angleY) * Matrix.RotationMatrixAboutX(angleX);
            List<Angle> results1 = test1.GetAnglesOutOfRotationMatrixForXYZRotationOrder();
            Matrix test2 = Matrix.RotationMatrixAboutX(angleX) * Matrix.RotationMatrixAboutY(angleY) * Matrix.RotationMatrixAboutZ(angleZ);
            List<Angle> results2 = test2.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            Matrix testMatrix = testSystem.RotationMatrix;
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

            Point testPoint = PointGenerator.MakePointWithInches(3, -1, -20);
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
        public void CoordinateSystem_AreDirectionsEquivalentTests()
        {
            CoordinateSystem same = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, -45));
            CoordinateSystem same2 = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, -45));

            CoordinateSystem equivalent = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 135));

            same.AreDirectionsEquivalent(same2).Should().BeTrue();
            same.AreDirectionsEquivalent(equivalent).Should().BeTrue();

            CoordinateSystem other = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem equavalentToOther = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 45));

            //the two 'others' should be equavlanet
            other.AreDirectionsEquivalent(equavalentToOther).Should().BeTrue();

            //these two sets of systems should not be equavalent
            same.AreDirectionsEquivalent(other).Should().BeFalse();
        }

        [Test]
        public void CoordinateSystem_ShiftFromAndTo()
        {
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, -3), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 23.6), new Angle(AngleType.Degree, 243));

            Shift resultFrom = test.ShiftFromThisTo();
            Shift resultTo = test.ShiftToThisFrom();

            List<Rotation> expectedRotations = new List<Rotation>();
            expectedRotations.Add(new Rotation(Line.XAxis, new Angle(AngleType.Degree, -45)));
            expectedRotations.Add(new Rotation(Line.YAxis, new Angle(AngleType.Degree, 23.6)));
            expectedRotations.Add(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 243)));
            Shift expected = new Shift(expectedRotations, PointGenerator.MakePointWithInches(1, -2, -3));
            resultFrom.Should().Be(expected);

            //it ends up being what we inputted because it shift reflects the movement of the objects so it is by definition the negative of the 
            //coordinate system. Therfore, whenever we want to revert it we are taking a "double negative" in implementation, giving us the origina
            resultTo.Should().Be(expected.Negate());
        }

        [Test]
        public void CoordinateSystem_FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem()
        {
            //test one that really is more just an origin test
            CoordinateSystem testCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 2, 3), new Angle(), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, 1), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));

            CoordinateSystem basedOnWorld1 = testRelativeToCurrent.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent);

            Point expectedOrigin = PointGenerator.MakePointWithInches(3, 3, 4);
            Angle xExpected = new Angle(AngleType.Degree, 0);
            Angle yExpected = new Angle(AngleType.Degree, 0);
            Angle zExpected = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined1 = new CoordinateSystem(expectedOrigin, xExpected, yExpected, zExpected);

            //this just helps with debugging and is redundent
            Point testPointWorld1 = PointGenerator.MakePointWithInches(3 - 2.12132034356, 3 - 0.70710678118, 7);
            Point resultPoint1 = testPointWorld1.Shift(basedOnWorld1.ShiftToThisFrom()); //1,2,3
            Point expectedPoint1 = testPointWorld1.Shift(expectedCombined1.ShiftToThisFrom()); //1,2,3

            (basedOnWorld1 == expectedCombined1).Should().BeTrue();


            //test that negates the coordinate system (this works in both intrinsic and extrensic scenarios
            CoordinateSystem testCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld2 = testRelativeToCurrent2.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent2);

            CoordinateSystem expectedCombined2 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());

            //this just helps with debugging and is redundent
            Point testPointWorld2 = PointGenerator.MakePointWithInches(3, -2, 3);
            Point resultPoint2 = testPointWorld2.Shift(basedOnWorld2.ShiftToThisFrom()); //1,2,3
            Point expectedPoint2 = testPointWorld2.Shift(expectedCombined2.ShiftToThisFrom()); //1,2,3

            basedOnWorld2.Should().Be(expectedCombined2);

            //test one that onlt works when using the current coords axes for the second one and not the worlds
            CoordinateSystem testCurrent3 = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem testRelativeToCurrent3 = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 45), new Angle(), new Angle());

            CoordinateSystem basedOnWorld3 = testRelativeToCurrent3.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent3);

            Point expectedOrigin3 = PointGenerator.MakePointWithInches(0, 0, 0); //1, 3, -2
            Angle xExpected3 = new Angle(AngleType.Degree, 90);
            Angle yExpected3 = new Angle(AngleType.Degree, 90);
            Angle zExpected3 = new Angle(AngleType.Degree, -45);
            CoordinateSystem expectedCombined3 = new CoordinateSystem(expectedOrigin3, xExpected3, yExpected3, zExpected3);

            Matrix e = expectedCombined3.RotationMatrix;
            List<Angle> ea = e.GetAnglesOutOfRotationMatrixForXYZRotationOrder();
            Matrix r = basedOnWorld3.RotationMatrix;
            List<Angle> ra = r.GetAnglesOutOfRotationMatrixForXYZRotationOrder();

            (basedOnWorld3 == expectedCombined3).Should().BeTrue();

            //now try another test just for a more robust case
            CoordinateSystem testCurrent4 = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 1, -1), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));
            CoordinateSystem testRelativeToCurrent4 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld4 = testRelativeToCurrent4.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent4);

            Point expectedOrigin4 = PointGenerator.MakePointWithInches(1, 3, -2); //1, 3, -2
            Angle xExpected4 = new Angle(AngleType.Degree, 0);
            Angle yExpected4 = new Angle(AngleType.Degree, 90);
            Angle zExpected4 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined4 = new CoordinateSystem(expectedOrigin4, xExpected4, yExpected4, zExpected4);

            //this just helps with debugging and is redundent
            Point testPointWorld4 = PointGenerator.MakePointWithInches(1 - 3.53553390611, 3 + 0.70710678032, -3);
            Point resultPoint4 = testPointWorld4.Shift(basedOnWorld4.ShiftToThisFrom()); //1,2,3
            Point expectedPoint4 = testPointWorld4.Shift(expectedCombined4.ShiftToThisFrom()); //1,2,3

            (basedOnWorld4 == expectedCombined4).Should().BeTrue();
        }

        [Test()]
        public void CoordinateSystem_TwoSystemsEqualExpectedCombinedSystemShift()
        {
            Point expectedPoint = PointGenerator.MakePointWithInches(1, 2, 3);

            //try our first one
            CoordinateSystem testCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 2, 3), new Angle(), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, 1), new Angle(), new Angle(), new Angle(AngleType.Degree, 45));

            Point expectedOrigin1 = PointGenerator.MakePointWithInches(3, 3, 4);
            Angle xExpected1 = new Angle(AngleType.Degree, 0);
            Angle yExpected1 = new Angle(AngleType.Degree, 0);
            Angle zExpected1 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined1 = new CoordinateSystem(expectedOrigin1, xExpected1, yExpected1, zExpected1);

            Point testPointWorld1 = PointGenerator.MakePointWithInches(3 - 2.12132034356, 3 - 0.70710678118, 7);
            Point point1InCurrent = testPointWorld1.Shift(testCurrent.ShiftToThisFrom());
            Point point1DoubleShifted = point1InCurrent.Shift(testRelativeToCurrent.ShiftToThisFrom());
            Point point1Combined = testPointWorld1.Shift(expectedCombined1.ShiftToThisFrom());

            (expectedPoint == point1DoubleShifted).Should().BeTrue();
            (expectedPoint == point1Combined).Should().BeTrue();

            //now another one
            CoordinateSystem testCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 1, -1), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));
            CoordinateSystem testRelativeToCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, -90), new Angle());

            Point expectedOrigin2 = PointGenerator.MakePointWithInches(1, 3, -2); //1, 3, -2
            Angle xExpected2 = new Angle(AngleType.Degree, 0);
            Angle yExpected2 = new Angle(AngleType.Degree, 90);
            Angle zExpected2 = new Angle(AngleType.Degree, 135);
            CoordinateSystem expectedCombined2 = new CoordinateSystem(expectedOrigin2, xExpected2, yExpected2, zExpected2);


            Point testPointWorld2 = PointGenerator.MakePointWithInches(1 - 3.53553390611, 3 + 0.70710678032, -3);
            Point point2InCurrent = testPointWorld2.Shift(testCurrent2.ShiftToThisFrom());
            Point point2DoubleShifted = point2InCurrent.Shift(testRelativeToCurrent2.ShiftToThisFrom());
            Point point2Combined = testPointWorld2.Shift(expectedCombined2.ShiftToThisFrom());

            (expectedPoint == point2DoubleShifted).Should().BeTrue();
            (expectedPoint == point2Combined).Should().BeTrue();

            //and one last one that negates out the anles
            CoordinateSystem testCurrent3 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CoordinateSystem testRelativeToCurrent3 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem expectedCombined3 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());

            Point testPointWorld3 = PointGenerator.MakePointWithInches(3, -2, 3);
            Point point3InCurrent = testPointWorld3.Shift(testCurrent3.ShiftToThisFrom());
            Point point3DoubleShifted = point3InCurrent.Shift(testRelativeToCurrent3.ShiftToThisFrom());
            Point point3Combined = testPointWorld3.Shift(expectedCombined3.ShiftToThisFrom());

            (expectedPoint == point3DoubleShifted).Should().BeTrue();
            (expectedPoint == point3Combined).Should().BeTrue();
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
            Point basePoint = new Point();
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
            Point bottomLeft = PointGenerator.MakePointWithInches(0, 0, 1);
            Point topLeft = PointGenerator.MakePointWithInches(0, 2, 1);
            Point topRight = PointGenerator.MakePointWithInches(4, 2, 1);
            Point bottomRight = PointGenerator.MakePointWithInches(4, 0, 1);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 2, 0);
            Point topRightBack = PointGenerator.MakePointWithInches(4, 2, 0);
            Point bottomRightBack = PointGenerator.MakePointWithInches(4, 0, 0);

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
            Point bottomLeft = PointGenerator.MakePointWithInches(2, -4, 1);
            Point topLeft = PointGenerator.MakePointWithInches(2, -2, 1);
            Point topRight = PointGenerator.MakePointWithInches(6, -2, 1);
            Point bottomRight = PointGenerator.MakePointWithInches(6, -4, 1);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(2, -4, 0);
            Point topLeftBack = PointGenerator.MakePointWithInches(2, -2, 0);
            Point topRightBack = PointGenerator.MakePointWithInches(6, -2, 0);
            Point bottomRightBack = PointGenerator.MakePointWithInches(6, -4, 0);

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
            Point bottomLeft = PointGenerator.MakePointWithInches(-3, -3, 3);
            Point topLeft = PointGenerator.MakePointWithInches(-3, -5, 3);
            Point topRight = PointGenerator.MakePointWithInches(-3, -5, -1);
            Point bottomRight = PointGenerator.MakePointWithInches(-3, -3, -1);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(-4, -3, 3);
            Point topLeftBack = PointGenerator.MakePointWithInches(-4, -5, 3);
            Point topRightBack = PointGenerator.MakePointWithInches(-4, -5, -1);
            Point bottomRightBack = PointGenerator.MakePointWithInches(-4, -3, -1);

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
            Point bottomLeft = PointGenerator.MakePointWithInches(-2, 0, 0);
            Point topLeft = PointGenerator.MakePointWithInches(0 , 0, 0); 
            Point topRight = PointGenerator.MakePointWithInches(0, 0, 4);
            Point bottomRight = PointGenerator.MakePointWithInches(-2, 0, 4);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(-2, 1, 0);
            Point topLeftBack = PointGenerator.MakePointWithInches(0, 1, 0); 
            Point topRightBack = PointGenerator.MakePointWithInches(0, 1, 4); 
            Point bottomRightBack = PointGenerator.MakePointWithInches(-2, 1, 4);

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
            Point bottomLeft = PointGenerator.MakePointWithInches(-1, 3, -5);
            Point topLeft = PointGenerator.MakePointWithInches(-3, 3, -5);
            Point topRight = PointGenerator.MakePointWithInches(-3, -1, -5);
            Point bottomRight = PointGenerator.MakePointWithInches(-1, -1, -5);

            Point bottomLeftBack = PointGenerator.MakePointWithInches(-1, 3, -6);
            Point topLeftBack = PointGenerator.MakePointWithInches(-3, 3, -6);
            Point topRightBack = PointGenerator.MakePointWithInches(-3, -1, -6);
            Point bottomRightBack = PointGenerator.MakePointWithInches(-1, -1, -6);

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

        [Test]
        public void CoordinateSystem_CoordinateSystemShifting()
        {
            //neither of these work right now
            CoordinateSystem LegoSetSystem = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CURRENT_COORDINATE_SYSTEM = LegoSetSystem;

            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(AngleType.Degree, -90), new Angle());
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 3, -5), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));

            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 4), new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), block2SystemRelativeToLegoSet);

            //make sure the blocks were made right
            (block1.Geometry == _makeExpectedBlock1InCurrent()).Should().BeTrue();
            (block2.Geometry == _makeExpectedBlock2InCurrent()).Should().BeTrue();

            //check their locations relative to the origin
            CoordinateSystem block1ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());
            CoordinateSystem block2ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(-3, -3, 3), new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 90), new Angle());

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
            //this would model how you can shift using coordinate systems in actual pratice
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

            //these last two exhibit how these coordinate systems would likely be used in a real context
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
    }
}
