using System;
using System.Collections.Generic;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    ///  "An ellipse is a curve on a plane surrounding two focal points such that a straight line drawn from one of the focal points to any point on the curve and then back to the other focal point has the same length for every point on the curve." - Wikipedia
    /// </summary>
    public class Ellipse : NonPolygon
    {
        #region Properties and Fields

        protected IList<Point> _foci; // The two focal points
        public IList<Point> Foci
        {
            get { return _foci; }
        }

        protected Distance _curveDefinition; // The distance that defines the perimeter of the ellipse
        public Distance CurveDefinition
        {
            get { return _curveDefinition; }
        }

        #endregion

        #region Constructors

        public Ellipse()
        {
            _foci = new List<Point>();
            _curveDefinition = new Distance();
        }

        public Ellipse(IList<Point> foci, Distance curveDefinition)
        {
            if (foci.Count != 2)
            {
                throw new ArgumentException("An ellipse must have exactly two foci.");
            }

            this._foci = foci;
            this._curveDefinition = curveDefinition;
        }

        #endregion
    }
}
