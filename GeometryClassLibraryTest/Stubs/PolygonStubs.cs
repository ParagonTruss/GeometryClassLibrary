using System;
using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    public class ConcaveQuadrilateral : Polygon
    {
        public ConcaveQuadrilateral()
            : base(_makeVertices())
        {

        }
        private static List<Point> _makeVertices()
        {
            List<Point> vertices = new List<Point>();
            vertices.Add(Point.MakePointWithInches(0, 1, 0));
            vertices.Add((Point.MakePointWithInches(4, 1, 0)));
            vertices.Add(Point.MakePointWithInches(4, 3, 0));
            vertices.Add(Point.MakePointWithInches(3, 2, 0));
            return vertices;
        }
    }

    public class ConcavePentagon : Polygon
    {
        public ConcavePentagon()
            : base(newConcavePentagon())
        {

        }
        private static Polygon newConcavePentagon()
        {
            Point point1 = Point.MakePointWithInches(0, 0, -1);
            Point point2 = Point.MakePointWithInches(2, 0, -1);
            Point point3 = Point.MakePointWithInches(2, 2, -1);
            Point point4 = Point.MakePointWithInches(0, 2, -1);
            Point point5 = Point.MakePointWithInches(1, 1, -1);

            Polygon concavePentagon = new Polygon(new List<Point> { point1, point2, point3, point4, point5 });
            return concavePentagon;
        }
    }
}

