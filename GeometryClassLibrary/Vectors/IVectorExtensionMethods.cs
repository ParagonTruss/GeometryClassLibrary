using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public static class IVectorExtensionMethods
    {
        public static Unit DotProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        {
            var result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
            var unitType = new DerivedUnitType(vector1.UnitType.Dimensions.Multiply(vector2.UnitType.Dimensions));
            return new Unit<DerivedUnitType>(unitType, result);
        }

        public static Vector_New<DerivedUnitType> CrossProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
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

            var newDimensions = vector1.UnitType.Dimensions.Multiply(vector2.UnitType.Dimensions);
            var newUnitType = new DerivedUnitType(newDimensions);
            return new Vector_New<DerivedUnitType>(newUnitType, newX, newY, newZ);
        }
    }
}
