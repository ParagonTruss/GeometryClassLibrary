using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryClassLibrary
{
    [Serializable]
    public class IrregularPolygon : Polygon
    {
        public override List<LineSegment> PlaneBoundaries
        {
            get { return FindOutsideBoundaries(); }
            set { _planeBoundaries = value; }
        }

        private List<LineSegment> FindOutsideBoundaries()
        {
            throw new NotImplementedException();
        }
    }
}
