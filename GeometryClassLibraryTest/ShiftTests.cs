using NUnit.Framework;
using UnitClassLibrary;
using FluentAssertions;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class ShiftTests
    {
        [Test()]
        public void Negate_Test()
        {
            var originalAngle = new Angle(AngleType.Degree, 45);
            var oppositeAngle = new Angle(AngleType.Degree, -45);

            var negatedOriginalAngle = originalAngle.Negate();
            var negatedNegatedOriginalAngle = negatedOriginalAngle.Negate();

            (originalAngle == negatedNegatedOriginalAngle).Should().BeTrue();
            (negatedOriginalAngle == oppositeAngle).Should().BeTrue();

        }
    }
}
