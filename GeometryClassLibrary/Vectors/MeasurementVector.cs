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

using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary.Vectors
{
    public sealed class DoubleVector : IDoubleVector, IRotate<DoubleVector>
    {
        public const double Tolerance = 0.00001;
        public static DoubleVector Zero => new DoubleVector(0.0, 0.0, 0.0);

        #region Local Properties
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public double Magnitude => this.Magnitude();

        #endregion

        #region Constructor
        public DoubleVector(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }
        
        public DoubleVector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DoubleVector(double magnitude, Direction direction)
        {
            this.X = magnitude * direction.X;
            this.Y = magnitude * direction.Y;
            this.Z = magnitude * direction.Z;
        }
        #endregion

        #region Public Methods
        public DoubleVector Reverse()
        {
            return new DoubleVector(-X, -Y, -Z);
        }

        public DoubleVector Rotate(Rotation rotation)
        {
            // update later, but for now:
            var point = new Point(Distance.Inches, X, Y, Z);
            point = point.Rotate3D(rotation);
            return new DoubleVector(point.X.ValueInInches, point.Y.ValueInInches, point.Z.ValueInInches);
        }


        #endregion

        #region Operator Overloads
        public static DoubleVector operator /(DoubleVector vector, double divisor)
        {
            return new DoubleVector(vector.X/divisor, vector.Y/divisor, vector.Z/divisor);
        }
        #endregion
    }
}
