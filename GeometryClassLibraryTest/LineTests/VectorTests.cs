using System;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class VectorTests
    {
        [Test()]
        public void Vector_DoesPointInOppositeDirectionOf()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(Point.MakePointWithInches(4, 1), Point.MakePointWithInches(1, 1));

            vector1.HasOppositeDirectionOf(vector2).Should().BeTrue();
        }

        [Test()]
        public void Vector_DoesNotPointInOppositeDirectionOf()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(Point.MakePointWithInches(4, 1), Point.MakePointWithInches(3, 3));

            vector1.HasOppositeDirectionOf(vector2).Should().BeFalse();
        }

        [Test()]
        public void Vector_DoesPointInSameDirectionAs()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(Point.MakePointWithInches(1,2), Point.MakePointWithInches(4, 2));

            vector1.HasSameDirectionAs(vector2).Should().BeTrue();
        }

        [Test()]
        public void Vector_DoesNotPointInSameDirectionAs()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 1));

            Vector vector2 = new Vector(Point.MakePointWithInches(2, 2), Point.MakePointWithInches(4, 4));

            vector1.HasSameDirectionAs(vector2).Should().BeFalse();
        }


        [Test()]
        public void Vector_Reverse()
        {
            Point point1 = Point.MakePointWithInches(2, 2);
            Point point2 = Point.MakePointWithInches(4, 3);
            Vector v = new Vector(point1, point2);

            v.Reverse().Should().Be(new Vector(point2, point1));
            v.Reverse().Should().NotBe(new Vector(Point.MakePointWithInches(5, 3), Point.MakePointWithInches(3, 2)));
        }
        [Test()]
        public void Vector_FlipAboutTail()
        {
            Vector v = new Vector(Point.MakePointWithInches(2, 2), Point.MakePointWithInches(4, 3));

            v.FlipAboutTail().Should().Be(new Vector(Point.MakePointWithInches(2, 2), Point.MakePointWithInches(0, 1)));
            v.FlipAboutTail().Should().NotBe(new Vector(Point.MakePointWithInches(3, 3), Point.MakePointWithInches(1, 2)));
        }

        [Test()]
        public void Vector_FlipAboutHead()
        {
            Vector v = new Vector(Point.MakePointWithInches(2, 2), Point.MakePointWithInches(4, 3));

            v.FlipAboutHead().Should().Be(new Vector(Point.MakePointWithInches(6,4), Point.MakePointWithInches(4, 3)));
            v.FlipAboutHead().Should().NotBe(new Vector(Point.MakePointWithInches(5, 3), Point.MakePointWithInches(3, 2)));
        }

        [Test()]
        public void Vector_CrossProduct()
        {
            Vector xAxis = new Vector(Point.Origin, Direction.Right);
            Vector yAxis = new Vector(Point.Origin, new Direction(new Angle(new Degree(), 90)));

            Vector result = xAxis.CrossProduct(yAxis);
            Vector expected = new Vector(Point.Origin, new Direction(Angle.Zero, Angle.Zero));

            result.Should().Be(expected);

            Vector resultParallel = xAxis.CrossProduct(xAxis);
            (resultParallel.Magnitude == Distance.Zero).Should().BeTrue();
        }

        [Test()]
        public void Vector_ProjectOntoPlane()
        {
            Vector testSegment = new Vector(Point.MakePointWithInches(2, 5, 3));
            Plane projectOnto = new Plane(Line.XAxis, Line.YAxis);

            Vector result = testSegment.ProjectOntoPlane(projectOnto);

            Vector expected = new Vector(Point.MakePointWithInches(2, 5, 0));

            result.Should().Be(expected);
        }

        [Test()]
        public void Vector_ContainsPoint()
        {
            Vector testVector = new Vector(Point.MakePointWithInches(4, 4, -4));
            Point testPoint = Point.MakePointWithInches(2, 2, -2);
            Point pointNotOnVector = Point.MakePointWithInches(2, 2, -3);

            bool resultOn = testVector.Contains(testPoint);
            bool resultNotOn = testVector.Contains(pointNotOnVector);

            resultOn.Should().BeTrue();
            resultNotOn.Should().BeFalse();
        }

        [Test()]
        public void Vector_ContainsVector()
        {
            Vector vector1 = new Vector(Point.Origin);
            Vector vector2 = new Vector(Point.MakePointWithInches(2, 2, 4));
            Vector vector3 = new Vector(Point.MakePointWithInches(2, 2, 4), Point.Origin);
            Vector vector4 = new Vector(Point.MakePointWithInches(1, 1, 2), Point.Origin);

            (vector1.Contains(vector2)).Should().BeFalse();
            (vector2.Contains(vector1)).Should().BeTrue();
            (vector2.Contains(vector3)).Should().BeTrue();
            (vector3.Contains(vector2)).Should().BeTrue();
            (vector2.Contains(vector4)).Should().BeTrue();
        }
        
        [Test()]
        public void Vector_DoesOverlapInSameDirection()
        {
            Vector testVector = new Vector(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(4, 4, -4));
            Vector testPartial = new Vector(Point.MakePointWithInches(2, 2, -2));
            Vector testContained = new Vector(Point.MakePointWithInches(2, 2, -2), Point.MakePointWithInches(3, 3, -3));
            Vector testContaining = new Vector(Point.MakePointWithInches(6, 6, -6));

            Vector testSameLineNotOverlap = new Vector(Point.MakePointWithInches(5, 5, -5), Point.MakePointWithInches(6, 6, -6));
            Vector testSameBaseNoOverlap = new Vector(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(2, 3, -3));
            Vector testIntersecting = new Vector(Point.MakePointWithInches(0, 0, -1), Point.MakePointWithInches(4, 4, -3));

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
        public void Vector_AngleBetween()
        {
            Vector vector1 = new Vector(Point.MakePointWithInches(1, 2, 3));
            Vector vector2 = new Vector(Point.MakePointWithInches(-2, 1, 0));
            Vector vector3 = new Vector(Point.MakePointWithInches(-2, 1, 1));
            Vector vector4 = new Vector(Point.MakePointWithInches(1, 0));
            Vector vector5 = new Vector(Point.MakePointWithInches(1, Math.Sqrt(3)));

            vector1.AngleBetween(vector2).Should().Be(new Angle(new Radian(), 1.57079632679));
            vector1.AngleBetween(vector3).Should().Be(new Angle(new Radian(), 1.23732315));
            vector4.AngleBetween(vector5).Should().Be(new Angle(new Degree(), 60));

        }

    }
}
