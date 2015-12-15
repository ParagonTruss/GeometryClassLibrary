using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.GenericUnit;
using GeometryClassLibrary;

namespace GeometryClassLibrary.Vectors
{
    public static class IVectorExtensionMethods
    {
        public static Direction Direction(this IVector vector)
        {
            return GeometryClassLibrary.Direction.DetermineDirection(vector.X, vector.Y, vector.Z);
        }

        public static Unit<T> Magnitude<T>(this IVector<T> vector) where T :IUnitType
        {
            var m = (vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z).SquareRoot();
            return new Unit<T>(vector.UnitType, m);
        }

        public static Unit DotProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
        {
            var result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
            var unitType = new DerivedUnitType(vector1.UnitType.Dimensions.Multiply(vector2.UnitType.Dimensions));
            return new Unit<DerivedUnitType>(unitType, result);
        }

        public static Vector_New<DerivedUnitType> CrossProduct(this IVector<IUnitType> vector1, IVector<IUnitType> vector2)
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
            return new Vector_New<DerivedUnitType>(newUnitType, newX, newY, newZ);
        }

        /// <summary>
        /// Projects this vector onto the given Line.
        /// </summary>
        public static Vector_New<T> ProjectOntoLine<T>(this IVector<T> vector, Line projectOnto) where T : IUnitType
        {
            //        Point basePoint = vector.ApplicationPoint.ProjectOntoLine(projectOnto);
            //        Measurement dotProduct = vector.DotProduct(projectOnto.Direction).Measurement;
            //        Measurement newX
            //        return new Vector_New<T>(vector.UnitType,basePoint,);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Projects this vector onto the given plane
        /// </summary>
        /// <param name="projectOnto">The Plane to project this Vector onto</param>
        /// <returns>Returns a new Vector that is this Vector projected onto the Plane</returns>
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
        public static bool HasSameDirectionAs(this IVector vector1, IVector vector2)
        {
            var direction1 = vector1.Direction();
            var direction2 = vector2.Direction();
            return direction1 == null || direction2 == null || direction1 == direction2;
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions 
        /// </summary>
        public static bool HasOppositeDirectionOf(this IVector vector1, IVector vector2)
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
            return new Vector_New<T>(vector.UnitType,vector.X.Negate(),vector.Y.Negate(),vector.Z.Negate(),vector.ApplicationPoint);
        }

        
        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public static Matrix ConvertToMatrixColumn(this IVector vector)
        {
            Matrix returnMatrix = new Matrix(3, 1);
            double[] vectorArray = { vector.X.Value, vector.Y.Value, vector.Z.Value };
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

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if it is a zero vector and you call the unitVector it will throw an exception.
        /// </summary>
        public static IVector UnitVector<T>(this IVector<T> vector, T passedType) where T : IUnitType
        {
            Direction direction = vector.Direction();
            if (direction == GeometryClassLibrary.Direction.NoDirection)
            {
                // return new Vector(Point.Origin);
                throw new Exception();
            }
            else
            {
                return new Vector_New<T>(vector.ApplicationPoint, new Unit<T>(passedType), direction);
            }

        }
    

        public static bool IsPerpendicularTo(this IVector vector1, IVector vector2)
        {
            return vector1.AngleBetween(vector2) == Angle.RightAngle;
        }

        public static bool IsParallelTo(this IVector vector1, IVector vector2)
        {
            Angle angle = vector1.AngleBetween(vector2);
            return angle == Angle.Zero || angle == Angle.StraightAngle;
        }

        public static Angle SmallestAngleBetween(this IVector vector1, IVector vector2)
        {
            var angle = vector1.AngleBetween(vector2);
            if (angle > Angle.RightAngle)
            {
                angle = angle - Angle.RightAngle;
            }
            return angle;
        }
        public static Angle AngleBetween(this IVector vector1, IVector vector2)
        {
            return vector1.Direction().AngleBetween(vector2.Direction());
        }
    }
}
