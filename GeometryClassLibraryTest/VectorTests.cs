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
            Point point1 = PointGenerator.MakePointWithInches(2, 2);
            Point point2 = PointGenerator.MakePointWithInches(4, 3);
            Vector v = new Vector(point1, point2);

            v.Reverse().Should().Be(new Vector(point2, point1));
            v.Reverse().Should().NotBe(new Vector(PointGenerator.MakePointWithInches(5, 3), PointGenerator.MakePointWithInches(3, 2)));
        }
        [Test()]
        public void Vector_FlipAboutTail()
        {
            Vector v = new Vector(PointGenerator.MakePointWithInches(2, 2), PointGenerator.MakePointWithInches(4, 3));

            v.FlipAboutTail().Should().Be(new Vector(PointGenerator.MakePointWithInches(2, 2), PointGenerator.MakePointWithInches(0, 1)));
            v.FlipAboutTail().Should().NotBe(new Vector(PointGenerator.MakePointWithInches(3, 3), PointGenerator.MakePointWithInches(1, 2)));
        }

        [Test()]
        public void Vector_FlipAboutHead()
        {
            Vector v = new Vector(PointGenerator.MakePointWithInches(2, 2), PointGenerator.MakePointWithInches(4, 3));

            v.FlipAboutHead().Should().Be(new Vector(PointGenerator.MakePointWithInches(6,4), PointGenerator.MakePointWithInches(4, 3)));
            v.FlipAboutHead().Should().NotBe(new Vector(PointGenerator.MakePointWithInches(5, 3), PointGenerator.MakePointWithInches(3, 2)));
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
        public void Vector_ContainsVector()
        {
            Vector vector1 = new Vector();
            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(2, 2, 4));
            Vector vector3 = new Vector(PointGenerator.MakePointWithInches(2, 2, 4), new Point());
            Vector vector4 = new Vector(PointGenerator.MakePointWithInches(1, 1, 2), new Point());

            (vector1.Contains(vector2)).Should().BeFalse();
            (vector2.Contains(vector1)).Should().BeTrue();
            (vector2.Contains(vector3)).Should().BeTrue();
            (vector3.Contains(vector2)).Should().BeTrue();
            (vector2.Contains(vector4)).Should().BeTrue();
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
        public void Vector_AngleBetween()
        {
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(1, 2, 3));
            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(-2, 1, 0));
            Vector vector3 = new Vector(PointGenerator.MakePointWithInches(-2, 1, 1));
            Vector vector4 = new Vector(PointGenerator.MakePointWithInches(1, 0));
            Vector vector5 = new Vector(PointGenerator.MakePointWithInches(1, Math.Sqrt(3)));

            vector1.AngleBetween(vector2).Should().Be(new Angle(AngleType.Radian, 1.57079632679));
            vector1.AngleBetween(vector3).Should().Be(new Angle(AngleType.Radian, 1.23732315));
            vector4.AngleBetween(vector5).Should().Be(new Angle(AngleType.Degree, 60));

        }

    }
}
