
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary.ComparisonMethods
{
    public class CompareByMagnitude : IComparer<Vector>
    {
        public int Compare(Vector vector1, Vector vector2)
        {
            if (vector1.Magnitude < vector2.Magnitude)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
