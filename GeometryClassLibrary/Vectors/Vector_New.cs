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
        public IUnitLessVector UnitLessVector { get; private set; }

        public Unit<T> X { get { return new Unit<T>(this.UnitType, UnitLessVector.X); } }
        public Unit<T> Y { get { return new Unit<T>(this.UnitType, UnitLessVector.Y); } }
        public Unit<T> Z { get { return new Unit<T>(this.UnitType, UnitLessVector.Z); } }

        public Point ApplicationPoint { get; private set; } = Point.Origin;

        public Unit<T> Magnitude { get { return this.Magnitude(); } }
        public Direction Direction { get { return this.Direction(); } }    
        #endregion

        #region Constructors
        public Vector_New(T unitType, Measurement x, Measurement y, Measurement z, Point applicationPoint = null)
        {
            this.UnitLessVector = new GenericVector(x, y, z);
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
            var newY = y.ValueIn(UnitType);
            var newZ = z.ValueIn(UnitType);
            this.UnitLessVector = new GenericVector(newX, newY, newZ);
            if (applicationPoint != null)
            {
                this.ApplicationPoint = applicationPoint;
            }
        }
        public Vector_New(Point basePoint, Unit<T> magnitude, Direction direction)
        {
            this.ApplicationPoint = basePoint;
            this.UnitType = (T)magnitude.UnitType;
            this.UnitLessVector = new GenericVector(magnitude.Measurement, direction);
        }

        public Vector_New(Vector_New<T> vector)
        {
            this.ApplicationPoint = vector.ApplicationPoint;
            this.UnitType = vector.UnitType;
            this.UnitLessVector = vector.UnitLessVector;
        }

        public Vector_New(Point applicationPoint, Vector_New<T> vector)
        {
            this.ApplicationPoint = applicationPoint;
            this.UnitType = vector.UnitType;
            this.UnitLessVector = vector.UnitLessVector;
        }

        public Vector_New(T unitType, IUnitLessVector underlyingVector)
        {
            this.UnitType = unitType;
            this.UnitLessVector = underlyingVector;
        }
        #endregion
    }


}
