using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    public class PlaneRegion : Plane, IComparable<PlaneRegion>
    {
        #region Properties and Fields

        protected IEnumerable<IEdge> Edges;
        public virtual Area Area { get { throw new NotImplementedException(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PlaneRegion()
            : base()
        {
            Edges = new List<IEdge>();
        }

        /// <summary>
        /// Creates a new planeregion with the passed in LineSegments
        /// Note: they must all be coplanar
        /// </summary>
        /// <param name="passedLines"></param>
        public PlaneRegion(IList<Line> passedLines)
            :base(passedLines) { }

        /// <summary>
        /// Makes a PlaneRegion Using the given boundaries to define it
        /// Note: Not fully Implemented - does not check if they are all coplanar
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(IEnumerable<IEdge> passedEdges)
            : base()
        {
            Edges = passedEdges;

            if (passedEdges.Count() > 0)
            {
                //we have to check against vectors until we find one that is not parralel with the first line we passed in
                //or else the normal vector will be zero (cross product of parralel lines is 0)
                Vector vector1 = passedEdges.ElementAt(0).Direction.UnitVector(DimensionType.Inch);
                for (int i = 1; i < passedEdges.Count(); i++)
                {
                    this.NormalVector = vector1.CrossProduct(passedEdges.ElementAt(i).Direction.UnitVector(DimensionType.Inch));
                    if (!base.NormalVector.Equals(new Vector()))
                        i = passedEdges.Count();
                }

                base.BasePoint = passedEdges.ElementAt(0).BasePoint;
            }
        }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion planeToCopy)
            : this(planeToCopy.Edges) 
        {
            List<IEdge> copiedEdges = new List<IEdge>();

            foreach (IEdge edge in planeToCopy.Edges)
            {
                copiedEdges.Add(edge.Copy());
            }

            this.Edges = copiedEdges;
        }

        /*public PlaneRegion(List<IEdge> shiftedBoundaries)
        {
                // TODO: Complete member initialization
            Edges = shiftedBoundaries;
        }*/

        #endregion

        #region Overloaded Operators

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
                if (this.Edges.Count() != comparableRegion.Edges.Count())
                {
                    return false;
                }

                //now check each edge in our list
                foreach (IEdge edge in this.Edges)
                {
                    //make sure each edge is represented exactly once
                    int timesUsed = 0;
                    foreach (IEdge edgeOther in comparableRegion.Edges)
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

        public virtual Polygon SmallestRectangleThatCanSurroundThisShape()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the centoid (geometric center point) of the Region
        /// </summary>
        /// <returns>the region's center as a point</returns>
        public virtual Point Centroid()
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
        /// <param name="passedShift">The shift to preform on this PlaneRegion</param>
        /// <returns>Returns a new PlaneRegion that has been shifted</returns>
        public virtual PlaneRegion Shift(Shift passedShift)
        {
            List<IEdge> shiftedBoundaries = new List<IEdge>();

            foreach (var edge in Edges)
            {
                shiftedBoundaries.Add(edge.Shift(passedShift));
            }

            return new PlaneRegion(shiftedBoundaries);
        }

        /// <summary>
        /// Rotates the given Plane Region
        /// </summary>
        /// <param name="passedRotation">The rotation to apply to the planeRegion</param>
        /// <returns>Returns a new PlaneRegion that has been rotated</returns>
        public new virtual PlaneRegion Rotate(Rotation passedRotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// translates the given Plane Region
        /// </summary>
        /// <param name="passedTranslation">The translates to apply to the planeRegion</param>
        /// <returns>Returns a new PlaneRegion that has been translated</returns>
        public virtual PlaneRegion Translate(Point passedTranslation)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
