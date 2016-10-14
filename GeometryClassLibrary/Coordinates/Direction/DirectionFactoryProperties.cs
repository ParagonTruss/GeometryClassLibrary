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

namespace GeometryClassLibrary
{
    public partial class Direction
    {
        public static Direction NoDirection = new Direction(0, 0, 0);
        
        /// <summary>
        /// A staticly defined Direction that is positive in the X direction. 
        /// Extends to the right in the XY plane.
        /// </summary>
        public static Direction Right = new Direction(1, 0, 0); 

        /// <summary>
        /// A staticly defined Direction that is negative in the X direction. 
        /// Extends to the left in the XY plane.
        /// </summary>
        public static Direction Left = new Direction(-1, 0, 0); 

        /// <summary>
        /// A staticly defined Direction that is positive in the Y direction. 
        /// Extends upward in the XY plane.
        /// </summary>
        public static Direction Up = new Direction(0, 1, 0); 

        /// <summary>
        /// A staticly defined Direction that is negative in the Y direction. 
        /// Extends downward in the XY plane.
        /// </summary>
        public static Direction Down = new Direction(0, -1, 0); 
        
        /// <summary>
        /// A staticly defined Direction that is positive in the Z direction.
        /// Extends towards the viewer when looking at the XY plane.
        /// </summary>
        public static Direction Out = new Direction(0, 0, 1); 

        /// <summary>
        /// A staticly defined Direction that is negative in the Z direction.
        /// Extends away from the viewer when looking at the XY plane.
        /// </summary>
        public static Direction Back = new Direction(0, 0, -1); 
    }
}
