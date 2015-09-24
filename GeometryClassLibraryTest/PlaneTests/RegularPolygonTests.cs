using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;
using System.Linq;
using MoreLinq;
using static UnitClassLibrary.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class RegularPolygonTests
    {
        [Test()]
        public void RegularPolygon_Constructor()
        {
            Polygon polygon = new RegularPolygon(4, 4*Foot);
            polygon.IsRectangle().Should().BeTrue();
            polygon.Area.Should().Be(new Area(AreaType.FeetSquared, 16));
        }

        [Test()]
        public void RegularPentagon()
        {
            Polygon pentagon = Polygon.Pentagon(5 * Inch);
            pentagon.Area.Should().Be(new Area(AreaType.InchesSquared, 43));

            //verify the bottom segment is flat and symmetric about zero.
            var bottomSegment =  pentagon.LineSegments.MinBy(s => s.MidPoint.Y);
            (bottomSegment.BasePoint.X == -1 * bottomSegment.EndPoint.X).Should().BeTrue();
            (bottomSegment.BasePoint.Y == bottomSegment.EndPoint.Y).Should().BeTrue();
            
            //verify the polygon is on the XY plane
            foreach(var vertex in pentagon.Vertices)
            {
                (vertex.Z == Distance.Zero).Should().BeTrue();
            }

            //verify the polygon is centered on the origin
            (pentagon.Centroid == Point.Origin).Should().BeTrue();
        }
    }
}
