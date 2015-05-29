﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public abstract class PlaneRegion : Plane, IComparable<PlaneRegion>
    {
        #region Properties and Fields

        protected List<IEdge> Edges = new List<IEdge>();
        public virtual Area Area { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Returns the centroid (geometric center point) of the Region
        /// </summary>
        /// <returns>the region's center as a point</returns>
        public virtual Point Centroid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public PlaneRegion(Direction passedNormalDirection = null, Point passedBasePoint = null)
            : base(passedNormalDirection, passedBasePoint)
        {
            this.Edges = new List<IEdge>();
        }

        ///// <summary>
        ///// Creates a new planeregion with the passed in LineSegments
        ///// Note: they must all be coplanar
        ///// </summary>
        ///// <param name="passedLines"></param>
        //public PlaneRegion(IList<Line> passedLines)
        //    :base(passedLines) { }

        /// <summary>
        /// Makes a PlaneRegion Using the given boundaries to define it
        /// Note: Not fully Implemented - does not check if they are all coplanar
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(IEnumerable<IEdge> passedEdges)
            : base()
        {
            foreach (var edge in passedEdges)
            {
                this.Edges.Add(edge);
            }

            if (passedEdges.Count() > 0)
            {
                //we have to check against vectors until we find one that is not parralel with the first line we passed in
                //or else the normal vector will be zero (cross product of parralel lines is 0)
                Vector vector1 = passedEdges.ElementAt(0).Direction.UnitVector(DistanceType.Inch);
                for (int i = 1; i < passedEdges.Count(); i++)
                {
                    this.NormalVector = vector1.CrossProduct(passedEdges.ElementAt(i).Direction.UnitVector(DistanceType.Inch));
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

       
        public virtual Solid Extrude(Vector directionVector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shifts this PlaneRegion by generically shifting each edge in the PlaneRegion
        /// </summary>
        /// <param name="passedShift">The shift to preform on this PlaneRegion</param>
        /// <returns>Returns a new PlaneRegion that has been shifted</returns>
        public abstract PlaneRegion ShiftAsPlaneRegion(Shift passedShift);

        /// <summary>
        /// Rotates the given Plane Region
        /// </summary>
        /// <param name="passedRotation">The rotation to apply to the planeRegion</param>
        /// <returns>Returns a new PlaneRegion that has been rotated</returns>
        public virtual PlaneRegion Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            return this.RotateAsPlaneRegion(new Rotation(passedAxisLine, passedRotationAngle));
        }

        /// <summary>
        /// Rotates the plane with the given rotation
        /// </summary>
        /// <param name="passedRotation">The rotation object that is to be applied to the plane</param>
        /// <returns>A new plane that has been rotated</returns>
        public abstract PlaneRegion RotateAsPlaneRegion(Rotation passedRotation);

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
