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
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using static GeometryClassLibrary.Point;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;

namespace GeometryClassLibrary
{
    public enum Axis { X, Y, Z };

    /// <summary>
    /// A line in 3d space.
    /// </summary>
    public partial class Line : IEquatable<Line>, IShift<Line>
    {
        #region Properties and Fields

        //Predefined lines to use as references
        public static Line XAxis = new Line(Point.Origin, Direction.Right);
        public static Line YAxis = new Line(Point.Origin, Direction.Up);
        public static Line ZAxis = new Line(Point.Origin, Direction.Out);

        /// <summary>
        /// A point on the line to use as a reference.
        /// </summary>
        public Point BasePoint { get; }

        /// <summary>
        /// The direction the line is extends from the base point in one direction
        /// Note: it also extends in the direction opposite
        /// </summary>
        public Direction Direction { get; }

        #endregion
     
        #region Constructors

        /// <summary>
        /// Null constructor
        /// </summary>
        protected Line() { }

        /// <summary>
        /// Creates a line through the origin and a point
        /// </summary>
        public Line(Point point)
        {
            this.BasePoint = Origin;
            this.Direction = new Direction(point);
        }

        /// <summary>
        /// Creates a line with the given direction and point if passed, otherwise it uses the origin as the base point
        /// </summary>
        public Line(Direction direction, Point basePoint = null)
        {
            this.BasePoint = basePoint ?? Origin;
            this.Direction = direction;
        }

        public Line(Point basePoint, Direction direction)
        {
            this.BasePoint = basePoint;
            this.Direction = direction;
        }
        /// <summary>
        /// Constructs a line through any 2 points
        /// </summary>
        public Line(Point basePoint, Point otherPoint)
        {
            this.BasePoint = basePoint;
            this.Direction = new Direction(basePoint, otherPoint);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public Line(Line toCopy)
            : this(toCopy.Direction, toCopy.BasePoint) { }

        /// <summary>
        /// Creates a new line with the same direction but different base 
        /// Useful for turning vectors, and segments back into lines.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="newBasePoint"></param>
        public Line(Point newBasePoint, Line line) : this(line.Direction, newBasePoint) { }

        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static bool operator ==(Line line1, Line line2)
        {
            if ((object)line1 == null)
            {
                if ((object)line2 == null)
                {
                    return true;
                }
                return false;
            }
            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
        {
            if ((object)line1 == null)
            {
                if ((object)line2 == null)
                {
                    return false;
                }
                return true;
            }
            return !line1.Equals(line2);
        }

        public override bool Equals(object obj)
        {
            //make sure the passed object is not null
            if (obj == null || !(obj is Line))
            {
                return false;
            }

            Line comparableLine = (Line)obj;

            bool linesAreParallel = IsParallelTo(comparableLine);
            bool basePointIsOnLine = BasePoint.IsOnLine(comparableLine);

            return (linesAreParallel && basePointIsOnLine);
           
        }
        public bool Equals(Line other)
        {
            //make sure the passed object is not null
            if (other == null)
            {
                return false;
            }

            bool linesAreParallel = IsParallelTo(other);
            bool basePointIsOnLine = BasePoint.IsOnLine(other);

            return (linesAreParallel && basePointIsOnLine);

        }
        ///// <summary>
        ///// planning on implementing by sorting based on smallest x intercept in 2d plane
        ///// from left to right and if they share then the one that occurs first
        ///// </summary>
        //public int CompareTo(Line other)
        //{
        //    //see if the first line doesnt intersect
        //    try
        //    {
        //        //if it doesnt throw an error it does and we can keep going
        //        Distance nullTest = this.XInterceptIn2D();
        //    }
        //    catch (Exception)
        //    {
        //        //see if the second line also doesnt intersect
        //        try
        //        {
        //            //the second one intersects, so the first is "greater" than the second
        //            Distance nullTest = this.XInterceptIn2D();
        //            return 1;
        //        }
        //        catch (Exception) //if they both dont intersect they are "equal" in this way of sorting
        //        {
        //            return 0;
        //        }
        //    }

        //    //see if only the second one doesnt intersect
        //    try
        //    {
        //        //if it doesnt throw an error it does and we can keep going
        //        Distance nullTest = other.XInterceptIn2D();
        //    }
        //    catch (Exception)
        //    {
        //        //the second doesnt intersect so the first is "smaller" than the second
        //        return -1;
        //    }

        //    //now that we've handled the cases where they dont intersect, we can check the values
        //    if (this.XInterceptIn2D() == other.XInterceptIn2D())
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return this.XInterceptIn2D().CompareTo(other.XInterceptIn2D());
        //    }
        //}

        #endregion

        #region Methods

        #region Intercept Methods
        /// <summary>
        /// Returns the X intercept of the line if the z Distance is ignored
        /// </summary>
        public Distance XInterceptIn2D() => XZIntercept()?.X;
       
        /// <summary>
        /// Returns the Y intecept of the Line if the z Distance is ignored
        /// </summary>
        public Distance YInterceptIn2D() => YZIntercept()?.Y;

        /// <summary>
        /// Returns the point at which this line intercepts the XY-Plane
        /// </summary>
        public Point XYIntercept() => this.IntersectWithPlane(Plane.XY);
        
        /// <summary>
        /// Returns the point at which this line intercepts the XZ-Plane
        /// </summary>
        public Point XZIntercept() => this.IntersectWithPlane(Plane.XZ);
 
        /// <summary>
        /// Returns the point at which this line intercepts the YZ-Plane
        /// </summary>
        public Point YZIntercept() => Plane.YZ.IntersectWithLine(this);
#endregion

        public Plane PlaneThroughLineInDirectionOf(Axis passedAxis)
        {
            Line extrusionLine;

            switch (passedAxis)
            {
                case Axis.X:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(1, 0));
                    break;
                case Axis.Y:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(0, 1));
                    break;
                case Axis.Z:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(0, 0, 1));
                    break;
                default:
                    throw new ArgumentException("You passed in an unknown Axis Enum");
            }
            return new Plane(extrusionLine, this);
        }

