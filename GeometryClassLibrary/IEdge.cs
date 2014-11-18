using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// This class allows us to make irregular polygons and shapes with curves as well as line segments and access
    /// them in a generic way while still allowing for more specific implementation when needed
    /// </summary>
    public interface IEdge
    {
        #region Properties and Fields

        Direction Direction { get; set; }
        Point BasePoint { get; set; }

        #endregion

        #region Methods

        IEdge Rotate(Rotation passedRotation);
        IEdge Shift(Shift passedShift);
        IEdge Copy();

        #endregion

    }
}
