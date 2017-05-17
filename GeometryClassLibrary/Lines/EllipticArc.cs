/*
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

using System;

namespace GeometryClassLibrary
{
    public class EllipticArc : IEdge
    {
        public Point Focus1 { get { return _focus1; } }
        public Point Focus2 { get { return _focus2; } }
        private Point _focus1;
        private Point _focus2;

        public Point BasePoint { get { return _basePoint; } }
        private Point _basePoint;

        public Point EndPoint { get { return _endPoint; } }
        private Point _endPoint;

        public bool IsClosed { get { return this.BasePoint == this.EndPoint; } }

        public Direction InitialDirection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private EllipticArc(Point basePoint, Point endPoint, Point focus1, Point focus2)
        {
            this._basePoint = basePoint;
            this._endPoint = endPoint;
            this._focus1 = focus1;
            this._focus2 = focus2;
        }

        public IEdge Copy()
        {
            throw new NotImplementedException();
        }

        public IEdge Reverse()
        {
            throw new NotImplementedException();
        }

        public IEdge Translate(Point point)
        {
            throw new NotImplementedException();
        }

        public IEdge Rotate(Rotation rotation)
        {
            throw new NotImplementedException();
        }

        public IEdge Shift(Shift shift)
        {
            throw new NotImplementedException();
        }
    }
}
