using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    //ToDo: Ask John how to write this with Enums, and simplify this down to 2 classes
    public class CompareByX : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            if (point1.X < point2.X)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
    public class CompareByY : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            if (point1.Y < point2.Y)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
    public class CompareByZ : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            if (point1.Z < point2.Z)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
    public class CompareInOrderXYZ : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            //Equality check is first, so that we don't give preference to one of the points
            //when their Components are negligibly different
            if (point1.X == point2.X)
            {
                if (point1.Y == point2.Y)
                {
                    if (point1.Z == point2.Z)
                    {
                        return 0;
                    }
                    return (new CompareByZ()).Compare(point1, point2);
                }
                return (new CompareByY()).Compare(point1, point2);
            }
            return (new CompareByX()).Compare(point1, point2);
        }
    }
    public class CompareInOrderYZX : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            //Equality check is first, so that we don't give preference to one of the points
            //when their Components are negligibly different
            if (point1.Y == point2.Y)
            {
                if (point1.Z == point2.Z)
                {
                    if (point1.X == point2.X)
                    {
                        return 0;
                    }
                    return (new CompareByX()).Compare(point1, point2);
                }
                return (new CompareByZ()).Compare(point1, point2);
            }
            return (new CompareByY()).Compare(point1, point2); 
        }
    }
    public class CompareInOrderZXY : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            //Equality check is first, so that we don't give preference to one of the points
            //when their Components are negligibly different
            if (point1.Z == point2.Z)
            {
                if (point1.X == point2.X)
                {
                    if (point1.Y == point2.Y)
                    {
                        return 0;
                    }
                    return (new CompareByY()).Compare(point1, point2);
                }
                return (new CompareByX()).Compare(point1, point2);
            }
            return (new CompareByZ()).Compare(point1, point2);
        }
    }
}