        public bool Contains(LineSegment segment)
        {
            return this.Contains(segment.BasePoint) && this.Contains(segment.EndPoint);
        }

        /// <summary>
        /// Returns the smaller of the two angles between these lines.
        /// </summary>
        public virtual Angle SmallestAngleBetween(Line line)
        {
            Angle returnAngle = AngleBetween(line);

            if (returnAngle.InDegrees > 90)
            {
                return (Angle.StraightAngle - returnAngle);
            }
            else
            {
                return returnAngle;
            }
        }

        /// <summary>
        /// Finds the angle between the two Lines.
        /// </summary>
        public Angle AngleBetween(Line otherLine)
        {
            return this.Direction.AngleBetween(otherLine.Direction);
        }        
	
        public Angle SignedAngleBetween(Line line, Line referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Line.ZAxis;
            }

            return this.Direction.AngleFromThisToThat(line.Direction, referenceNormal.Direction);
	    }

        /// <summary>
        /// Returns the point a given distance along the line.
        /// </summary>
        public Point GetPointAlongLine(Distance distance)
        {
            Distance newX = BasePoint.X + distance * Direction.X;
            Distance newY = BasePoint.Y + distance * Direction.Y;
            Distance newZ = BasePoint.Z + distance * Direction.Z;
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Determines if this line and the passed line are parallel.
        /// </summary>
        public bool IsParallelTo(Line passedLine)
        {
            return (passedLine.Direction == this.Direction || passedLine.Direction == this.Direction.Reverse());
        }

        /// <summary>
        /// Determines if two lines are parallel up to a custom angular tolerance.
        /// That is, if the lines are within that angle of each other the lines are considered "parallel"
        /// </summary>
        public bool IsParallelTo(Line line, Angle tolerance)
        {
            var angle = this.SmallestAngleBetween(line);
            angle = Angle.Exactly(angle.InRadians.Value, Angle.Radians);
            return angle == tolerance;
        }

       // public 
        /// <summary>
        /// Returns whether or not the two lines are perindicular to each other
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual bool IsPerpendicularTo(Line passedLine)
        {
            return this.SmallestAngleBetween(passedLine) == 90 * new Angle(1, Degrees);
        }

        /// <summary>
        /// Returns the point at which a line intersects the passed line
        /// </summary>
        public virtual Point IntersectWithLine(Line passedLine)
        {
            if (this.Equals(passedLine))
            {
                return this.BasePoint;
            }
            if (!this.IsCoplanarWith(passedLine) || this.IsParallelTo(passedLine))
            {
                //The lines do not intersect
                return null;
            }

            //Following a formula from (http://mathworld.wolfram.com/Line-LineIntersection.html)

            Vector directionVectorA = new Vector(this.BasePoint, this.UnitVector(new Inch()));
            Vector directionVectorB = new Vector(passedLine.BasePoint, passedLine.UnitVector(new Inch()));
            Vector basePointDiffVectorC = new Vector(this.BasePoint, passedLine.BasePoint);

            Vector crossProductCB = basePointDiffVectorC.CrossProduct(directionVectorB);
            Vector crossProductAB = directionVectorA.CrossProduct(directionVectorB);

            double crossProductABMagnitudeSquared = crossProductAB.Magnitude.ValueInInches * crossProductAB.Magnitude.ValueInInches;
            double dotProductOfCrossProducts = (crossProductCB.DotProduct(crossProductAB)).ValueIn(new SquareInch());

            double solutionVariable = dotProductOfCrossProducts / crossProductABMagnitudeSquared;
            Distance solutionVariableDistance = new Distance(new Inch(), solutionVariable);

            Point intersectionPoint = this.GetPointAlongLine(solutionVariableDistance);

            return intersectionPoint;
        }

        public virtual Point IntersectWithPlane(Plane plane)
        {
            return plane.IntersectWithLine(this);
        }

        /// <summary>
        /// return the point of intersection, if any, between this line and the passed polygon
        /// </summary>
        public virtual Point IntersectWithPolygon(Polygon polygon)
        {
            return polygon.IntersectWithLine(this);
        }

        public virtual List<Point> IntersectionCoplanarPoints(Polygon polygon)
        {
            return polygon.IntersectionCoplanarPoints(this);
        }

        /// <summary>
        /// Returns whether or not the two lines intersect
        /// </summary>
        public bool IntersectsLine(Line passedLine)
        {
            Point intersect = this.IntersectWithLine(passedLine);

            if (intersect != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not the vector and line intersect
        /// </summary>
        public virtual bool DoesIntersect(LineSegment segment)
        {
            Line newLine = new Line(segment);
            Point intersect = this.IntersectWithLine(newLine);

            if (intersect == null)
            {
                return false;
            }
            return intersect.IsOnLineSegment(segment);
        }

        /// <summary>
        /// Returns whether or not the Polygon and Line intersect
        /// </summary>
        public virtual bool DoesIntersect(Polygon passedPolygon)
        {
            return (passedPolygon.DoesIntersect(this));
        }

        /// <summary>
        /// Returns whether ot not this line itnersects the Polyhedron
        /// </summary>
        public virtual bool DoesIntersect(Polyhedron passedPolyhedron) => passedPolyhedron.Polygons.Any(DoesIntersect);
        

        /// <summary>
        /// Rotates a line about the given axis by the amount of the passed angle
        /// </summary>
        /// <param name="rotationToApply">The Rotation to apply to the point that stores the axis to rotate around and the angle to rotate</param>
        /// <returns></returns>
        public Line Rotate(Rotation rotationToApply)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Vector newDirectionVector = this.UnitVector(new Inch()).Rotate(rotationToApply);
            return new Line(newDirectionVector.Direction, newBasePoint);
        }

        /// <summary>
        /// Returns true if the passed line is in the same plane as this one, AKA if it intersects or is parallel to the other line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsCoplanarWith(Line passedLine)
        {
            double[] point1Line1 = { this.BasePoint.X.ValueInInches, this.BasePoint.Y.ValueInInches, this.BasePoint.Z.ValueInInches };

            Point anotherPointOnLine1 = this.GetPointAlongLine(new Distance(1, Inches));
            double[] point2Line1 = { anotherPointOnLine1.X.ValueInInches, anotherPointOnLine1.Y.ValueInInches, anotherPointOnLine1.Z.ValueInInches };

            double[] point1Line2 = { passedLine.BasePoint.X.ValueInInches, passedLine.BasePoint.Y.ValueInInches, passedLine.BasePoint.Z.ValueInInches };

            Point anotherPointOnLine2 = passedLine.GetPointAlongLine(new Distance(1, Inches) * 2);
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
        /// Translates the line the given distance in the given direction
        /// </summary>
        /// <returns></returns>
        public Line Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newOtherPoint = this.GetPointAlongLine(new Distance(2, Inches)).Translate(translation);

            return new Line(newBasePoint, newOtherPoint);
        }

        /// <summary>
        /// Shifts the Line with the given shift
        /// </summary>
        public Line Shift(Shift passedShift)
        {
            //shift it as a vector since we currently don't shift directions
            Vector linesVector = this.UnitVector(new Inch());

            Vector shifted = linesVector.Shift(passedShift);

            //then construct a new line with that information
            return new Line(shifted);
        }

        /// <summary>
        /// Creates a vector on this line, of unit length for the passed distance type.
        /// </summary>
        public virtual Vector UnitVector(DistanceType passedType)
        {
            return new Vector(this.BasePoint, this.Direction.UnitVector(passedType));
        }

        /// <summary>
        /// Makes a Perpendicular Line to this line that is in the passed plane
        /// this assumes the line is in the plane
        /// </summary>
        public Line MakePerpendicularLineInGivenPlane(Plane planeToMakePerpendicularLineIn)
        {
            if (planeToMakePerpendicularLineIn.IsParallelTo(this))
            {
                //rotate it 90 degrees in the nornal of the plane and it will be perpendicular to the original
                return this.Rotate(new Rotation(new Vector(this.BasePoint, planeToMakePerpendicularLineIn.NormalVector), RightAngle));
            }
            else
            {
                throw new ArgumentException("The given line is not in the given plane");
            }
        }

        /// <summary>
        /// Projects the given line onto the Plane
        /// </summary>
        /// <param name="projectOnto">The Plane to Project this Line onto</param>
        /// <returns>Returns a new Line that is this Line projected onto the Plane</returns>
        public Line ProjectOntoPlane(Plane projectOnto)
        {
            //http://www.euclideanspace.com/maths/geometry/elements/plane/lineOnPlane/index.htm
            //When using unit vectors, the project on the plane is simply planeNormal X (lineDirection X planeNormal)
            Vector aCrossB = this.Direction.UnitVector(new Inch()).CrossProduct(projectOnto.NormalVector.UnitVector(new Inch()));
            Vector projectionVector = projectOnto.NormalVector.UnitVector(new Inch()).CrossProduct(aCrossB);

            return new Line(projectionVector.Direction, this.BasePoint);
        }

        /// <summary>
        /// Determines if the line contains the point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Point point)
        {
            if (point == null)
            {
                return false;
            }
            Distance distance = point.DistanceTo(this);
            bool equal = distance.Equals(Distance.ZeroDistance);
            return equal;
        }

        public LineSegment GetSegmentAlongLine(Distance distance1, Distance distance2)
        {
            return new LineSegment(GetPointAlongLine(distance1), GetPointAlongLine(distance2));
        }
        #endregion
    }
}
