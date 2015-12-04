using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public interface IVector
    {
        IUnitType UnitType { get; }       
        Measurement X { get; }
        Measurement Y { get; }
        Measurement Z { get; }
        Point ApplicationPoint { get; }

        Unit Magnitude { get; }
        Direction Direction { get; }
    }

    public interface IVector<T> where T : IUnitType { }

    public class Vector
    {

        #region Properties
        public IUnitType UnitType { get; private set; }
        public Measurement X { get; private set; }
        public Measurement Y { get; private set; }
        public Measurement Z { get; private set; }

        public Point ApplicationPoint { get { return _applicationPoint; } private set { _applicationPoint = value; } }
        private Point _applicationPoint = Point.Origin;

        public Unit Magnitude { get { return _getMagnitude(); } }

        private Unit _getMagnitude()
        {       
            var result = (X ^ 2 + Y ^ 2 + Z ^ 2).SquareRoot();
            return new Unit<IUnitType>(UnitType, result);
        }

        public Direction Direction { get { return _getDirection(); } }

        private Direction _getDirection()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructors
        public Vector(IUnitType unitType, Measurement x, Measurement y, Measurement z, Point applicationPoint = null)
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
        public Vector(Unit<T> x, Unit<T> y, Unit<T> z)
        {
            this.UnitType = (T)x.UnitType;
            this.X = x.Measurement;
            this.Y = y.ValueInThisUnit(UnitType);
            this.Z = z.ValueInThisUnit(UnitType);
        }
        public Vector(Point basePoint, Unit<T> magnitude, Direction direction)
        {
            this.ApplicationPoint = basePoint;
            this.UnitType = (T)magnitude.UnitType;
            this.X = magnitude.Measurement * direction.XComponent;
            this.Y = magnitude.Measurement * direction.YComponent;
            this.Z = magnitude.Measurement * direction.ZComponent;
        }
        #endregion

      
     
    }

    public class LineSegment : IVector<DistanceType>
    {
        public Point BasePoint { get; private set; }
        public Point EndPoint { get; private set; }

        public Measurement X { get { return (EndPoint.X - BasePoint.X).ValueInThisUnit(UnitType); } }
        public Measurement Y { get { return (EndPoint.Y - BasePoint.Y).ValueInThisUnit(UnitType); } }
        public Measurement Z { get { return (EndPoint.Z - BasePoint.Z).ValueInThisUnit(UnitType); } }
        public DistanceType UnitType { get { return BasePoint.X.UnitType; } }

        public Point ApplicationPoint { get { return BasePoint; } }
        public Direction Direction
        {
            get
            {
                return new Direction(BasePoint, EndPoint);
            }
        }

        public Unit Magnitude
        {
            get
            {
                return BasePoint.DistanceTo(EndPoint);
            }
        }
    }

    public static class IVectorExtensionMethods
    {
        public static Unit DotProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        {
            var result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
            var unitType = new DerivedUnitType(vector1.UnitType.Dimensions.Multiply(vector2.UnitType.Dimensions));
            return new Unit<DerivedUnitType>(unitType, result);
        }

        public static Vector<DerivedUnitType> CrossProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        {
            var x1 = vector1.X;
            var y1 = vector1.Y;
            var z1 = vector1.Z;
            var x2 = vector2.X;
            var y2 = vector2.Y;
            var z2 = vector2.Z;

            var newX = y1 * z2 - y2 * z1;
            var newY = z1 * x2 - z2 * x1;
            var newZ = x1 * y2 - x2 * y1;

            var newDimensions = vector1.UnitType.Dimensions.Multiply(vector2.UnitType.Dimensions);
            var newUnitType = new DerivedUnitType(newDimensions);
            return new Vector<DerivedUnitType>(newUnitType, newX, newY, newZ);

        }
    }
}
