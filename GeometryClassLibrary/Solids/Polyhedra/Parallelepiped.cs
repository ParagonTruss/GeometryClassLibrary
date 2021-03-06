﻿/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

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
    public class Parallelepiped : Polyhedron
    {
        /// <summary>
        /// Private null constructor for the use of data frameworks like Entity Framework and Json.NET
        /// </summary>
        private Parallelepiped() { }

        public Parallelepiped(IVector vector1, IVector vector2, IVector vector3, Point basePoint = null)
            : base(MakeParallelepiped(vector1, vector2, vector3)) { }

        protected Parallelepiped(Polyhedron isParallelepiped) : base(isParallelepiped) { }
       
    }
}
