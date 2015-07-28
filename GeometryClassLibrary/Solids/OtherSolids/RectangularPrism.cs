﻿using System;
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
        #region Properties

        #endregion

        public RectangularPrism(Rectangle rectangle, Distance height) : base(_makePrismFromRectangle(rectangle, height)) { }

        private static Polyhedron _makePrismFromRectangle(Rectangle rectangle, Distance height)
        {
            var prism = rectangle.Extrude(rectangle.NormalVector * height.Inches);
            return prism;
        }

        /// <summary>
        /// Constructs the rectangular prism between the origin and the given point as opposite corners.
        /// </summary>
        /// <param name="point"></param>
        public RectangularPrism(Point oppositeCorner, Point basePoint = null) : this(oppositeCorner.X, oppositeCorner.Y, oppositeCorner.Z, basePoint) { }

        /// <summary>
        /// Creates a rectangular prism with the given Distances in the x,y,z directions
        /// </summary>
        public RectangularPrism(Distance length, Distance width, Distance height, Point basePoint = null) : base(_makeSolid(length, width, height, basePoint)) { }

        private static Polyhedron _makeSolid(Distance length, Distance width, Distance height, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = new Point();
            }
            Distance zero = new Distance();

            Vector vector1 = new Vector(new Point(zero, width, zero));
            Vector vector2 = new Vector(new Point(length, zero, zero));
            Vector vector3 = new Vector(new Point(zero, zero, height));

            var solid = Polyhedron.MakeParallelepiped(vector1, vector2, vector3, basePoint);
            return solid;
        }
    }
}
