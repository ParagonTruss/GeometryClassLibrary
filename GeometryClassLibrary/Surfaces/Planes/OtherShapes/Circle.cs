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

using System.Collections.Generic;
using MoreLinq;
using UnitClassLibrary;
using static UnitClassLibrary.DistanceUnit.Distance;
using System;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;

namespace GeometryClassLibrary
{
    public class Circle : PlaneRegion
    {
        #region Properties and Fields

        public Point Center => ((Arc)this._Edges[0]).CenterPoint;

        public Distance Radius => ((Arc)this._Edges[0]).RadiusOfCurvature;

        public override Area Area => (Area)(Math.PI * new Area(new SquareInch(),Radius.ValueInInches.ToThe(2)));

        public override Point Centroid => Center;

        #endregion

        #region Constructors



        public Circle(Point center, Distance radius, Direction normalDirection = null)    
        {
            if (normalDirection == null)
            {
                normalDirection = Direction.Out;
            }
            var vector1 = new Vector(Point.MakePointWithInches(1, 0, 0)).CrossProduct(normalDirection * new Distance(1, Inches));
            var vector2 = new Vector(Point.MakePointWithInches(0, 1, 0)).CrossProduct(normalDirection * new Distance(1, Inches));
            var vector3 = new Vector(Point.MakePointWithInches(0, 0, 1)).CrossProduct(normalDirection * new Distance(1, Inches));
            var chosen = new List<Vector>() { vector1, vector2, vector3 }.SafeMaxBy(v => v.Magnitude);

            var basePoint = center.Translate(chosen.Direction * radius);
            var arc = new Arc(basePoint, basePoint, new Line(center, normalDirection));

            this._Edges = new List<IEdge>() { arc };
            this.NormalLine = new Line(arc.CenterPoint, arc.NormalDirection);
        }

        #endregion
    }
}