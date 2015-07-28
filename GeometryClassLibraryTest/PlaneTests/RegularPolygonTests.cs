using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;
using GeometryClassLibrary;
using NUnit;
using FluentAssertions;
using NUnit.Framework;

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
