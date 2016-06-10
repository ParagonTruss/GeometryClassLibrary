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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    public class Vector_New<T> : IVector<T> where T : IUnitType
    {
        #region Properties
        public T UnitType { get; private set; }
        public IMeasurementVector MeasurementVector { get; private set; }

        public Unit<T> X { get { return new Unit<T>(this.UnitType, MeasurementVector.X); } }
        public Unit<T> Y { get { return new Unit<T>(this.UnitType, MeasurementVector.Y); } }
        public Unit<T> Z { get { return new Unit<T>(this.UnitType, MeasurementVector.Z); } }

        public Point ApplicationPoint { get; private set; } = Point.Origin;

        public Unit<T> Magnitude { get { return this.Magnitude(); } }
        public Direction Direction { get { return this.MeasurementVector.Direction(); } }    
        #endregion

        #region Constructors
        public Vector_New(T unitType, Measurement x, Measurement y, Measurement z, Point applicationPoint = null)
        {
            this.MeasurementVector = new MeasurementVector(x, y, z);
            this.UnitType = unitType;
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Unit<T> x, Unit<T> y, Unit<T> z, Point applicationPoint = null)
        {
            this.UnitType = (T)x.UnitType;
            var newX = x.Measurement;
            var newY = y.MeasurementIn(UnitType);
            var newZ = z.MeasurementIn(UnitType);
            this.MeasurementVector = new MeasurementVector(newX, newY, newZ);
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Point basePoint, Unit<T> magnitude, Direction direction)
        {
            this.ApplicationPoint = basePoint;
            this.UnitType = (T)magnitude.UnitType;
            this.MeasurementVector = new MeasurementVector(magnitude.Measurement, direction);
        }

        public Vector_New(Vector_New<T> vector)
        {
            this.ApplicationPoint = vector.ApplicationPoint;
            this.UnitType = vector.UnitType;
            this.MeasurementVector = vector.MeasurementVector;
        }

        public Vector_New(Point applicationPoint, Vector_New<T> vector)
        {
            this.ApplicationPoint = applicationPoint;
            this.UnitType = vector.UnitType;
            this.MeasurementVector = vector.MeasurementVector;
        }

        public Vector_New(T unitType, IMeasurementVector underlyingVector)
        {
            this.UnitType = unitType;
            this.MeasurementVector = underlyingVector;
        }

        public Vector_New(Point applicationPoint, IMeasurementVector underlyingVector, T unitType)
        {
            this.ApplicationPoint = applicationPoint; 
            this.MeasurementVector = underlyingVector;
            this.UnitType = unitType;
        }
        #endregion
    }


}
