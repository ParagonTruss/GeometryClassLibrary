using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public interface ISurface
    {
        bool IsBounded { get; }

        ISurface Shift(Shift shift);
    }
}
