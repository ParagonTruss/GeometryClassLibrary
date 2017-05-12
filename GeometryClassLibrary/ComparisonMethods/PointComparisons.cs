/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System.Collections.Generic;

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
    public class CompareInOrderZYX : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            //Equality check is first, so that we don't give preference to one of the points
            //when their Components are negligibly different
            if (point1.Z == point2.Z)
            {
                if (point1.X == point2.Y)
                {
                    if (point1.Y == point2.X)
                    {
                        return 0;
                    }
                    return (new CompareByX()).Compare(point1, point2);
                }
                return (new CompareByY()).Compare(point1, point2);
            }
            return (new CompareByZ()).Compare(point1, point2);
        }
    }
}
