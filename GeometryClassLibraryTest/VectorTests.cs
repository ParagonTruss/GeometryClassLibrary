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
