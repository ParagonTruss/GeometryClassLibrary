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

using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    public interface IVector : ILinear
    {
        Distance Magnitude { get; }
        Point EndPoint { get; }
    }

    public static class VectorExtensions
    {
        /// <summary>
        /// Returns the cross product of the 2 vectors
        /// which is always perpendicular to both vectors
        /// and whose magnitude is the area of the parellelogram spanned by those two vectors
        /// Consequently if they point in the same (or opposite) direction, than the cross product is zero
        /// </summary>
        public static Vector CrossProduct(this IVector thisVector, IVector otherVector)
        {
            Vector v1 = new Vector(thisVector);
            Vector v2 = new Vector(otherVector);

            //Find each component 

            double xProduct1 = v1.YComponent.ValueInInches * v2.ZComponent.ValueInInches;
            double xProduct2 = v1.ZComponent.ValueInInches * v2.YComponent.ValueInInches;

            double yProduct1 = v1.ZComponent.ValueInInches * v2.XComponent.ValueInInches;
            double yProduct2 = v1.XComponent.ValueInInches * v2.ZComponent.ValueInInches;

            double zProduct1 = v1.XComponent.ValueInInches * v2.YComponent.ValueInInches;
            double zProduct2 = v1.YComponent.ValueInInches * v2.XComponent.ValueInInches;

            double newX = (xProduct1) - (xProduct2);
            double newY = (yProduct1) - (yProduct2);
            double newZ = (zProduct1) - (zProduct2);

            return new Vector(Point.MakePointWithInches(newX, newY, newZ));
        }
        
        
        /// <summary>
        /// Returns the DotProduct between two Vectors as an area.
        /// </summary>
        public static Area DotProduct(this IVector thisVector, IVector otherVector)
        {
            Vector vector1 = new Vector(thisVector);
            Vector vector2 = new Vector(otherVector);

            var sum = 0.0;
            sum += vector1.XComponent.ValueInInches * vector2.XComponent.ValueInInches;
            sum += vector1.YComponent.ValueInInches * vector2.YComponent.ValueInInches;
            sum += vector1.ZComponent.ValueInInches * vector2.ZComponent.ValueInInches;

            return new Area(new SquareInch(),sum);
        }
    }
}