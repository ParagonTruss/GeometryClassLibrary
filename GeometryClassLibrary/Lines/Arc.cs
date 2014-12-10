using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved (not straight)
    /// </summary>
    [DebuggerDisplay("BasePoint = {BasePoint.X.Inches}, {BasePoint.Y.Inches}, {BasePoint.Z.Inches}, EndPoint = {EndPoint.X.Inches}, {EndPoint.Y.Inches}, {EndPoint.Z.Inches}, Direction: Azumuth = {Direction.Phi.Degrees}, Inclination{Direction.Theta.Degrees}")]
    public class Arc : IEdge, IComparable<Arc>
    {
        #region Properties and Fields

        /// <summary>
        /// The length of an arc of a circle with radius r and subtending an angle theta, with the circle center — i.e., the central angle 
        /// </summary>
        public Distance ArcLength
        {
            get 
            { 
                double lengthInInches = (_centralAngle.Degrees * _arcRadius.Inches) / (2 * Math.PI);
                return new Distance(DistanceType.Inch, lengthInInches);
            }
        }

        /// <summary>
        /// The area between an arc and the center of a would be circle.
        /// </summary>
        public Area ArcArea
        {
            get 
            {
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(_arcRadius.Inches, 2) * _centralAngle.Radians);
            }
        }

        /// <summary>
        /// The area of the shape limited by the arc and a straight line between the two end points.
        /// </summary>
        public Area ArcSegmentArea
        {
            get 
            {
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(_arcRadius.Inches, 2) * (_centralAngle.Radians - Math.Sin(_centralAngle.Radians ))); 
            }
        }


        Angle _centralAngle;

        /// <summary>
        ///  an angle whose apex (vertex) is the center O of a circle and whose legs (sides) 
        ///  are radii intersecting the circle in two distinct points A and B 
        ///  thereby subtending an arc between those two points whose angle is (by definition) equal to that of the central angle itself.
        ///  It is also known as the arc segment's angular distance.
        /// http://en.wikipedia.org/wiki/Central_angle
        /// </summary>
        public Angle CentralAngle
        {
            get { return _centralAngle; }
        }



        Distance _arcRadius;

        /// <summary>
        /// The radius of the would be circle formed by the arc
        /// </summary>
        public Distance ArcRadius
        {
            get { return _arcRadius; }
        }


        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _basePoint;
        public virtual Point BasePoint
        {
            get { return _basePoint; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// One of the points where the arc ends
        /// http://math.stackexchange.com/questions/275201/how-to-find-an-end-point-of-an-arc-given-another-end-point-radius-and-arc-dire
        /// </summary>
        public virtual Point EndPoint
        {
            get 
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// The direction that the arc travels in from the base point
        /// </summary>
        public virtual Direction Direction 
        { 
            get
            {
                return new Direction(this._basePoint, this.EndPoint);
            }
        }



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a empty arc
        /// </summary>
        public Arc()
        {
            _basePoint = new Point();
            _centralAngle = new Angle();
            _arcRadius = new Distance();
        }

        public Arc(Point passedBasePoint, Angle passedAngle, Distance passedRadius)
        {
            _basePoint = passedBasePoint;
            _centralAngle = passedAngle;
            _arcRadius = passedRadius;
        }

        public Arc(Point passedBasePoint, Point passedEndPoint, Distance passedRadius)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a copy of this Arc
        /// </summary>
        /// <param name="toCopy">The Arc to copy</param>
        public Arc(Arc toCopy)
        {
            _basePoint = new Point(toCopy.BasePoint);
            _centralAngle = new Angle(toCopy.CentralAngle);
            _arcRadius = new Distance();
        }

        #endregion

        //These are currently only based on the points and not anything else about it!
        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Arc arc1, Arc arc2)
        {
            if ((object)arc1 == null)
            {
                if ((object)arc1 == null)
                {
                    return true;
                }
                return false;
            }
            return arc1.Equals(arc2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Arc arc1, Arc arc2)
        {
            if ((object)arc1 == null)
            {
                if ((object)arc2 == null)
                {
                    return false;
                }
                return true;
            }
            return !arc1.Equals(arc2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                Arc comparableArc = (Arc)obj;

                // if the two points' x and y are equal
                bool arcAreEqual = comparableArc._basePoint.Equals(this._basePoint) && comparableArc.EndPoint.Equals(this.EndPoint);
                bool arcAreRevese = comparableArc._basePoint.Equals(this.EndPoint) && comparableArc.EndPoint.Equals(this._basePoint);

                bool areAnglesEqual = comparableArc.CentralAngle == this.CentralAngle;
                bool areRadiusEqual = comparableArc.ArcRadius == this.ArcRadius;

                return (arcAreEqual || arcAreRevese) && areAnglesEqual && areRadiusEqual;
            }
            //if it wasnt an arc than its obviously not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other segment
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Arc other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the given shift on the Arc and returns a new Arc that has been shifted
        /// </summary>
        /// <param name="passedShift">The shift to perform on the Arc</param>
        /// <returns></returns>
        public Arc Shift(Shift passedShift)
        {
            throw new NotImplementedException();
        }

        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Returns a copy of this Arc
        /// </summary>
        /// <returns></returns>
        public IEdge Copy()
        {
            return new Arc(this);
        }


        #endregion


        public IEdge Rotate(UnitClassLibrary.Angle passedRotationAngle)
        {
            throw new NotImplementedException();
        }


        public IEdge RotateAsIEdge(Rotation passedRotation)
        {
            throw new NotImplementedException();
        }
    }
}
