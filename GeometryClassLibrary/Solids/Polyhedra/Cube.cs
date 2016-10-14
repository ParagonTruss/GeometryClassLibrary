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

using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class Cube : RectangularPrism
    {
        /// <summary>
        /// Private null constructor for the use of data frameworks like Entity Framework and Json.NET
        /// </summary>
        private Cube() { }

        /// <summary>
        /// Creates a new cube with with the given length of one of the sides
        /// </summary>
        /// <param name="passedSize"></param>
        public Cube(Distance passedSize):base(passedSize, passedSize, passedSize){ }
    }
}
