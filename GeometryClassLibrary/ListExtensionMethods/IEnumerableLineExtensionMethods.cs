using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class IEnumerableLineExtensionMethods
    {

        public static bool AreAllParallel(this IEnumerable<Line> passedLines)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLines);

            bool returnBool = true;
            for (int i = 0; i < passedLineListCasted.Count - 2; i++)
            {
                if (!passedLineListCasted[i].IsParallelTo(passedLineListCasted[i + 1]))
                {
                    returnBool = false;
                }
            }

            return returnBool;
        }

        /// <summary>
        /// Returns true if all of the passed lines are in the same plane, false otherwise
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public static bool AreAllCoplanar(this IEnumerable<Line> passedLineList)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLineList);

            for (int i = 0; i < passedLineList.Count() - 1; i++)
            {
                if (!passedLineListCasted[i].IsCoplanarWith(passedLineListCasted[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
