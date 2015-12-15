using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public class Vector_New<T> : IVector<T> where T : IUnitType
    {
        #region Properties
        public T UnitType { get; private set; }
        public Measurement X { get; private set; }
        public Measurement Y { get; private set; }
        public Measurement Z { get; private set; }

        public Point ApplicationPoint { get; private set; } = Point.Origin;

        public Unit<T> Magnitude { get { return this.Magnitude(); } }
        public Direction Direction { get { return this.Direction(); } }    
        #endregion

        #region Constructors
        public Vector_New(T unitType, Measurement x, Measurement y, Measurement z, Point applicationPoint = null)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.UnitType = unitType;
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Unit<T> x, Unit<T> y, Unit<T> z)
        {
            this.UnitType = (T)x.UnitType;
            this.X = x.Measurement;
            this.Y = y.ValueInThisUnit(UnitType);
            this.Z = z.ValueInThisUnit(UnitType);
        }
        public Vector_New(Point basePoint, Unit<T> magnitude, Direction direction)
        {
            this.ApplicationPoint = basePoint;
            this.UnitType = (T)magnitude.UnitType;
            this.X = magnitude.Measurement * direction.X;
            this.Y = magnitude.Measurement * direction.Y;
            this.Z = magnitude.Measurement * direction.Z;
        }

        public Vector_New(Vector_New<T> vector)
        {
            this.ApplicationPoint = vector.ApplicationPoint;
            this.UnitType = vector.UnitType;
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public Vector_New(Point applicationPoint, Vector_New<T> vector)
        {
            this.ApplicationPoint = applicationPoint;
            this.UnitType = vector.UnitType;
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }
        #endregion
    }

    public class UnitLessVector : Vector_New<DimensionLess>
    {
        public static implicit operator UnitLessVector(Direction d)
        {
            return new UnitLessVector(d);
        }

        public UnitLessVector(Direction d)
            : base(DimensionLess.Instance, d.X, d.Y, d.Z) { }
    }
}
