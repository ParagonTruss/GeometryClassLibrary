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

using System;

namespace GeometryClassLibrary.Vectors
{
    public static class DoubleVectorExtensions
    {
        public static double Magnitude(this IDoubleVector vector)
        {
            var result = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            return result;
        }

        public static Direction Direction(this IDoubleVector vector)
        {
            return new Direction(vector.X, vector.Y, vector.Z);
        }

        public static double DotProduct(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var xTerm = vector1.X * vector2.X;
            var yTerm = vector1.Y * vector2.Y;
            var zTerm = vector1.Z * vector2.Z;

            var sum = xTerm + yTerm + zTerm;
            return sum;
        }

        public static DoubleVector CrossProduct(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var x1 = vector1.X;
            var y1 = vector1.Y;
            var z1 = vector1.Z;
            var x2 = vector2.X;
            var y2 = vector2.Y;
            var z2 = vector2.Z;

            var newX = y1 * z2 - y2 * z1;
            var newY = z1 * x2 - z2 * x1;
            var newZ = x1 * y2 - x2 * y1;

            return new DoubleVector(newX, newY, newZ);
        }
    }
}
