using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class Rectangle : Polygon
    {
        public Rectangle(Vector baseSegment, Distance height, Vector referencePlaneNormal = null)
            : base(_makeRectangle(baseSegment, height, referencePlaneNormal)) { }

        private static Polygon _makeRectangle(Vector baseSegment, Distance height, Vector referencePlaneNormal)
        {
            if (referencePlaneNormal == null)
            {
                referencePlaneNormal = new Vector(Point.MakePointWithInches(0, 0, 1));
            }

            Vector heightVector = referencePlaneNormal.CrossProduct(baseSegment);
            heightVector.Magnitude = height;
            Polygon polygon = Polygon.MakeParallelogram(baseSegment, heightVector);

            return polygon;
        }
    }
}
