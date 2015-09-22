using System;
using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryStubLibrary
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


    public class PolygonStubs : Polygon
    {
        /// <summary>
        /// 
        /// </summary>
        public PolygonStubs()
            : base(_makeLineSegments())
        {
            //appears in polygon rotate and round test
        }

        private static List<LineSegment> _makeLineSegments()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 0, 0), Point.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 4, 0), Point.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 4, 0), Point.MakePointWithInches(0, 0, 0)));
            return lineSegments;
        }


    }
    public class StubPolygon2 : Polygon
    {
        public StubPolygon2()
            : base(_makeLineSegments())
        {

        }

        private static List<LineSegment> _makeLineSegments()
        {
            List<LineSegment> lineSegments2 = new List<LineSegment>();
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(8, 0, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(8, 0, 0), Point.MakePointWithInches(4, -4, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(4, -4, 0), Point.MakePointWithInches(0, 0, 0)));
            return lineSegments2;
        }
    }
    public class OverlappedPolygon : Polygon
    {
        public OverlappedPolygon()
            : base(_makeLineSegments())
        {

        }

        private static List<LineSegment> _makeLineSegments()
        {
            List<LineSegment> lineSegments3 = new List<LineSegment>();
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(6, 6, 0), Point.MakePointWithInches(9, 9, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(9, 9, 0), Point.MakePointWithInches(12, 4, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(12, 4, 0), Point.MakePointWithInches(9, 10, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(9, 10, 0), Point.MakePointWithInches(6, 6, 0)));
            return lineSegments3;
        }
    }
    public class ExpectedOverlappedBoundariesPolygon : Polygon
    {
        public ExpectedOverlappedBoundariesPolygon()
            : base(_makeLineSegments())
        {

        }

        private static List<LineSegment> _makeLineSegments()
        {
            List<LineSegment> lineSegments4 = new List<LineSegment>();
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(6, 4, 0), Point.MakePointWithInches(8, 4, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(8, 4, 0), Point.MakePointWithInches(8, 2, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(8, 2, 0), Point.MakePointWithInches(6, 4, 0)));
            return lineSegments4;
        }
    }
    public class Polygon4 : Polygon
    {
        public Polygon4()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, 2, 0), Point.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(lineSegments);
            //Polygon_FindVertexNotOnTheGivenPlane()
            //Polygon_DoesShareOrContainSide()
            //Polygon_IntersectWithLine()

        }
    }
    public class Polygon5 : Polygon
    {
        public Polygon5()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, -5, -4), Point.MakePointWithInches(0, 0, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(-1, 5, 4)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 4), Point.MakePointWithInches(-2, 10, 8)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-2, 10, 8), Point.MakePointWithInches(1, -5, -4)));
            Polygon testPolygon = new Polygon(lineSegments);
            // appears polygon translate
        }
    }
    public class Polygon6 : Polygon
    {
        public Polygon6()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(-1, 5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(-4, 2, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-4, 2, 0), Point.MakePointWithInches(-5, 5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(-1, 5, 0), Point.MakePointWithInches(-5, 5, 0)));
            Polygon testPolygon = new Polygon(bounds);
            // appears in centroid
            //appears in polygon copy
            // appears in contains exclusive inclusive and touching point

        }
    }
    public class Polygon7 : Polygon
    {
        public Polygon7() : base(_segments()) { }

        private static List<LineSegment> _segments()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 2, 3), Point.MakePointWithInches(-3, -2, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(-3, -2, 0), Point.MakePointWithInches(1, 1, -1)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, 1, -1), Point.MakePointWithInches(0, 2, 3)));
            return lineSegments;
        }
    }
    public class Polygon8 : Polygon
    {
        public Polygon8()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 3, 1), Point.MakePointWithInches(4, 4, 1)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 3, 1), Point.MakePointWithInches(4, 3, 3)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 4, 1), Point.MakePointWithInches(4, 4, 3)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 3, 3), Point.MakePointWithInches(4, 4, 3)));
            Polygon testPolygon = new Polygon(bounds);
            // appears in polygon overlapping polygon one enclosed in other test
            // appears in polygon overlapping polygon intersecting boundries test
        }
    }
    public class Polygon9 : Polygon
    {
        public Polygon9()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 12, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 0, 0), Point.MakePointWithInches(4, 0, 4)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 12, 0), Point.MakePointWithInches(4, 12, 4)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 0, 4), Point.MakePointWithInches(4, 12, 4)));
            Polygon testPolygon2 = new Polygon(lineSegments);
            // appears in polygon overlapping polygon one enclosed in other test
            // appears in polygon overlapping polygon intersecting boundries test
        }
    }
    public class Polygon10 : Polygon
    {
        public Polygon10()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(4, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3, 0), Point.MakePointWithInches(4, 3, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(4, 1, 0), Point.MakePointWithInches(4, 3, 0)));
            Polygon testPolygon = new Polygon(bounds);
            //appears in polygon overlapping polygon
            // appears in Polygon area test
            // appears in polygon slice on line test
        }
    }
    public class Polygon11 : Polygon
    {
        public Polygon11()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, -1, 0), Point.MakePointWithInches(1, -1, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, -1, 0), Point.MakePointWithInches(3, 5, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(1, -1, 0), Point.MakePointWithInches(4, 5, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(3, 5, 0), Point.MakePointWithInches(4, 5, 0)));
            Polygon testPolygon2 = new Polygon(lineSegments);
            //appears in polygon overlapping polygon
        }
    }
    public class Polygon12 : Polygon
    {
        public Polygon12()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(2, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.MakePointWithInches(6, 1, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(6, 0, 0), Point.MakePointWithInches(0, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(6, 1, 0), Point.MakePointWithInches(6, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);
            // appears in polygon slice with line test
        }
    }
    public class Polygon13 : Polygon
    {
        public Polygon13()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 3.5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 3.5, 0), Point.MakePointWithInches(240, 3.5, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 3.5, 0), Point.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);
            // appears in polygon slice a case that didn't work before
        }
    }
    public class Polygon14 : Polygon
    {
        public Polygon14()
        {
            List<LineSegment> bounds = new List<LineSegment>();
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 0, 1.5)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(0, 0, 1.5), Point.MakePointWithInches(240, 0, 1.5)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 1.5), Point.MakePointWithInches(240, 0, 0)));
            bounds.Add(new LineSegment(Point.MakePointWithInches(240, 0, 0), Point.MakePointWithInches(0, 0, 0)));
            Polygon testPolygon = new Polygon(bounds);
            //appears in Polygon_SliceAnotherCaseThatDidntWorkBefore
        }
    }
    public class Polygon15 : Polygon
    {
        public Polygon15()
        {
            List<LineSegment> bounds1 = new List<LineSegment>();
            bounds1.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(3, 0, 0)));
            bounds1.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 2, 0)));
            bounds1.Add(new LineSegment(Point.MakePointWithInches(3, 0, 0), Point.MakePointWithInches(3, 2, 0)));
            bounds1.Add(new LineSegment(Point.MakePointWithInches(0, 2, 0), Point.MakePointWithInches(3, 2, 0)));
            Polygon testPolygon1 = new Polygon(bounds1);
            //Polygon_SharedPointNotOnThisPolygonsBoundary

        }
    }
    public class Polygon16 : Polygon
    {
        public Polygon16()
        {
            List<LineSegment> bounds2 = new List<LineSegment>();
            bounds2.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(3.5, 0, 0)));
            bounds2.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(2, 1, 0)));
            bounds2.Add(new LineSegment(Point.MakePointWithInches(3.5, 0, 0), Point.MakePointWithInches(3.5, 1, 0)));
            bounds2.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.MakePointWithInches(3.5, 1, 0)));
            Polygon testPolygon2 = new Polygon(bounds2);
            // Polygon_SharedPointNotOnThisPolygonsBoundary
        }
    }
    public class Polygon17 : Polygon
    {
        public Polygon17()
        {
            List<LineSegment> bounds3 = new List<LineSegment>();
            bounds3.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(5, 0, 0)));
            bounds3.Add(new LineSegment(Point.MakePointWithInches(2, 0, 0), Point.MakePointWithInches(2, 1, 0)));
            bounds3.Add(new LineSegment(Point.MakePointWithInches(5, 0, 0), Point.MakePointWithInches(5, 1, 0)));
            bounds3.Add(new LineSegment(Point.MakePointWithInches(2, 1, 0), Point.MakePointWithInches(5, 1, 0)));
            Polygon testPolygon3 = new Polygon(bounds3);
            // Polygon_SharedPointNotOnThisPolygonsBoundary
        }
    }
    public class Polygon18 : Polygon
    {
        public Polygon18()
        {
            List<LineSegment> bounds4 = new List<LineSegment>();
            bounds4.Add(new LineSegment(Point.MakePointWithInches(3, 0, 0), Point.MakePointWithInches(5, 0, 0)));
            bounds4.Add(new LineSegment(Point.MakePointWithInches(3, 0, 0), Point.MakePointWithInches(3, 1, 0)));
            bounds4.Add(new LineSegment(Point.MakePointWithInches(5, 0, 0), Point.MakePointWithInches(5, 1, 0)));
            bounds4.Add(new LineSegment(Point.MakePointWithInches(3, 1, 0), Point.MakePointWithInches(5, 1, 0)));
            Polygon testPolygon4 = new Polygon(bounds4);
            //Polygon_SharedPointNotOnThisPolygonsBoundary()
        }
    }
    public class Polygon19 : Polygon
    {
        public Polygon19()
        {
            List<LineSegment> bounds5 = new List<LineSegment>();
            bounds5.Add(new LineSegment(Point.MakePointWithInches(-1, 0, 0), Point.MakePointWithInches(9, 0, 0)));
            bounds5.Add(new LineSegment(Point.MakePointWithInches(-1, 0, 0), Point.MakePointWithInches(-1, .5, 0)));
            bounds5.Add(new LineSegment(Point.MakePointWithInches(9, 0, 0), Point.MakePointWithInches(9, .5, 0)));
            bounds5.Add(new LineSegment(Point.MakePointWithInches(-1, .5, 0), Point.MakePointWithInches(9, .5, 0)));
            Polygon testPolygon5 = new Polygon(bounds5);
            //Polygon_SharedPointNotOnThisPolygonsBoundary()
        }
    }
    public class Polygon20 : Polygon
    {
        public Polygon20()
        {
            List<LineSegment> bounds6 = new List<LineSegment>();
            bounds6.Add(new LineSegment(Point.MakePointWithInches(6, 6, 2), Point.MakePointWithInches(6, 5, 2)));
            bounds6.Add(new LineSegment(Point.MakePointWithInches(6, 6, 2), Point.MakePointWithInches(12, 0, 2)));
            bounds6.Add(new LineSegment(Point.MakePointWithInches(6, 5, 2), Point.MakePointWithInches(10, 1, 2)));
            bounds6.Add(new LineSegment(Point.MakePointWithInches(10, 1, 2), Point.MakePointWithInches(12, 0, 2)));
            Polygon testPolygon6 = new Polygon(bounds6);
            //Polygon_SharedPointNotOnThisPolygonsBoundary()
        }
    }
    public class Polygon21 : Polygon
    {
        public Polygon21()
        {
            List<LineSegment> bounds7 = new List<LineSegment>();
            bounds7.Add(new LineSegment(Point.MakePointWithInches(0, 0, 2), Point.MakePointWithInches(2, 1, 2)));
            bounds7.Add(new LineSegment(Point.MakePointWithInches(0, 0, 2), Point.MakePointWithInches(12, 0, 2)));
            bounds7.Add(new LineSegment(Point.MakePointWithInches(2, 1, 2), Point.MakePointWithInches(10, 1, 2)));
            bounds7.Add(new LineSegment(Point.MakePointWithInches(10, 1, 2), Point.MakePointWithInches(12, 0, 2)));
            Polygon testPolygon7 = new Polygon(bounds7);
            //Polygon_SharedPointNotOnThisPolygonsBoundary()
        }
    }
    public class Polygon22 : Polygon
    {
        public Polygon22()
        {
            List<LineSegment> lineSegments2 = new List<LineSegment>();
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-7, 1, 0), Point.MakePointWithInches(0, 2, 0)));
            lineSegments2.Add(new LineSegment(Point.MakePointWithInches(-7, 1, 0), Point.MakePointWithInches(0, 0, 0)));
            Polygon testExactSide = new Polygon(lineSegments2);
            //Polygon_DoesShareOrContainSide
        }
    }
    public class Polygon23 : Polygon
    {
        public Polygon23()
        {
            List<LineSegment> lineSegments3 = new List<LineSegment>();
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(0, 1, 0), Point.MakePointWithInches(0, 3, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(1, -2, 0), Point.MakePointWithInches(0, 3, 0)));
            lineSegments3.Add(new LineSegment(Point.MakePointWithInches(1, -2, 0), Point.MakePointWithInches(0, 1, 0)));
            Polygon testOverlappingSide = new Polygon(lineSegments3);
            //Polygon_DoesShareOrContainSide()
        }
    }
    public class Polygon24 : Polygon
    {
        public Polygon24()
        {
            List<LineSegment> lineSegments4 = new List<LineSegment>();
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(1, 1, 0), Point.MakePointWithInches(-1, 3, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(2, -3, 0), Point.MakePointWithInches(-1, 3, 0)));
            lineSegments4.Add(new LineSegment(Point.MakePointWithInches(2, -3, 0), Point.MakePointWithInches(1, 1, 0)));
            Polygon testIntersecting = new Polygon(lineSegments4);
            //Polygon_DoesShareOrContainSide()
        }
    }

}

