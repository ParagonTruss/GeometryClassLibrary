/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

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
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.AngleUnit.Angle;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved as around a circle.
    /// </summary>
    public class Arc : IEdge
    {
        #region Properties and Fields

        /// <summary>
        /// The length of an arc of a circle with radius r and subtending an angle theta, with the circle center — i.e., the central angle 
        /// </summary>
        public Distance ArcLength => RadiusOfCurvature * CentralAngle.InRadians.Value;

        /// <summary>
        /// The area between an arc and the center of a would be circle.
        /// </summary>
        public Area SectorArea => new Area(new SquareInch(), 0.5 * CentralAngle.InRadians * (RadiusOfCurvature.ValueInInches*RadiusOfCurvature.ValueInInches));

        /// <summary>
        /// The area of the shape limited by the arc and a straight line between the two end points.
        /// </summary>
        public Area SegmentArea => new Area(new SquareInch(), .5 * Math.Pow(RadiusOfCurvature.ValueInInches,2) * (CentralAngle.InRadians - Angle.Sine(CentralAngle)));

        /// <summary>
        ///  an angle whose apex (vertex) is the center O of a circle and whose legs (sides) 
        ///  are radii intersecting the circle in two distinct points A and B 
        ///  thereby subtending an arc between those two points whose angle is (by definition) equal to that of the central angle itself.
        ///  It is also known as the arc segment's angular distance.
        /// http://en.wikipedia.org/wiki/Central_angle
        /// </summary>
        public Angle CentralAngle
        {
            get
            {
                if (this.IsClosed)
                {
                    return Angle.FullCircle;
                }
                else
                {
                    var direction1 = new Direction(CenterPoint, BasePoint);
                    var direction2 = new Direction(CenterPoint, EndPoint);
                    //var normal = direction1.CrossProduct(direction2);
                    //if (normal != NormalDirection)
                    //{
                    //    throw new Exception();
                    //}
                    var result = direction1.AngleFromThisToThat(direction2, NormalDirection);
                    var modded = result.ProperAngle;
                    return modded;
                }
            }
        }

        /// <summary>
        /// The radius of the would be circle formed by the arc
        /// </summary>
        public Distance RadiusOfCurvature => CenterPoint.DistanceTo(BasePoint);

        /// <summary>
        /// The direction from the arc's start point to end point as if a straight line were drawn between then
        /// </summary>
        public virtual Direction StraightLineDirection => new Direction(this.BasePoint, this.EndPoint);

        /// <summary>
        /// The direction from the arc's start point to end point as if a straight line were drawn between them.
        /// This is defined and used by IEdge
        /// </summary>
        public Direction Direction => InitialDirection;

        /// <summary>
        /// The direction tangent to the basepoint.
        /// </summary>
        public Direction InitialDirection => new Direction(BasePoint, CenterPoint).CrossProduct(NormalDirection);

        /// <summary>
        /// Where the arc begins.
        /// </summary>
        public Point BasePoint { get; }

        /// <summary>
        /// Where the arc ends.
        /// </summary>
        public Point EndPoint { get; }

        /// <summary>
        /// The center point of the circle that the arc fits along.
        /// </summary>
        public Point CenterPoint { get; }
        // The centerPoint could be calculated from other data, in some cases.
        // However in the case of a full closed arc, one needs to specify the size, since the basepoint is the endpoint.

        public Direction NormalDirection { get; }

        public bool IsClosed => BasePoint == EndPoint;
        public bool IsAcute => this.CentralAngle < StraightAngle;

        #endregion

        #region Constructors

        private Arc() { }

        public Arc(Point basePoint, Point endPoint, Direction initialDirection)
        {
            this.BasePoint = basePoint;
            this.EndPoint = endPoint;
            if (this.IsClosed)
            {
                throw new GeometricException("Not enough information given to determine curvature.");
            }
            var segmentBetweenPoints = new LineSegment(basePoint,endPoint);
            this.NormalDirection = initialDirection.CrossProduct(segmentBetweenPoints.Direction);

            var plane1 = new Plane(BasePoint, initialDirection);
            var plane2 = new Plane(segmentBetweenPoints.MidPoint, segmentBetweenPoints.Direction);
            var intersectingLine = plane1.IntersectWithPlane(plane2);
            var containingPlane = new Plane(BasePoint, NormalDirection);
            var centerPoint = intersectingLine.IntersectWithPlane(containingPlane);
            this.CenterPoint = centerPoint;
            //var line1 = new Line(BasePoint, NormalDirection.CrossProduct(initialDirection));
            //var line2 = new Line(segmentBetweenPoints.MidPoint, NormalDirection.CrossProduct(segmentBetweenPoints.Direction));
            //this._centerPoint = line1.IntersectWithLine(line2);
        }

        public Arc(Point basePoint, Point endPoint, Line normalLine)
        {
            if (basePoint != endPoint && !new Direction(basePoint, endPoint).IsPerpendicularTo(normalLine.Direction))
            {
                throw new GeometricException();
            }
            var projected = basePoint.ProjectOntoLine(normalLine);
            if (projected == basePoint)
            {
                throw new GeometricException();
            }
            this.BasePoint = basePoint;
            this.EndPoint = endPoint;
            this.CenterPoint = projected;
            this.NormalDirection = normalLine.Direction;            
        }

        private Arc(Point basePoint, Point endPoint, Point centerPoint, Direction normalDirection)
        {
            this.BasePoint = basePoint;
            this.EndPoint = endPoint;
            this.CenterPoint = centerPoint;
            this.NormalDirection = normalDirection;
        }

        /// <summary>
        /// Creates a copy of this Arc
        /// </summary>
        /// <param name="toCopy">The Arc to copy</param>
        public Arc(Arc toCopy)
        {
            BasePoint = toCopy.BasePoint;
            EndPoint = toCopy.EndPoint;
            CenterPoint = toCopy.CenterPoint;
            NormalDirection = toCopy.NormalDirection;
        }

        #endregion



        #region Methods

        public Plane ContainingPlane()
        {
            return new Plane(CenterPoint, NormalDirection);
        }

        /// <summary>
        /// Performs the given shift on the Arc and returns a new Arc that has been shifted
        /// </summary>
        public Arc Shift(Shift passedShift)
        {
            Point newBasePoint = BasePoint.Shift(passedShift);
            Point newEndPoint = EndPoint.Shift(passedShift);

            // make the direction into a line and then shift it
            Line directionLine = new Line(CenterPoint, NormalDirection).Shift(passedShift);

            return new Arc(newBasePoint, newEndPoint, directionLine.BasePoint, directionLine.Direction);
        }

        /// <summary>
        /// Performs the given shift on this arc as an IEdge and returns it as an IEdge
        /// </summary>
        /// <param name="passedShift">The shift to apply to this arc</param>
        /// <returns>A new Arc as an IEdge that has been shifted</returns>
        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Returns a copy of this Arc
        /// </summary>
        /// <returns>A new Arc object that is the same as this one</returns>
        IEdge IEdge.Copy()
        {
            return new Arc(this);
        }

        /// <summary>
        /// Perfomrs the given rotation on the Arc a returns a new object that has been rotated
        /// </summary>
        /// <param name="passedRotation">The Rotation to rotate this Arc with</param>
        /// <returns>A new Arc that has been rotated</returns>
        public Arc Rotate(Rotation passedRotation)
        {
            Point newBasePoint = BasePoint.Rotate3D(passedRotation);
            Point newEndPoint = EndPoint.Rotate3D(passedRotation);

            //cheat a bit and make the direction into a line and then shift it
            Line directionLine = new Line(CenterPoint,NormalDirection).Rotate(passedRotation);

            return new Arc(newBasePoint, newEndPoint, directionLine.BasePoint, directionLine.Direction);
        }

        /// <summary>
        /// Perfomrs the given rotation on the Arc as an IEdge and returns a new object that has been rotated
        /// </summary>
        /// <param name="passedRotation">The Rotation to rotate this Arc with</param>
        /// <returns>A new Arc as an IEdge that has been rotated</returns>
        IEdge IEdge.Rotate(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        /// <summary>
        /// Translates the arc with the given translation
        /// </summary>
        /// <param name="translation">The translation to apply to the Arc</param>
        /// <returns>A new Arc object that has been translated</returns>
        public Arc Translate(Translation translation)
        {
            Point newBasePoint = BasePoint.Translate(translation);
            Point newEndPoint = EndPoint.Translate(translation);
            Point newCenterPoint = CenterPoint.Translate(translation);
       
            return new Arc(newBasePoint, newEndPoint, newCenterPoint, this.NormalDirection);
        }

        /// <summary>
        /// Translates the arc as an IEdge with the given translation
        /// </summary>
        IEdge IEdge.Translate(Point point)
        {
            return this.Translate(point);
        }

        public Arc Reverse()
        {
            return new Arc(EndPoint, BasePoint, CenterPoint, NormalDirection.Reverse());
        }

        IEdge IEdge.Reverse()
        {
            return new Arc(EndPoint, BasePoint, CenterPoint, NormalDirection.Reverse());
        }
        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return BasePoint.GetHashCode() ^ EndPoint.GetHashCode() ^ CenterPoint.GetHashCode() ^ NormalDirection.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Arc arc1, Arc arc2)
        {
            if ((object)arc1 == null)
            {
                if ((object)arc1 == null)
                {
                    return true;
                }
                return false;
            }
            return arc1.Equals(arc2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Arc arc1, Arc arc2)
        {
            if ((object)arc1 == null)
            {
                if ((object)arc2 == null)
                {
                    return false;
                }
                return true;
            }
            return !arc1.Equals(arc2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Arc))
            {
                return false;
            }
            Arc arc = (Arc)obj;
            if (this.CenterPoint != arc.CenterPoint)
            {
                return false;
            }
            if (this.IsClosed && arc.IsClosed)
            {
                return this.NormalDirection == arc.NormalDirection ||
                       this.NormalDirection == arc.NormalDirection.Reverse();
            }
            return (this.BasePoint == arc.BasePoint &&
                    this.EndPoint == arc.EndPoint &&
                    this.NormalDirection == arc.NormalDirection) ||
                   (this.BasePoint == arc.EndPoint &&
                    this.EndPoint == arc.BasePoint &&
                    this.NormalDirection == arc.NormalDirection.Reverse());
        }

        #endregion
    }
}
