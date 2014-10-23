using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [Serializable]
    //For an explaination of why we use generics here see: http://stackoverflow.com/questions/3551012/override-a-property-with-a-derived-type-and-same-name-c-sharp
    public class PlaneRegion : Plane, IComparable<PlaneRegion>
    {
        #region Fields and Properties

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
        /// Makes a PlaneRegion Using the given boundaries to define it
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

        public PlaneRegion(IList<Line> passedLines)
            :base(passedLines) { }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion planeToCopy)
            //note: we do not need to call List<LineSegment>(newplaneToCopy.Edges) because it does this in the base case for 
            //constructing a plane fron a List<LineSegment>
            : this(planeToCopy.Edges) { }

        /*public PlaneRegion(List<IEdge> shiftedBoundaries)
        {
                // TODO: Complete member initialization
            Edges = shiftedBoundaries;
        }*/

        #endregion

        #region Overloaded Operators

        public static bool operator ==(PlaneRegion region1, PlaneRegion region2)
        {
            if ((object)region1 == null || (object)region2 == null)
            {
                if ((object)region1 == null && (object)region2 == null)
                {
                    return true;
                }
                return false;
            }
            return region1.Equals(region2);
        }

        public static bool operator !=(PlaneRegion region1, PlaneRegion region2)
        {
            if ((object)region1 == null || (object)region2 == null)
            {
                if ((object)region1 == null && (object)region2 == null)
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
            PlaneRegion comparableRegion = null;

            //try to cast the object to a Plane Region, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableRegion = (PlaneRegion)obj;
                bool areEqual = true;
                foreach (IEdge edge in comparableRegion.Edges)
                {
                    if (!Edges.Contains(edge))
                    {
                        areEqual = false;
                    }

                }

                return areEqual;
            }
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

        public virtual Solid Extrude(Dimension dimension)
        {
            throw new NotImplementedException();
        }

        public virtual PlaneRegion Shift(Shift passedShift)
        {

            List<IEdge> shiftedBoundaries = new List<IEdge>();

            foreach (var edge in Edges)
            {
                shiftedBoundaries.Add(edge.Shift(passedShift));
            }

            return new PlaneRegion(shiftedBoundaries);
        }

        public virtual PlaneRegion Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            throw new NotImplementedException();
        }

        public virtual PlaneRegion Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
