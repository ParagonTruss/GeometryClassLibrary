using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Translation : Vector
    {
        public Translation(Dimension xTranslation, Dimension yTranslation, Dimension zTranslation)
            : base(new Point(xTranslation, yTranslation, zTranslation))
        { }

        public Translation(Point translations)
            : base(translations)
        { }
    }
}
