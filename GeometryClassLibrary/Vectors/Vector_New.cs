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

        public Point ApplicationPoint { get { return _applicationPoint; } private set { _applicationPoint = value; } }
        private Point _applicationPoint = Point.Origin;

        public Unit<T> Magnitude { get { return _getMagnitude(); } }

        private Unit<T> _getMagnitude()
        {
            var result = (X ^ 2 + Y ^ 2 + Z ^ 2).SquareRoot();
            return new Unit<T>(UnitType, result);
        }

        //public Direction Direction { get { return _getDirection(); } }

        //private Direction _getDirection()
        //{
        //    throw new NotImplementedException();
        //}
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
            this.X = magnitude.Measurement * direction.XComponent;
            this.Y = magnitude.Measurement * direction.YComponent;
            this.Z = magnitude.Measurement * direction.ZComponent;
        }
        #endregion  
    }

    public class UnitLessVector : Vector_New<DimensionLess>
    {
        public implicit operator UnitLessVector(Direction d)
        {
            return new UnitLessVector(d);
        }
        public UnitLessVector(Direction d)
            : base(DimensionLess.Instance, d.XComponent, d.YComponent, d.ZComponent) { }
    }
}
