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
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.AreaUnit;

using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A vector is a line segment that has a direction
    /// Except it derives from Line. So that it doesn't cut the way linesegments do.
    /// </summary>
    public class Vector : IVector, IEquatable<Vector>
    {
        #region Properties and Fields

        public static Vector Zero => new Vector(Point.Origin);
        
        /// <summary>
        /// A point on the line to use as a reference.
        /// </summary>
        public Point BasePoint { get; }

        /// <summary>
        /// The direction the line is extends from the base point in one direction
        /// Note: it also extends in the direction opposite
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public Distance Magnitude { get; }

        /// <summary>
        /// Returns the x-component of this vector
        /// </summary>
        public Distance XComponent => Magnitude * Direction.X;

        /// <summary>
        /// Returns the y-component of this vector
        /// </summary>
        public Distance YComponent => Magnitude * Direction.Y;

        /// <summary>
        /// Returns the z-component of this vector
        /// </summary>
        public Distance ZComponent => Magnitude * Direction.Z;

        /// <summary>
        /// Returns the end point of the vector
        /// </summary>
        public Point EndPoint => new Point(XComponent, YComponent, ZComponent) + BasePoint;

        /// <summary>
        /// Allows the xyz components of the vector to be able to be accessed as an array
        /// </summary>
        private Distance this[int i]
        {
            get
            {
                if (i == 0) return XComponent;
                else if (i == 1) return YComponent;
                else if (i == 2) return ZComponent;
                else throw new IndexOutOfRangeException("No item of that index!");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>   
        protected Vector() { }

        /// <summary>
        /// Creates a vector that extends from the Origin to the passed reference point  
        /// </summary>
        public Vector(Point passedEndPoint)
        {
            this.BasePoint = Point.Origin;
            this.Direction = new Direction(passedEndPoint);
            this.Magnitude = passedEndPoint.DistanceTo(Point.Origin);
        }

        /// <summary>
        /// Creates a vector that starts at the given base point and goes to the given end point
        /// </summary>
        public Vector(Point basePoint, Point endPoint)
        {
            this.BasePoint = basePoint;
            this.Direction = new Direction(basePoint, endPoint);
            this.Magnitude = basePoint.DistanceTo(endPoint);
        }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        public Vector(Point basePoint, IVector vector)
        {
            this.BasePoint = basePoint ?? Point.Origin;
            this.Direction = vector.Direction;
            this.Magnitude = vector.Magnitude;
        }

        public Vector(Direction direction, Distance magnitude)
        {
            this.BasePoint = Point.Origin;
            this.Direction = direction;
            this.Magnitude = magnitude;
        }

        public Vector(ILinear line, Distance magnitude) : this(line.BasePoint, line.Direction, magnitude) { }

        public Vector(Vector vector, Distance magnitude) : this(vector.BasePoint, vector.Direction, magnitude) { }

        /// <summary>
        /// Creates a new vector with the given BasePoint in the given direction with the given magnitude
        /// </summary>
        public Vector(Point passedBasePoint, Direction direction, Distance magnitude)
        {
            this.BasePoint = passedBasePoint ?? Point.Origin;
            this.Direction = direction;
            this.Magnitude = magnitude;
        }

        /// <summary>
        /// Creates a Vector with the same basepoint and end point as this edge
        /// </summary>
        public Vector(IEdge edge) : this(edge.BasePoint, edge.EndPoint) { }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        public Vector(IVector toCopy)
            : this(toCopy.BasePoint, toCopy.Direction, toCopy.Magnitude) { }

        public Vector(LineSegment segment)
            : this(segment.BasePoint, segment.EndPoint) { }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return this.BasePoint.GetHashCode() ^ this.EndPoint.GetHashCode();
        }

        /// <summary>
        /// Adds the two vectors and returns the resultant vector
        /// </summary>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            Distance x = vector1.XComponent + vector2.XComponent;
            Distance y = vector1.YComponent + vector2.YComponent;
            Distance z = vector1.ZComponent + vector2.ZComponent;

            Point p = new Point(x,y,z);

            return new Vector(vector1.BasePoint, p + vector1.BasePoint);
        }

        /// <summary>
        /// Subtracts the two vectors and returns the resultant vector
        /// </summary>
        public static Vector operator -(Vector passedVector1, Vector passedVector2)
        {
            Vector negatedVector2 = -1 * passedVector2;
            return passedVector1 + negatedVector2;
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the scalar (order of terms does not matter)
        /// </summary>
        public static Vector operator *(Vector passedVector, double scalar)
        {
            return new Vector(passedVector.BasePoint, passedVector.Direction, passedVector.Magnitude * scalar);
        }

        /// <summary>
        /// Returns a new Vector with each component multiplied by the scalar (order of terms does not matter)
        /// </summary>
        public static Vector operator *(double scalar, Vector passedVector)
        {
            return passedVector * scalar;
        }

        /// <summary>
        /// Returns a new Vector with each component divided by the divisor
        /// </summary>
        public static Vector operator /(Vector passedVector, double divisor)
        {
            return new Vector(passedVector.BasePoint, passedVector.Direction, passedVector.Magnitude / divisor);
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if ((object)vector1 == null)
            {
                return (object)vector2 == null;
            }
            return vector1.Equals(vector2);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
            => !(vector1 == vector2);
        

        public override bool Equals(object obj) => Equals(obj as Vector);        

        public bool Equals(Vector vector)
        {
            return this.BasePoint == vector?.BasePoint &&
                   this.EndPoint == vector.EndPoint;
        }

        /// <summary>
        /// returns the vector as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"BasePoint= {BasePoint}, Direction= {Direction}, Magnitude= {Magnitude}";
        

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new vector with the same base point and direction but a different magnitude.
        /// </summary>
        public Vector Resize(Distance newMagnitude)
        {
            return new Vector(BasePoint, Direction, newMagnitude);
        }

        /// <summary>
        /// Projects this vector onto the given Line, which is the projected length of this LineSegment in the direction of the Line projected onto
        /// </summary>
        public Vector ProjectOntoLine(Line projectOnto)
        {
            Point basePoint = this.BasePoint.ProjectOntoLine(projectOnto);
            Point endPoint = this.EndPoint.ProjectOntoLine(projectOnto);
            return new Vector(basePoint, endPoint);
        }

        /// <summary>
        /// Projects this vector on the given plane
        /// </summary>
        public Vector ProjectOntoPlane(Plane plane)
        {
            //find the line in the plane and then project this line onto it
            Point newBasePoint = BasePoint.ProjectOntoPlane(plane);
            Point newEndPoint = EndPoint.ProjectOntoPlane(plane);
            return new Vector(newBasePoint, newEndPoint);
        }

        public Vector OrthogonalProjection(ILinear line)
        {
            return ProjectOntoPlane(new Plane(line));
        }

        public Vector OrthogonalProjection(Plane plane)
        {
            return ProjectOntoLine(plane.NormalLine);
        }

        public Distance ScalarProjection(Line line)
        {
            var projection = this.ProjectOntoLine(line);
            if (projection.Direction == line.Direction)
            {
                return projection.Magnitude;
            }
            return projection.Magnitude*-1;
        }

        public Distance ScalarProjection(Direction direction) => ScalarProjection(new Line(Point.Origin, direction));

        /// <summary>
        /// determines whether two vectors point in the same direction
        /// </summary>
        public bool HasSameDirectionAs(IVector passedVector)
        {
            Vector vector1 = this;
            IVector vector2 = passedVector;

            if (vector2 == null)
            {
                return false; //Doesn't have a direction. Should be false
            }
            if (vector1.Magnitude == Distance.ZeroDistance || vector2.Magnitude == Distance.ZeroDistance)
            {
                return true;
            }
            return vector1.Direction == vector2.Direction;
            
            
        }

        /// <summary>
        /// determines whether two vectors point in opposite directions 
        /// </summary>
        public bool HasOppositeDirectionOf(Vector passedVector)
        {
            Vector vector1 = this;
            Vector vector2 = passedVector;
            if (vector2 == null)
            {
                return false; //Doesn't have a direction. Should be false
            }
            if (vector1.Magnitude == Distance.ZeroDistance || vector2.Magnitude == Distance.ZeroDistance)
            {
                return true;
            }
            return vector1.AngleBetween(vector2) == Angle.StraightAngle;
        }

        /// <summary>
        /// determines whether two vectors point in the same or opposite directions.
        /// </summary>
        public bool HasSameOrOppositeDirectionAs(Vector vector)
        {
            bool sameDirection = this.HasSameDirectionAs(vector);
            bool oppositeDirections = this.HasOppositeDirectionOf(vector);

            return sameDirection || oppositeDirections;
        }

        /// <summary>
        /// returns a vector with its base and end points swapped.
        /// </summary>
        public Vector Reverse()
        {
            return new Vector(this.EndPoint, this.BasePoint);
        }
        
       /// <summary>
        /// Flips the vector about its tail.
        /// Like this:  {-------|-------}
       /// </summary>
        public Vector FlipAboutTail()
        {
            Vector reversed = this.Reverse();
            return new Vector(this.BasePoint, (reversed + reversed).EndPoint);
        }

        /// <summary>
        /// Flips the vector about its head.
        /// Like this: ------}|{-------
        /// </summary>
        public Vector FlipAboutHead()
        {
            return new Vector((this + this).EndPoint, this.EndPoint);
        }

        /// <summary>
        /// Converts a vector into a matrix with one column
        /// </summary>
        /// <returns></returns>
        public Matrix ConvertToMatrixColumn()
        {
            var vectorArray = new []
            {
                XComponent.ValueInInches,
                YComponent.ValueInInches,
                ZComponent.ValueInInches
            };

            Matrix returnMatrix = new Matrix(vectorArray);
            return returnMatrix;
        }

        /// <summary>
        /// Rotates the vector about the given axis by the passed angle
        /// </summary>
        public Vector Rotate(Rotation rotationToApply)
        {
            Point rotatedBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Point rotatedEndPoint = this.EndPoint.Rotate3D(rotationToApply);
            return new Vector(rotatedBasePoint, rotatedEndPoint);
        }

        /// <summary>
        /// Performs the Shift on this vector
        /// </summary>
        public Vector Shift(Shift passedShift)
        {
            return new Vector(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Translates the vector the given distance in the given direction
        /// </summary>
        public Vector Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newEndPoint = this.EndPoint.Translate(translation);

            return new Vector(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if it is a zero vector and you call the unitVector it will throw an exception.
        /// </summary>
        public Vector UnitVector(DistanceType passedType)
        {
            if (Magnitude == ZeroDistance)
            {
                throw new Exception();
            }
            return Direction.UnitVector(passedType);
        }
        #endregion

    }
}
