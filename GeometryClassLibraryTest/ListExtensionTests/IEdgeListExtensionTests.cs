using System.Collections.Generic;
using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using static GeometryClassLibrary.Point;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class IEdgeListExtensionTests
    {
        [Test]
        public void IEdge_Shift()
        {
            //make a bunch of edges and shift 'em
            List<IEdge> edges = new List<IEdge>();
            edges.Add(new LineSegment( MakePointWithInches(1, 2, 3)));
            edges.Add(new Arc( MakePointWithInches(5,4,0), MakePointWithInches(2,-1,0), Direction.Right));

            List<IEdge> results = edges.Shift(new Shift(MakePointWithInches(2,0,0)));

            (results.Contains(new LineSegment( MakePointWithInches(2, 0, 0),  MakePointWithInches(3, 2, 3)))).Should().BeTrue();
            (results.Contains(new Arc( MakePointWithInches(7, 4, 0),  MakePointWithInches(4, -1, 0), Direction.Right))).Should().BeTrue();
        }
    }
}
