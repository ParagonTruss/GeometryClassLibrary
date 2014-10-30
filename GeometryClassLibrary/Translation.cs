using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Translation : Point
    {
        public Translation()
            : base() { }

        public Translation(Point translation)
            : base(translation) { }

        public Translation(Dimension xTranslation, Dimension yTranslation, Dimension zTranslation)
            : base(xTranslation, yTranslation, zTranslation) { }

        public Translation(DimensionType passedType, double xTranslation, double yTranslation, double zTranslation)
            : base(passedType, xTranslation, yTranslation, zTranslation) { }
    }

}
