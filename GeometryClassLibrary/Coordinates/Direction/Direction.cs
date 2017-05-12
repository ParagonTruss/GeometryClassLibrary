/*
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
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.AngleUnit;
using GeometryClassLibrary.Vectors;
using static UnitClassLibrary.AngleUnit.Angle;


namespace GeometryClassLibrary
{
    public partial class Direction : IRotate<Direction>
    {
        #region Properties and Fields

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azimuth)
        /// Will be between -180 degrees and +180 degrees.
        /// </summary>     
        public Angle Phi => new Angle(Math.Atan2(Y, X), Radians);

        /// <summary>
        /// The angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// </summary>
        public Angle Theta
        {
            get
            {
                var x = X;
                var y = Y;
                var r = Math.Sqrt(x*x + y*y);
                return new Angle(Math.Atan2(r, Z), Radians);
            }
        }

        /// <summary>
        /// Gets for the x-component of this directions unitVector
        /// </summary>
        public double X => Normalized.X;

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public double Y => Normalized.Y;

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public double Z => Normalized.Z;

        private DoubleVector _vector;
        private DoubleVector _normalized;

        private DoubleVector Normalized
        {
            get
            {
                if (_normalized == null)
                {
                    _normalized = Normalize(_vector);
                }
                return _normalized;
            }
            set
            {
                _normalized = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        private Direction() { }

        public Direction(double x, double y, double z)
        {
            this._vector = new DoubleVector(x, y, z);
        }
   
        /// <summary>
        /// Create a Direction with no Z component, by giving the angle in the XY plane.
        /// </summary>
        public Direction(Angle xyPlaneAngle)
        {
            var X = Math.Cos(xyPlaneAngle.InRadians.Value);
            var Y = Math.Sin(xyPlaneAngle.InRadians.Value);
            this._vector = new DoubleVector(X, Y);
            this._normalized = _vector;
        }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        public Direction(Point point)
        {
            this._vector = new DoubleVector(point.X.ValueInInches, point.Y.ValueInInches, point.Z.ValueInInches);
        }

        /// <summary>
        /// Makes a direction that represents the angle of the second point realtive to the first
        /// </summary>
        public Direction(Point basePoint, Point endPoint)
            : this(endPoint - basePoint) { }

        /// <summary>
        /// Creates a direction with the given angles. 
        /// </summary>
        public Direction(Angle phi, Angle theta)
        {
            var X = Math.Cos(phi.InRadians.Value) * Math.Sin(theta.InRadians.Value);
            var Y = Math.Sin(phi.InRadians.Value) * Math.Sin(theta.InRadians.Value);
            var Z = Math.Cos(theta.InRadians.Value);
            this._vector = new DoubleVector(X, Y, Z);
            this._normalized = _vector;
        }
       
        /// <summary>
        /// Creates a copy of the given Direction
        /// </summary>
        /// <param name="toCopy"></param>
        public Direction(Direction toCopy)
        {
            this._vector = toCopy._vector;
            this._normalized = toCopy._normalized;
        }

        
        public Direction(DoubleVector genericVector)
        {
            this._vector = genericVector;
        }

        public static DoubleVector Normalize(DoubleVector vector)
        {
            var r = vector.Magnitude;
            if (r < DoubleVector.Tolerance)
            {
                return DoubleVector.Zero;
            }
            else
            {
                var d = r;
                return vector/d;
            }
        }
        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector operator *(Direction d, Distance m)
        {
            return new Vector(d, m);
        }
        public static Vector operator *(Distance m, Direction d)
        {
            return new Vector(d, m);
        }

        public static bool operator ==(Direction direction1, Direction direction2)
        {
            if ((object)direction1 == null)
            {
                return (object)direction2 == null;
            }
            return direction1.Equals(direction2);
        }

        public static bool operator !=(Direction direction1, Direction direction2)
        {
            return !(direction1 == direction2);
        }

        public override bool Equals(object obj)
        {
            return (obj as Direction)?.Equals(this) ?? false;
        }
        public bool Equals(Direction dir)
        {
            if (ReferenceEquals(dir, null))
            {
                return false;
            }
            if (this._vector.Magnitude() < DoubleVector.Tolerance &&
                dir._vector.Magnitude() < DoubleVector.Tolerance)
            {
                return true;
            }
            Angle angleBetween = this.AngleBetween(dir);
            bool angleIsCloseToZero = (angleBetween == Angle.ZeroAngle);
            return angleIsCloseToZero;
        }

        public override string ToString()
        {
            return $"X = {Math.Round(X, 2)}, Y = {Math.Round(Y, 2)}, Z = {Math.Round(Z, 2)}";
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates a vector with this direction and unit length in the passed distance type.
        /// </summary>
        public Vector UnitVector(DistanceType passedType)
        {
            return this * new Distance(passedType, 1);
        }

        /// <summary>
        /// Creates a new direction in the reverse direction of this one.
        /// </summary>
        public Direction Reverse()
        {
            return new Direction( _vector.Reverse());
        }

        /// <summary>
        /// 
        /// </summary>
        public double DotProduct(Direction otherDirection)
        {
            var xTerm = this.X * otherDirection.X;
            var yTerm = this.Y * otherDirection.Y;
            var zTerm = this.Z * otherDirection.Z;

            var sum = xTerm + yTerm + zTerm;
            return sum;
        }

        public Direction CrossProduct(Direction otherdirection)
        {
            var result = this._vector.CrossProduct(otherdirection._vector);
            return new Direction(result);
        }

        public Angle AngleBetween(Direction direction)
        {
            var result = this._vector.AngleBetween(direction._vector);
            return result;
        }
        public Angle SmallestAngleBetween(Direction direction)
        {
            var angle = this.AngleBetween(direction);
            if (angle > Angle.RightAngle)
            {
                angle = angle - Angle.RightAngle;
            }
            return angle;
        }
        


        /// <summary>
        /// finds the signed angle between two directions.
        /// i.e. the order you input the vectors matters: the angle from direction1 to direction2 is negative the angle from direction2 to direction1
        /// The reference normal is what counts as "up" for determining sign.
        /// it defaults to the z direction, because this method will usuallly be used on the XYPLane.
        /// </summary>
        /// <returns></returns>
        public Angle AngleFromThisToThat(Direction direction, Direction referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Out;
            }

            Angle testAngle = this.AngleBetween(direction);
            Direction testNormal = this.CrossProduct(direction);
           
            if (testNormal == null || testAngle % Angle.StraightAngle == Angle.ZeroAngle || testNormal == referenceNormal)
            {
                return testAngle;
            }
            if (testNormal.Reverse() == referenceNormal)
            {
                return (Angle.FullCircle - testAngle);
            }
            

            throw new Exception("The reference normal is not perpendicular to these vectors");
        }

        public bool IsPerpendicularTo(Direction d)
        {
            return AngleBetween(d) == Angle.RightAngle;
        }

        public bool IsParallelTo(Direction d)
        {
            var angle = AngleBetween(d);
            return angle == Angle.ZeroAngle ||
                angle == 180 * new Angle(1, Degrees);
        }

        public Direction Rotate(Rotation rotation)
        {
            return new Direction(_vector.Rotate(rotation));
        }
        #endregion
    }
}
