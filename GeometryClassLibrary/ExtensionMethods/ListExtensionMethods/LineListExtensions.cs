using System;
using System.Collections.Generic;
using UnitClassLibrary;
using System.Linq;

namespace GeometryClassLibrary
{
    public static class LineListExtensions
    {
        /// <summary>
        /// Returns the line with the smallest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the smallest x from</param>
        /// <returns>Returns the Line with the smallest x intercept</returns>
        public static Line LineWithSmallestXInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line smallestXLine = null;
            Distance smallestX = null;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    if (smallestX == null || passedLines[i].XInterceptIn2D < smallestX)
                    {
                        smallestXLine = passedLines[i];
                        smallestX = passedLines[i].XInterceptIn2D;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return smallestXLine;
        }

        /// <summary>
        /// Returns the line with the largest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest x from</param>
        /// <returns>Returns the Line with the largest x intercept</returns>
        public static Line LineWithLargestXInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line largestXLine = null;// passedLines[0];
            Distance largestX = null;// passedLines[0].XInterceptIn2D;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    if (largestX == null || passedLines[i].XInterceptIn2D > largestX)
                    {
                        largestXLine = passedLines[i];
                        largestX = passedLines[i].XInterceptIn2D;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return largestXLine;
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
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line closetXLine = null;// passedLines[0];
            Distance closestX = null;// passedLines[0].XInterceptIn2D;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    Distance distanceAway = passedLines[i].XInterceptIn2D - pointToFindTheClosestInterceptTo;
                    distanceAway = distanceAway.AbsoluteValue();
                    if (closestX == null || distanceAway < closestX)
                    {
                        closetXLine = passedLines[i];
                        closestX = distanceAway;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return closetXLine;
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
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line farthestXLine = null;// passedLines[0];
            Distance farthestX = null;// passedLines[0].XInterceptIn2D;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    Distance distanceAway = passedLines[i].XInterceptIn2D - pointToFindTheFarthestInterceptTo;
                    distanceAway = distanceAway.AbsoluteValue();
                    if (farthestX == null || distanceAway > farthestX)
                    {
                        farthestXLine = passedLines[i];
                        farthestX = distanceAway;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return farthestXLine;
        }

        /// <summary>
        /// Returns the line with the smallest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the smallest y from</param>
        /// <returns>Returns the Line with the smallest y intercept</returns>
        public static Line LineWithSmallestYInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line smallestYLine = null;
            Distance smallestY = null;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    if (smallestY == null || passedLines[i].YInterceptIn2D < smallestY)
                    {
                        smallestYLine = passedLines[i];
                        smallestY = passedLines[i].YInterceptIn2D;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return smallestYLine;
        }

        /// <summary>
        /// Returns the line with the largest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines">The list of lines to find the largest y from</param>
        /// <returns>Returns the Line with the largest y intercept</returns>
        public static Line LineWithLargestYInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line largestYLine = null;
            Distance largestY = null;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    if (largestY == null || passedLines[i].YInterceptIn2D > largestY)
                    {
                        largestYLine = passedLines[i];
                        largestY = passedLines[i].YInterceptIn2D;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return largestYLine;
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
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line closetYLine = null;
            Distance closestY = null;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    Distance distanceAway = passedLines[i].YInterceptIn2D - pointToFindTheClosestInterceptTo;
                    distanceAway = distanceAway.AbsoluteValue();
                    if (closestY == null || distanceAway < closestY)
                    {
                        closetYLine = passedLines[i];
                        closestY = distanceAway;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return closetYLine;
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
            if (passedLines.Count < 1)
            {
                return null;
            }

            //make them null so that we can handle if the first element in the list never intersects
            Line farYLine = null;
            Distance farY = null;

            for (int i = 0; i < passedLines.Count; i++)
            {
                try
                {
                    Distance distanceAway = passedLines[i].YInterceptIn2D - pointToFindTheFarthestInterceptTo;
                    distanceAway = distanceAway.AbsoluteValue();
                    if (farY == null || distanceAway > farY)
                    {
                        farYLine = passedLines[i];
                        farY = distanceAway;
                    }
                }
                //if the line doesnt intersect it throws an exception but we dont have to worry about it
                catch (Exception) { }
            }

            return farYLine;
        }

        /*public static Line LineWithSmallesZIntercept(this IList<Line> passedLines)
        {
            throw new NotImplementedException();
        }

        public static Line LineWithLargestZIntercept(this IList<Line> passedLines)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// checks to see whether every line is parallel
        /// </summary>
        /// <param name="passedLines">passed List of Lines</param>
        /// <returns>returns a bool of whether or not they are all parallel</returns>
        public static bool AreAllParallel(this IList<Line> passedLines)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLines);

            for (int i = 0; i < passedLineListCasted.Count - 1; i++)
            {
                if (!passedLineListCasted[i].IsParallelTo(passedLineListCasted[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all of the passed lines are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine">passed lines</param>
        /// <returns>returns a bool of whether or not they are all coplanar</returns>
        public static bool AreAllCoplanar(this IList<Line> passedLineList)
        {
            List<LineSegment> segments = passedLineList.AsSegments(Distance.Inch);

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
