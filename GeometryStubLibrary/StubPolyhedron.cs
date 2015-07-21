using System.Collections.Generic;
using GeometryClassLibrary;

namespace GeometryStubLibrary
{
    public class StubPolyhedron1 : Polyhedron
    {
        public StubPolyhedron1()
        {
             List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(0, 8)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0), Point.MakePointWithInches(4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 0), Point.MakePointWithInches(4, 8)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(4, 8), Point.MakePointWithInches(0, 8)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);
            //Polyhedron_ShiftXYTest()
            //Polyhedron_ShiftYZTest()
        }
    
    }
   
        
    
    public class StubPolyhedron3 : Polyhedron
    {
        public StubPolyhedron3()
        {
            List<LineSegment> lineSegments = new List<LineSegment>();
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(0, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(0, 0, 0), Point.MakePointWithInches(8, 0, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 0, 0), Point.MakePointWithInches(8, 4, 0)));
            lineSegments.Add(new LineSegment(Point.MakePointWithInches(8, 4, 0), Point.MakePointWithInches(0, 4, 0)));
            Polyhedron polyhedron = new Polyhedron(lineSegments);
            //Polyhedron_MultiShiftReturnToOriginalTest()
            //Polyhedron_MultiShiftReturnToOriginalTest()
            //Polyhedron_ShiftTest_RotationOnly()
            //Polyhedron_ShiftTest_TranslationOnly()
            //Polyhedron_ShiftTest_RotateAndTranslate()
            //Polyhedron_ShiftTest_RotateAndTranslate_ThenReturnToOriginal()
            //Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate()
            //Polyhedron_ShiftTest_RotateNotThroughOriginAndTranslate_ThenReturnToOriginal()
            //

        }
    }
    public class StubPolyhedron4 : Polyhedron
    {
        public StubPolyhedron4()
        {
            Point basePoint = Point.MakePointWithInches(0, 0, 0);
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
    public class StubPolyhedron0 : Polyhedron
    {
        public StubPolyhedron0()
        {

        }
    }
   
}
