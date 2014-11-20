using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class IListLineExtensionMethods
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
            List<Line> passedLineListCasted = new List<Line>(passedLineList);

            for (int i = 0; i < passedLineList.Count(); i++)
            {
                for (int j = 0; j < passedLineList.Count(); j++)
                {
                    if (!passedLineListCasted[i].IsCoplanarWith(passedLineListCasted[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
