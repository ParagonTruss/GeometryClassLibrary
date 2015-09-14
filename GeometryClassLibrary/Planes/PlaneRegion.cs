using System;
using System.Collections.Generic;
using System.Linq;
using UnitClassLibrary;
using static UnitClassLibrary.Distance;

namespace GeometryClassLibrary
{
    public class PlaneRegion : Plane, IComparable<PlaneRegion>
    {
        #region Properties and Fields

        protected List<IEdge> _Edges = new List<IEdge>();
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
        public PlaneRegion(IEnumerable<IEdge> passedEdges)
        {
            _Edges.AddRange(passedEdges);
            this.BasePoint = this._Edges[0].EndPoint;
            this.NormalVector = _getUnitNormalVector();

        }

        // Returns a normalVector of the planeregion.
        // or the zero vector, if the planereg has no sides.
        protected Vector _getUnitNormalVector()
        {
            if (this.Edges.Count() > 2)
            {
                Vector vector1 = new Vector(_Edges.OrderBy(s => s.BasePoint.X).ThenBy(s => s.BasePoint.Y).ThenBy(s => s.BasePoint.Z).First());

                Vector vector2;
                Vector normal = null;
                for (int i = 0; i < _Edges.Count; i++)
                {
                    vector2 = new Vector(vector1.BasePoint, Vertices[i]);
                    normal = vector1.CrossProduct(vector2);
                    if (normal.Magnitude != Distance.Zero)
                    {
                        break;
                    }
                }

                return new Vector(this.BasePoint, normal.Direction, Inch);
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
               return new Vector(nonSegment.BasePoint, normalDirection, Inch);
            }
        }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion planeToCopy)
            : base(planeToCopy) 
        {
            this._Edges.AddRange(planeToCopy._Edges);
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

        /// <summary>
        /// Should return the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other PlaneRegion
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(PlaneRegion other)
        {
            throw new NotImplementedException();
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
            return new PlaneRegion(this._Edges.Select(e => e.Shift(shift)));
        }

     

        /// <summary>
        /// Rotates the plane with the given rotation
        /// </summary>
        public new PlaneRegion Rotate(Rotation rotation)
        {
            return new PlaneRegion(this._Edges.Select(e => e.Rotate(rotation)));
        }

        /// <summary>
        /// translates the given Plane Region
        /// </summary>
        public PlaneRegion Translate(Point translation)
        {
            return new PlaneRegion(this._Edges.Select(e => e.Translate((Translation)translation)));
        }

        #endregion 
    }
}
