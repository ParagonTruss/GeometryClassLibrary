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
    /// <summary>
    /// This class allows us to make irregular polygons and shapes with curves as well as line segments and access
    /// them in a generic way while still allowing for more specific implementation when needed
    /// </summary>
    public interface IEdge
    {
        #region Properties and Fields

        Direction InitialDirection { get; }
        Point BasePoint { get; }
        Point EndPoint { get; }

        bool IsClosed { get; }
        #endregion

        #region Methods
        IEdge Copy();
        IEdge Reverse();
        IEdge Translate(Point point);
        IEdge Rotate(Rotation rotation);
        IEdge Shift(Shift shift);
       

        #endregion

    }
}
