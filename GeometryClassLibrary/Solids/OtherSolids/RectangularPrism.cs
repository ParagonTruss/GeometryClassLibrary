using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A prism is "a solid geometric figure whose two end faces are similar, equal, and parallel rectilinear figures, and whose sides are parallelograms."
    /// </summary>

    public class RectangularPrism : Polyhedron
    {
        /// <summary>
        /// Creates a rectangular prism with the given Distances in the x,y,z directions
        /// </summary>

        public RectangularPrism(Distance passedWidth, Distance passedHeight, Distance passedLength )
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, passedHeight.Inches, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(passedWidth.Inches, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(passedWidth.Inches, passedHeight.Inches, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, passedLength.Inches);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, passedHeight.Inches, passedLength.Inches);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(passedWidth.Inches, 0, passedLength.Inches);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(passedWidth.Inches, passedHeight.Inches, passedLength.Inches);

            List<Polygon> polygonsMade = new List<Polygon>();
            polygonsMade.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            polygonsMade.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            polygonsMade.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            polygonsMade.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            polygonsMade.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            polygonsMade.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
            this.Polygons = polygonsMade;
        }
    }
}
