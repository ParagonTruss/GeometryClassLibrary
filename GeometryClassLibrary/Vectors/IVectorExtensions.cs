﻿/*
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

using System;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;

namespace GeometryClassLibrary.Vectors
{
    public static class IVectorExtensions
    {
        public static Unit<T> X<T>(this IVector<T> vector) where T : IUnitType
        {
            return new Unit<T>(vector.UnitType, vector.DoubleVector.X);
        }
        public static Unit<T> Y<T>(this IVector<T> vector) where T : IUnitType
        {
            return new Unit<T>(vector.UnitType, vector.DoubleVector.Y);
        }
        public static Unit<T> Z<T>(this IVector<T> vector) where T : IUnitType
        {
            return new Unit<T>(vector.UnitType, vector.DoubleVector.Z);
        }
        //public static Direction Direction<T>(this IVector<T> vector) where T : IUnitType
        //{
        //    return vector.doubleVector.Direction();
        //}
        public static Unit<T> Magnitude<T>(this IVector<T> vector) where T : IUnitType
        {
            var result = vector.DoubleVector.Magnitude();
            return new Unit<T>(vector.UnitType, result);
        }

        //public static Unit DotProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        //{
        //    var result = vector1.DoubleVector.DotProduct(vector2.DoubleVector);
        //    var unitType = new DerivedUnitType(vector1.UnitType.Dimensions().Multiply(vector2.UnitType.Dimensions()));
        //    return new Unit<DerivedUnitType>(unitType, result);
        //}

        //public static Vector_New<DerivedUnitType> CrossProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        //{
        //    var underlyingVector = vector1.DoubleVector.CrossProduct(vector2.DoubleVector);
        //    var newDimensions = vector1.UnitType.Dimensions().Multiply(vector2.UnitType.Dimensions());
        //    var newUnitType = new DerivedUnitType(newDimensions);
        //    return new Vector_New<DerivedUnitType>(newUnitType, underlyingVector);
        //}

        /// <summary>
        /// Projects this vector onto the given Line.
        /// </summary>
        public static Vector_New<T> ProjectOntoLine<T>(this IVector<T> vector, Line projectOnto) where T : IUnitType
        {
            //        Point basePoint = vector.ApplicationPoint.ProjectOntoLine(projectOnto);
            //        double dotProduct = vector.DotProduct(projectOnto.Direction).double;
            //        double newX
            //        return new Vector_New<T>(vector.UnitType,basePoint,);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Projects this vector onto the given plane
        /// </summary>
        public static Vector_New<T> ProjectOntoPlane<T>(this IVector<T> vector, Plane plane) where T :IUnitType
        {
            //    //find the line in the plane and then project this line onto it
            //    Point newBasePoint = BasePoint.ProjectOntoPlane(plane);
            //    Point newEndPoint = EndPoint.ProjectOntoPlane(plane);
            //    return new Vector(newBasePoint, newEndPoint);
            throw new NotImplementedException();
        }

     

        /// <summary>
        /// determines whether two vectors point in the same direction
        /// </summary>
        public static bool HasSameDirectionAs(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var direction1 = vector1.Direction();
            var direction2 = vector2.Direction();
            return direction1 == null || direction2 == null || direction1 == direction2;
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions 
        /// </summary>
        public static bool HasOppositeDirectionOf(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var direction1 = vector1.Direction();
            var direction2 = vector2.Direction();
            return direction1 == null || direction2 == null || direction1 == direction2.Reverse();
        }
        
        /// <summary>
        /// returns a vector pointing in the opposite direction.
        /// </summary>
        public static Vector_New<T> Reverse<T>(this Vector_New<T> vector) where T : IUnitType
        {
            return new Vector_New<T>(vector.X.Negate(),vector.Y.Negate(),vector.Z.Negate(),vector.ApplicationPoint);
        }

        
        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public static Matrix ConvertToMatrixColumn(this IDoubleVector vector)
        {
            Matrix returnMatrix = new Matrix(3, 1);
            double[] vectorArray = { vector.X, vector.Y, vector.Z };
            returnMatrix.SetColumn(0, vectorArray);
            return returnMatrix;
        }

        ///// <summary>
        ///// Rotates the vector about the given axis by the passed angle
        ///// </summary>
        //public static Vector_New<T> Rotate<T>(this Vector_New<T> vector, Rotation rotationToApply) where T : IUnitType
        //{
        //    return null;
        //}

        ///// <summary>
        ///// Performs the Shift on this vector
        ///// </summary>
        ///// <param name="passedShift"></param>
        ///// <returns></returns>
        //public static Vector_New<T> Shift<T>(this Vector_New<T> vector, Shift passedShift)
        //{
        //    var newApplicationPoint = vector.Ap
        //}

        ///// <summary>
        ///// Translates the vector the given distance in the given direction
        ///// </summary>
        //public static Vector_New<T> Translate<T>(this Vector_New<T> vector, Translation translation) where T : IUnitType
        //{
        //    Point newBasePoint = vector.ApplicationPoint.Translate(translation);
        //    var shifted = new Vector_New<T>(newBasePoint,vector);
        //    return shifted;
        //}
        public static Vector_New<T> ToVector<T>(this IVector<T> vector) where T : IUnitType
        {
            return new Vector_New<T>(vector.ApplicationPoint, vector.DoubleVector, vector.UnitType);
        }

        public static LineSegment ToLineSegment<T>(this IVector<T> vector) where T : DistanceType
        {
            var magnitude = vector.Magnitude();
            return new LineSegment(vector.ApplicationPoint, vector.Direction, new Distance(magnitude.Measurement.Value, (DistanceType)magnitude.UnitType));
        }

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if it is a zero vector and you call the unitVector it will throw an exception.
        /// </summary>
        public static Vector_New<T> UnitVector<T>(this IVector<T> vector, T passedType) where T : IUnitType
        {
            Direction direction = vector.Direction;
            if (direction == GeometryClassLibrary.Direction.NoDirection)
            {
                // return new Vector(Point.Origin);
                throw new Exception();
            }
            else
            {
                return new Vector_New<T>(vector.ApplicationPoint, new Unit<T>(passedType, 1), direction);
            }

        }
    

        public static bool IsPerpendicularTo(this IDoubleVector vector1, IDoubleVector vector2)
        {
            return vector1.AngleBetween(vector2) == Angle.RightAngle;
        }

        public static bool IsParallelTo(this IDoubleVector vector1, IDoubleVector vector2)
        {
            Angle angle = vector1.AngleBetween(vector2);
            return angle == Angle.ZeroAngle || angle == Angle.StraightAngle;
        }

        public static Angle SmallestAngleBetween(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var angle = vector1.AngleBetween(vector2);
            if (angle > Angle.RightAngle)
            {
                angle = angle - Angle.RightAngle;
            }
            return angle;
        }
        public static Angle AngleBetween(this IDoubleVector vector1, IDoubleVector vector2)
        {
            var dotProduct = vector1.DotProduct(vector2);
            var crossProduct = vector1.CrossProduct(vector2).Magnitude;
            var angle = Math.Atan2(crossProduct, dotProduct);
         
            return new Angle(angle,Angle.Radians);
        }
    }
}
