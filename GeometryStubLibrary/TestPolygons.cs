using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryClassLibrary;
using UnitClassLibrary;

namespace GeometryStubLibrary
{
    class TestPolygons : Polygon
    {
        public TestPolygons()
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, 4, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(8, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(8, 4, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, -4);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, 4, -4);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(8, 0, -4);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(8, 4, -4);

            LineSegment left = new LineSegment(basePoint, topLeftPoint);
            LineSegment right = new LineSegment(bottomRightPoint, topRightPoint);
            LineSegment top = new LineSegment(topLeftPoint, topRightPoint);
            LineSegment bottom = new LineSegment(basePoint, bottomRightPoint);

            LineSegment backLeft = new LineSegment(backbasepoint, backtopleftpoint);
            LineSegment backRight = new LineSegment(backbottomrightpoint, backtoprightpoint);
            LineSegment backTop = new LineSegment(backtopleftpoint, backtoprightpoint);
            LineSegment backBottom = new LineSegment(backbasepoint, backbottomrightpoint);

            LineSegment topleftConnector = new LineSegment(topLeftPoint, backtopleftpoint);
            LineSegment toprightConnector = new LineSegment(topRightPoint, backtoprightpoint);
            LineSegment baseConnector = new LineSegment(basePoint, backbasepoint);
            LineSegment bottomRightConnector = new LineSegment(bottomRightPoint, backbottomrightpoint);

            Polygon frontRegion = new Polygon(new List<LineSegment> { left, top, bottom, right });
            Polygon backRegion = new Polygon(new List<LineSegment> { backLeft, backRight, backTop, backBottom });
            Polygon topRegion = new Polygon(new List<LineSegment> { top, backTop, topleftConnector, toprightConnector });
            Polygon bottomRegion = new Polygon(new List<LineSegment> { bottom, backBottom, baseConnector, bottomRightConnector });
            Polygon leftRegion = new Polygon(new List<LineSegment> { left, backLeft, baseConnector, topleftConnector });
            Polygon rightRegion = new Polygon(new List<LineSegment> { right, backRight, toprightConnector, bottomRightConnector });
        }
    }
}
