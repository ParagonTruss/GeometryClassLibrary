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
        /// Constructs the rectangular prism between the origin and the given point as opposite corners.
        /// </summary>
        /// <param name="point"></param>
        public RectangularPrism(Point point) : this(point.X, point.Y, point.Z){ }

        /// <summary>
        /// Creates a rectangular prism with the given Distances in the x,y,z directions
        /// </summary>
        public RectangularPrism(Distance length, Distance width, Distance height)
        {
            Distance zero = new Distance();
            Point basePoint1 = new Point();
            Point basePoint2 = new Point(zero, width, zero);
            Point basePoint3 = new Point(length, width, zero);
            Point basePoint4 = new Point(length, zero, zero);
            Point topPoint1 = new Point(zero, zero, height);
            Point topPoint2 = new Point(zero, width, height);
            Point topPoint3 = new Point(length, width, height);
            Point topPoint4 = new Point(length, zero, height);

            //Note: the ordering on these vertices is very important.
            //Each face has positive orientation (normal vector pointing outward.)
            //Its setup here so that we don't have to call the time consuming polyhedron constructor.
            List<Polygon> faces = new List<Polygon>();
            faces.Add(new Polygon(new List<Point> { basePoint1, basePoint2, basePoint3, basePoint4}));
            faces.Add(new Polygon(new List<Point> { topPoint4, topPoint3, topPoint2, topPoint1 }));
            faces.Add(new Polygon(new List<Point> { basePoint1, topPoint1, topPoint2, basePoint2}));
            faces.Add(new Polygon(new List<Point> { basePoint2, topPoint2, topPoint3, basePoint3 }));
            faces.Add(new Polygon(new List<Point> { basePoint3, topPoint3, topPoint4, basePoint4 }));
            faces.Add(new Polygon(new List<Point> { basePoint4, topPoint4, topPoint1, basePoint1 }));
            this.Polygons = faces; // we bypass the polyhedron constructor because we know the prism forms a a single bounded region
        }
    }
}
