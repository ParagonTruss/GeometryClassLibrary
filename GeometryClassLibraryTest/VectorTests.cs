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
        public void Vector_Negate()
        {
            Vector v = new Vector(PointGenerator.MakePointWithInches(1, 1), PointGenerator.MakePointWithInches(4, 1));

            v.Negate().Should().Be(new Vector(PointGenerator.MakePointWithInches(4, 1), PointGenerator.MakePointWithInches(1, 1)));
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
    }
}
