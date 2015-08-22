using System.Collections.Generic;

namespace GeometryClassLibrary.ExtensionMethods.ListExtensionMethods
{
    public static class VectorListExtensions
    {
        public static List<Vector> Shift(this List<Vector> vectorList, Shift toApply)
        {
            List<Vector> toReturn = new List<Vector>();
            foreach(Vector vector in vectorList)
            {
                toReturn.Add(vector.Shift(toApply));
            }
            return toReturn;
        }
    }
}
