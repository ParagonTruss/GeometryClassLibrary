using System;
using System.Collections.Generic;
using UnitClassLibrary;
using System.Linq;
using UnitClassLibrary.DistanceUnit;
using MoreLinq;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibrary
{
    public static class LineListExtensions
    {
        /// <summary>
        /// Returns the line with the smallest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        public static Line LineWithLowestXInterceptIn2D(this IList<Line> passedLines)
        {
            var withoutNullIntercepts = passedLines.Where(line => line.YInterceptIn2D() != null);

            var dictionary = withoutNullIntercepts.Select(line => new KeyValuePair<Line, Distance>(line, line.XInterceptIn2D()));
            var list = dictionary.OrderBy(pair =>(pair.Value.InInches.Value));
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
            var list = dictionary.OrderByDescending(pair => (pair.Value.InInches.Value));
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
            var list = dictionary.OrderBy(pair => Math.Abs(pair.Value.InInches.Value));
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
            var list = dictionary.OrderByDescending(pair => Math.Abs(pair.Value.InInches.Value));
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
            var list = dictionary.OrderBy(pair => pair.Value.InInches.Value);
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
            var list = dictionary.OrderBy(pair => Math.Abs(pair.Value.InInches.Value));
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
            var list = dictionary.OrderByDescending(pair => Math.Abs(pair.Value.InInches.Value));
            var result = list.FirstOrDefault().Key;

            return result;
        }

        /// <summary>
        /// checks to see whether every line is parallel
        /// </summary>
        /// <param name="passedLines">passed List of Lines</param>
        /// <returns>returns a bool of whether or not they are all parallel</returns>
        public static bool AreAllParallel(this List<Line> passedLines)
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
        public static bool AreAllCoplanar(this IList<Line> passedLineList)
        {
            List<LineSegment> segments = passedLineList.AsSegments(new Distance(1, Inches));

            return segments.AreAllCoplanar();
        }

        public static List<LineSegment> AsSegments(this IList<Line> lineList, Distance lengthOfSegments)
        {
            List<LineSegment> segments = new List<LineSegment>();
            foreach(Line line in lineList)
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
        public static List<Line> OnlyLinesParallelTo(this IList<Line> passedLines, Line lineToCheckIfParallelTo)
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
