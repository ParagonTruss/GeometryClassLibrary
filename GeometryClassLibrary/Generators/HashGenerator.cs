using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public static class HashGenerator
    {
        public static int GetHashCode(List<object> objList)
        {
            unchecked
            {
                int hash = (int)2166136261;
                foreach (object obj in objList)
                {
                    hash = hash * 16777619 + obj.GetHashCode();
                }

                return hash;
            }
        }
    }
}
