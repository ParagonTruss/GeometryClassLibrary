/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class Polygon : ISurface
    {
        public override bool IsBounded { get { return true; } }

        ISurface ISurface.Shift(Shift shift)
        {
            return this.Shift(shift);
        }

        ///// <summary>
        ///// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other PlaneRegion
        ///// NOTE: BASED SOLELY ON AREA.  MAY WANT TO CHANGE LATER
        ///// </summary>
        //public int CompareTo(Polygon other)
        //{
        //    if (this.Area.Equals(other.Area))
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return this.Area.CompareTo(other.Area);
        //    }
        //}
    }
}
