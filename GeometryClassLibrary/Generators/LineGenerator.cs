/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

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
