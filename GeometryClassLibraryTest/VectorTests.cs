using NUnit.Framework;
using FluentAssertions;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void Vector_DotProductIsEqualToZero()
        {
            //by default the DotProductIsEqualToZero should have a equality range of 0.625% when using an accepted
            //deviation contstant of 0.03125(1/32) for inches 

            //test one that should be and then test one that is close but should not be
            Vector vector1 = new Vector(PointGenerator.MakePointWithInches(0, 2, 4));
            Vector vector2 = new Vector(PointGenerator.MakePointWithInches(-4, 3, 2), PointGenerator.MakePointWithInches(0, 1, 3));
            Vector vectorNear = new Vector(PointGenerator.MakePointWithInches(-4, -2 - 2 * .625, 1));

            vector1.DotProductIsEqualToZero(vector2).Should().BeTrue();
            vector1.DotProductIsEqualToZero(vectorNear).Should().BeFalse();

            //now test small ones
            Vector vector1Small = new Vector(PointGenerator.MakePointWithInches(0, .2, .4));
            Vector vector2Small = new Vector(PointGenerator.MakePointWithInches(-.4, .3, .2), PointGenerator.MakePointWithInches(0, .1, .3));
            Vector vectorNearSmall = new Vector(PointGenerator.MakePointWithInches(-.4, -.200 - .20 * .625, .1));

            vector1Small.DotProductIsEqualToZero(vector2Small).Should().BeTrue();
            vector1Small.DotProductIsEqualToZero(vectorNearSmall).Should().BeFalse();

            //now test large ones
            Vector vector1Large = new Vector(PointGenerator.MakePointWithInches(0, 200, 400));
            Vector vector2Large = new Vector(PointGenerator.MakePointWithInches(-400, 300, 200), PointGenerator.MakePointWithInches(0, 100, 300));
            Vector vectorNearLarge = new Vector(PointGenerator.MakePointWithInches(-400, -200 - 200 * .625, 100));

            vector1Large.DotProductIsEqualToZero(vector2Large).Should().BeTrue();
            vector1Large.DotProductIsEqualToZero(vectorNearLarge).Should().BeFalse();


            //now test mixed sized ones
            vector1.DotProductIsEqualToZero(vector2Small).Should().BeTrue();
            vector1.DotProductIsEqualToZero(vector2Large).Should().BeTrue();
            vector1Small.DotProductIsEqualToZero(vector2Large).Should().BeTrue();

            vector1.DotProductIsEqualToZero(vectorNearLarge).Should().BeFalse();
            vector1.DotProductIsEqualToZero(vectorNearSmall).Should().BeFalse();
            vector1Small.DotProductIsEqualToZero(vectorNearLarge).Should().BeFalse();
        }
    }
}
