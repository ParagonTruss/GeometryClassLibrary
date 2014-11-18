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
            CoordinateSystem expected = new CoordinateSystem(new Point(), new Angle(AngleType.Degree, 90), 
                new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, 45));

            Vector xVector = new Vector(PointGenerator.MakePointWithInches(-.5, -.707106781, -.5));
            Vector yVector = new Vector(PointGenerator.MakePointWithInches(.707106781, 0, -.707106781));
            Vector zVector = new Vector(PointGenerator.MakePointWithInches(.5, -.707106781, .5));

            /*make sure our vectors are right
            xVector.IsPerpindicularTo(yVector).Should().BeTrue();
            yVector.IsPerpindicularTo(zVector).Should().BeTrue();
            zVector.IsPerpindicularTo(xVector).Should().BeTrue();*/

            Plane xyPlane = new Plane(xVector, yVector);
            CoordinateSystem results = new CoordinateSystem(xyPlane, xVector, Enums.Axis.X, Enums.AxisPlanes.XYPlane);

            results.AreDirectionsEquivalent(expected).Should().BeTrue();

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
        public void CoordinateSystem_ShiftThatReturnsThisToWorldCoordinateSystem()
        {
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 2, 3), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -23.6), new Angle(AngleType.Degree, -243));

            Shift result = test.ShiftThatReturnsThisToWorldCoordinateSystem();

            //it ends up being what we inputted because it shift reflects the movement of the objects so it is by definition the negative of the 
            //coordinate system. Therfore, whenever we want to revert it we are taking a "double negative" in implementation, giving us the original
            List<Rotation> expectedRotations = new List<Rotation>();
            expectedRotations.Add(new Rotation(Line.YAxis, new Angle(AngleType.Degree, -243)));
            expectedRotations.Add(new Rotation(Line.XAxis, new Angle(AngleType.Degree, -23.6)));
            expectedRotations.Add(new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 45)));
            Shift expected = new Shift(expectedRotations, PointGenerator.MakePointWithInches(-1, 2, 3));

            //ignore this for now
            result.Should().Be(expected);
        }

        [Test]
        public void CoordinateSystem_FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem()
        {
            CoordinateSystem testCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 2, 3), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
  
            CoordinateSystem testRelativeToCurrent = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, 1), new Angle(AngleType.Degree, 45), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld = testRelativeToCurrent.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent);

            Point expectedOrigin = PointGenerator.MakePointWithInches(1 + 1, 2 + 1, 3 -2); //2,3,1
            Angle zExpected = new Angle(AngleType.Degree, 135);
            Angle xExpected = new Angle(AngleType.Degree, 0);
            Angle yExpected = new Angle(AngleType.Degree, 0);

            CoordinateSystem expected = new CoordinateSystem(expectedOrigin, zExpected, xExpected, yExpected);

            basedOnWorld.Should().Be(expected);


            //now try another test to make sure it works
            CoordinateSystem testCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(1, 1, -1), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));

            CoordinateSystem testRelativeToCurrent2 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 1), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, -90), new Angle());

            CoordinateSystem basedOnWorld2 = testRelativeToCurrent2.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testCurrent2);

            Point expectedOrigin2 = PointGenerator.MakePointWithInches(1 - 1, 1 - 0, -1 + 2); //0, 1, 1
            Angle zExpected2 = new Angle(AngleType.Degree, -135);
            Angle xExpected2 = new Angle(AngleType.Degree, 90);
            Angle yExpected2= new Angle(AngleType.Degree, 0);

            CoordinateSystem expected2 = new CoordinateSystem(expectedOrigin2, zExpected2, xExpected2, yExpected2);

            basedOnWorld2.Should().Be(expected2);


            //test that negates the coordinate system
            CoordinateSystem testSystem3 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));

            CoordinateSystem testRelativeToCurrent3 = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(), new Angle(new Angle(AngleType.Degree, -90)));

            CoordinateSystem basedOnWorld3 = testRelativeToCurrent3.FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(testSystem3);

            CoordinateSystem expected3 = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());

            basedOnWorld3.Should().Be(expected3);
            
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

            Point backBasePoint = new Point(new Dimension(), new Dimension(), zDimension);
            Point backTopLeftPoint = new Point(new Dimension(), yDimension, zDimension);
            Point backBottomRightPoint = new Point(xDimension, new Dimension(), zDimension);
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
            toReturn.Geometry = toReturn.Geometry.Shift(new Shift(CURRENT_COORDINATE_SYSTEM));

            //then return it
            return toReturn;
        }

        #endregion

        [Test]
        public void CoordinateSystem_CoordinateSystemShifting()
        {
            //neither of these work right now
            CoordinateSystem LegoSetSystem = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -2, 0), new Angle(AngleType.Degree, 90), new Angle(), new Angle(AngleType.Degree, 90));
            CURRENT_COORDINATE_SYSTEM = LegoSetSystem;

            CoordinateSystem block1SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(-2, 0, 0), new Angle(AngleType.Degree, -90), new Angle(), new Angle(new Angle(AngleType.Degree, -90)));
            CoordinateSystem block2SystemRelativeToLegoSet = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 3, -5), new Angle(AngleType.Degree, 180), new Angle(), new Angle(AngleType.Degree, -90));

            LegoBlock block1 = createBlockInGivenCoordinateSystem(new Dimension(DimensionType.Inch, 6), new Dimension(DimensionType.Inch, 2), new Dimension(DimensionType.Inch, 1), block1SystemRelativeToLegoSet);
            LegoBlock block2 = createBlockInGivenCoordinateSystem(new Dimension(DimensionType.Inch, 6), new Dimension(DimensionType.Inch, 2), new Dimension(DimensionType.Inch, 1), block2SystemRelativeToLegoSet);

            //check their locations relative to the origin
            CoordinateSystem block1ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(2, -4, 0), new Angle(), new Angle(), new Angle());
            CoordinateSystem block2ExpectedCoords = new CoordinateSystem(PointGenerator.MakePointWithInches(-3, -3, 3), new Angle(), new Angle(AngleType.Degree, -90), new Angle());

            block1.BlockSystem.Should().Be(block1ExpectedCoords);
            block2.BlockSystem.Should().Be(block2ExpectedCoords);
        }
    }
}
