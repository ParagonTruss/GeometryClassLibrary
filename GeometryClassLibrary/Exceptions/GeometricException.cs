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

namespace GeometryClassLibrary
{
    /// <summary>
    /// Throw this when a geometric object should be created but the object would have an invalid state.
    /// Note: Some places should return null instead. e.g. if you intersect two lines which don't intersect. Return null. Don't throw this.
    /// </summary>
    public class GeometricException : Exception
    {
        public GeometricException() { }
        public GeometricException(string message) : base(message) { }   
    }

    public class InvalidLineSegmentException : GeometricException
    {
        public InvalidLineSegmentException() { }
        public InvalidLineSegmentException(string message) : base(message) { }
    }

    public class InvalidPolygonException : GeometricException
    {
        public InvalidPolygonException() { }
        public InvalidPolygonException(string message) : base(message) { }
    }

    public class InvalidPolyhedronException : GeometricException
    {
        public InvalidPolyhedronException() { }
        public InvalidPolyhedronException(string message) : base(message) { }
    }
}