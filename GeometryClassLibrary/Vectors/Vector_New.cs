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

namespace GeometryClassLibrary.Vectors
{
    public class Vector_New<T> : IVector<T> where T : IUnitType
    {
        #region Properties
        public T UnitType { get; private set; }
        public IDoubleVector DoubleVector { get; private set; }

        public Unit<T> X { get { return new Unit<T>(this.UnitType, DoubleVector.X); } }
        public Unit<T> Y { get { return new Unit<T>(this.UnitType, DoubleVector.Y); } }
        public Unit<T> Z { get { return new Unit<T>(this.UnitType, DoubleVector.Z); } }

        public Point ApplicationPoint { get; private set; } = Point.Origin;

        public Unit<T> Magnitude { get { return this.Magnitude(); } }
        public Direction Direction { get { return this.DoubleVector.Direction(); } }    
        #endregion

        #region Constructors
        public Vector_New(T unitType, double x, double y, double z, Point applicationPoint = null)
        {
            this.DoubleVector = new DoubleVector(x, y, z);
            this.UnitType = unitType;
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Unit<T> x, Unit<T> y, Unit<T> z, Point applicationPoint = null)
        {
            this.UnitType = (T)x.UnitType;
            var newX = x.Measurement.Value;
            var newY = y.ValueIn(UnitType);
            var newZ = z.ValueIn(UnitType);
            this.DoubleVector = new DoubleVector(newX, newY, newZ);
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Point basePoint, Unit<T> magnitude, Direction direction)
        {
            this.ApplicationPoint = basePoint;
            this.UnitType = (T)magnitude.UnitType;
            this.DoubleVector = new DoubleVector(magnitude.Measurement.Value, direction);
        }

        public Vector_New(Vector_New<T> vector)
        {
            this.ApplicationPoint = vector.ApplicationPoint;
            this.UnitType = vector.UnitType;
            this.DoubleVector = vector.DoubleVector;
        }

        public Vector_New(Point applicationPoint, Vector_New<T> vector)
        {
            this.ApplicationPoint = applicationPoint;
            this.UnitType = vector.UnitType;
            this.DoubleVector = vector.DoubleVector;
        }

        public Vector_New(T unitType, IDoubleVector underlyingVector)
        {
            this.UnitType = unitType;
            this.DoubleVector = underlyingVector;
        }

        public Vector_New(Point applicationPoint, IDoubleVector underlyingVector, T unitType)
        {
            this.ApplicationPoint = applicationPoint; 
            this.DoubleVector = underlyingVector;
            this.UnitType = unitType;
        }
        #endregion
    }


}
