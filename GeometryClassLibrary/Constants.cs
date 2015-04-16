
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    public static class Constants
    {

        public static double AcceptedEqualityDeviationDouble
        {
            get
            {
                try
                {
                    return double.Parse(GeometryClassLibrary.Properties.Resources.ResourceManager.GetString("AcceptedEqualityDeviationDouble"));
                }
                catch (Exception)
                {

                    return 0.0001;
                }

            }

        }
    }
}
