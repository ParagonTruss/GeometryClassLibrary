using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public interface IVector
    {
        Measurement X { get; }
        Measurement Y { get; }
        Measurement Z { get; }    
    }
    public interface IVector<T> : IVector where T : IUnitType
    {
        T UnitType { get; }

        Direction Direction { get; }
        Unit<T> Magnitude { get; }      
    }
}
