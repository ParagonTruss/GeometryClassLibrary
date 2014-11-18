using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Pentagon : RegularPolygon
    {
        /// <summary>
        /// Creates a regular Pentagon where all sides are the passed length
        /// </summary>
        /// <param name="passedSideLength"></param>
        public Pentagon(Distance passedSideLength)
            :base(5,passedSideLength) {  }
    }
}
