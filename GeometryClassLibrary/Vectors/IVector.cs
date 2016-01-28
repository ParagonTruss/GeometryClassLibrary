using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    public interface IVector<T> where T : IUnitType
    {
        T UnitType { get; }
        Point ApplicationPoint { get; }
        IMeasurementVector MeasurementVector { get; }
        Direction Direction { get; }
        //Unit<T> Magnitude { get; }
    }
}
