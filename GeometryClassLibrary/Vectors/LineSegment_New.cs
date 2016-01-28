﻿using System;
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
        public Distance Magnitude { get { return BasePoint.DistanceTo(EndPoint); } }

        public IMeasurementVector MeasurementVector { get { return new MeasurementVector(_x, _y, _z); } }
        private Measurement _x { get { return (EndPoint.X - BasePoint.X).MeasurementIn(UnitType); } }
        private Measurement _y { get { return (EndPoint.Y - BasePoint.Y).MeasurementIn(UnitType); } }
        private Measurement _z { get { return (EndPoint.Z - BasePoint.Z).MeasurementIn(UnitType); } }
        #endregion
    }
}
