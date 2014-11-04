﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved (not straight)
    /// </summary>
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

        public virtual Direction Direction { get; set; }

        #endregion

        #region Constructors

        public Arc()
        {
            this.Direction = new Direction();
            _basePoint = new Point();
            _endPoint = new Point();
        }

        public Arc(Point originPointTwo, Direction passedDirection = null)
        {
            if (passedDirection == null)
            {
                this.Direction = new Direction(originPointTwo);
            }
            else
            {
                this.Direction = passedDirection;
            }

            _basePoint = new Point();
            _endPoint = originPointTwo;
        }

        public Arc(Point basePoint, Point originPointTwo, Direction passedDirection = null)
        {
            if (passedDirection == null)
            {
                this.Direction = new Direction(basePoint, originPointTwo);
            }
            else
            {
                this.Direction = passedDirection;
            }

            _basePoint = basePoint;
            _endPoint = originPointTwo;
        }

        #endregion

        //These are currently only based on the points and not anything else about it!
        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
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
        /// Not a perfect inequality operator, is only accurate up to the Dimension Class's accuracy
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

        public Arc Shift(Shift passedShift)
        {
            return new Arc(this.BasePoint.Shift(passedShift), this.EndPoint.Shift(passedShift), this.Direction);
        }

        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        #endregion
    }
}
