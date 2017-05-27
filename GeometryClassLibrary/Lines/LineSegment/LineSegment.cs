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
using System.Linq;
using System.Collections.Generic;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A line segment is a portion of a line, whether curved or straight.
    /// </summary>
    public class LineSegment : IVector, IEdge, IEquatable<LineSegment>, IShift<LineSegment>
    {
        #region Properties and Fields
        /// <summary>
        /// A point on the line to use as a reference.
        /// </summary>
        public Point BasePoint { get; }
        
        /// <summary>
        /// Returns the end point of the vector
        /// </summary>
        public Point EndPoint { get; }

        /// <summary>
        /// The direction the line is extends from the base point in one direction
        /// Note: it also extends in the direction opposite
        /// </summary>
        public Direction Direction => new Direction(BasePoint, EndPoint);

        /// <summary>
        /// Returns this lineSegment's length (this is the same as the magnitude)
        /// </summary>
        public Distance Length => BasePoint.DistanceTo(EndPoint);

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public Distance Magnitude => Length;

        /// <summary>
        /// Returns the midpoint of this lineSegment
        /// </summary>
        public Point MidPoint => EndPoints.CenterPoint();

        public List<Point> EndPoints => new List<Point>(2) { BasePoint, EndPoint };

        public bool IsClosed => false;

        public Direction InitialDirection => Direction;

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        protected LineSegment() { }

        /// <summary>
        /// Creates a Line Segment that extends from the origin to the given end point
        /// </summary>
        public LineSegment(Point passedEndPoint)
        {
            this.BasePoint = Point.Origin;
            this.EndPoint = passedEndPoint;
            _checkSegment();
        }

        /// <summary>
        /// Constructs a line segment that corresponds to the given vector, with the same basepoint, direction and length
        /// </summary>
        /// <param name="passedVector"></param>
        public LineSegment(Vector passedVector)
        {
            this.BasePoint = passedVector.BasePoint;
            this.EndPoint = passedVector.EndPoint;
            _checkSegment();
        }

        /// <summary>
        /// Constructs a line segment from a given start point to a given end point
        /// </summary>
        public LineSegment(Point passedBasePoint, Point passedEndPoint)
        {
            this.BasePoint = passedBasePoint;
            this.EndPoint = passedEndPoint;
            _checkSegment();
        }
        
        /// <summary>
        /// Creates a new line segment with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        public LineSegment(Point passedBasePoint, IVector passedVector)
        {
            this.BasePoint = passedBasePoint ?? Point.Origin;
            this.EndPoint = new Vector(this.BasePoint, passedVector).EndPoint;
            _checkSegment();
        }

        public LineSegment(Point basePoint, Direction direction, Distance magnitude)
        {
            this.BasePoint = basePoint ?? Point.Origin;
            this.EndPoint = new Vector(this.BasePoint, direction, magnitude).EndPoint;
            _checkSegment();
        }
        public LineSegment(Direction direction, Distance magnitude)
        {
            this.BasePoint = Point.Origin;
            this.EndPoint = new Vector(direction, magnitude).EndPoint;
            _checkSegment();
        }

        public LineSegment(List<Point> points)
        {
            this.BasePoint = points[0];
            this.EndPoint = points[1];
            _checkSegment();
        }

        /// <summary>
        /// Copies a Linesegment
        /// </summary>
        public LineSegment(LineSegment toCopy)
            : this(toCopy.BasePoint, toCopy.EndPoint) { }

        public LineSegment(DistanceType distanceUnit, double x1, double y1, double x2, double y2)
            : this(new Point(distanceUnit, x1, y1), new Point(distanceUnit, x2, y2)) { }

        public LineSegment(DistanceType distanceUnit, double x1, double y1, double z1, double x2, double y2, double z2)
            : this(new Point(distanceUnit, x1, y1, z1), new Point(distanceUnit, x2, y2, z2)) { }

        private void _checkSegment()
        {
            if (this.BasePoint == null || this.EndPoint == null)
            {
                throw new ArgumentNullException("BasePoint or EndPoint is null!");
            }
            if (this.BasePoint == this.EndPoint)
            {
                throw new InvalidLineSegmentException("LineSegment has no breadth!");
            }
        }
        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return this.BasePoint.GetHashCode() ^ this.EndPoint.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(LineSegment segment1, LineSegment segment2)
        {
            if ((object)segment1 == null)
            {
                if ((object)segment2 == null)
                {
                    return true;
                }
                return false;
            }
            return segment1.Equals(segment2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(LineSegment segment1, LineSegment segment2)
        {
            if ((object)segment1 == null)
            {
                if ((object)segment2 == null)
                {
                    return false;
                }
                return true;
            }
            return !segment1.Equals(segment2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is LineSegment))
            {
                return false;
            }
           
            LineSegment comparableSegment = (LineSegment)obj;

            // if the two points' x and y are equal, returns true
            bool pointsAreEqual = comparableSegment.BasePoint.Equals(this.BasePoint) && comparableSegment.EndPoint.Equals(this.EndPoint);
            bool pointsAreReverse = comparableSegment.BasePoint.Equals(this.EndPoint) && comparableSegment.EndPoint.Equals(this.BasePoint);

            return pointsAreEqual || pointsAreReverse;  
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public bool Equals(LineSegment segment)
        {
            if (segment == null)
            {
                return false;
            }

            // if the two points' x and y are equal, returns true
            bool pointsAreEqual = segment.BasePoint.Equals(this.BasePoint) && segment.EndPoint.Equals(this.EndPoint);
            bool pointsAreReverse = segment.BasePoint.Equals(this.EndPoint) && segment.EndPoint.Equals(this.BasePoint);

            return pointsAreEqual || pointsAreReverse;  
        }

        /// <summary>
        /// returns the line segment as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("BasePoint= {0}, EndPoint= {1}, Length={2}", this.BasePoint.ToString(), this.EndPoint.ToString(), this.Length.ToString());
        }
        #endregion

        #region Methods

        /// <summary>
        /// Checks if this Vector intersects the given line and returns the point if it does or null otherwise
        /// </summary>
        /// <param name="line">The line to check if this intersects with</param>
        /// <returns>returns the intersection point of the two lines or null if they do not</returns>
        public Point IntersectWithLine(Line line)
        {
            Point intersect = new Line(this).IntersectWithLine(new Line(line));

            if (!ReferenceEquals(intersect, null) && intersect.IsOnLineSegment(this))
            {
                return intersect;
            }
            return null;
        }

        public Point IntersectWithPlane(Plane plane)
        {
            return plane.IntersectWithSegment(this);
        }

        /// <summary>
        /// Checks if this LineSegment intersects with the given LineSegment and returns the point of intersection
        /// </summary>
        public Point IntersectWithSegment(LineSegment segment)
        {
            if (this.BasePoint.IsBaseOrEndPointOf(segment))
            {
                return this.BasePoint;
            }
            else if (this.EndPoint.IsBaseOrEndPointOf(segment))
            {
                return this.EndPoint;
            }
            Point potentialIntersect = new Line(this).IntersectWithLine(new Line(segment));

            if (potentialIntersect != null && potentialIntersect.IsOnLineSegment(segment) && potentialIntersect.IsOnLineSegment(this))
            {
                return potentialIntersect;
            }
            return null;
        }


        /// <summary>
        /// Returns true if the vector shares a base point or endpoint with the passed vector
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public bool SharesABaseOrEndPointWith(LineSegment segment)
        {
            return (this.BasePoint == segment.EndPoint
                || this.BasePoint == segment.BasePoint
                || this.EndPoint == segment.EndPoint
                || this.EndPoint == segment.BasePoint);
        }



        /// <summary>
        /// Checks to see if a vector contains another vector.  Useful for checking if members touch
        /// </summary>
        /// <param name="passedVector">The Vector to see if is contained in this one</param>
        /// <returns>Returns a bool of whether or not the Vector is contained</returns>
        public bool Contains(LineSegment segment)
        {
            bool containsBasePoint = (this.Contains(segment.BasePoint));
            bool containsEndPoint = (this.Contains(segment.EndPoint));

            return containsBasePoint && containsEndPoint;
        }
        
        public List<LineSegment> Slice(List<LineSegment> slicingSegments)
        {
            var currentSegments = new List<LineSegment> { this };
            foreach (var slicer in slicingSegments)
            {
                var tempList = new List<LineSegment>();
                foreach (var segment in currentSegments)
                {
                    tempList.AddRange(segment.SliceWithSegment(slicer));
                }
                currentSegments = tempList;
            }
            return currentSegments.OrderBy(segment => segment.BasePoint.DistanceTo(this.BasePoint)).ToList();
        }

        public List<LineSegment> SliceWithSegment(LineSegment slicingSegment)
        {
            var splitPoint = this.IntersectWithSegment(slicingSegment);
            if (splitPoint == null || this.IsParallelTo(slicingSegment))
            {
                return new List<LineSegment>() { this };
            }
            return Slice(splitPoint);
        }
        public List<LineSegment> SliceWithLine(Line slicingSegment)
        {
            var splitPoint = this.IntersectWithLine(slicingSegment);
            if (splitPoint == null || this.IsParallelTo(slicingSegment))
            {
                return new List<LineSegment>() { this };
            }
            return Slice(splitPoint);
        }
        /// <summary>
        /// Returns them with the piece containing the base point first.
        /// </summary>
        public List<LineSegment> Slice(Point spotToSliceAt)
        {
            if (this.ContainsOnInside(spotToSliceAt))
            {
                List<LineSegment> splitSegments = new List<LineSegment>();

                //add the two segments to the return list
                splitSegments.Add(new LineSegment(this.BasePoint, spotToSliceAt));
                splitSegments.Add(new LineSegment(spotToSliceAt, this.EndPoint));

                return splitSegments;
            }
            else
            {
                //if its not on the line just return the original segment
                return new List<LineSegment>() { this };
            }
        }

        /// <summary>
        /// Projects the linesegment onto a line (calls the method in its base class Vector)
        /// </summary>
        /// <param name="projectOnto"></param>
        /// <returns></returns>
        public LineSegment ProjectOntoLine(Line projectOnto)
        {
            Point basePoint = this.BasePoint.ProjectOntoLine(projectOnto);
            Point endPoint = this.EndPoint.ProjectOntoLine(projectOnto);
            if (basePoint != endPoint)
            {
                return new LineSegment(basePoint, endPoint);
            }
            return null;
        }

        public LineSegment ProjectOntoPlane(Plane plane)
        {
            Point newBasePoint = BasePoint.ProjectOntoPlane(plane);
            Point newEndPoint = EndPoint.ProjectOntoPlane(plane);
            if (newBasePoint != newEndPoint)
            {
                return new LineSegment(newBasePoint, newEndPoint);
            }
            return null;
        }
        /// <summary>
        /// returns a copy of the line segment pointing in the opposite direction as the original
        /// </summary>
        /// <returns></returns>
        public LineSegment Reverse()
        {
            return new LineSegment(this.EndPoint, this.Direction.Reverse(), this.Length);
        }

        IEdge IEdge.Reverse()
        {
            return new LineSegment(this).Reverse();
        }

        /// <summary>
        /// Rotates the LineSegment about the given axis the given angle (calls the method in its base class)
        /// </summary>
        /// <param name="rotationToApply">The Rotation to apply(that stores the axis to rotate around and the angle to rotate) to the LineSegment</param>
        /// <returns></returns>
        public LineSegment Rotate(Rotation rotationToApply)
        {
            Point rotatedBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Point rotatedEndPoint = this.EndPoint.Rotate3D(rotationToApply);
            return new LineSegment(rotatedBasePoint, rotatedEndPoint);
        }

        IEdge IEdge.Rotate(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        /// <summary>
        /// Performs the given shift on this LineSegment
        /// </summary>
        /// <param name="passedShift"></param>
        /// <returns></returns>
        public LineSegment Shift(Shift passedShift)
        {
            return new LineSegment(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Allows for generic shifting on an IEdge
        /// </summary>
        /// <param name="passedShift"></param>
        /// <returns></returns>
        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Translates linesegment the given distance in the given direction
        /// </summary>
        public LineSegment Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newEndPoint = this.EndPoint.Translate(translation);

            return new LineSegment(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// Allows for generic translation on an IEdge
        /// </summary>
        /// <param name="passedTranslation"The Translation to apply></param>
        /// <returns>A new LineSegment as a IEdge that has been translated</returns>
        IEdge IEdge.Translate(Point point)
        {
            return this.Translate(point);
        }

        /// <summary>
        /// Returns a copy of this lineSegment
        /// </summary>
        /// <returns></returns>
        IEdge IEdge.Copy()
        {
            return new LineSegment(this);
        }

        /// <summary>
        /// returns the line segment of overlap, if they don't then it returns null
        /// </summary>
        public LineSegment OverlappingSegment(LineSegment segment)
        {
            HashSet<Point> pointList = new HashSet<Point>();
            foreach(Point point in segment.EndPoints)
            {
                if(this.Contains(point))
                {
                    pointList.Add(point);
                }
            }
            foreach(Point point in this.EndPoints)
            {
                if(segment.Contains(point))
                {
                    pointList.Add(point);
                }
            }
            if (pointList.Count == 2)
            {
                return new LineSegment(pointList.ElementAt(0), pointList.ElementAt(1));
            }
            else
            {
                return null;
            }
        }

        public bool Overlaps(LineSegment segment)
        {
            return (this == segment) ||
                (this.IsParallelTo(segment) &&
                    (this.EndPoints.Any(p => segment.ContainsOnInside(p)) ||
                    segment.EndPoints.Any(p => this.ContainsOnInside(p))));
        }

        public bool DoesIntersectNotTouching(Plane passedPlane)
        {
            if (passedPlane.Contains(this.BasePoint) || passedPlane.Contains(this.EndPoint))
            {
                return false;
            }
            if (passedPlane.PointIsOnSameSideAs(this.BasePoint, this.EndPoint))
            {
                return false;
            }
            return true;
        }

        public bool DoesIntersectNotTouching(LineSegment segment)
        {
            Point point = this.IntersectWithSegment(segment);
            if (point != null)
            {
                if (point.IsBaseOrEndPointOf(this) ||
                    point.IsBaseOrEndPointOf(segment))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool ContainsOnInside(Point point)
        {
            if (!point.IsBaseOrEndPointOf(this))
            {
                return this.Contains(point);
            }
            return false;
        }
        /// <summary>
        /// Determines whether or not the point is along/contained by this line segment
        /// </summary>
        public bool Contains(Point point)
        {
            var dist1 = BasePoint.DistanceTo(point);
            var dist2 = EndPoint.DistanceTo(point);
            var samelength = (dist1 + dist2).Equals(this.Length); //if the two lengths are the same, and the point is on the line, then the point is also on the linesegment within error margins
           
            return point.IsOnLine(new Line(this)) && samelength;

        }

        public bool ContainsOnInside(LineSegment other)
        {
            return this.ContainsOnInside(other.BasePoint) &&
                   this.ContainsOnInside(other.EndPoint);
        }

        public Point HypotheticalIntersection(Line line2)
        {
            return new Line(this).IntersectWithLine(line2);
        }
        #endregion
    }
}

