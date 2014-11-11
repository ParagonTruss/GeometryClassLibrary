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

            result.Should().Be(expected);


           /* Matrix testX = Matrix.RotationMatrixAboutX(new Angle(AngleType.Degree, 45));
            Matrix testY = Matrix.RotationMatrixAboutY(new Angle(AngleType.Degree, -23.6));
            Matrix testZ = Matrix.RotationMatrixAboutZ(new Angle(AngleType.Degree, -243));

            Matrix summed = testX * testY * testZ;

            Matrix testX2 = Matrix.RotationMatrixAboutX(new Angle(AngleType.Degree, -45));
            Matrix testY2 = Matrix.RotationMatrixAboutY(new Angle(AngleType.Degree, 23.6));
            Matrix testZ2 = Matrix.RotationMatrixAboutZ(new Angle(AngleType.Degree, 243));
            Matrix summed2 = summed * (testX2 * testY2 * testZ2);

            Matrix cancel = testX * testX2;
            Matrix cancelY = testY * testY2;
            Matrix cancelZ = testZ * testZ2;
            Matrix allCancel = testX * testX2 * testY * testY2 * testZ * testZ2;

            Point testPoint = PointGenerator.MakePointWithInches(0, 3, 1);

            //This works : http://stackoverflow.com/questions/1996957/conversion-euler-to-matrix-and-matrix-to-euler
            Matrix toAnglesMatrix = new Matrix(3, 1, new double[] 
            {
                Math.Atan2(summed.GetElement(0,1), summed.GetElement(1,1)),
                -Math.Asin(summed.GetElement(2,1)),
                Math.Atan2(summed.GetElement(2,0), summed.GetElement(2,2)),
            });

            Angle[] angles = summed.getAnglesOutOfRotationMatrix();

            Shift testShift = new Shift(new List<Rotation>()
            {
                new Rotation(Line.ZAxis, angles[0]),
                new Rotation(Line.YAxis, angles[1]),
                new Rotation(Line.XAxis, angles[2])
            });

            Point resultMat = testPoint.Shift(testShift);

            Matrix pointMatrix2 = testPoint.ConvertToMatrixColumn();

            Matrix rotatedPointMatrix2 = summed * pointMatrix2;

            double xOfRotatedPoint2 = rotatedPointMatrix2.GetElement(0, 0);
            double yOfRotatedPoint2 = rotatedPointMatrix2.GetElement(1, 0);
            double zOfRotatedPoint2 = rotatedPointMatrix2.GetElement(2, 0);


            Point resultOne = testPoint.Shift(expected);

            Matrix pointMatrix = testPoint.ConvertToMatrixColumn();

            Matrix rotatedPointMatrix = summed2 * pointMatrix;

            double xOfRotatedPoint = rotatedPointMatrix.GetElement(0, 0);
            double yOfRotatedPoint = rotatedPointMatrix.GetElement(1, 0);
            double zOfRotatedPoint = rotatedPointMatrix.GetElement(2, 0);

            Point pointToReturn = PointGenerator.MakePointWithInches(xOfRotatedPoint, yOfRotatedPoint, zOfRotatedPoint);*/
        }

        [Test]
        public void CoordinateSystem_FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem()
        {
            CoordinateSystem testCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));

            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, 1), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -23.6), new Angle(AngleType.Degree, -243));

            CoordinateSystem basedOnWorld = testRelativeToCurrent.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent);

            Point expectedOrigin = PointGenerator.MakePointWithInches(2, 3, 1);
            Angle xExpected = new Angle(AngleType.Degree, 90 + 45);
            Angle yExpected = new Angle(AngleType.Degree, -23.6);
            Angle zExpected = new Angle(AngleType.Degree, 90 - 243);

            CoordinateSystem expected = new CoordinateSystem(expectedOrigin, xExpected, yExpected, zExpected);

            basedOnWorld.Should().Be(expected);
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

        private LegoBlock createBlockInGivenCoordinateSystem(Dimension xDimension, Dimension yDimension, Dimension zDimension, CoordinateSystem blocksCoordinateSystem)
        {
            LegoBlock toReturn = new LegoBlock();

            //make our geometry
            Point basePoint = new Point();
            Point topLeftPoint = new Point(new Dimension(), yDimension, new Dimension());
            Point bottomRightPoint = new Point(xDimension, new Dimension(), new Dimension());
            Point topRightPoint = new Point(xDimension, yDimension, new Dimension());

            Point backbasepoint = new Point(new Dimension(), new Dimension(), zDimension);
            Point backtopleftpoint = new Point(new Dimension(), yDimension, zDimension);
            Point backbottomrightpoint = new Point(xDimension, new Dimension(), zDimension);
            Point backtoprightpoint = new Point(xDimension, yDimension, zDimension);

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
            CoordinateSystem LegoSetSystem = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 45));
            CURRENT_COORDINATE_SYSTEM = LegoSetSystem;

            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(0, -1, 0), new Angle(), new Angle(AngleType.Degree, 90), new Angle());
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(0, -2, -1), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, -90), new Angle());

            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Dimension(DimensionType.Inch, 2), new Dimension(DimensionType.Inch, 1), new Dimension(DimensionType.Inch, 6), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Dimension(DimensionType.Inch, 2), new Dimension(DimensionType.Inch, 1), new Dimension(DimensionType.Inch, 6), block2SystemRelativeToLegoSet);

            //check their locations relative to the origin
            CoordinateSystem block1ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(0, 0, 0), new Angle(AngleType.Degree, 90), new Angle(AngleType.Degree, -45), new Angle());
            CoordinateSystem block2ExpectedCoords = new CoordinateSystem();

            block1.BlockSystem.Should().Be(block1ExpectedCoords);
            block2.BlockSystem.Should().Be(block2ExpectedCoords);
        }
    }
}
