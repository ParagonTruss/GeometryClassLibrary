using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    public static class IUnitLessVectorExtensions
    {
        public static Measurement Magnitude(this IUnitLessVector vector)
        {
            var result = (vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z).SquareRoot();
            return result;
        }

        public static Direction Direction(this IUnitLessVector vector)
        {
            return new Direction(vector.X, vector.Y, vector.Z);
        }

        public static Measurement DotProduct(this IUnitLessVector vector1, IUnitLessVector vector2)
        {
            var xTerm = vector1.X * vector2.X;
            var yTerm = vector1.Y * vector2.Y;
            var zTerm = vector1.Z * vector2.Z;

            var sum = xTerm + yTerm + zTerm;
            return sum;
        }

        public static GenericVector CrossProduct(this IUnitLessVector vector1, IUnitLessVector vector2)
        {
            var x1 = vector1.X;
            var y1 = vector1.Y;
            var z1 = vector1.Z;
            var x2 = vector2.X;
            var y2 = vector2.Y;
            var z2 = vector2.Z;

            var newX = y1 * z2 - y2 * z1;
            var newY = z1 * x2 - z2 * x1;
            var newZ = x1 * y2 - x2 * y1;

            return new GenericVector(newX, newY, newZ);
        }
    }
}
