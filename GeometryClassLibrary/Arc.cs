using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved (not straight)
    /// </summary>
    [DebuggerDisplay("BasePoint = {BasePoint.X.Inches}, {BasePoint.Y.Inches}, {BasePoint.Z.Inches}, EndPoint = {EndPoint.X.Inches}, {EndPoint.Y.Inches}, {EndPoint.Z.Inches}, Direction: Azumuth = {Direction.Phi.Degrees}, Inclination{Direction.Theta.Degrees}")]
    [Serializable]
    public class Arc : IEdge, IComparable<Arc>
    {
        #region Properties and Fields

        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _basePoint;
        public virtual Point BasePoint
        {
            get { return _basePoint; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _endPoint;
        public virtual Point EndPoint
        {
            get { return _endPoint; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// The direction that the arc travels in from the base point
        /// </summary>
        public virtual Direction Direction { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a empty arc
        /// </summary>
        public Arc()
        {
            this.Direction = new Direction();
            _basePoint = new Point();
            _endPoint = new Point();
        }

        /// <summary>
        /// Creates an arc with the base point at the origin, the endpoint at the passed point and in the 
        /// given direction, or if omitted, the direction to the end point
        /// </summary>
        /// <param name="passedEndPoint">The point at which the arc ends</param>
        /// <param name="passedDirection">The direction the arc travels from the base point</param>
        public Arc(Point passedEndPoint, Direction passedDirection = null)
        {
            if (passedDirection == null)
            {
                this.Direction = new Direction(passedEndPoint);
            }
            else
            {
                this.Direction = new Direction(passedDirection);
            }

            _basePoint = new Point();
            _endPoint = new Point(passedEndPoint);
        }
        
        /// <summary>
        /// Creates an arc with the base point at the passed base point, the endpoint at the passed end point and in the 
        /// passed direction, or if omitted, the direction to the end point
        /// </summary>
        /// <param name="basePoint">The point from which the Arc originates</param>
        /// <param name="passedEndPoint">The point at which the Arc ends</param>
        /// <param name="passedDirection"></param>
        public Arc(Point basePoint, Point passedEndPoint, Direction passedDirection = null)
        {
            if (passedDirection == null)
            {
                this.Direction = new Direction(basePoint, passedEndPoint);
            }
            else
            {
                this.Direction = new Direction(passedDirection);
            }

            _basePoint = new Point(basePoint);
            _endPoint = new Point(passedEndPoint);
        }

        /// <summary>
        /// Creates a copy of this Arc
        /// </summary>
        /// <param name="toCopy">The Arc to copy</param>
        public Arc(Arc toCopy)
        {
            _basePoint = new Point(toCopy.BasePoint);
            _endPoint = new Point(toCopy.EndPoint);
            this.Direction = new Direction(toCopy.Direction);
        }

        #endregion

        //These are currently only based on the points and not anything else about it!
        #region Overloaded Operators

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
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                Arc comparableArc = (Arc)obj;

                // if the two points' x and y are equal, returns true
                bool arcAreEqual = comparableArc._basePoint.Equals(this._basePoint) && comparableArc._endPoint.Equals(this._endPoint);
                bool arcAreRevese = comparableArc._basePoint.Equals(this._endPoint) && comparableArc._endPoint.Equals(this._basePoint);

                return arcAreEqual || arcAreRevese;
            }
            //if it wasnt an arc than its obviously not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other segment
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Arc other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the given shift on the Arc and returns a new Arc that has been shifted
        /// </summary>
        /// <param name="passedShift">The shift to perform on the Arc</param>
        /// <returns></returns>
        public Arc Shift(Shift passedShift)
        {
            return new Arc(this.BasePoint.Shift(passedShift), this.EndPoint.Shift(passedShift), this.Direction);
        }

        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Returns a copy of this Arc
        /// </summary>
        /// <returns></returns>
        public IEdge Copy()
        {
            return new Arc(this);
        }


        #endregion


        public IEdge Rotate(UnitClassLibrary.Angle passedRotationAngle)
        {
            throw new NotImplementedException();
        }


        public IEdge RotateAsIEdge(Rotation passedRotation)
        {
            throw new NotImplementedException();
        }
    }
}
