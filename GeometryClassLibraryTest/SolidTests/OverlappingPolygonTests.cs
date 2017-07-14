using FluentAssertions;
using GeometryClassLibrary;
using NUnit.Framework;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibraryTest
{
    [TestFixture]
    public class OverlappingPolygonTests
    {
        [Test]
        public void T_1_And_W_6_Should_Overlap()
        {
            var T_1 = new Polyhedron(true, new[]
            {
                new Polygon(true,
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5)),
                new Polygon(true,
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0)),
                new Polygon(true,
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0))
            });
            
            var W_6 = new Polyhedron(true, new[]
            {
                new Polygon(true,
                    new Point(Inches, 286.2499999998, 95.667, 0),
                    new Point(Inches, 287.9999999998, 96.25, 0),
                    new Point(Inches, 289.7499999998, 95.667, 0),
                    new Point(Inches, 289.75, 3.5, 0),
                    new Point(Inches, 286.25, 3.5, 0)),
                new Polygon(true,
                    new Point(Inches, 287.9999999998, 96.25, 0),
                    new Point(Inches, 286.2499999998, 95.667, 0),
                    new Point(Inches, 286.2499999998, 95.667, 1.5),
                    new Point(Inches, 287.9999999998, 96.25, 1.5)),
                new Polygon(true,
                    new Point(Inches, 289.7499999998, 95.667, 0),
                    new Point(Inches, 287.9999999998, 96.25, 0),
                    new Point(Inches, 287.9999999998, 96.25, 1.5),
                    new Point(Inches, 289.7499999998, 95.667, 1.5)),
                new Polygon(true,
                    new Point(Inches, 289.75, 3.5, 0),
                    new Point(Inches, 289.7499999998, 95.667, 0),
                    new Point(Inches, 289.7499999998, 95.667, 1.5),
                    new Point(Inches, 289.75, 3.5, 1.5)),
                new Polygon(true,
                    new Point(Inches, 286.25, 3.5, 0),
                    new Point(Inches, 289.75, 3.5, 0),
                    new Point(Inches, 289.75, 3.5, 1.5),
                    new Point(Inches, 286.25, 3.5, 1.5)),
                new Polygon(true,
                    new Point(Inches, 286.2499999998, 95.667, 0),
                    new Point(Inches, 286.25, 3.5, 0),
                    new Point(Inches, 286.25, 3.5, 1.5),
                    new Point(Inches, 286.2499999998, 95.667, 1.5)),
                new Polygon(true,
                    new Point(Inches, 287.9999999998, 96.25, 1.5),
                    new Point(Inches, 286.2499999998, 95.667, 1.5),
                    new Point(Inches, 286.25, 3.5, 1.5),
                    new Point(Inches, 289.75, 3.5, 1.5),
                    new Point(Inches, 289.7499999998, 95.667, 1.5))
            });

            var webFirst = W_6.OverlappingPolygon(T_1);
            var chordFirst = T_1.OverlappingPolygon(W_6);

            webFirst.Should().NotBe(null);
            chordFirst.Should().NotBe(null);
            webFirst.Equals(chordFirst).Should().BeTrue();
        }
        
        [Test]
        public void T_1_And_W_5_Should_Overlap()
        {
            var T_1 = new Polyhedron(true, new[]
            {
                new Polygon(true,
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 287.9999999972, 96.249999998778, 1.5)),
                new Polygon(true,
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 1.5),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0)),
                new Polygon(true,
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 1.5),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0)),
                new Polygon(true,
                    new Point(Inches, 287.9999999972, 96.249999998778, 0),
                    new Point(Inches, 106.960000000064, 35.9029999999812, 0),
                    new Point(Inches, 105.853000000065, 39.2239999999352, 0),
                    new Point(Inches, 287.999999997234, 99.9389999987189, 0))
            });
            var W_5 = new Polyhedron(true, new[]
            {
                new Polygon(true,
                    new Point(Inches, 218.437999999992, 73.0629999999931, 1.5),
                    new Point(Inches, 218.437999999966, 70.5560000000604, 1.5),
                    new Point(Inches, 283.805999999169, 3.49999999973778, 1.5),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 1.5),
                    new Point(Inches, 286.24999999922, 6.00699999967618, 1.5),
                    new Point(Inches, 220.281999999948, 73.6769999999957, 1.5)),
                new Polygon(true,
                    new Point(Inches, 218.437999999966, 70.5560000000604, 0),
                    new Point(Inches, 218.437999999966, 70.5560000000604, 1.5),
                    new Point(Inches, 218.437999999992, 73.0629999999931, 1.5),
                    new Point(Inches, 218.437999999992, 73.0629999999931, 0)),
                new Polygon(true,
                    new Point(Inches, 218.437999999966, 70.5560000000604, 0),
                    new Point(Inches, 283.805999999169, 3.49999999973778, 0),
                    new Point(Inches, 283.805999999169, 3.49999999973778, 1.5),
                    new Point(Inches, 218.437999999966, 70.5560000000604, 1.5)),
                new Polygon(true,
                    new Point(Inches, 283.805999999169, 3.49999999973778, 0),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 0),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 1.5),
                    new Point(Inches, 283.805999999169, 3.49999999973778, 1.5)),
                new Polygon(true,
                    new Point(Inches, 286.24999999922, 6.00699999967618, 0),
                    new Point(Inches, 286.24999999922, 6.00699999967618, 1.5),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 1.5),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 0)),
                new Polygon(true,
                    new Point(Inches, 220.281999999948, 73.6769999999957, 0),
                    new Point(Inches, 220.281999999948, 73.6769999999957, 1.5),
                    new Point(Inches, 286.24999999922, 6.00699999967618, 1.5),
                    new Point(Inches, 286.24999999922, 6.00699999967618, 0)),
                new Polygon(true,
                    new Point(Inches, 218.437999999992, 73.0629999999931, 0),
                    new Point(Inches, 218.437999999992, 73.0629999999931, 1.5),
                    new Point(Inches, 220.281999999948, 73.6769999999957, 1.5),
                    new Point(Inches, 220.281999999948, 73.6769999999957, 0)),
                new Polygon(true,
                    new Point(Inches, 218.437999999966, 70.5560000000604, 0),
                    new Point(Inches, 218.437999999992, 73.0629999999931, 0),
                    new Point(Inches, 220.281999999948, 73.6769999999957, 0),
                    new Point(Inches, 286.24999999922, 6.00699999967618, 0),
                    new Point(Inches, 286.249999999121, 3.49999999967368, 0),
                    new Point(Inches, 283.805999999169, 3.49999999973778, 0))
            });

            var webFirst = W_5.OverlappingPolygon(T_1);
            var chordFirst = T_1.OverlappingPolygon(W_5);

            webFirst.Should().NotBe(null);
            chordFirst.Should().NotBe(null);
            webFirst.Equals(chordFirst).Should().BeTrue();
        }
        
    }
}