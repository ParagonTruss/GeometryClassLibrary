using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public class CompareByMidPoint : IComparer<LineSegment>
    {
        public int Compare(LineSegment segment1, LineSegment segment2)
        {
            return (new CompareInOrderZXY()).Compare(segment1.MidPoint, segment2.MidPoint);
        }
    }
}
