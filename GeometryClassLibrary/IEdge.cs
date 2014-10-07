using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public interface IEdge
    {
        Vector DirectionVector { get; set; }
        Point BasePoint { get; set; }
        IEdge Shift(Shift passedShift);
    }
}
