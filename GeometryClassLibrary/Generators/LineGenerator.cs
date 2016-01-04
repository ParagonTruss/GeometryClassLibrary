using System;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;

namespace GeometryClassLibrary.Generators
{
    public class LineGenerator
    {
        public static Line MakeLineThroughTwoPointsMadeFromInches2D(double x1, double y1, double x2, double y2)
        {
            return new Line(Point.MakePointWithInches(x1, y1), Point.MakePointWithInches(x2, y2));
        }

        public static Line MakeLineThroughTwoPointsMadeFromInches3D(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return new Line(Point.MakePointWithInches(x1, y1, z1), Point.MakePointWithInches(x2, y2, z2));
        }

        public static Line MakeLineOffsetByInchesAndParallel(Line originalLine, double passedOffset, Vector offsetDirection)
        {
            throw new NotImplementedException();
            //Vector shiftVector = new Vector(
            //return new Line(originalLine.BasePoint.Shift(offsetDirection, originalLine;
        }

        public static Line X_Axis()
        {
            return new Line(Point.Origin, new Point(new Distance(new Inch(), 1), Distance.ZeroDistance, Distance.ZeroDistance));
        }

        public static Line Y_Axis()
        {
            return new Line(Point.Origin, new Point(Distance.ZeroDistance, new Distance(new Inch(), 1), Distance.ZeroDistance));
        }

        public static Line Z_Axis()
        {
            return new Line(Point.Origin, new Point(Distance.ZeroDistance, Distance.ZeroDistance, new Distance(new Inch(), 1)));
        }
    }
}
