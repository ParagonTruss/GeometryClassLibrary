using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryClassLibrary
{
    public class PlaneRegion : Plane
    {
        private List<LineSegment> passedBoundaries;

        public PlaneRegion()
        {
            // TODO: Complete member initialization
            this.passedBoundaries = passedBoundaries;
        }

        public PlaneRegion(List<LineSegment> passedBoundaries)
        {
            // TODO: Complete member initialization
            this.passedBoundaries = passedBoundaries;
        }

        internal bool Contains(Point passedPoint)
        {
            throw new NotImplementedException();
        }
    }
}
