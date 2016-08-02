using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{

    public class StubPolyhedron4 : Polyhedron
    {
        public StubPolyhedron4()
        {
            Point basePoint = Point.Origin;
            Point topLeftPoint = Point.MakePointWithInches(0, 12, 0);
            Point bottomRightPoint = Point.MakePointWithInches(4, 0, 0);
            Point topRightPoint = Point.MakePointWithInches(4, 12, 0);

            Point backbasepoint = Point.MakePointWithInches(0, 0, 2);
            Point backtopleftpoint = Point.MakePointWithInches(0, 12, 2);
            Point backbottomrightpoint = Point.MakePointWithInches(4, 0, 2);
            Point backtoprightpoint = Point.MakePointWithInches(4, 12, 2);

            List<Polygon> planes = new List<Polygon>();
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            planes.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            planes.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            planes.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            Polyhedron testPolyhedron = new Polyhedron(planes);
            //Polyhedron_SimpleSlice()
            //Polyhedron_DiagonalSlice()
            //Polyhedron_MultiSlice()
            //Polyhedron_SliceAtVertex()
            //Polyhedron_DoesContainPointAlongSides()
            //Polyhedron_vertices()

        }
    }
}
