using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public static class ISurfaceListExtensions
    {
        public static List<ISurface> Shift(this List<ISurface> surfaces, Shift shift)
        {
            return surfaces.Select(s => s.Shift(shift)).ToList();
        }
    }
}
