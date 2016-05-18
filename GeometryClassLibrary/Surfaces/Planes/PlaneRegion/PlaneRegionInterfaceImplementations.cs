using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class PlaneRegion : ISurface
    {
        public override bool IsBounded { get { return true; } }

        ISurface ISurface.Shift(Shift shift)
        {
            return this.Shift(shift);
        }


    }
}
