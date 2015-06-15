using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class PointGenerator
    {
        public static Point Origin
        {
            get
            {
                return new Point();
            }
        }

        public static Point MakePointWithInches(double inputValue1, double inputValue2, double inputValue3 = 0)
        {
            Distance dim1 = new Distance(DistanceType.Inch, inputValue1);
            Distance dim2 = new Distance(DistanceType.Inch, inputValue2);
            Distance dim3 = new Distance(DistanceType.Inch, inputValue3);

            return new Point(dim1, dim2, dim3);
        }

        public static Point MakePointWithMillimeters(double inputValue1, double inputValue2, double inputValue3 = 0)
        {
            Distance dim1 = new Distance(DistanceType.Millimeter, inputValue1);
            Distance dim2 = new Distance(DistanceType.Millimeter, inputValue2);
            Distance dim3 = new Distance(DistanceType.Millimeter, inputValue3);

            return new Point(dim1, dim2, dim3);
        }

        public static Point MakePointWithInches(string inputString1, string inputString2)
        {
            double inputValue1 = double.Parse(inputString1);
            double inputValue2 = double.Parse(inputString2);

            Distance dim1 = new Distance(DistanceType.Inch, inputValue1);
            Distance dim2 = new Distance(DistanceType.Inch, inputValue2);
            return new Point(dim1, dim2);
        }

        public static Point[] Make2DPointArrayWithInches(double[] values)
        {
            Point[] toReturn = null;

            if (values.Length % 2 == 0)
            {
                toReturn = new Point[values.Length / 2];

                for (int i = 0; i < values.Length; i += 2)
                {
                    toReturn[i / 2] = MakePointWithInches(values[i], values[i + 1]);
                }
            }

            return toReturn;
        }


    }
}
