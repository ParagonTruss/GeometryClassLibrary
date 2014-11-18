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
        public void CoordinateSystem_ShiftThatReturnsThisToWorldCoordinateSystem()
        {
            //-1, 2, 3
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -23.6), new Angle(AngleType.Degree, -243));

            Shift result = test.ShiftThatReturnsThisToWorldCoordinateSystem();

            //it ends up being what we inputted because it shift reflects the movement of the objects so it is by definition the negative of the 
            //coordinate system. Therfore, whenever we want to revert it we are taking a "double negative" in implementation, giving us the original
            List<Rotation> expectedRotations = new List<Rotation>();
            expectedRotations.Add(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, -243)));
            expectedRotations.Add(new Rotation(Line.YAxis, new Angle(AngleType.Degree, -23.6)));
            expectedRotations.Add(new Rotation(Line.XAxis, new Angle(AngleType.Degree, 45)));
            Shift expected = new Shift(expectedRotations, PointGenerator.MakePointWithInches(0, 0, 0));

            //ignore this for now
            //result.Should().Be(expected);



            //TIM:
            //matrix rotating testing


            //matrix canceling stuff
            Matrix testX = Matrix.RotationMatrixAboutX(new Angle(AngleType.Degree, 45));
            Matrix testY = Matrix.RotationMatrixAboutY(new Angle(AngleType.Degree, -23.6));
            Matrix testZ = Matrix.RotationMatrixAboutZ(new Angle(AngleType.Degree, -243));

            Matrix summed = testX * testY * testZ;

            //Shouldnt this give a matrix with a 1's in the diagonal?
            //Does this not work due to matrices being non communitive?
            Matrix testXNegated = Matrix.RotationMatrixAboutX(new Angle(AngleType.Degree, -45));
            Matrix testYNegated = Matrix.RotationMatrixAboutY(new Angle(AngleType.Degree, 23.6));
            Matrix testZNegated = Matrix.RotationMatrixAboutZ(new Angle(AngleType.Degree, 243));
            Matrix summedNegated = summed * (testXNegated * testYNegated * testZNegated);

            //this does work how i'd expect
            Matrix cancel = testX * testXNegated;
            Matrix cancelY = testY * testYNegated;
            Matrix cancelZ = testZ * testZNegated;
            Matrix allCancel = testX * testXNegated * testY * testYNegated * testZ * testZNegated;


            //a point we can see if the rotation works right with
            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 1);

            //what we expect the point to be once translated
            Point resultExpected = testPoint.Shift(expected);

            //Try getting the angles out of the matrix 
            //http://stackoverflow.com/questions/1996957/conversion-euler-to-matrix-and-matrix-to-euler
            //
            //this works but it returns them in a different order than the expected shift did
            //The expected went x then y then x, this one needs to go z then x then y.
            //I know that the same orientation can be represented by multiple euler triples
            //but is there a way to get them back out in x,y,z rotations or whatever order of rotations
            //that makes physical sense to you? Or is this just how it is/the standard order to do rotations?
            //
            //this is also in Matrix.getAnglesOutOfRotationMatrix() currently
            Angle[] extractedAngles = new Angle[3];
            extractedAngles[0] = new Angle(AngleType.Radian, Math.Atan2(summed.GetElement(1, 0), summed.GetElement(1, 1))); //z
            extractedAngles[1] = new Angle(AngleType.Radian, -Math.Asin(summed.GetElement(1, 2))); //x
            extractedAngles[2] = new Angle(AngleType.Radian, Math.Atan2(summed.GetElement(0, 2), summed.GetElement(2, 2))); //y

            List<Angle> angles = summed.getAnglesOutOfRotationMatrix();
            Shift testShift = new Shift(new List<Rotation>()
            {
                new Rotation(Line.ZAxis, angles[0]),
                new Rotation(Line.XAxis, angles[1]),
                new Rotation(Line.YAxis, angles[2])
            });
            Point resultAnglesExtracted = testPoint.Shift(testShift);

            //yet another way it works
            Matrix pointMatrix = testPoint.ConvertToMatrixColumn();

            Matrix rotatedPoint = summed * pointMatrix;

            Point matrixRotatedPoint = PointGenerator.MakePointWithInches(rotatedPoint.GetElement(0, 0), rotatedPoint.GetElement(1, 0), rotatedPoint.GetElement(2, 0));

            resultAnglesExtracted.Should().Be(resultExpected);
            matrixRotatedPoint.Should().Be(resultAnglesExtracted);
        }

        [Test]
        public void CoordinateSystem_FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem()
        {
            //1, 2, 3
            CoordinateSystem testCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(0,0,0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));

            //1, -2, 1
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(0,0,0), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld = testRelativeToCurrent.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent);

            //2, 3, 1
            Point expectedOrigin = PointGenerator.MakePointWithInches(0, 0, 0);
            Angle zExpected = new Angle(AngleType.Degree, -45);
            Angle xExpected = new Angle(AngleType.Degree, 180);
            Angle yExpected = new Angle(AngleType.Degree, 0);

            CoordinateSystem expected = new CoordinateSystem(expectedOrigin, xExpected, yExpected, zExpected);

            basedOnWorld.AreDirectionsEquivalent(expected).Should().BeTrue();
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
            public List<LegoBlock> Blocks;
            public CoordinateSystem SetSystem;
        }

        private LegoBlock createBlockInGivenCoordinateSystem(Distance xDistance, Distance yDistance, Distance zDistance, CoordinateSystem blocksCoordinateSystem)
        {
            LegoBlock toReturn = new LegoBlock();

            //make our geometry
            Point basePoint = new Point();
            Point topLeftPoint = new Point(new Distance(), yDistance, new Distance());
            Point bottomRightPoint = new Point(xDistance, new Distance(), new Distance());
            Point topRightPoint = new Point(xDistance, yDistance, new Distance());

            Point backbasepoint = new Point(new Distance(), new Distance(), zDistance);
            Point backtopleftpoint = new Point(new Distance(), yDistance, zDistance);
            Point backbottomrightpoint = new Point(xDistance, new Distance(), zDistance);
            Point backtoprightpoint = new Point(xDistance, yDistance, zDistance);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));

            toReturn.Geometry = new Polyhedron(planes);

            //now figure out is base coordinate system relative to the world and store it
            toReturn.BlockSystem = blocksCoordinateSystem.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(CURRENT_COORDINATE_SYSTEM);

            //now move it so it is in ouur current coordinate system
            toReturn.Geometry = toReturn.Geometry.Shift(new Shift(CURRENT_COORDINATE_SYSTEM));

            //then return it
            return toReturn;
        }

        #endregion

        [Test]
        public void CoordinateSystem_CoordinateSystemShifting()
        {
            CoordinateSystem LegoSetSystem = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CURRENT_COORDINATE_SYSTEM = LegoSetSystem;

            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 1, 0), new Angle(), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 2, 1), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, -90), new Angle());

            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), new Distance(DistanceType.Inch, 6), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Distance(DistanceType.Inch, 2), new Distance(DistanceType.Inch, 1), new Distance(DistanceType.Inch, 6), block2SystemRelativeToLegoSet);

            //check their locations relative to the origin
            CoordinateSystem block1ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 1), new Angle(), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem block2ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(3, -2, 2), new Angle(), new Angle(), new Angle(AngleType.Degree, -90));

            block1.BlockSystem.Should().Be(block1ExpectedCoords);
            block2.BlockSystem.Should().Be(block2ExpectedCoords);
        }
    }
}
