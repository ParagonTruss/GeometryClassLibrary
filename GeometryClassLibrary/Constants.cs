
using System;
using GeometryClassLibrary.Properties;

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
                    return double.Parse(Resources.ResourceManager.GetString("AcceptedEqualityDeviationDouble"));
                }
                catch (Exception)
                {

                    return 0.0001;
                }

            }

        }
    }
}
