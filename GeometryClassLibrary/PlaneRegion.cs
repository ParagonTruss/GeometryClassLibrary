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
            this.passedBoundaries = new List<LineSegment>();
        }

        public PlaneRegion(List<LineSegment> passedBoundaries)
            : base(passedBoundaries)
        {
            this.passedBoundaries = passedBoundaries;
        }

        internal bool Contains(Point passedPoint)
        {
            throw new NotImplementedException();
        }
    }
}
