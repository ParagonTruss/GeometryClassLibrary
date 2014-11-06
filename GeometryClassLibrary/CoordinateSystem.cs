using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class CoordinateSystem : Direction
    {
        public static CoordinateSystem CurrentSystem = new CoordinateSystem();

        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        public Point Origin;

        //public bool IsCurrent = false;

        public CoordinateSystem()
            : base(new Angle(), new Angle())
        {
            this.Origin = new Point();
        }

        public CoordinateSystem(Point passedOrigin)
            : base()
        {
            //make it so its 0,0 and not the default 0,90
            this.Theta = new Angle();
            Origin = passedOrigin;
        }

        public CoordinateSystem(Point passedOrigin, Direction passedDirection)
            : base(passedDirection)
        {
            Origin = passedOrigin;
        }

        public CoordinateSystem(CoordinateSystem toCopy)
            : base(toCopy)
        {
            Origin = new Point(toCopy.Origin);
        }
        /*
        public CoordinateSystem Negate()
        {
            CoordinateSystem toReturn = new CoordinateSystem(this);
            toReturn.IsCurrent = !IsCurrent;
            return toReturn;
        }*/
    }
}
