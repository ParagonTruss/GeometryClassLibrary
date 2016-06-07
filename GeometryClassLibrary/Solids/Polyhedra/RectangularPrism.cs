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

using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A prism is "a solid geometric figure whose two end faces are similar, equal, and parallel rectilinear figures, and whose sides are parallelograms."
    /// </summary>
    public class RectangularPrism : Polyhedron
    {
        #region Properties

        #endregion

        /// <summary>
        /// Protected null constructor for the use of data frameworks like Entity Framework and Json.NET
        /// </summary>
        protected RectangularPrism() { }

        public RectangularPrism(Rectangle rectangle, Distance height) : base(_makePrismFromRectangle(rectangle, height)) { }

        private static Polyhedron _makePrismFromRectangle(Rectangle rectangle, Distance height)
        {
            var prism = rectangle.Extrude(rectangle.NormalVector * height.InInches);
            return prism;
        }

        /// <summary>
        /// Constructs the rectangular prism between the two passed points and the given point as opposite corners.
        /// </summary>
        public RectangularPrism(Point oppositeCorner, Point basePoint = null) : this(oppositeCorner.X, oppositeCorner.Y, oppositeCorner.Z, basePoint) { }

        /// <summary>
        /// Creates a rectangular prism with the given Distances in the x,y,z directions
        /// </summary>
        public RectangularPrism(Distance length, Distance width, Distance height, Point basePoint = null) : base(_makeSolid(length, width, height, basePoint)) { }

        private static Polyhedron _makeSolid(Distance length, Distance width, Distance height, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = Point.Origin;
            }
            Distance zero = Distance.ZeroDistance;

            Vector vector1 = new Vector(new Point(zero, width, zero));
            Vector vector2 = new Vector(new Point(length, zero, zero));
            Vector vector3 = new Vector(new Point(zero, zero, height));

            var solid = MakeParallelepiped(vector1, vector2, vector3, basePoint);
            return solid;
        }
    }
}
