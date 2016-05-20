﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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