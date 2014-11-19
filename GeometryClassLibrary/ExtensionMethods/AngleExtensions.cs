using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public static class AngleExtensions
    {
        public static bool AngleIsEqualOrOpposite(this Angle passedAngle1, Angle passedAngle2)
        {
            if (passedAngle1 == passedAngle2 || passedAngle1 + new Angle(AngleType.Degree, 180) == passedAngle2)
            {
                return true;
            }

            return false;
        }
    }
}
