using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public partial class Direction
    {
        public static Direction NoDirection { get { return new Direction(0, 0, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is positive in the X direction, which can be thought of as going "right" in the XY plane.
        /// </summary>
        public static Direction Right { get { return new Direction(1, 0, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is negative in the X direction, which can be thought of as going "left" in the XY plane
        /// </summary>
        public static Direction Left { get { return new Direction(-1, 0, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is positive in the Y direction, which can be thought of as going "up" in the XY plane
        /// </summary>
        public static Direction Up { get { return new Direction(0, 1, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is negative in the Y direction, which can be thought of as going "down" in the XY plane
        /// </summary>
        public static Direction Down { get { return new Direction(0, -1, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is positive in the Z direction, which can be thought of coming out of the screen towards you when viewing the XY plane
        /// </summary>
        public static Direction Out { get { return new Direction(0, 0, 1); } }
        
        /// <summary>
        /// A staticly defined Direction that is negative in the Z direction, which can be thought of going back out of the screen away you when viewing the XY plane
        /// </summary>
        public static Direction Back { get { return new Direction(0, 0, -1); } }
    }
}
