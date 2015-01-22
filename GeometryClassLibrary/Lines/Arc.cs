using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved as around a circle.
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
                double lengthInInches = (CentralAngle.Degrees * ArcRadius.Inches) / (2 * Math.PI);
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
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(ArcRadius.Inches, 2) * CentralAngle.Radians);
            }
        }

        /// <summary>
        /// The area of the shape limited by the arc and a straight line between the two end points.
        /// </summary>
        public Area ArcSegmentArea
        {
            get
            {
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(ArcRadius.Inches, 2) * (CentralAngle.Radians - Math.Sin(CentralAngle.Radians)));
            }
        }

        /// <summary>
        ///  an angle whose apex (vertex) is the center O of a circle and whose legs (sides) 
        ///  are radii intersecting the circle in two distinct points A and B 
        ///  thereby subtending an arc between those two points whose angle is (by definition) equal to that of the central angle itself.
        ///  It is also known as the arc segment's angular distance.
        /// http://en.wikipedia.org/wiki/Central_angle
        /// </summary>
        public Angle CentralAngle
        {
            get
            {
                //find the angle between the lines formed form the endpoints and the center
                Line startPointToCenter = new Line(_basePoint, ArcCenterPoint);
                Line endPointToCenter = new Line(_endPoint, ArcCenterPoint);

                return startPointToCenter.AngleBetweenIntersectingLine(endPointToCenter);
            }
        }

        /// <summary>
        /// The center point of the sphere that would be formed by the arc
        /// </summary>
        public Point ArcCenterPoint
        {
            get
            {
                //we find two lines to define the plane: the initial direction and the straight line direction (we use
                //LineSegment because we need it as a line segment later on and it saves us from creating another object)
                Line tangentLine = new Line(_initialDirection, _basePoint);
                LineSegment straightLineSegment = new LineSegment(_basePoint, _endPoint);

                //we need to find the plane it is in first
                Plane containingPlane = new Plane(tangentLine, straightLineSegment);

                //now we can find the direction to the center point by finding the perpindicular of the tangent line
                Line toCenterPointFromTangent = tangentLine.MakePerpindicularLineInGivenPlane(containingPlane);

                //now we can find the center point of the arc and the direction to the center of the arc from there based on the lineSegment to the endpoint
                Direction toCenterFromMidPoint = straightLineSegment.MakePerpindicularLineInGivenPlane(containingPlane).Direction;

                Line arcCenterLine = new Line(toCenterFromMidPoint, straightLineSegment.MidPoint);

                //now find where centerLine and the toCenterFromTagnent line intersect to get the center point
                return toCenterPointFromTangent.Intersection(arcCenterLine);
            }
        }

        /// <summary>
        /// The radius of the would be circle formed by the arc
        /// </summary>
        public Distance ArcRadius
        {
            get
            {
                return ArcCenterPoint.DistanceTo(_basePoint);
            }
        }


        /// <summary>
        /// One of the points where the arc arises from
        /// </summary>
        private Point _basePoint;
        public virtual Point BasePoint
        {
            get { return _basePoint; }
        }

        private Point _endPoint;
        /// <summary>
        /// One of the points where the arc ends
        /// </summary>
        public virtual Point EndPoint
        {
            get { return _endPoint; }
        }

        /// <summary>
        /// The direction from the arc's start point to end point as if a straight line were drawn between then
        /// </summary>
        public virtual Direction StraightLineDirection
        {
            get
            {
                return new Direction(this._basePoint, this.EndPoint);
            }
        }

        /// <summary>
        /// The direction from the arc's start point to end point as if a straight line were drawn between them.
        /// This is defined and used by IEdge
        /// </summary>
        public Direction Direction
        {
            get { return _initialDirection; }
        }

        private Direction _initialDirection;
        /// <summary>
        /// The initial direction the line of the arc goes from the start point (or the tangent of arc at the start point)
        /// </summary>
        public virtual Direction InitialDirection
        {
            get { return _initialDirection; }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a empty arc
        /// </summary>
        public Arc()
        {
            _basePoint = new Point();
            _endPoint = new Point();
            _initialDirection = new Direction();
        }

        /// <summary>
        /// Creates an arc defined by the given arguements
        /// </summary>
        /// <param name="passedBasePoint">The base/start point of the arc</param>
        /// <param name="passedEndPoint">The end point of the arc</param>
        /// <param name="passedIntialDirection">The direction the arc initially travels from the start point (aka the tangent of the arc at the start point)</param>
        public Arc(Point passedBasePoint, Point passedEndPoint, Direction passedIntialDirection)
        {
            _basePoint = passedBasePoint;
            _endPoint = passedEndPoint;
            _initialDirection = passedIntialDirection;
        }

        /// <summary>
        /// Creates an arc with the given specifications
        /// </summary>
        /// <param name="passedBasePoint">The base or start point of the arc</param>
        /// <param name="passedCentralAngle">The central angle of the arc (or angular distance)</param>
        /// <param name="passedCenterPoint">The center point of the circle that would be formed by the arc if it continued</param>
        public Arc(Point passedBasePoint, Point passedCenterPoint, Angle passedCentralAngle, Direction passedInitialDirection)
        {
            _basePoint = passedBasePoint;
            _initialDirection = passedInitialDirection;

            //find the end point by creating the plane we are in first
            Line tangentLine = new Line(_initialDirection, _basePoint);
            LineSegment toStartPointFromCenter = new LineSegment(passedCenterPoint, passedBasePoint);

            Plane containingPlane = new Plane(tangentLine, toStartPointFromCenter);

            //now we can use our central angle to find a line that will point to our endpoint
            //by rotating the segment to the basePoint around an axis in the normal direction of our normal plane
            Line axisToRotateAround = new Line(containingPlane.NormalVector.Direction, _basePoint);
            LineSegment toEndPointFromCenter = toStartPointFromCenter.Rotate(new Rotation(axisToRotateAround, passedCentralAngle));

            _endPoint = toEndPointFromCenter.EndPoint;
        }

        /// <summary>
        /// Creates an arc given the following definitions and defaults to the XY-plane if none is given
        /// Note: The normal vector of the plane is Important!
        /// </summary>
        /// <param name="passedBasePoint">The base point of the arc</param>
        /// <param name="passedCenterPoint">The center point of the circle that would be formed by the arc if it continued</param>
        /// <param name="passedCentralAngle">The angle between the start point and end point of the arc at the center point</param>
        /// <param name="planeToBeContainedIn">The plane the arc is contained in</param>
        public Arc(Point passedBasePoint, Point passedCenterPoint, Angle passedCentralAngle, Plane planeToBeContainedIn = null)
        {
            if (planeToBeContainedIn == null)
            {
                planeToBeContainedIn = new Plane(Plane.XY);
            }

            //make sure the points on on the plane
            if (!planeToBeContainedIn.Contains(passedBasePoint))
            {
                throw new ArgumentException("The base point is not on the passed plane");
            }
            if (!planeToBeContainedIn.Contains(passedCenterPoint))
            {
                throw new ArgumentException("The center point is not on the passed plane");
            }

            _basePoint = passedBasePoint;

            //create our line segment from the center to the start point
            LineSegment toStartPointFromCenter = new LineSegment(passedCenterPoint, passedBasePoint);

            //now find the tangent line in the plane were in to find the initial direction
            Line tangentLine = toStartPointFromCenter.MakePerpindicularLineInGivenPlane(planeToBeContainedIn);
            _initialDirection = tangentLine.Direction;

            //now we can use our central angle to find a line that will point to our endpoint
            //by rotating the segment to the basePoint around an axis in the normal direction of our normal plane
            Line axisToRotateAround = new Line(planeToBeContainedIn.NormalVector.Direction, _basePoint);
            LineSegment toEndPointFromCenter = toStartPointFromCenter.Rotate(new Rotation(axisToRotateAround, passedCentralAngle));

            _endPoint = toEndPointFromCenter.EndPoint;
        }

        /// <summary>
        /// Creates an arc with the specifications. If planeToBeContainedIn is left out it defaults to the XY-Plane
        /// Note: The normal vector of the plane is Important!
        /// </summary>
        /// <param name="passedBasePoint">The arc's base point</param>
        /// <param name="passedEndPoint">The arc's end point</param>
        /// <param name="passedRadius">The radius of the circle formed by the arc if the arc continued</param>
        /// <param name="planeToBeContainedIn">The plane the arc is contained in or the XY-plane if it is left out</param>
        public Arc(Point passedBasePoint, Point passedEndPoint, Distance passedRadius, Plane planeToBeContainedIn = null)
        {
            if (planeToBeContainedIn == null)
            {
                planeToBeContainedIn = new Plane(Plane.XY);
            }

            //make sure the points on on the plane
            if (!planeToBeContainedIn.Contains(passedBasePoint))
            {
                throw new ArgumentException("The base point is not on the passed plane");
            }
            if (!planeToBeContainedIn.Contains(passedEndPoint))
            {
                throw new ArgumentException("The end point is not on the passed plane");
            }

            _basePoint = passedBasePoint;
            _endPoint = passedEndPoint;

            //we can find teh center point using the law of Cosines: c^2 = a^2 + b^2 - 2abCos(C)
            //which in the case of isoceles triangles can be simplified to cos(C) = b/2r when finding the double angles
            //which is what we want in theis case

            //first find the straight line between the two points to find the b side
            LineSegment straightSegment = new LineSegment(passedBasePoint, passedEndPoint);

            Angle angleToCenterFromStraightLine = new Angle(AngleType.Radian, Math.Acos(straightSegment.Length / (passedRadius * 2)));

            //now we can rotate the straightSegment to find the direction to the center
            Line toCenter = straightSegment.Rotate(new Rotation(new Line(planeToBeContainedIn.NormalVector.Direction, passedBasePoint), angleToCenterFromStraightLine));

            //now find the perpindicular to that line segment in the plane to find the initial direction
            _initialDirection = toCenter.MakePerpindicularLineInGivenPlane(planeToBeContainedIn).Direction;
        }

        /// <summary>
        /// Creates a copy of this Arc
        /// </summary>
        /// <param name="toCopy">The Arc to copy</param>
        public Arc(Arc toCopy)
        {
            _basePoint = new Point(toCopy.BasePoint);
            _endPoint = new Point(toCopy.EndPoint);
            _initialDirection = new Direction(toCopy.InitialDirection);
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
        /// <returns>A new Arc that has been shifted with the passed shift</returns>
        public Arc Shift(Shift passedShift)
        {
            Point newBasePoint = BasePoint.Shift(passedShift);
            Point newEndPoint = EndPoint.Shift(passedShift);

            //cheat a bit and make the direction into a line and then shift it
            Line directionLine = new Line(InitialDirection, BasePoint).Shift(passedShift);

            return new Arc(newBasePoint, newEndPoint, directionLine.Direction);
        }

        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Returns a copy of this Arc
        /// </summary>
        /// <returns></returns>
        IEdge IEdge.Copy()
        {
            return new Arc(this);
        }

        public Arc Rotate(Rotation passedRotation)
        {
            throw new NotImplementedException();
        }

        IEdge IEdge.Rotate(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        /// <summary>
        /// Translates the arc with the given translation
        /// </summary>
        /// <param name="translation">The translation to apply to the Arc</param>
        /// <returns></returns>
        public Arc Translate(Point translation)
        {
            Point newBasePoint = BasePoint.Translate(translation);
            Point newEndPoint = EndPoint.Translate(translation);

            return new Arc(newBasePoint, newEndPoint, this.InitialDirection);
        }

        IEdge IEdge.Translate(Point translation)
        {
            return this.Translate(translation);
        }

        #endregion

    }
}
