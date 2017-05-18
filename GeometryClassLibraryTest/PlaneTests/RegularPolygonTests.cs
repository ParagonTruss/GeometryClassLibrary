using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using MoreLinq;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.DerivedUnits.Area.AreaTypes.Imperial;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class RegularPolygonTests
    {
        [Test()]
        public void RegularPolygon_Constructor()
        {
            Polygon polygon = new RegularPolygon(4, new Distance(4, Feet));
            polygon.IsRectangle().Should().BeTrue();
            polygon.Area.Should().Be(new Area(new SquareFoot(), 16));
        }

        [Test()]
        public void RegularPentagon()
        {
            Distance sideLength = new Distance(5, Inches);
            Polygon pentagon = Polygon.Pentagon(sideLength);

            //Check each segment individually
            foreach(var segment in pentagon.LineSegments)
            {
                (segment.Length == sideLength).Should().BeTrue(); 
            }

            Area actualArea = pentagon.Area;
            Area expectedArea = new Area(new SquareInch(), 43);
            (actualArea == expectedArea).Should().BeTrue();

            //verify the bottom segment is flat and symmetric about zero.
            var bottomSegment =  pentagon.LineSegments.MinBy(s => s.MidPoint.Y);
            (bottomSegment.BasePoint.X == -1 * bottomSegment.EndPoint.X).Should().BeTrue();
            (bottomSegment.BasePoint.Y == bottomSegment.EndPoint.Y).Should().BeTrue();
            
            //verify the polygon is on the XY plane
            foreach(var vertex in pentagon.Vertices)
            {
                (vertex.Z == Distance.ZeroDistance).Should().BeTrue();
            }

            //verify the polygon is centered on the origin
            (pentagon.Centroid == Point.Origin).Should().BeTrue();
        }
    }
}
