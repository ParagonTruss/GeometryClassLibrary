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
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;


namespace GeometryClassLibrary.Vectors
{
    public sealed class LineSegment_New : IVector<DistanceType>
    {
        #region Properties
        public Point BasePoint { get; private set; }
        public Point EndPoint { get; private set; }

        public DistanceType UnitType { get { return (DistanceType)BasePoint.X.UnitType; } }

        public Point ApplicationPoint { get { return BasePoint; } }
        public Direction Direction { get { return new Direction(BasePoint, EndPoint); } }

        public Distance Length { get { return BasePoint.DistanceTo(EndPoint); } }
        public Unit<DistanceType> Magnitude { get { return Length; } }

        public IMeasurementVector MeasurementVector { get { return new MeasurementVector(_x, _y, _z); } }
        private Measurement _x { get { return (EndPoint.X - BasePoint.X).MeasurementIn(UnitType); } }
        private Measurement _y { get { return (EndPoint.Y - BasePoint.Y).MeasurementIn(UnitType); } }
        private Measurement _z { get { return (EndPoint.Z - BasePoint.Z).MeasurementIn(UnitType); } }
        #endregion
    }
}
