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
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        public Point Origin;

        public CoordinateSystem()
            : base()
        {
            this.Origin = new Point();
        }
        public CoordinateSystem(Point passedOrigin, Direction passedDirection)
            : base(passedDirection)
        {
            Origin = passedOrigin;
        }
    }
}
