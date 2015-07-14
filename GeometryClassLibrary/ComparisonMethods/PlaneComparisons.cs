using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class CompareByNormalAngleWithZ : IComparer<Plane>
    {
        public int Compare(Plane plane1, Plane plane2)
        {
            Vector normal1 = plane1.NormalVector;
            Angle angle1 = normal1.SmallestAngleBetween(Direction.Out.UnitVector(DistanceType.Inch));
            
            Vector normal2 = plane2.NormalVector;
            Angle angle2 = normal2.SmallestAngleBetween(Direction.Out.UnitVector(DistanceType.Inch));

            if (angle1 == angle2)
            {
                return 0;
            }
            if (angle1 < angle2)
            {
                return -1;
            }
            return 1;
        }
    }
}
