using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public interface IVector<T> where T : IUnitType
    {
        T UnitType { get; }
        Measurement X { get; }
        Measurement Y { get; }
        Measurement Z { get; }
        //Point ApplicationPoint { get; }

        Unit Magnitude { get; }
        Direction Direction { get; }
    }
}
