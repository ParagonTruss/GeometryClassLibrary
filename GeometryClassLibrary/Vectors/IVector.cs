using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public interface IVector
    {

    }
    
    public interface IVector<T> where T :IUnitType
    {
        Unit<T> Magnitude { get; }
        Direction Direction { get; }
        IVector<DerivedUnitType> CrossProduct(IVector vector);
        Unit<DerivedUnitType> DotProduct(IVector vector);
        
    }

    public class Vector<T> : IVector where T : IUnitType
    {
        public Unit<T> Magnitude { get; private set; }
        public Direction Direction { get; private set; }
    }

    public static class IVectorExtensionMethods
    {
        public static IVector<DerivedUnitType> CrossProduct(this IVector vector1, IVector vector2)
        {
            
        }
    }
}
