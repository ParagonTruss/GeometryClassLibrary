using System.Collections.Generic;
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
        [Test]
        public void Paragon_MinBy()
        {
            new List<Plane>{Plane.XY,Plane.XZ,Plane.YZ}.MinByUnit(plane => plane.NormalVector.DotProduct(new Vector(Line.XAxis, Distance.QuarterInch)));
        }
    }
}
