/*
    This file is part of Geometry Class Library.
    Copyright (C) 2016 Paragon Component Systems, LLC.

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
using System.Linq;
using UnitClassLibrary;
using UnitClassLibrary.AreaUnit;
using static UnitClassLibrary.DistanceUnit.Distance;

namespace GeometryClassLibrary
{
    public partial class PlaneRegion : Plane, ISurface
    {
        public static implicit operator PlaneRegion(Polygon p)
        {
            return new PlaneRegion(p.LineSegments.Select(s => (IEdge)s).ToList());
        }

        #region Properties and Fields

        protected List<IEdge> _Edges;
        public virtual List<IEdge> Edges
        {
            get
            {
                return _Edges;
            }
            set
            {
                _Edges = value;
            }
        }

        /// <summary>
        /// The vertices of this plane region
        /// </summary>
        public List<Point> Vertices
        {
            get
            {
                if (_vertices == null)
                {
                    _vertices = _Edges.GetAllPoints();
                }
                return _vertices;
            }
        }
        private List<Point> _vertices;

        public virtual Area Area { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Returns the centroid (geometric center point) of the Region
        /// </summary>
        public virtual Point Centroid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual bool IsConvex
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Null constructor
        /// </summary>
        protected PlaneRegion() { }

        /// <summary>
        /// 
        /// </summary>
        public PlaneRegion(List<IEdge> passedEdges)
        {
            this._Edges = passedEdges.FixEdgeOrientation();
            this.NormalLine = _getNormalLine();

        }

        // Returns a normalVector of the planeregion.
        // or the zero vector, if the planereg has no sides.
        protected Line _getNormalLine()
        {
            if (this.Edges.Count() > 2)
            {
                Vector vector1 = new Vector(_Edges.OrderBy(s => s.BasePoint.X).ThenBy(s => s.BasePoint.Y).ThenBy(s => s.BasePoint.Z).First());
               // Vector vector2 = new Vector(_Edges.First(s => s.BasePoint == vector1.EndPoint));
                Vector vector2 = null;
                for (int i = 0; i < Edges.Count; i++)
                {
                    if (Edges[i].BasePoint == vector1.EndPoint)
                    {
                        vector2=new Vector(Edges[i]);
                    }
                }

                var normal = vector1.CrossProduct(vector2);
                
                return new Line(_Edges[0].BasePoint, normal.Direction);
            }
            else
            {
                IEdge nonSegment;
                if (!(Edges[0] is LineSegment))
                {
                    nonSegment = Edges[0];
                }
                else if (!(Edges[1] is LineSegment))
                {
                    nonSegment = Edges[0];
                }
                else throw new Exception("Not Allowed.");
                var otherVertex = this.Vertices.First(v => v != nonSegment.BasePoint);
                var normalDirection = nonSegment.InitialDirection.CrossProduct(new LineSegment(nonSegment.BasePoint, otherVertex).Direction);
               return new Line(nonSegment.BasePoint, normalDirection);
            }
        }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion planeToCopy)
            : base(planeToCopy) 
        {
            this._Edges = planeToCopy._Edges;
        }

        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(PlaneRegion region1, PlaneRegion region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return true;
                }
                return false;
            }
            return region1.Equals(region2);
        }

        public static bool operator !=(PlaneRegion region1, PlaneRegion region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return false;
                }
                return true;
            }
            return !region1.Equals(region2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure obj is not null
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Plane Region, if it fails then we know the user passed in the wrong type of object
            try
            {
                PlaneRegion comparableRegion = (PlaneRegion)obj;

                //make sure there are the smae number of edges
                if (this._Edges.Count() != comparableRegion._Edges.Count())
                {
                    return false;
                }

                //now check each edge in our list
                foreach (IEdge edge in this._Edges)
                {
                    //make sure each edge is represented exactly once
                    int timesUsed = 0;
                    foreach (IEdge edgeOther in comparableRegion._Edges)
                    {
                        if (edge.Equals(edgeOther))
                        {
                            timesUsed++;
                        }
                    }

                    //make sure each is used exactly once
                    if (timesUsed != 1)
                    {
                        return false;
                    }
                }

                //if we didnt find any that werent in the edges than they are equal
                return true;
            }
            //if it was not a planeregion than it is not equal
            catch
            {
                return false;
            }
        }

       

        #endregion

       #region Methods

        public virtual Polygon SmallestEnclosingRectangle()
        {
            throw new NotImplementedException();
        }

       
        public virtual Solid Extrude(Vector directionVector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shifts this PlaneRegion by generically shifting each edge in the PlaneRegion
        /// </summary>
        public new PlaneRegion Shift(Shift shift)
        {
            return new PlaneRegion(this._Edges.Select(e => e.Shift(shift)).ToList());
        }

     

        /// <summary>
        /// Rotates the plane with the given rotation
        /// </summary>
        public new PlaneRegion Rotate(Rotation rotation)
        {
            return new PlaneRegion(this._Edges.Select(e => e.Rotate(rotation)).ToList());
        }

        /// <summary>
        /// translates the given Plane Region
        /// </summary>
        //public PlaneRegion Translate(Point translation)
        //{
        //    return new PlaneRegion(this._Edges.Select(e => e.Translate((Translation)translation)));
        //}

        #endregion 
    }
}
