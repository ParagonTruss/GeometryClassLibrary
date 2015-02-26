using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class VectorTests
    {
        [Test()]
        public void Vector_DoPointInOppositeDirections()
        {
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(4, 1), PointGenerator.MakePointWithInches(1, 1));

            vector1.PointInOppositeDirections(vector2).Should().BeTrue();
        }

        [Test()]
        public void Vector_DoNotPointInOppositeDirections()
        {
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(4, 1), PointGenerator.MakePointWithInches(3, 3));

            vector1.PointInOppositeDirections(vector2).Should().BeFalse();
        }

        [Test()]
        public void Vector_DoPointInSameDirections()
        {
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(1,2), PointGenerator.MakePointWithInches(4, 2));

            vector1.PointInSameDirection(vector2).Should().BeTrue();
        }

        [Test()]
        public void Vector_DoNotPointInSameDirections()
        {
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(2, 2), PointGenerator.MakePointWithInches(4, 4));

            vector1.PointInSameDirection(vector2).Should().BeFalse();
        }

        [Test()]
        public void Vector_Reverse()
        {
            Vector v = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            v.Reverse().Should().Be(new Vector(PointGenerator.MakePointWithInches(4, 1), PointGenerator.MakePointWithInches(1, 1)));
        }

        [Test()]
        public void Vector_Negate()
        {
            Vector v = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            v.Reverse().Should().Be(new Vector(PointGenerator.MakePointWithInches(4, 1), PointGenerator.MakePointWithInches(7, 1)));
        }

        [Test()]
        public void Vector_CrossProduct()
        {
            Vector xAxis = new Vector(new Point(), new Direction());
            Vector yAxis = new Vector(new Point(), new Direction(new Angle(AngleType.Degree, 90)));

            Vector result = xAxis.CrossProduct(yAxis);
            Vector expected = new Vector(new Point(), new Direction(new Angle(), new Angle()));

            result.Should().Be(expected);

            Vector resultParallel = xAxis.CrossProduct(xAxis);
            (resultParallel.Magnitude == new Distance()).Should().BeTrue();
        }

        [Test()]
        public void Vector_ProjectOntoPlane()
        {
            Vector testSegment = new Vector(PointGenerator.MakePointWithInches(2, 5, 3));
            Plane projectOnto = new Plane(Line.XAxis, Line.YAxis);

            Vector result = testSegment.ProjectOntoPlane(projectOnto);

            Vector expected = new Vector(PointGenerator.MakePointWithInches(2, 5, 0));

            result.Should().Be(expected);
        }

        [Test()]
        public void Vector_ContainsPoint()
        {
            Vector testVector = new Vector(PointGenerator.MakePointWithInches(4, 4, -4));
            Point testPoint = PointGenerator.MakePointWithInches(2, 2, -2);
            Point pointNotOnVector = PointGenerator.MakePointWithInches(2, 2, -3);

            bool resultOn = testVector.Contains(testPoint);
            bool resultNotOn = testVector.Contains(pointNotOnVector);

            resultOn.Should().BeTrue();
            resultNotOn.Should().BeFalse();
        }

        [Test()]
        public void Vector_DoesOverlapInSameDirection()
        {
            Vector testVector = new Vector(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(4, 4, -4));
            Vector testPartial = new Vector(PointGenerator.MakePointWithInches(2, 2, -2));
            Vector testContained = new Vector(PointGenerator.MakePointWithInches(2, 2, -2), PointGenerator.MakePointWithInches(3, 3, -3));
            Vector testContaining = new Vector(PointGenerator.MakePointWithInches(6, 6, -6));

            Vector testSameLineNotOverlap = new Vector(PointGenerator.MakePointWithInches(5, 5, -5), PointGenerator.MakePointWithInches(6, 6, -6));
            Vector testSameBaseNoOverlap = new Vector(PointGenerator.MakePointWithInches(1, 1, -1), PointGenerator.MakePointWithInches(2, 3, -3));
            Vector testIntersecting = new Vector(PointGenerator.MakePointWithInches(0, 0, -1), PointGenerator.MakePointWithInches(4, 4, -3));

            bool resultPartial = testVector.DoesOverlapInSameDirection(testPartial);
            bool resultContained = testVector.DoesOverlapInSameDirection(testContained);
            bool resultContaining = testVector.DoesOverlapInSameDirection(testContaining);

            bool resultSameLineNotOverlap = testVector.DoesOverlapInSameDirection(testSameLineNotOverlap);
            bool resultSameBaseNoOverlap = testVector.DoesOverlapInSameDirection(testSameBaseNoOverlap);
            bool resultIntersecting = testVector.DoesOverlapInSameDirection(testIntersecting);

            resultPartial.Should().BeTrue();
            resultContained.Should().BeTrue();
            resultContaining.Should().BeTrue();

            resultSameLineNotOverlap.Should().BeFalse();
            resultSameBaseNoOverlap.Should().BeFalse();
            resultIntersecting.Should().BeFalse();
        }

        [Test()]
        public void Vector_ShiftCoordinateSystemsToFrom()
        {
            //make our polyhedron
            Vector testVector = new Vector(PointGenerator.MakePointWithInches(8, -3, 0.435),PointGenerator.MakePointWithInches(4, 1.47, 0));

            //now make one not in the World coordinate system
            CoordinateSystem testSystem = new CoordinateSystem(PointGenerator.MakePointWithInches(4, -2, 1), new Angle(), new Angle(AngleType.Degree, 43), new Angle());
            Vector notAtWorld = testVector.SystemShift(testSystem);

            //now make yet another CoordinateSystem vector
            CoordinateSystem testSystem2 = new CoordinateSystem(PointGenerator.MakePointWithInches(-1, 0, 1), new Angle(AngleType.Degree, 65), new Angle(AngleType.Degree, -27), new Angle());
            Vector notAtWorld2 = testVector.SystemShift(testSystem2);



            //now test shifting from world to another
            Vector worldTo1 = testVector.ShiftCoordinateSystemsToFrom(testSystem);
            (worldTo1 == notAtWorld).Should().BeTrue();

            //now from another to the world
            Vector twoToWorld = notAtWorld2.ShiftCoordinateSystemsToFrom(CoordinateSystem.WorldCoordinateSystem, testSystem2);
            (twoToWorld == testVector).Should().BeTrue();

            //now from one to another
            Vector oneToTwo = notAtWorld.ShiftCoordinateSystemsToFrom(testSystem2, testSystem);
            (oneToTwo == notAtWorld2).Should().BeTrue();
        }
    }
}
