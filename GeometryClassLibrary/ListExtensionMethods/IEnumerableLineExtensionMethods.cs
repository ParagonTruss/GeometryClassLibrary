using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IEnumerableLineExtensionMethods
    {
        /// <summary>
        /// checks to see whether every line is parallel
        /// </summary>
        /// <param name="passedLines">passed List of Lines</param>
        /// <returns></returns>
        public static bool AreAllParallel(this List<Line> passedLines)
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
        public static bool AreAllCoplanar(this List<Line> passedLineList)
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
