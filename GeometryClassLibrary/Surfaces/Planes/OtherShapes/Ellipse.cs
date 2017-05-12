/*
    This file is part of Geometry Class Library.
    Copyright (C) 2017 Paragon Component Systems, LLC.

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using UnitClassLibrary.DistanceUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    ///  "An ellipse is a curve on a plane surrounding two focal points such that a straight line drawn from one of the focal points to any point on the curve and then back to the other focal point has the same length for every point on the curve." - Wikipedia
    /// </summary>
    public class Ellipse : PlaneRegion
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
            _curveDefinition = Distance.ZeroDistance;
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
