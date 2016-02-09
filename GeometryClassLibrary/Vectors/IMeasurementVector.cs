using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;


namespace GeometryClassLibrary
{
    public interface IMeasurementVector
    {
        Measurement X { get; }
        Measurement Y { get; }
        Measurement Z { get; }    
    }
    
}
