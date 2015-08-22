using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture()]
    public class RegularPolygonTests
    {
        [Test()]
        public void RegularPolygon_Constructor()
        {
            Polygon polygon = new RegularPolygon(4, Distance.Foot * 4);
            polygon.IsRectangle().Should().BeTrue();
            polygon.Area.Should().Be(new Area(AreaType.FeetSquared, 16));
        }
    }
}
