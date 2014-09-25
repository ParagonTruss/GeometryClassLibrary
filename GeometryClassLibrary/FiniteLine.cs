using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A finite line does not extend infinitely in all directions. It is a portion of a line that may be either curved or straight.
    /// </summary>
    public class FiniteLine : Line
    {
        #region Internal Variables
        /// <summary>
        /// The length of the line portion, since it is finite and therefore measureable
        /// </summary>
        protected double _length;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Zero constructor
        /// </summary>
        public FiniteLine()
        {
            _length = 0;
        }

        /// <summary>
        /// Pass up to Line class
        /// </summary>
        /// <param name="passedEndPoint"></param>
        public FiniteLine(Point passedEndPoint)
            : base(passedEndPoint)
        {

        }

        /// <summary>
        /// Pass up to Line class
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedEndPoint"></param>
        public FiniteLine(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint)
        {

        }

        /// <summary>
        /// Pass up to Line class
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedDirection"></param>
        public FiniteLine(Point passedBasePoint, Vector passedDirection)
            : base(passedBasePoint, passedDirection)
        {

        }
        #endregion

    }
}
