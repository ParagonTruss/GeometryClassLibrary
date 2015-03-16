using System;
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
    public class LineSegment : Vector, IComparable<LineSegment>, IEdge
    {
        #region Properties and Fields

        /// <summary>
        /// Returns this lineSegment's length (this is the same as the magnitude)
        /// </summary>
        public virtual Distance Length
        {
            get { return base.Magnitude; }
            set { base.Magnitude = value; }
        }

        /// <summary>
        /// Returns the midpoint of this lineSegment
        /// </summary>
        public virtual Point MidPoint
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
        public LineSegment(Point passedBasePoint, Direction passedDirection, Distance passedLength = null)
            : base(passedBasePoint, passedDirection, passedLength) { }

        /// <summary>
        /// Copies a Linesegment creating new objects so the original is not changed with the copy
        /// </summary>
        /// <param name="toCopy">LineSegment to copy</param>
        public LineSegment(LineSegment toCopy)
            : base(toCopy) { }

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
        /// <param name="projectOnto"></param>
        /// <returns></returns>
        public new LineSegment ProjectOntoLine(Line projectOnto)
        {
            return new LineSegment(base.ProjectOntoLine(projectOnto));
        }

        /// <summary>
        /// returns a copy of the line segment pointing in the opposite direction as the original
        /// </summary>
        /// <returns></returns>
        public new LineSegment Reverse()
        {
            return new LineSegment(this.EndPoint, this.Direction.Reverse(), this.Length);
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
        /// shifts this LineSegment with the passed Coordinate System
        /// </summary>
        /// <param name="toShiftWith">the Coordinate System to shift this LineSegment with</param>
        /// <returns>A new LineSegment that has been shifted with the given Coordinate System</returns>
        public new LineSegment SystemShift(CoordinateSystem toShiftWith)
        {
            return new LineSegment(base.SystemShift(toShiftWith));
        }

        /// <summary>
        /// Shifts this LineSegment from one Coordinate System to the other
        /// </summary>
        /// <param name="to">The Coordinate System to shift this LineSegment to</param>
        /// <param name="from">The coordinate System this LineSegment is currently in. If left out it defaults to the 
        /// World Coordinate System</param>
        /// <returns>Returns a new LineSegment that has been shifted to the given Coordinate System</returns>
        /// 
        public new LineSegment ShiftCoordinateSystemsToFrom(CoordinateSystem to, CoordinateSystem from = null)
        {
            return new LineSegment(base.ShiftCoordinateSystemsToFrom(to, from));
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
        /// Translates the vector the given distance in the given direction
        /// </summary>
        /// <param name="translation">The Translation to apply to the lineSegment</param>
        /// <returns>returns a new LineSegment as a IEdge that has been translated</returns>
        public new LineSegment Translate(Translation translation)
        {
            return new LineSegment(base.Translate(translation));
        }

        /// <summary>
        /// Allows for generic translation on an IEdge
        /// </summary>
        /// <param name="passedTranslation"The Translation to apply></param>
        /// <returns>A new LineSegment as a IEdge that has been translated</returns>
        IEdge IEdge.Translate(Translation passedTranslation)
        {
            return this.Translate(passedTranslation);
        }

        /// <summary>
        /// Returns a copy of this lineSegment
        /// </summary>
        /// <returns></returns>
        IEdge IEdge.Copy()
        {
            return new LineSegment(this);
        }

        #endregion

        public override string ToString()
        {
                return string.Format("BasePoint= {0}, EndPoint= {1}, Length={2}", this.BasePoint.ToString(), this.EndPoint.ToString(), this.Length.ToString());
        }
    }
}
