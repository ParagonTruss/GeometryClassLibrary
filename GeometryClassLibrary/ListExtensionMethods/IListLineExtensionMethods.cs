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
        /// <param name="passedLines"></param>
        /// <returns></returns>
        public static Line LineWithSmallestXInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            Line smallestXLine = passedLines[0];
            Dimension smallestX = passedLines[0].XInterceptIn2D;

            for (int i = 1; i < passedLines.Count; i++)
            {
                if (passedLines[i].XInterceptIn2D != null)
                {
                    if (smallestX == null || passedLines[i].XInterceptIn2D < smallestX)
                    {
                        smallestXLine = passedLines[i];
                        smallestX = passedLines[i].XInterceptIn2D;
                    }
                }
            }

            return smallestXLine;
        }

        /// <summary>
        /// Returns the line with the largest x intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines"></param>
        /// <returns></returns>
        public static Line LineWithLargestXInterceptIn2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            Line largestXLine = passedLines[0];
            Dimension largestX = passedLines[0].XInterceptIn2D;

            for (int i = 1; i < passedLines.Count; i++)
            {
                if (passedLines[i].XInterceptIn2D != null && (largestX == null || passedLines[i].XInterceptIn2D > largestX))
                {
                    largestXLine = passedLines[i];
                    largestX = passedLines[i].XInterceptIn2D;
                }
            }

            return largestXLine;
        }

        /// <summary>
        /// Returns the line with the smallest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines"></param>
        /// <returns></returns>
        public static Line LineWithSmallestYIntercept2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            Line smallestYLine = passedLines[0];
            Dimension smallestY = passedLines[0].YInterceptIn2D;

            for (int i = 1; i < passedLines.Count; i++)
            {
                if (passedLines[i].YInterceptIn2D != null && (smallestY == null || passedLines[i].YInterceptIn2D < smallestY))
                {
                    smallestYLine = passedLines[i];
                    smallestY = passedLines[i].YInterceptIn2D;
                }
            }

            return smallestYLine;
        }

        /// <summary>
        /// Returns the line with the largest y intercept on the 2D xy-plane. If two share the same intercept it returns the
        /// first line in the list with that segment
        /// </summary>
        /// <param name="passedLines"></param>
        /// <returns></returns>
        public static Line LineWithLargestYIntercept2D(this IList<Line> passedLines)
        {
            if (passedLines.Count < 1)
            {
                return null;
            }

            Line largestYLine = passedLines[0];
            Dimension largestY = passedLines[0].YInterceptIn2D;

            for (int i = 1; i < passedLines.Count; i++)
            {
                if (passedLines[i].YInterceptIn2D != null && (largestY == null || passedLines[i].YInterceptIn2D > largestY))
                {
                    largestYLine = passedLines[i];
                    largestY = passedLines[i].YInterceptIn2D;
                }
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
        /// <returns></returns>
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
        /// <returns></returns>
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
