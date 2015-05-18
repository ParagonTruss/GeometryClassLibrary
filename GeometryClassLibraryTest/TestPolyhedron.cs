using GeometryClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibraryTest
{
    
    public class TestRectangularBox : Polyhedron
    {
        public static Volume ExpectedVolume = new Volume(VolumeType.CubicInches, 96);
         public TestRectangularBox() : base(_makeFaces())
        {

        }

        private static List<Polygon> _makeFaces()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(4, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(4, 12, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(4, 12, 2);

            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            faces.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            faces.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            faces.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            faces.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            faces.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            return faces;
        }
    }
    public class TestTetrahedron : Polyhedron
    {
        public static Volume ExpectedVolume = new Volume(VolumeType.CubicInches, 144);
        public TestTetrahedron() : base(_makeFaces())
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
        public static Volume ExpectedVolume = new Volume(VolumeType.CubicInches, 18);
        public TestConcavePentagonalPrism() : base(_makeFaces())
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
}
