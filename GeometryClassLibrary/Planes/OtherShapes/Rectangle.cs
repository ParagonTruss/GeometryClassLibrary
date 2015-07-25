using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{

    public class Rectangle : Parallelogram
    {
        public Rectangle(Vector baseSegment, Distance height, Vector referencePlaneNormal = null)
            : base(baseSegment, _getHeightVector(baseSegment, height, referencePlaneNormal)) { }

        private static Vector _getHeightVector(Vector baseSegment, Distance height, Vector referencePlaneNormal)
        {
            if (referencePlaneNormal == null)
            {
                referencePlaneNormal = new Vector(Point.MakePointWithInches(0, 0, 1));
            }

            Vector heightVector = referencePlaneNormal.CrossProduct(baseSegment);
           
            return heightVector;
        }
    }
}
