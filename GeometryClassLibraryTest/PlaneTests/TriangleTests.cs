using System;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;
using GeometryClassLibrary;

namespace GeometryClassLibraryTests
{
    [TestFixture()]
    public class TriangleTests
    {
        [Test()]
        public void RightTriangle_LineSegmentConstructorTest()
        {
            // Build right triangle with line segments
            LineSegment rt_sideOne = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(4, 1));
            LineSegment rt_sideTwo = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(2, 4));
            LineSegment rt_sideThree = new LineSegment(Point.MakePointWithInches(2, 4), Point.MakePointWithInches(4, 1));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);
        }

        [Test()]
        public void RightTriangle_PointConstructorTest()
        {
            // Build right triangle with line segments
            Point rt_cornerOne = new Point(Point.MakePointWithInches(2, 1));
            Point rt_cornerTwo = new Point(Point.MakePointWithInches(2, 4));
            Point rt_cornerThree = new Point(Point.MakePointWithInches(4, 1));
            List<Point> rt_sides = new List<Point>() { rt_cornerOne, rt_cornerTwo, rt_cornerThree };
            RightTriangle rt = new RightTriangle(rt_sides);
        }
        
        [Test()]
        [ExpectedException(typeof(ArgumentException))]
        public void RightTriangle_ConstructorTests_NoRightAngle()
        {
            // Build right triangle
            LineSegment rt_sideOne = new LineSegment(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(5, 1));
            LineSegment rt_sideTwo = new LineSegment(Point.MakePointWithInches(1, 1), Point.MakePointWithInches(4, 3));
            LineSegment rt_sideThree = new LineSegment(Point.MakePointWithInches(5, 1), Point.MakePointWithInches(4, 3));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);
        }

        [Test()]
        public void RightTriangle_AreaTest()
        {
            LineSegment rt_sideOne = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(4, 1));
            LineSegment rt_sideTwo = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(2, 4));
            LineSegment rt_sideThree = new LineSegment(Point.MakePointWithInches(2, 4), Point.MakePointWithInches(4, 1));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);

            (rt.Area == new Area(AreaType.InchesSquared, 3)).Should().BeTrue();
        }

        [Test()]
        public void RightTriangle_HypotenuseTest()
        {
            LineSegment rt_sideOne = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(4, 1));
            LineSegment rt_sideTwo = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(2, 4));
            LineSegment rt_sideThree = new LineSegment(Point.MakePointWithInches(2, 4), Point.MakePointWithInches(4, 1));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);

            (rt.Hypotenuse == rt_sideThree).Should().BeTrue();
        }

        [Test()]
        public void RightTriangle_LegsTest()
        {
            LineSegment rt_sideOne = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(4, 1));
            LineSegment rt_sideTwo = new LineSegment(Point.MakePointWithInches(2, 1), Point.MakePointWithInches(2, 4));
            LineSegment rt_sideThree = new LineSegment(Point.MakePointWithInches(2, 4), Point.MakePointWithInches(4, 1));
            List<LineSegment> rt_sides = new List<LineSegment>() { rt_sideOne, rt_sideTwo, rt_sideThree };
            RightTriangle rt = new RightTriangle(rt_sides);

            (rt.ShortLeg == rt_sideOne).Should().BeTrue();
            (rt.LongLeg == rt_sideTwo).Should().BeTrue();
        }
    }
}
