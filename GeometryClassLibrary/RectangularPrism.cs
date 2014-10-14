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
    [Serializable]
    public class RectangularPrism : Polyhedron
    {

        public RectangularPrism(Dimension passedWidth, Dimension passedHeight, Dimension passedLength)
        {
            Point basePoint = PointGenerator.MakePointWithInches(0, 0, 0);
            Point topLeftPoint = PointGenerator.MakePointWithInches(0, passedLength.Inches, 0);
            Point bottomRightPoint = PointGenerator.MakePointWithInches(passedWidth.Inches, 0, 0);
            Point topRightPoint = PointGenerator.MakePointWithInches(passedWidth.Inches, passedLength.Inches, 0);

            Point backbasepoint = PointGenerator.MakePointWithInches(0, 0, passedHeight.Inches);
            Point backtopleftpoint = PointGenerator.MakePointWithInches(0, passedLength.Inches, passedHeight.Inches);
            Point backbottomrightpoint = PointGenerator.MakePointWithInches(passedWidth.Inches, 0, passedHeight.Inches);
            Point backtoprightpoint = PointGenerator.MakePointWithInches(passedWidth.Inches, passedLength.Inches, passedHeight.Inches);

            this.Polygons.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, topRightPoint, bottomRightPoint }));
            this.Polygons.Add(new Polygon(new List<Point> { backbasepoint, backtopleftpoint, backtoprightpoint, backbottomrightpoint }));
            this.Polygons.Add(new Polygon(new List<Point> { topLeftPoint, topRightPoint, backtoprightpoint, backtopleftpoint }));
            this.Polygons.Add(new Polygon(new List<Point> { basePoint, bottomRightPoint, backbottomrightpoint, backbasepoint }));
            this.Polygons.Add(new Polygon(new List<Point> { basePoint, topLeftPoint, backtopleftpoint, backbasepoint }));
            this.Polygons.Add(new Polygon(new List<Point> { bottomRightPoint, topRightPoint, backtoprightpoint, backbottomrightpoint }));
        }
    }
}
