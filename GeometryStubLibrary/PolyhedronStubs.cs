using System.Collections.Generic;
using GeometryClassLibrary;
using UnitClassLibrary;

namespace GeometryStubLibrary
{
    public class TestRectangularBox1 : RectangularPrism
    {
        public override Volume Volume { get { return new Volume(VolumeType.CubicInches, 96); } }
        public override Point Centroid { get { return PointGenerator.MakePointWithInches(2, 6, 1); } }
        public TestRectangularBox1() : base(PointGenerator.MakePointWithInches(4, 12, 2)) { }
    }

    public class TestRectangularBox2 : RectangularPrism
    {
        public override Volume Volume { get { return new Volume(VolumeType.CubicInches, 96); } }
        public override Point Centroid  { get { return PointGenerator.MakePointWithInches(2, 4, 1.5); } }
        public TestRectangularBox2() : base(PointGenerator.MakePointWithInches(4, 8, 3)) { }
    }

    public class TestTetrahedron : Polyhedron
    {
        public override Volume Volume { get { return new Volume(VolumeType.CubicInches, 48); } }
        public override Point Centroid { get { return PointGenerator.MakePointWithInches(1, 3, 1.5); } }
        public TestTetrahedron()
            : base(_makeFaces())
        {

        }
        private static List<Polygon> _makeFaces()
        {
            Point basePoint1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point basePoint2 = PointGenerator.MakePointWithInches(0, 12, 0);
            Point basePoint3 = PointGenerator.MakePointWithInches(4, 0, 0);
            Point peakPoint = PointGenerator.MakePointWithInches(0, 0, 6);

            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3 }));
            faces.Add(new Polygon(new List<Point> { basePoint1, basePoint2, peakPoint }));
            faces.Add(new Polygon(new List<Point> { basePoint1, basePoint3, peakPoint }));
            faces.Add(new Polygon(new List<Point> { basePoint2, basePoint3, peakPoint }));

            return faces;
        }

    }

    public class TestConcavePentagonalPrism : Polyhedron
    {
        public override Volume Volume { get { return new Volume(VolumeType.CubicInches, 18); } }
        public override Point Centroid { get { return PointGenerator.MakePointWithInches(11.0 / 9, 1, 2); } }
        public TestConcavePentagonalPrism()
            : base(_makeFaces())
        {

        }
        private static List<Polygon> _makeFaces()
        {
            Point basePoint1 = PointGenerator.MakePointWithInches(0, 0, -1);
            Point basePoint2 = PointGenerator.MakePointWithInches(2, 0, -1);
            Point basePoint3 = PointGenerator.MakePointWithInches(2, 2, -1);
            Point basePoint4 = PointGenerator.MakePointWithInches(0, 2, -1);
            Point basePoint5 = PointGenerator.MakePointWithInches(1, 1, -1);
            Point topPoint1 = PointGenerator.MakePointWithInches(0, 0, 5);
            Point topPoint2 = PointGenerator.MakePointWithInches(2, 0, 5);
            Point topPoint3 = PointGenerator.MakePointWithInches(2, 2, 5);
            Point topPoint4 = PointGenerator.MakePointWithInches(0, 2, 5);
            Point topPoint5 = PointGenerator.MakePointWithInches(1, 1, 5);

            Polygon bottomFace = new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4, basePoint5 });
            Polygon topFace = new Polygon(new List<Point> { topPoint1, topPoint2, topPoint3, topPoint4, topPoint5 });

            Polygon sideFace1 = new Polygon(new List<Point> { basePoint1, basePoint2, topPoint2, topPoint1 });
            Polygon sideFace2 = new Polygon(new List<Point> { basePoint2, basePoint3, topPoint3, topPoint2 });
            Polygon sideFace3 = new Polygon(new List<Point> { basePoint3, basePoint4, topPoint4, topPoint3 });
            Polygon sideFace4 = new Polygon(new List<Point> { basePoint4, basePoint5, topPoint5, topPoint4 });
            Polygon sideFace5 = new Polygon(new List<Point> { basePoint5, basePoint1, topPoint1, topPoint5 });

            List<Polygon> faces = new List<Polygon>();
            faces.Add(bottomFace);
            faces.Add(topFace);
            faces.Add(sideFace1);
            faces.Add(sideFace2);
            faces.Add(sideFace3);
            faces.Add(sideFace4);
            faces.Add(sideFace5);


            return faces;
        }
    }

    public class ConcaveDecahedron : Polyhedron
    {
        public ConcaveDecahedron() : base(_makeFaces()) { }

        private static List<Polygon> _makeFaces()
        {
            Point bottomPoint1 = PointGenerator.MakePointWithInches(0, 0, 0);
            Point bottomPoint2 = PointGenerator.MakePointWithInches(0, 4, 0);
            Point bottomPoint3 = PointGenerator.MakePointWithInches(4, 4, 0);
            Point bottomPoint4 = PointGenerator.MakePointWithInches(4, 0, 0);
            Point middlePoint1 = PointGenerator.MakePointWithInches(1, 1, 5);
            Point middlePoint2 = PointGenerator.MakePointWithInches(1, 3, 5);
            Point middlePoint3 = PointGenerator.MakePointWithInches(3, 3, 5);
            Point middlePoint4 = PointGenerator.MakePointWithInches(3, 1, 5);
            Point topPoint1 = PointGenerator.MakePointWithInches(0, 0, 10);
            Point topPoint2 = PointGenerator.MakePointWithInches(0, 4, 10);
            Point topPoint3 = PointGenerator.MakePointWithInches(4, 4, 10);
            Point topPoint4 = PointGenerator.MakePointWithInches(4, 0, 10);

            Polygon bottomFace = new Polygon(new List<Point>() { bottomPoint1, bottomPoint2, bottomPoint3, bottomPoint4 });
            Polygon topFace = new Polygon(new List<Point>() { topPoint1, topPoint2, topPoint3, topPoint4 });
            Polygon bottomSideFace1 = new Polygon(new List<Point>() { bottomPoint1, bottomPoint2, middlePoint2, middlePoint1 });
            Polygon bottomSideFace2 = new Polygon(new List<Point>() { bottomPoint2, bottomPoint3, middlePoint3, middlePoint2 });
            Polygon bottomSideFace3 = new Polygon(new List<Point>() { bottomPoint3, bottomPoint4, middlePoint4, middlePoint3 });
            Polygon bottomSideFace4 = new Polygon(new List<Point>() { bottomPoint4, bottomPoint1, middlePoint1, middlePoint4 });
            Polygon topSideFace1 = new Polygon(new List<Point>() { topPoint1, topPoint2, middlePoint2, middlePoint1 });
            Polygon topSideFace2 = new Polygon(new List<Point>() { topPoint2, topPoint3, middlePoint3, middlePoint2 });
            Polygon topSideFace3 = new Polygon(new List<Point>() { topPoint3, topPoint4, middlePoint4, middlePoint3 });
            Polygon topSideFace4 = new Polygon(new List<Point>() { topPoint4, topPoint1, middlePoint1, middlePoint4 });

            return new List<Polygon>(){ bottomFace, topFace,
                                        bottomSideFace1, bottomSideFace2, bottomSideFace3, bottomSideFace4, 
                                        topSideFace1, topSideFace2, topSideFace3, topSideFace4 };

        }
    }
}
