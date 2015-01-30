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
            CoordinateSystem test = new CoordinateSystem(PointGenerator.MakePointWithInches(1, -2, -3), new Angle(AngleType.Degree, -45), new Angle(AngleType.Degree, 23.6), new Angle(AngleType.Degree, 243));

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
            toReturn.Geometry = toReturn.Geometry.Shift(new Shift(CURRENT_COORDINATE_SYSTEM));

            //then return it
            return toReturn;
        }

        #endregion

       
    }
}
