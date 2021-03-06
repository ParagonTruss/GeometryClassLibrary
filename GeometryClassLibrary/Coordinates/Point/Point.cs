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
using static UnitClassLibrary.DistanceUnit.Distance;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.FootUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Metric.MillimeterUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Point class that gracefully handles 2d and 3d points
    /// </summary>
    public class Point : IEquatable<Point>
    {
        #region Properties and Fields

        public static Point Origin { get; } = new Point(ZeroDistance, ZeroDistance, ZeroDistance);

        public Distance X { get; }
        public Distance Y { get; }
        public Distance Z { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        protected Point() { }

        /// <summary>
        ///Create a point that lies in the XY plane.
        /// </summary>
        public Point(Distance passedX, Distance passedY)
        {
            this.X = passedX;
            this.Y = passedY;
            this.Z = Distance.ZeroDistance;
        }

        /// <summary>
        /// Create any point by its coordinates.
        /// </summary>
        public Point(Distance x, Distance y, Distance z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new point with the given values with the given Distance type
        /// </summary>
        public Point(DistanceType passedType, double passedX, double passedY, double passedZ = 0)
        {
            X = new Distance(passedType, passedX);
            Y = new Distance(passedType, passedY);
            Z = new Distance(passedType, passedZ);
        }

        public Point(List<Distance> coordinates)
        {
            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public Point(Point toCopy)
        {
            X = toCopy.X;
            Y = toCopy.Y;
            Z = toCopy.Z;
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public static Point operator +(Point point1, Point point2)
        {
            Distance newX = point1.X + point2.X;
            Distance newY = point1.Y + point2.Y;
            Distance newZ = point1.Z + point2.Z;

            return new Point(newX, newY, newZ);
        }

        public static Point operator -(Point point1, Point point2)
        {
            Distance newX = point1.X - point2.X;
            Distance newY = point1.Y - point2.Y;
            Distance newZ = point1.Z - point2.Z;

            return new Point(newX, newY, newZ);
        }

        public static Point operator *(double scalar, Point point)
        {
            return new Point(scalar * point.X, scalar * point.Y, scalar * point.Z);
        }

        public static Point operator /(Point point, double divisor)
        {
            return new Point(point.X/ divisor, point.Y/divisor, point.Z/divisor);

        }
        
        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            // covers null reference checks
            if ((object) point1 != null)
            {
                return point1.Equals(point2);
            }
            if ((object) point2 == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        public bool Equals(Point other)
        {
            var distance = other?.DistanceTo(this);
            return distance == ZeroDistance;
        }
        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //check for null (wont throw a castexception)
            if (!(obj is Point))
            {
                return false;
            }
            Point comparablePoint = (Point)obj;

            return this.Equals(comparablePoint);
        }
       
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", this.X.ToString(), this.Y.ToString(), this.Z.ToString());
        }

        #endregion

        #region Methods

        public List<Distance> ToListOfCoordinates()
        {
            return new List<Distance>() { this.X, this.Y, this.Z };
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        public Point Rotate2D(Angle rotateAngle, Point centerPoint = null)
        {
            if (centerPoint == null)
            {
                centerPoint = Origin;
            }
           double cosTheta = Angle.Cosine(rotateAngle);
           double sinTheta = Angle.Sine(rotateAngle);

            var point = this - centerPoint;
            return new Point(
                cosTheta * point.X - sinTheta * point.Y,
                sinTheta * point.X + cosTheta * point.Y)
                + centerPoint;
        }

        /// <summary>
        /// Moves the point by the specified amount based on the passed point
        /// </summary>
        public Point Translate(Translation passedTranslation)
        {
            return (this + passedTranslation.Point);
        }

        /// <summary>
        /// Mirror this point across a line
        /// </summary>
        public Point MirrorAcross(Line passedAxisLine)
        {
            return this.Rotate3D(new Rotation(passedAxisLine, Angle.StraightAngle));
        }

        /// <summary>
        /// Flips the sign of each coordinate
        /// </summary>
        public Point Negate()
        {
            return new Point(X * -1, Y * -1, Z * -1);
        }

        /// <summary>
        /// uses the distance formula to find a the distance between this point and another
        /// </summary>
        public Distance DistanceTo(Point endPoint)
        {
            //distance formula
            double term1 = Math.Pow(X.ValueInInches - endPoint.X.ValueInInches, 2);
            double term2 = Math.Pow(Y.ValueInInches - endPoint.Y.ValueInInches, 2);
            double term3 = Math.Pow(Z.ValueInInches - endPoint.Z.ValueInInches, 2);

            double distanceInInches = Math.Sqrt(term1 + term2 + term3);

            return new Distance(new Inch(), distanceInInches);
        }

        /// <summary>
        /// returns the shortest distance from the line to the point
        /// </summary>
        public Distance DistanceTo(Line line)
        {
            Point projected = this.ProjectOntoLine(line);
            Distance distance = this.DistanceTo(projected);
            return distance;
        }
        
        public Distance DistanceTo(LineSegment segment)
        {
            var projected = ProjectOntoLine(new Line(segment));
            return segment.Contains(projected)
                ? this.DistanceTo(new Line(segment))
                : segment.EndPoints.Min(p => DistanceTo(p));
        }

        public Distance DistanceTo(Plane plane)
        {
            Vector toPlane = new Vector(this, plane.BasePoint);
            var cosineOfAngle = toPlane.Direction.DotProduct(plane.NormalDirection);
            var distance = toPlane.Magnitude * cosineOfAngle;

            return distance.AbsoluteValue();
        }

        /// <summary>
        /// Returns a vector that extends from the origin to this point
        /// </summary>
        public Vector ConvertToVector()
        {
            return new Vector(this);
        }

        /// <summary>
        /// Rotates a point about an axis with the given angle (returns a new point in that location)
        /// </summary>
        public Point Rotate3D(Rotation rotationToApply)
        {
            return Matrix.ShiftPoint(this, rotationToApply.Matrix);
        }

        /// <summary>
        /// Returns a line segment that goes through this point, is perpendicular to the destination line, and ends on that line
        /// </summary>
        public LineSegment MakePerpendicularLineSegment(Line passedDestinationLine)
        {
            if (!this.IsOnLine(passedDestinationLine))
            {
                //Make line from this point to the base point of the destination line
                Vector hypotenuse = new Vector(passedDestinationLine.BasePoint, this);
                Vector projection = hypotenuse.ProjectOntoLine(passedDestinationLine);
                LineSegment normalSegment = new LineSegment(projection.EndPoint, hypotenuse.EndPoint);
                return normalSegment;
            }
            return null;
        }

        /// <summary>
        /// Projects the point onto the line, by extending the normal direction from the line to the point.
        /// </summary>
        public Point ProjectOntoLine(Line projectOnto)
        {
            Vector hypotenuse = new Vector(projectOnto.BasePoint, this);
            Direction lineDirection = projectOnto.Direction;
            double dotProduct = hypotenuse.Direction.DotProduct(projectOnto.Direction);
            return projectOnto.GetPointAlongLine(dotProduct * hypotenuse.Magnitude);
        }

        /// <summary>
        /// projects a point onto a plane from the normal direction.
        /// </summary>
        public Point ProjectOntoPlane(Plane plane)
        {
            Vector toPlane = new Vector(this, plane.BasePoint);
            var cosineOfAngle = toPlane.Direction.DotProduct(plane.NormalDirection);
            var distance =  toPlane.Magnitude * cosineOfAngle;

            Line line = new Line(this, plane.NormalDirection);
                
            return line.GetPointAlongLine(distance);
        }

        /// <summary>
        /// Returns true if the point is on the passed line, false otherwise
        /// </summary>
        public bool IsOnLine(Line line)
        {
            return line.Contains(this);
        }

        /// <summary>
        /// Returns true if the point is on the passed vector, false otherwise
        ///// </summary>
        //public bool IsOnVector(Vector passedVector)
        //{
        //    return passedVector.Contains(this);
        //}

        /// <summary>
        /// Returns true if the point is on the passed line segment, false otherwise
        /// </summary>
        public bool IsOnLineSegment(LineSegment lineSegment)
        {
            return lineSegment.Contains(this);
        }

        public bool IsBaseOrEndPointOf(LineSegment segment)
        {
            return this == segment.BasePoint ||
                   this == segment.EndPoint;
        }

        /// <summary>
        /// Determines if the point is on the plane.
        /// </summary>
        public bool IsOnPlane(Plane plane)
        {
            return plane.Contains(this);
        }

        public Matrix ConvertToMatrixColumn()
        {
            return this.ConvertToVector().ConvertToMatrixColumn();
        }

        /// <summary>
        /// Shifts the Point with the given Shift
        /// </summary>
        public Point Shift(Shift shift)
        {
           return  Matrix.ShiftPoint(this, shift.Matrix);
        }

        #endregion

        #region Static Factory Methods
        public static Point MakePointWithFeet(double x, double y, double z = 0)
        {
            return new Point(new Foot(), x, y, z);
        }
        public static Point MakePointWithInches(double x, double y, double z = 0)
        {
            return new Point(new Inch(), x, y, z);
        }
        //public static Point MakePointWithInches(double x, double y, double z)
        //{
        //    return new Point(new Inch(), x, y, z);
        //}

        public static Point MakePointWithMillimeters(double x, double y, double z = 0)
        {
            return new Point(new Millimeter(), x, y, z);
        }

        public static Point MakePointWithInches(string inputString1, string inputString2)
        {
            double inputValue1 = double.Parse(inputString1);
            double inputValue2 = double.Parse(inputString2);

            Distance dim1 = new Distance(new Inch(), inputValue1);
            Distance dim2 = new Distance(new Inch(), inputValue2);
            return new Point(dim1, dim2);
        }

        public static Point[] Make2DPointArrayWithInches(double[] values)
        {
            Point[] toReturn = null;

            if (values.Length % 2 == 0)
            {
                toReturn = new Point[values.Length / 2];

                for (int i = 0; i < values.Length; i += 2)
                {
                    toReturn[i / 2] = MakePointWithInches(values[i], values[i + 1]);
                }
            }

            return toReturn;
        }
        #endregion

        
    }
}
