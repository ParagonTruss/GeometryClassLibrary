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
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;
using static UnitClassLibrary.AngleUnit.Angle;

namespace GeometryClassLibrary
{
    public interface ILinear
    {
        Point BasePoint { get; }
        Direction Direction { get; }
    }

    public static class LinearExtensions
    {
        /// <summary>
        /// Returns the point a given distance along the line.
        /// </summary>
        public static Point GetPointAlongLine(this ILinear thisLine, Distance distance)
        {
            Distance newX = thisLine.BasePoint.X + distance * thisLine.Direction.X;
            Distance newY = thisLine.BasePoint.Y + distance * thisLine.Direction.Y;
            Distance newZ = thisLine.BasePoint.Z + distance * thisLine.Direction.Z;
            return new Point(newX, newY, newZ);
        }
        
        /// <summary>
        /// Finds the angle between the two Lines.
        /// </summary>
        public static Angle AngleBetween(this ILinear thisLine, ILinear otherLine)
        {
            return thisLine.Direction.AngleBetween(otherLine.Direction);
        }
        
        /// <summary>
        /// Returns the smaller of the two angles between these lines.
        /// </summary>
        public static Angle SmallestAngleBetween(this ILinear thisLine, ILinear otherLine)
        {
            Angle angle = thisLine.AngleBetween(otherLine);

            if (angle.InDegrees > 90)
            {
                angle = Angle.StraightAngle - angle;
            }
            return angle;
        }
        
        public static Angle SignedAngleBetween(this ILinear thisLine, ILinear otherLine, ILinear referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Line.ZAxis;
            }

            return thisLine.Direction.AngleFromThisToThat(otherLine.Direction, referenceNormal.Direction);
        }
        
        /// <summary>
        /// Returns true if the passed line is in the same plane as this one, AKA if it intersects or is parallel to the other line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public static bool IsCoplanarWith(this ILinear thisLine, ILinear otherLine)
        {
            double[] point1Line1 = { thisLine.BasePoint.X.ValueInInches, thisLine.BasePoint.Y.ValueInInches, thisLine.BasePoint.Z.ValueInInches };

            Point anotherPointOnLine1 = thisLine.GetPointAlongLine(new Distance(1, Inches));
            double[] point2Line1 = { anotherPointOnLine1.X.ValueInInches, anotherPointOnLine1.Y.ValueInInches, anotherPointOnLine1.Z.ValueInInches };

            double[] point1Line2 = { otherLine.BasePoint.X.ValueInInches, otherLine.BasePoint.Y.ValueInInches, otherLine.BasePoint.Z.ValueInInches };

            Point anotherPointOnLine2 = otherLine.GetPointAlongLine(new Distance(1, Inches) * 2);
            double[] point2Line2 = { anotherPointOnLine2.X.ValueInInches, anotherPointOnLine2.Y.ValueInInches, anotherPointOnLine2.Z.ValueInInches };

            Matrix pointsMatrix = new Matrix(4, 4);

            pointsMatrix.SetRow(0, point1Line1);
            pointsMatrix.SetRow(1, point2Line1);
            pointsMatrix.SetRow(2, point1Line2);
            pointsMatrix.SetRow(3, point2Line2);

            double[] onesColumn = { 1, 1, 1, 1 };
            pointsMatrix.SetColumn(3, onesColumn);

            // checks if it is equal to 0
            double determinant = Math.Abs(pointsMatrix.Determinant());
            Distance determinateDistance = new Distance(determinant, Inches);
            return determinateDistance == Distance.ZeroDistance;
        }
        
        /// <summary>
        /// Determines if this line and the passed line are parallel.
        /// </summary>
        public static bool IsParallelTo(this ILinear thisLine, ILinear otherLine)
        {
            return (otherLine.Direction == thisLine.Direction || otherLine.Direction == thisLine.Direction.Reverse());
        }
        
        /// <summary>
        /// Returns whether or not the two lines are perindicular to each other
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public static bool IsPerpendicularTo(this ILinear thisLine, ILinear otherLine)
        {
            return thisLine.SmallestAngleBetween(otherLine) == new Angle(90, Degrees);
        }

        public static Plane PlaneThroughLineInDirectionOf(this ILinear thisLine, Axis passedAxis)
        {
            Line extrusionLine;

            switch (passedAxis)
            {
                case Axis.X:
                    extrusionLine = new Line(thisLine.BasePoint,
                        thisLine.BasePoint - Point.MakePointWithInches(1, 0));
                    break;
                case Axis.Y:
                    extrusionLine = new Line(thisLine.BasePoint,
                        thisLine.BasePoint - Point.MakePointWithInches(0, 1));
                    break;
                case Axis.Z:
                    extrusionLine = new Line(thisLine.BasePoint,
                        thisLine.BasePoint - Point.MakePointWithInches(0, 0, 1));
                    break;
                default:
                    throw new ArgumentException("You passed in an unknown Axis Enum");
            }
            return new Plane(extrusionLine, thisLine);
        }
        
        /// <summary>
        /// Makes a Perpendicular Line to this line that is in the passed plane
        /// this assumes the line is in the plane
        /// </summary>
        public static Line MakePerpendicularLineInGivenPlane(this ILinear thisLine, Plane planeToMakePerpendicularLineIn)
        {
            if (planeToMakePerpendicularLineIn.IsParallelTo(thisLine))
            {
                //rotate it 90 degrees in the nornal of the plane and it will be perpendicular to the original
                return new Line(thisLine).Rotate(new Rotation(new Vector(thisLine.BasePoint, planeToMakePerpendicularLineIn.NormalVector), RightAngle));
            }
            else
            {
                throw new ArgumentException("The given line is not in the given plane");
            }
        }
    }
}
