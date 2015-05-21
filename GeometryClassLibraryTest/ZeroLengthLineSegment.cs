using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using GeometryClassLibrary;

namespace GeometryClassLibraryTest
{
    public class ZeroLengthLineSegment : LineSegment
    {
        public ZeroLengthLineSegment(): base(new Point(), new Point())
        {
        }

      
        //make a scenario in which both the starting point and ending are the same


    }
}
