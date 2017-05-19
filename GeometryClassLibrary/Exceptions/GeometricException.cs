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
using System.Collections.Generic;

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

    #region Polygon Exceptions
    public class InvalidPolygonException : GeometricException
    {
        public InvalidPolygonException(string message)
            : base(message) { }
    }
    public class NotEnoughPointsPolygonException : InvalidPolygonException
    {
        public NotEnoughPointsPolygonException(string message)
            : base(message) { }
    }
    public class NotEnoughSegmentsPolygonException : InvalidPolygonException
    {
        public NotEnoughSegmentsPolygonException(string message)
            : base(message) { }
    }
    public class NotClosedPolygonException : InvalidPolygonException
    {
        public NotClosedPolygonException(string message)
            : base(message) { }
    }
    public class NotCoplanarPolygonException : InvalidPolygonException
    {
        public NotCoplanarPolygonException(string message)
            : base(message) { }
    }
    public class SelfIntersectionPolygonException : InvalidPolygonException
    {
        public SelfIntersectionPolygonException(string message)
            : base(message) { }
    }
    #endregion

    public class InvalidPolyhedronException : GeometricException
    {
        public List<Polygon> Polygons { get; }

        public InvalidPolyhedronException(List<Polygon> polygons, string message = null) : base(message)
        {
            Polygons = polygons;
        }
    }
}