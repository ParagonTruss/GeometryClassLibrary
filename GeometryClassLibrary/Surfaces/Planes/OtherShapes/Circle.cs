using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnitClassLibrary;
using static UnitClassLibrary.DistanceUnit.Distance;
using System;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.InSquareInchesUnit;

namespace GeometryClassLibrary
{
    public class Circle : PlaneRegion
    {
        #region Properties and Fields

        public Point Center
        {
            get { return ((Arc)this._Edges[0]).CenterPoint; }
        }

        public Distance Radius
        {
            get { return ((Arc)this._Edges[0]).RadiusOfCurvature; }
        }

        public override Area Area
        {
            get
            {
                return (Area)(Math.PI * new Area(new SquareInch(),Radius* Radius));
            }
        }

        public override Point Centroid
        {
            get
            {
                return Center;
            }
        }
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
            var chosen = new List<Vector>() { vector1, vector2, vector3 }.MaxBy(v => v.Magnitude);

            var basePoint = center.Translate(chosen.Direction * radius);
            var arc = new Arc(basePoint, basePoint, new Line(center, normalDirection));

            this._Edges = new List<IEdge>() { arc };
            this.NormalLine = new Line(arc.CenterPoint, arc.NormalDirection);
        }

        #endregion
    }
}