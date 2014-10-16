using System;
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
        #region Fields and Properties
        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _basePoint;
        public Point BasePoint
        {
            get { return _basePoint; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _originPointTwo;
        public Point OriginPointTwo
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        private Direction _direction;
        public Direction Direction
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
        #endregion

        #region Constructors

        public Arc(Point originPointTwo)
        {
            this._basePoint = new Point();
            this._originPointTwo = originPointTwo;
        }

        public Arc(Point originPointOne, Point originPointTwo)
        {
            this._basePoint = originPointOne;
            this._originPointTwo = originPointTwo;
        }

        #endregion

        //These are currently only based on the points and not anything else about it!
        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(Arc segment1, Arc segment2)
        {
            if (segment1 == null || segment2 == null)
            {
                if (segment1 == null && segment2 == null)
                {
                    return true;
                }
                return false;
            }
            return segment1.Equals(segment2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(Arc segment1, Arc segment2)
        {
            if (segment1 == null || segment2 == null)
            {
                if (segment1 == null && segment2 == null)
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
            Arc comparableArc = null;

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableArc = (Arc)obj;

                // if the two points' x and y are equal, returns true
                return (comparableArc._basePoint.Equals(this._basePoint) && comparableArc._originPointTwo.Equals(this._originPointTwo))
                    || (comparableArc._basePoint.Equals(this._originPointTwo) && comparableArc._originPointTwo.Equals(this._basePoint));
            }
            catch
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
            return new Arc(_basePoint.Shift(passedShift), _originPointTwo.Shift(passedShift));
        }

        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        #endregion
    }
}
