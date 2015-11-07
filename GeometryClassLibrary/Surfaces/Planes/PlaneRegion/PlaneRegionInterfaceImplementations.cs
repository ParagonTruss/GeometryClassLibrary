using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class PlaneRegion : ISurface, IComparable<PlaneRegion>
    {
        public override bool IsBounded { get { return true; } }

        ISurface ISurface.Shift(Shift shift)
        {
            return this.Shift(shift);
        }

        /// <summary>
        /// Should return the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other PlaneRegion
        /// </summary>
        public int CompareTo(PlaneRegion other)
        {
            throw new NotImplementedException();
        }
    }
}
