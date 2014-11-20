using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    [Serializable]
    public class IrregularPolygon : Polygon
    {
        /*private List<LineSegment> _planeBoundaries;
        public override List<LineSegment> PlaneBoundaries
        {
            get { return FindOutsideBoundaries(); }
            set { this.PlaneBoundaries = value; }
        }*/

        private List<LineSegment> FindOutsideBoundaries()
        {
            throw new NotImplementedException();
        }
    }
}
