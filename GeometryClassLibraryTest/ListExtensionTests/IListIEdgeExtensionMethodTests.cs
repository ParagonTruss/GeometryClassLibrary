using NUnit.Framework;
using GeometryClassLibrary;
using System.Collections.Generic;
using FluentAssertions;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class IListIEdgeExtensionMethodTests
    {
        [Test]
        public void IEdge_Shift()
        {
            //make a bunch of edges and shift 'em
            List<IEdge> edges = new List<IEdge>();
            edges.Add(new LineSegment(Point.MakePointWithInches(1, 2, 3)));
            edges.Add(new Arc(Point.MakePointWithInches(5,4,0), Point.MakePointWithInches(2,-1,0), Direction.Right));

            List<IEdge> results = edges.Shift(new Shift(Point.MakePointWithInches(2,0,0)));

            (results.Contains(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(3, 2, 3)))).Should().BeTrue();
            (results.Contains(new Arc(Point.MakePointWithInches(7, 4, 0), Point.MakePointWithInches(4, -1, 0), Direction.Right))).Should().BeTrue();
        }
    }
}
