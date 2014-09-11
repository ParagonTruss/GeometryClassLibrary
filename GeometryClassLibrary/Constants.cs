
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    internal static class Constants
    {

        public static double AcceptedEqualityDeviationConstant
        {
            get
            {
                try
                {
                    //double.Parse(Resources.ResourceManager.GetString("AcceptedEqualityDeviationConstant"));
                }
                catch (Exception)
                {

                    return 0.000005;
                }
                return 0.000005;

            } // a big hippo was here

        }
    }
}
