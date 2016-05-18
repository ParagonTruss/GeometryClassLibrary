using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.AreaUnit;

namespace GeometryClassLibrary
{
    public interface ISurface
    {
        bool IsBounded { get; }

        /// <summary>
        ///  The surface area. Return null, or positive infinity for unbounded surfaces.
        /// </summary>
        Area Area { get; }

        ISurface Shift(Shift shift);
    }
}
