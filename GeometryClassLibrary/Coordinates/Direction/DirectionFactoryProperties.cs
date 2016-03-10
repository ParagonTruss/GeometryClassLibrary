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
        /// A staticly defined Direction that is positive in the X direction. 
        /// Extends to the right in the XY plane.
        /// </summary>
        public static Direction Right { get { return new Direction(1, 0, 0); } }

        /// <summary>
        /// A staticly defined Direction that is negative in the X direction. 
        /// Extends to the left in the XY plane.
        /// </summary>
        public static Direction Left { get { return new Direction(-1, 0, 0); } }

        /// <summary>
        /// A staticly defined Direction that is positive in the Y direction. 
        /// Extends upward in the XY plane.
        /// </summary>
        public static Direction Up { get { return new Direction(0, 1, 0); } }

        /// <summary>
        /// A staticly defined Direction that is negative in the Y direction. 
        /// Extends downward in the XY plane.
        /// </summary>
        public static Direction Down { get { return new Direction(0, -1, 0); } }
        
        /// <summary>
        /// A staticly defined Direction that is positive in the Z direction.
        /// Extends towards the viewer when looking at the XY plane.
        /// </summary>
        public static Direction Out { get { return new Direction(0, 0, 1); } }

        /// <summary>
        /// A staticly defined Direction that is negative in the Z direction.
        /// Extends away from the viewer when looking at the XY plane.
        /// </summary>
        public static Direction Back { get { return new Direction(0, 0, -1); } }
    }
}
