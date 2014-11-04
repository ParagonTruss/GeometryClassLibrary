using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IlistPointListExtensionMethods
    {
        public static List<LineSegment> MakeIntoLineSegmentsThatMeet(this List<Point> points)
        {
            List<LineSegment> toReturn = new List<LineSegment>();

            for (int l = 0; l < points.Count(); l++)
            {
                if (l != points.Count() - 1)
                {
                    LineSegment newLine = new LineSegment(points[l], points[l + 1]);
                    Point ponte = newLine.EndPoint;
                    toReturn.Add(newLine);
                }
                else
                {
                    toReturn.Add(new LineSegment(points[l], points[0]));
                }
            }

            return toReturn;
        }
    }
}
