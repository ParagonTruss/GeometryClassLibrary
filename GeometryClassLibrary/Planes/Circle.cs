using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Circle : Ellipse
    {
        #region Properties and Fields

        public Point Center
        {
            get { return this._foci[0]; }
        }

        public Distance Radius
        {
            get { return this._curveDefinition / 2; }
        }

        #endregion

        #region Constructors

        public Circle()
            : base()
        { }

        public Circle(Point center, Distance radius)
            : base(new List<Point>() { center, center }, radius * 2)
        { }

        #endregion
    }
}