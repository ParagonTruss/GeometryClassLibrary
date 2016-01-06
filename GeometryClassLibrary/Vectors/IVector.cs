using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;


namespace GeometryClassLibrary.Vectors
{
    public interface IMeasurementVector
    {
        Measurement X { get; }
        Measurement Y { get; }
        Measurement Z { get; }    
    }
    //public interface IVector<T> : IVector where T : IUnitType
    public interface IVector<T> where T : IUnitType
    {
        T UnitType { get; }
        Point ApplicationPoint { get; }
        IMeasurementVector UnitLessVector { get; }
        //Direction Direction { get; }
        //Unit<T> Magnitude { get; }      
    }
}
