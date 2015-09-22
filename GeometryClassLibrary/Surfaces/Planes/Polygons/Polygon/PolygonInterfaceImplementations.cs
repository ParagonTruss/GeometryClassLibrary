using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class Polygon : ISurface, IComparable<Polygon>
    {
        public override bool IsBounded { get { return true; } }

        ISurface ISurface.Shift(Shift shift)
        {
            return this.Shift(shift);
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other PlaneRegion
        /// NOTE: BASED SOLELY ON AREA.  MAY WANT TO CHANGE LATER
        /// </summary>
        public int CompareTo(Polygon other)
        {
            if (this.Area.Equals(other.Area))
            {
                return 0;
            }
            else
            {
                return this.Area.CompareTo(other.Area);
            }
        }
    }
}
