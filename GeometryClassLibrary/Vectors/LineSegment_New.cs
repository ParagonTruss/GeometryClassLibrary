using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public class LineSegment_New : IVector<DistanceType>
    {
        #region Properties
        public Point BasePoint { get; private set; }
        public Point EndPoint { get; private set; }

        public Measurement X { get { return (EndPoint.X - BasePoint.X).ValueInThisUnit(UnitType); } }
        public Measurement Y { get { return (EndPoint.Y - BasePoint.Y).ValueInThisUnit(UnitType); } }
        public Measurement Z { get { return (EndPoint.Z - BasePoint.Z).ValueInThisUnit(UnitType); } }
        public DistanceType UnitType { get { return BasePoint.X.UnitType; } }

        public Point ApplicationPoint { get { return BasePoint; } }
        public Direction Direction { get { return new Direction(BasePoint, EndPoint); } }
        public Unit Magnitude { get { return BasePoint.DistanceTo(EndPoint); } }
        #endregion
    }
}
