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

using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{

    public class Rectangle : Polygon
    {
        public Distance Length { get; }
        public Distance Width { get; }

        /// <summary>
        /// Creates a Rectangle in the XY Plane, with one corner at the origin, and the specified length, width.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        public Rectangle(Distance length, Distance width, Point basePoint = null) 
            : base(_makeRectangle(length, width))
        {
            this.Length = length.AbsoluteValue();
            this.Width = width.AbsoluteValue();
        }

        private static Polygon _makeRectangle(Distance length, Distance width, Point basePoint = null)
        {
            var vector1 = new Vector(Direction.Right, length);
            var vector2 = new Vector(Direction.Up, width);
            
            var rectangle = Parallelogram(vector1, vector2, basePoint);
            return rectangle;
        }
        

        public Rectangle(Vector baseSegment, Distance height, Direction referencePlaneNormal = null)
            : base(_makeRectangle(baseSegment, height, referencePlaneNormal)) { }

        private static Polygon _makeRectangle(Vector baseSegment, Distance height, Direction referencePlaneNormal)
        {
            if (referencePlaneNormal == null)
            {
                referencePlaneNormal = Direction.Out;
            }

            Direction heightDirection = referencePlaneNormal.CrossProduct(baseSegment.Direction);
            var heightVector = new Vector(heightDirection, height);
            
            Polygon polygon = Parallelogram(baseSegment, heightVector);

            return polygon;
        }
    }
}
