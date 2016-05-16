﻿using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnitClassLibrary;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A line segment is a portion of a line, whether curved or straight.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class LineSegment : Vector, IEdge, IEquatable<LineSegment>
    {
        #region Properties and Fields

        /// <summary>
        /// Returns this lineSegment's length (this is the same as the magnitude)
        /// </summary>
        public virtual Distance Length
        {
            get { return base.Magnitude; }
        }

        /// <summary>
        /// Returns the midpoint of this lineSegment
        /// </summary>
        public Point MidPoint
        {
            get { return EndPoints.CenterPoint(); }
        }

        public List<Point> EndPoints
        {
            get { return new List<Point>() { BasePoint, EndPoint }; }
        }

        public bool IsClosed { get { return false; } }

        public Direction InitialDirection { get { return Direction; } }
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
            : base(passedEndPoint)
        {
            _checkSegment();
        }

        /// <summary>
        /// Constructs a line segment that corresponds to the given vector, with the same basepoint, direction and length
        /// </summary>
        /// <param name="passedVector"></param>
        public LineSegment(Vector passedVector)
            : base(passedVector)
        {
            _checkSegment();
        }

        /// <summary>
        /// Constructs a line segment from a given start point to a given end point
        /// </summary>
        public LineSegment(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint)
        {
            _checkSegment();
        }
        
        /// <summary>
        /// Creates a new line segment with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        public LineSegment(Point passedBasePoint, Vector passedVector)
            : base(passedBasePoint, passedVector)
        {
            _checkSegment();
        }


        [JsonConstructor]
        public LineSegment(Point basePoint, Direction direction, Distance magnitude = null)
            : base(basePoint, direction, magnitude)
        {
            _checkSegment();
        }

        public LineSegment(List<Point> points) : base(points[0],points[1])
        {
            _checkSegment();
        }

        /// <summary>
        /// Copies a Linesegment
        /// </summary>
        public LineSegment(LineSegment toCopy)
            : base(toCopy.BasePoint, toCopy.Direction,toCopy.Magnitude) { }


        private void _checkSegment()
        {
            if (this.BasePoint == null || this.EndPoint == null)
            {
                throw new GeometricException("BasePoint or EndPoint is null!");
            }
            if (this.BasePoint == this.EndPoint)
            {
                throw new GeometricException("LineSegment has no breadth!");
            }
        }
        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
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
        /// <param name="passedLine">The line to check if this intersects with</param>
        /// <returns>returns the intersection point of the two lines or null if they do not</returns>
        public override Point IntersectWithLine(Line passedLine)
        {
            Point intersect = new Line(this).IntersectWithLine(passedLine);

            if (!ReferenceEquals(intersect, null) && intersect.IsOnLineSegment(this))
            {
                return intersect;
            }
            return null;
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
            Point potentialIntersect = base.IntersectWithLine(segment);

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

        /// <summary>
        /// Slices this lineSegment into two lineSegments at the given point and returns them with the longer segment first
        /// or the original line segment if the point is not on it
        /// </summary>
        /// <param name="spotToSliceAt">Spot on the line to slice at</param>
        /// <returns>returns the two parts the LineSegment is sliced into, or the original line segment if the point is not on it</returns>
        public List<LineSegment> Slice(Point spotToSliceAt)
        {
            if (this.ContainsOnInside(spotToSliceAt))
            {
                List<LineSegment> returnLines = new List<LineSegment>();

                //add the two segments to the return list
                returnLines.Add(new LineSegment(this.BasePoint, spotToSliceAt));
                returnLines.Add(new LineSegment(spotToSliceAt, this.EndPoint));

                return returnLines;
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
        public new LineSegment ProjectOntoLine(Line projectOnto)
        {
            Vector projection = base.ProjectOntoLine(projectOnto);
            if (projection.Magnitude != Distance.ZeroDistance)
            {
                return new LineSegment(projection);
            }
            return null;
        }

        public new LineSegment ProjectOntoPlane(Plane plane)
        {
            Vector projection = base.ProjectOntoPlane(plane);
            if (projection.Magnitude != Distance.ZeroDistance)
            {
                return new LineSegment(projection);
            }
            return null;
        }
        /// <summary>
        /// returns a copy of the line segment pointing in the opposite direction as the original
        /// </summary>
        /// <returns></returns>
        public new LineSegment Reverse()
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
        public new LineSegment Rotate(Rotation rotationToApply)
        {
            return new LineSegment(base.Rotate(rotationToApply));
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
        public new LineSegment Shift(Shift passedShift)
        {
            return new LineSegment(base.Shift(passedShift));
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
        public new LineSegment Translate(Translation translation)
        {
            return new LineSegment(base.Translate(translation));
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
                var vector1 = new Vector(this.BasePoint, point);
                var vector2 = new Vector(point, this.EndPoint);
                if (vector1.HasSameDirectionAs(this) && vector2.HasSameDirectionAs(this))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether or not the point is along/contained by this vector
        /// </summary>
        public new bool Contains(Point point)
        {
            //This method can make or break the Slice method. Handle very carefully.
            //This checks for a null point, and checks the distance from the point to the line.
            if (!new Line(this).Contains(point))
            {
                return false;
            }
            //We need this check before we check directions, because there is no direction if the point is the vector's basepoint
            if (point == this.BasePoint || point == this.EndPoint)
            {
                return true;
            }
            Vector pointVector = new Vector(this.BasePoint, point);
            bool sameDirection = this.HasSameDirectionAs(pointVector);
            bool greaterMagnitude = (this.Magnitude >= pointVector.Magnitude);

            return sameDirection && greaterMagnitude;
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
