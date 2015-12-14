using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.GenericUnit;

namespace GeometryClassLibrary.Vectors
{
    public static class IVectorExtensionMethods
    {
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
        /// Projects this LineSegment onto the given Line, which is the projected length of this LineSegment in the direction of the Line projected onto
        /// </summary>
        /// <param name="projectOnto">the Line on which to project the LineSegment</param>
        /// <returns></returns>
        public static IVector ProjectOntoLine<T>(this Vector_New<T> vector, Line projectOnto) where T : IUnitType
        {
            //        Point basePoint = vector.ApplicationPoint.ProjectOntoLine(projectOnto);
            //        Measurement dotProduct = vector.DotProduct(projectOnto.Direction).Measurement;
            //        Measurement newX
            //        return new Vector_New<T>(vector.UnitType,basePoint,);
            return null;
        }

        /// <summary>
        /// Projects this vector on the given plane
        /// </summary>
        /// <param name="projectOnto">The Plane to project this Vector onto</param>
        /// <returns>Returns a new Vector that is this Vector projected onto the Plane</returns>
        public new Vector ProjectOntoPlane(Plane plane)
        {
            //find the line in the plane and then project this line onto it
            Point newBasePoint = BasePoint.ProjectOntoPlane(plane);
            Point newEndPoint = EndPoint.ProjectOntoPlane(plane);
            return new Vector(newBasePoint, newEndPoint);
        }

     

        /// <summary>
        /// determines whether two vectors point in the same direction
        /// </summary>
        public static bool HasSameDirectionAs(this IVector vector1, IVector vector2)
        {
            return vector1.Direction == null || vector2.Direction == null || vector1.Direction == vector2.Direction;
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions 
        /// </summary>
        /// <param name="v1">vector to compare against</param>
        /// <returns></returns>
        public static bool HasOppositeDirectionOf(this IVector vector1, IVector vector2)
        {      
            return vector1.Direction == null || vector2.Direction == null || vector1.Direction == vector2.Direction.Reverse();
        }
        
        /// <summary>
        /// returns a vector with its base and end points swapped.
        /// </summary>
        public static Vector_New<T> Reverse<T>(this Vector_New<T> vector) where T : IUnitType
        {
            return new Vector_New<T>(vector.UnitType,vector.X.Negate(),vector.Y.Negate(),vector.Z.Negate(),vector.ApplicationPoint);
        }

        
        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public Matrix ConvertToMatrixColumn()
        {
            throw new NotImplementedException("Make matrices use measurements");
            //Matrix returnMatrix = new Matrix(3, 1);
            //double[] vectorArray = { XComponent.Inches.Value, YComponent.Inches.Value, ZComponent.Inches.Value };
            //returnMatrix.SetColumn(0, vectorArray);
            //return returnMatrix;
        }

        /// <summary>
        /// Rotates the vector about the given axis by the passed angle
        /// </summary>
        public static Vector_New<T> Rotate<T>(this Vector_New<T> vector, Rotation rotationToApply) where T : IUnitType
        {
            return null;
        }

        /// <summary>
        /// Performs the Shift on this vector
        /// </summary>
        /// <param name="passedShift"></param>
        /// <returns></returns>
        public static Vector Shift(this Vector vector, Shift passedShift)
        {
            return new Vector(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Translates the vector the given distance in the given direction
        /// </summary>
        /// <param name="passedDirection"></param>
        /// <param name="passedDisplacement"></param>
        /// <returns></returns>
        public new Vector Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newEndPoint = this.EndPoint.Translate(translation);

            return new Vector(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if it is a zero vector and you call the unitVector it will throw an exception.
        /// </summary>
        public static IVector UnitVector<T>(this Vector_New<T> vector, T  passedType) where T : IUnitType
        {
            if (vector.Direction == null)
            {
                // return new Vector(Point.Origin);
                throw new Exception();
            }
            else
            {
                return new Vector_New<T>(vector.ApplicationPoint, new Unit<T>(passedType), vector.Direction);
            }

        }
    

        public static bool IsPerpendicularTo(this IVector vector1, IVector vector2)
        {
            return vector1.SmallestAngleBetween(vector2) == 90 * Angle.Degree;
        }

        public static bool IsParallelTo(this IVector vector1, IVector vector2)
        {
            return vector1.SmallestAngleBetween(vector2) == Angle.Zero;
        }

        public static Angle SmallestAngleBetween(this IVector vector1, IVector vector2)
        {
            return null;
        }
        public static Angle AngleBetween(this IVector vector1, IVector vector2)
        {
            return null;
        }
    }
}
