﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A line segment is a portion of a line, whether curved or straight.
    /// </summary>
    [DebuggerDisplay("Base Point = {BasePoint.X.Inches}, {BasePoint.Y.Inches}, {BasePoint.Z.Inches}, End Point = {EndPoint.X.Inches}, {EndPoint.Y.Inches}, {EndPoint.Z.Inches}, Length = {Length.Inches},  Direction Vector = {XComponentOfDirection.Inches}, {YComponentOfDirection.Inches}, {ZComponentOfDirection.Inches}")]
    [Serializable]
    public class LineSegment : Vector, IComparable<LineSegment>, IEdge
    {
        #region Properties and Fields

        /// <summary>
        /// Returns this lineSegment's length (this is the same as the magnitude)
        /// </summary>
        public Distance Length
        {
            get { return base.Magnitude; }
            set { base.Magnitude = value; }
        }

        /// <summary>
        /// Returns the midpoint of this lineSegment
        /// </summary>
        public Point MidPoint
        {
            get
            {
                //Find midpoint for each component
                Distance xMid = (base.Magnitude / 2) * base.Direction.XComponentOfDirection;
                Distance yMid = (base.Magnitude / 2) * base.Direction.YComponentOfDirection;
                Distance zMid = (base.Magnitude / 2) * base.Direction.ZComponentOfDirection;

                //then add our base point so it is in the right location
                return new Point(xMid, yMid, zMid) + this.BasePoint;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default zero constructor
        /// </summary>
        public LineSegment()
            : base() { }

        /// <summary>
        /// Creates a Line Segment that extends from the origin to the given end point
        /// </summary>
        /// <param name="passedEndPoint"></param>
        public LineSegment(Point passedEndPoint)
            : base(passedEndPoint) { }

        /// <summary>
        /// Constructs a line segment that corresponds to the given vector, with the smae basepoint, direction and length
        /// </summary>
        /// <param name="passedVector"></param>
        public LineSegment(Vector passedVector)
            : base(passedVector) { }

        /// <summary>
        /// Constructs a line segment from a given start point to a given end point
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedEndPoint"></param>
        public LineSegment(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint) { }
        
        /// <summary>
        /// Creates a new ineSegment with the given BasePoint in the same direction and magnitude as the passed Vector
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedDirection"></param>
        public LineSegment(Point passedBasePoint, Vector passedVector)
            : base(passedBasePoint, passedVector) { }

        /// <summary>
        /// Creates a new LineSegment with the given BasePoint in the given direction with the given length, or 0 if it is omitted
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedDirection"></param>
        /// <param name="passedLength"></param>
        public LineSegment(Point passedBasePoint, Direction passedDirection, Distance? passedLength = null)
            : base(passedBasePoint, passedDirection, passedLength) { }

        /// <summary>
        /// Copies a Linesegment creating new objects so the original is not changed with the copy
        /// </summary>
        /// <param name="toCopy">LineSegment to copy</param>
        public LineSegment(LineSegment toCopy)
            : base(toCopy) { }

        #endregion

        #region Overloaded Operators

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
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                LineSegment comparableSegment = (LineSegment)obj;

                // if the two points' x and y are equal, returns true
                bool pointsAreEqual = comparableSegment.BasePoint.Equals(this.BasePoint) && comparableSegment.EndPoint.Equals(this.EndPoint);
                bool pointsAreReverse = comparableSegment.BasePoint.Equals(this.EndPoint) && comparableSegment.EndPoint.Equals(this.BasePoint);

                return pointsAreEqual || pointsAreReverse;
            }
            //if it wasnt a linesegment its not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other segment
        /// NOTE: BASED SOLELY ON LENGTH.  MAY WANT TO CHANGE LATER
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(LineSegment other)
        {
            if (this.Length.Equals(other.Length))
                return 0;
            else
                return this.Length.CompareTo(other.Length);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Slices this lineSegment into two lineSegments at the given point and returns them with the longer segment first
        /// or the original line segment if the point is not on it
        /// </summary>
        /// <param name="spotToSliceAt">Spot on the line to slice at</param>
        /// <returns>returns the two parts the LineSegment is sliced into, or the original line segment if the point is not on it</returns>
        public List<LineSegment> Slice(Point spotToSliceAt)
        {
            if (spotToSliceAt.IsOnLineSegment(this))
            {
                List<LineSegment> returnLines = new List<LineSegment>();

                //add the two segments to the return list
                returnLines.Add(new LineSegment(this.BasePoint, spotToSliceAt));
                returnLines.Add(new LineSegment(spotToSliceAt, this.EndPoint));

                //sort them so the longest is first (sort is ascending by default so we need to reverse if)
                returnLines.Sort();
                returnLines.Reverse();

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
        /// <param name="projectOnto">The line to project the segment onto</param>
        /// <returns>A new line segment that has that is in the same direction as the line projected onto and whose length
        /// is the length of the line segment along the projected line</returns>
        public new LineSegment ProjectOntoLine(Line projectOnto)
        {
            return new LineSegment(base.ProjectOntoLine(projectOnto));
        }

        /// <summary>
        /// returns a copy of the line segment pointing in the opposite direction as the original
        /// </summary>
        /// <returns>Returns a line segment with the base and end points flipped</returns>
        public LineSegment Reverse()
        {
            return new LineSegment(this.EndPoint, this.Direction.Reverse(), this.Length);
        }

        /// <summary>
        /// Rotates the LineSegment about the given axis the given angle (calls the method in its base class)
        /// </summary>
        /// <param name="passedRotation">The Rotation to appply to the Segment</param>
        /// <returns>A new Line Segment that has been rotated</returns>
        public LineSegment Rotate(Rotation passedRotation)
        {
            return new LineSegment(base.Rotate(passedRotation));
        }

        /// <summary>
        /// Rotates the LineSegment about the given axis the given angle (calls the method in its base class)
        /// </summary>
        /// <param name="passedRotation">The Rotation to appply to the Segment</param>
        /// <returns>A new Line Segment that has been rotated</returns>
        IEdge IEdge.Rotate(Rotation passedRotation)
        {
            return new LineSegment(base.Rotate(passedRotation));
        }

        /// <summary>
        /// Performs the given shift on this LineSegment
        /// </summary>
        /// <param name="passedShift">The shift to preform on the Line Segment</param>
        /// <returns>A new Line Segment that has been shifted with the given shift</returns>
        public new LineSegment Shift(Shift passedShift)
        {
            return new LineSegment(base.Shift(passedShift));
        }

        /// <summary>
        /// Allows for generic shifting on an IEdge
        /// </summary>
        /// <param name="passedShift">Shift to preform on the segment</param>
        /// <returns>Returns a new LineSegment that has been shifted, but as an IEdge object</returns>
        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Translates the vector the given distance in the given direction
        /// </summary>
        /// <param name="translation">The distance to translate the line segment in each direction</param>
        /// <returns>returns a new segment that has been translated</returns>
        public new LineSegment Translate(Point translation)
        {
            return new LineSegment(base.Translate(translation));
        }

        /// <summary>
        /// Returns a copy of this lineSegment
        /// </summary>
        /// <returns>A copy of the lineSegment</returns>
        public IEdge Copy()
        {
            return new LineSegment(this);
        }

        #endregion
    }
}
