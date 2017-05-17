using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GeometryClassLibrary;
using MoreLinq;
using NUnit.Framework;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        public class DummyKey : IEquatable<DummyKey>
        {
            public bool Equals(DummyKey other) => true;
        }

        [Test]
        public void GroupByEquatable_Test()
        {
            var planes = new List<Plane>
            {
                Plane.XY,
                Plane.XZ,
                Plane.YZ,
                // Should be roughly The YZ plane
                new Plane(Direction.Left, new Point(Distance.Inches, 0.01, 0.01)),
                // Should be roughly The XZ plane
                new Plane(Direction.Down, new Point(Distance.Inches, 0.01, 0, 0.01)),
            };
            // Plane implements IEquatable
            var grouped = planes.GroupByEquatable(_.Identity);

            grouped.Count.Should().Be(3);
            grouped[0].Elements.Count.Should().Be(1);
            grouped[1].Elements.Count.Should().Be(2);
            grouped[2].Elements.Count.Should().Be(2);

            // Can group by any Key that implements IEquatable
            var groupedByAnotherClass = planes.GroupByEquatable(plane => new DummyKey());
            groupedByAnotherClass.Count.Should().Be(1);
            groupedByAnotherClass[0].Elements.Count.Should().Be(5);
        }

        [Test]
        public void MinByUnitOrDefault_Test()
        {
            var planes = new List<Plane>
            {
                Plane.XY,
                Plane.XZ,
                Plane.YZ,
                // Should be roughly The YZ plane
                new Plane(Direction.Left, new Point(Distance.Inches, 0.01, 0.01)),
                // Should be roughly The XZ plane
                new Plane(Direction.Down, new Point(Distance.Inches, 0.01, 0, 0.01)),
            };

            var minimum_Z_intersection = planes.MinByUnitOrDefault(plane => plane.IntersectWithLine(
                    new Line(new Direction(1.3, .4, -.8),
                             new Point(Distance.Inches, 2, 10, -6))).Z);

            minimum_Z_intersection.Should().Be(Plane.YZ);
        }

        public struct DummyStruct { }

        [Test]
        public void MinByOrDefault_Test()
        {
            var planes = new List<Plane>
            {
                Plane.XY,
                Plane.XZ,
                Plane.YZ,
                // Should be roughly The YZ plane
                new Plane(Direction.Left, new Point(Distance.Inches, 0.01, 0.01)),
                // Should be roughly The XZ plane
                new Plane(Direction.Down, new Point(Distance.Inches, 0.01, 0, 0.01)),
            };

            var minByEmptyList = new List<Plane>().MinByOrDefault(plane => 2);
            minByEmptyList.Should().Be(null);

            var valueTypeMinByEmptyList = new List<double>().MinByOrDefault(x => -5);
            valueTypeMinByEmptyList.Should().Be(0);

            var structMinByEmptyList = new List<DummyStruct>().MinByOrDefault(x => 57);
            structMinByEmptyList.Should().Be(new DummyStruct());

            var minimum_Z_intersection = planes.MinByOrDefault(plane => plane.IntersectWithLine(
                new Line(new Direction(1.3, .4, -.8),
                    new Point(Distance.Inches, 2, 10, -6))).Z.ValueInInches);

            minimum_Z_intersection.Should().Be(Plane.YZ);
        }
    }
}
