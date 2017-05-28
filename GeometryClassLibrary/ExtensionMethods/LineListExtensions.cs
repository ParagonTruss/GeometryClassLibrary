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

using System;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary.DistanceUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibrary
{
    public static class LineListExtensions
    {
        /// <summary>
        /// Defines a plane region using the given lines and where they intersect as long as the lines are all coplanar
        /// ToDo: Needs a unit test
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public static Polygon GeneratePolygonFromIntersectingLines(this List<Line> passedLines)
        {
            List<LineSegment> toUse = new List<LineSegment>();

            //find where they each intersect
            for (int i = 0; i < passedLines.Count; i++)
            {
                List<Point> intersections = new List<Point>();

                for (int j = i + 1; j < passedLines.Count; j++)
                {
                    Point intersection = passedLines[i].IntersectWithLine(passedLines[j]);
                    if (intersection != null && !intersections.Contains(intersection))
                    {
                        intersections.Add(intersection);
                    }

                    if (intersections.Count == 2)
                    {
                        toUse.Add(new LineSegment(intersections[0], intersections[1]));
                    }
                    else
                    {
                        throw new ArgumentException("lines are invalid");
                    }
                }
            }
            return new Polygon(toUse);
        }

        /// <summary>
        /// Returns the line with the smallest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        public static Line LineWithLowestXInterceptIn2D(this IList<Line> passedLines)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.YInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.XInterceptIn2D()));
            var list = dictionary.OrderBy(pair =>(pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line with the largest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest x from</param>
        /// <returns>Returns the Line with the largest x intercept</returns>
        public static Line LineWithLargestXInterceptIn2D(this IList<Line> passedLines)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.XInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.XInterceptIn2D()));
            var list = dictionary.OrderByDescending(pair => (pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line whose x intercept on the 2D xy-plane is closest to the given distance on the x-axis. If two share the same intercept it returns the
        /// first line in the list
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest x from</param>
        /// <param name="pointToFindTheClosestInterceptTo">The value on the x-axis to find the line whose intercept is closest to</param>
        /// <returns>Returns the Line whose intercept is closest to the specified point on the x-axis</returns>
        public static Line LineWithXInterceptIn2DClosestTo(this IList<Line> passedLines, Distance pointToFindTheClosestInterceptTo)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.XInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.XInterceptIn2D() - pointToFindTheClosestInterceptTo));
            var list = dictionary.OrderBy(pair => Math.Abs(pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line whose x intercept on the 2D xy-plane is farthest from the given distance on the x-axis. If two share the same intercept it returns the
        /// first line in the list
        /// </summary>
        /// <param name="passedLines">The list of lines to find the farthest in x from</param>
        /// <param name="pointToFindTheFarthestInterceptTo">The value on the x-axis to find the line whose intercept is farthest from</param>
        /// <returns>Returns the Line whose intercept is farthest from the specified point on the x-axis</returns>
        public static Line LineWithXInterceptIn2DFarthestFrom(this IList<Line> passedLines, Distance pointToFindTheFarthestInterceptTo)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.XInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.XInterceptIn2D() - pointToFindTheFarthestInterceptTo));
            var list = dictionary.OrderByDescending(pair => Math.Abs(pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line with the smallest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the smallest y from</param>
        /// <returns>Returns the Line with the smallest y intercept</returns>
        public static Line LineWithSmallestYInterceptIn2D(this IList<Line> passedLines)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.YInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.YInterceptIn2D()));
            var list = dictionary.OrderBy(pair => pair.Value.ValueInInches);
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line with the largest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest y from</param>
        /// <returns>Returns the Line with the largest y intercept</returns>
        public static Line LineWithLargestYInterceptIn2D(this IList<Line> passedLines)
        {
            return passedLines.LineWithYInterceptIn2DFarthestFrom(Distance.ZeroDistance);
        }

        /// <summary>
        /// Returns the line whose y intercept on the 2D xy-plane is closest to the given distance on the y-axis. If two share the same intercept it returns the
        /// first line in the list
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest y from</param>
        /// <param name="pointToFindTheClosestInterceptTo">The value on the y-axis to find the line whose intercept is closest to</param>
        /// <returns>Returns the Line whose intercept is closest to the specified point on the y-axis</returns>
        public static Line LineWithYInterceptIn2DClosestTo(this IList<Line> passedLines, Distance pointToFindTheClosestInterceptTo)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.YInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.YInterceptIn2D() - pointToFindTheClosestInterceptTo));
            var list = dictionary.OrderBy(pair => Math.Abs(pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// Returns the line whose y intercept on the 2D xy-plane is farthest from the given distance on the y-axis. If two share the same intercept it returns the
        /// first line in the list
        /// </summary>
        /// <param name="passedLines">The list of lines to find the fartherst in y from</param>
        /// <param name="pointToFindTheFarthestInterceptTo">The value on the y-axis to find the line whose intercept is fartherst from</param>
        /// <returns>Returns the Line whose intercept is farthest from the specified point on the y-axis</returns>
        public static Line LineWithYInterceptIn2DFarthestFrom(this IList<Line> passedLines, Distance pointToFindTheFarthestInterceptTo)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.YInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.YInterceptIn2D() - pointToFindTheFarthestInterceptTo));
            var list = dictionary.OrderByDescending(pair => Math.Abs(pair.Value.ValueInInches));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// checks to see whether every line is parallel
        /// </summary>
        /// <param name="passedLines">passed List of Lines</param>
        /// <returns>returns a bool of whether or not they are all parallel</returns>
        public static bool AreAllParallel(this List<ILinear> passedLines)
        {
            for (int i = 0; i < passedLines.Count - 1; i++)
            {
                if (!passedLines[i].IsParallelTo(passedLines[i + 1]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if all of the passed lines are in the same plane, false otherwise
        /// </summary>
        public static bool AreAllCoplanar(this IList<ILinear> passedLineList)
        {
            List<LineSegment> segments = passedLineList.AsSegments(new Distance(1, Inches));

            return segments.AreAllCoplanar();
        }

        public static List<LineSegment> AsSegments(this IList<ILinear> lineList, Distance lengthOfSegments)
        {
            List<LineSegment> segments = new List<LineSegment>();
            foreach(ILinear line in lineList)
            {
                LineSegment lineAsSegment = new LineSegment(line.BasePoint, line.GetPointAlongLine(lengthOfSegments));
                segments.Add(lineAsSegment);
            }
            return segments;
        }
        /// <summary>
        /// Sorts out and returns only the lines in this list that are parallel to the passed Line
        /// </summary>
        /// <param name="passedLines">passed List of Lines</param>
        /// <param name="lineToCheckIfParallelTo">passed List of Lines</param>
        /// <returns>returns a List of Lines of only the Lines that are parallel to the passed line</returns>
        public static List<Line> OnlyLinesParallelTo(this IList<Line> passedLines, ILinear lineToCheckIfParallelTo)
        {
            ////List<Line> passedLineListCasted = new List<Line>(passedLines);
            //List<Line> parallelLines = new List<Line>();

            //foreach(Line currentLine in passedLines)
            //{
            //    if (currentLine.IsParallelTo(lineToCheckIfParallelTo))
            //    {
            //        parallelLines.Add(currentLine);
            //    }
            //}

            //return parallelLines;
            return passedLines.Where(l => l.IsParallelTo(lineToCheckIfParallelTo)).ToList();
        }
    }
}
