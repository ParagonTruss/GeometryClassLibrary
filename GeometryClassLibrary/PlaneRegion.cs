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
    public class PlaneRegion<T> : Plane, IComparable<PlaneRegion<T>> where T : IEdge
    {
        #region Fields and Properties

        public virtual List<T> PlaneBoundaries { get; set; }
        public virtual Area Area { get {throw new NotImplementedException();} }

        #endregion

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PlaneRegion()
        {
            PlaneBoundaries = new List<T>();
        }

        /// <summary>
        /// Makes a PlaneRegion Using the given boundaries to define it
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(List<T> passedBoundaries)
            : base()
        {
            PlaneBoundaries = passedBoundaries;
        }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion<T> planeToCopy)
            //note: we do not need to call List<LineSegment>(newplaneToCopy.PlaneBoundaries) because it does this in the base case for 
            //constructing a plane fron a List<LineSegment>
            : this(planeToCopy.PlaneBoundaries) { }

        /*public PlaneRegion(List<IEdge> shiftedBoundaries)
        {
                // TODO: Complete member initialization
            PlaneBoundaries = shiftedBoundaries;
        }*/

        #endregion

        #region Overloaded Operators

        public static bool operator ==(PlaneRegion<T> region1, PlaneRegion<T> region2)
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

        public static bool operator !=(PlaneRegion<T> region1, PlaneRegion<T> region2)
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
            PlaneRegion<T> comparableRegion = null;

            //try to cast the object to a Plane Region, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableRegion = (PlaneRegion<T>)obj;
                bool areEqual = true;
                foreach (T edge in comparableRegion.PlaneBoundaries)
                {
                    if (!PlaneBoundaries.Contains(edge))
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
        public int CompareTo(PlaneRegion<T> other)
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

        public virtual PlaneRegion<T> Shift<T>(Shift passedShift) where T : IEdge
        {

            List<T> shiftedBoundaries = new List<T>();

            foreach (var edge in PlaneBoundaries)
            {
               shiftedBoundaries.Add((T)edge.Shift(passedShift));
            }

            return new PlaneRegion<T>(shiftedBoundaries);
        }

        public virtual PlaneRegion<T> Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            throw new NotImplementedException();
        }

        public virtual PlaneRegion<T> Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
