using NUnit.Framework;
using GeometryClassLibrary;
using System.Collections.Generic;
using FluentAssertions;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class IListIEdgeExtensionMethodTests
    {
        [Test]
        public void IEdge_ShiftTest()
        {
            //make a bunch of edges and shift 'em
            List<IEdge> edges = new List<IEdge>();
            edges.Add(new LineSegment(PointGenerator.MakePointWithMillimeters(1, 2, 3)));
            edges.Add(new Arc(PointGenerator.MakePointWithMillimeters(0,4,5)));

            List<IEdge> results = edges.Shift(new Shift(new Vector(PointGenerator.MakePointWithMillimeters(2,0,0))));

            (results.Contains(new LineSegment(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(3, 2, 3)))).Should().BeTrue();
            (results.Contains(new Arc(PointGenerator.MakePointWithMillimeters(2, 0, 0), PointGenerator.MakePointWithMillimeters(2, 4, 5)))).Should().BeTrue();
        }
    }
}
