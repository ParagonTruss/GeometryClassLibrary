using System;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// An arc is a finite line (having a start and end) that is curved as around a circle.
    /// </summary>
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
                //length = r * theta), when theta is in radians
                return RadiusOfCurvature * CentralAngle.Radians;
            }
        }

        /// <summary>
        /// The area between an arc and the center of a would be circle.
        /// </summary>
        public Area SectorArea
        {
            get
            {
                //arcArea = r^2/2 * (theta), where theta is in radians
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(RadiusOfCurvature.Inches, 2) * CentralAngle.Radians);
            }
        }

        /// <summary>
        /// The area of the shape limited by the arc and a straight line between the two end points.
        /// </summary>
        public Area SegmentArea
        {
            get
            {
                //arcSegmentArea = r^2 / 2 * (theta - sin(theta))
                return new Area(AreaType.InchesSquared, .5 * Math.Pow(RadiusOfCurvature.Inches, 2) * (CentralAngle.Radians - Math.Sin(CentralAngle.Radians)));
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
                return new Angle(new Direction(CenterPoint, BasePoint).SignedAngleBetween(new Direction(CenterPoint, EndPoint), NormalDirection));
            }
        }

        /// <summary>
        /// The radius of the would be circle formed by the arc
        /// </summary>
        public Distance RadiusOfCurvature { get { return CenterPoint.DistanceTo(BasePoint); } }

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
        public Direction Direction { get { return InitialDirection; } }

        /// <summary>
        /// The direction tangent to the basepoint.
        /// </summary>
        public Direction InitialDirection
        {
            get { return new Direction(BasePoint, CenterPoint).CrossProduct(NormalDirection); }
        }
        
        /// <summary>
        /// Where the arc begins.
        /// </summary>
        public Point BasePoint { get { return _basePoint; } }
        private Point _basePoint;

        /// <summary>
        /// Where the arc ends.
        /// </summary>
        public Point EndPoint { get { return _endPoint; } }
        private Point _endPoint;

        /// <summary>
        /// The center point of the circle that the arc fits along.
        /// </summary>
        public Point CenterPoint { get { return _centerPoint; } }
        // The centerPoint could be calculated from other data, in some cases.
        // However in the case of a full closed arc, one needs to specify the size, since the basepoint is the endpoint.
        private Point _centerPoint;

        public Direction NormalDirection { get { return _normalDirection; } }
        private Direction _normalDirection;

        public bool IsClosed { get { return BasePoint == EndPoint; } }
        public bool IsAcute { get { return this.CentralAngle < 180 * Angle.Degree; } }
        #endregion

        #region Constructors

        private Arc() { }

        public Arc(Point basePoint, Point endPoint, Direction initialDirection)
        {
            this._basePoint = basePoint;
            this._endPoint = endPoint;
            if (this.IsClosed)
            {
                throw new Exception("Not enough information given to determine curvature.");
            }
            var segmentBetweenPoints = new LineSegment(basePoint,endPoint);
            this._normalDirection = initialDirection.CrossProduct(segmentBetweenPoints.Direction);

            var line1 = new Line(BasePoint, NormalDirection.CrossProduct(initialDirection));
            var line2 = new Line(segmentBetweenPoints.MidPoint, NormalDirection.CrossProduct(segmentBetweenPoints.Direction));
            this._centerPoint = line1.IntersectWithLine(line2);
        }

        public Arc(Point basePoint, Point endPoint, Line normalLine)
        {
            if (basePoint != endPoint && !new Direction(basePoint, endPoint).IsPerpendicularTo(normalLine.Direction))
            {
                throw new Exception();
            }
            var projected = basePoint.ProjectOntoLine(normalLine);
            if (projected == basePoint)
            {
                throw new Exception();
            }
            this._basePoint = basePoint;
            this._endPoint = endPoint;
            this._centerPoint = projected;
            this._normalDirection = normalLine.Direction;            
        }

        private Arc(Point basePoint, Point endPoint, Point centerPoint, Direction normalDirection)
        {
            this._basePoint = basePoint;
            this._endPoint = endPoint;
            this._centerPoint = centerPoint;
            this._normalDirection = normalDirection;
        }

        /// <summary>
        /// Creates a copy of this Arc
        /// </summary>
        /// <param name="toCopy">The Arc to copy</param>
        public Arc(Arc toCopy)
        {
            _basePoint = toCopy.BasePoint;
            _endPoint = toCopy.EndPoint;
            _centerPoint = toCopy.CenterPoint;
            _normalDirection = toCopy.NormalDirection;
        }

        #endregion

        //These are currently only based on the points and not anything else about it!
        #region Overloaded Operators


        public override int GetHashCode()
        {
            return BasePoint.GetHashCode() ^ EndPoint.GetHashCode() ^ CenterPoint.GetHashCode() ^ NormalDirection.GetHashCode();
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
            if (obj == null || !(obj is Arc))
            {
                return false;
            }
            Arc arc = (Arc)obj;
            if (this.CenterPoint != arc.CenterPoint)
            {
                return false;
            }
            if (this.IsClosed && arc.IsClosed)
            {
                return this.NormalDirection == arc.NormalDirection ||
                       this.NormalDirection == arc.NormalDirection.Reverse();
            }
            return (this.BasePoint == arc.BasePoint &&
                    this.EndPoint == arc.EndPoint &&
                    this.NormalDirection == arc.NormalDirection) ||
                   (this.BasePoint == arc.EndPoint &&
                    this.EndPoint == arc.BasePoint &&
                    this.NormalDirection == arc.NormalDirection.Reverse());
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
        public Arc Shift(Shift passedShift)
        {
            Point newBasePoint = BasePoint.Shift(passedShift);
            Point newEndPoint = EndPoint.Shift(passedShift);

            // make the direction into a line and then shift it
            Line directionLine = new Line(CenterPoint, NormalDirection).Shift(passedShift);

            return new Arc(newBasePoint, newEndPoint, directionLine.BasePoint, directionLine.Direction);
        }

        /// <summary>
        /// Performs the given shift on this arc as an IEdge and returns it as an IEdge
        /// </summary>
        /// <param name="passedShift">The shift to apply to this arc</param>
        /// <returns>A new Arc as an IEdge that has been shifted</returns>
        IEdge IEdge.Shift(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Returns a copy of this Arc
        /// </summary>
        /// <returns>A new Arc object that is the same as this one</returns>
        IEdge IEdge.Copy()
        {
            return new Arc(this);
        }

        /// <summary>
        /// Perfomrs the given rotation on the Arc a returns a new object that has been rotated
        /// </summary>
        /// <param name="passedRotation">The Rotation to rotate this Arc with</param>
        /// <returns>A new Arc that has been rotated</returns>
        public Arc Rotate(Rotation passedRotation)
        {
            Point newBasePoint = BasePoint.Rotate3D(passedRotation);
            Point newEndPoint = EndPoint.Rotate3D(passedRotation);

            //cheat a bit and make the direction into a line and then shift it
            Line directionLine = new Line(CenterPoint,NormalDirection).Rotate(passedRotation);

            return new Arc(newBasePoint, newEndPoint, directionLine.BasePoint, directionLine.Direction);
        }

        /// <summary>
        /// Perfomrs the given rotation on the Arc as an IEdge and returns a new object that has been rotated
        /// </summary>
        /// <param name="passedRotation">The Rotation to rotate this Arc with</param>
        /// <returns>A new Arc as an IEdge that has been rotated</returns>
        IEdge IEdge.Rotate(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        /// <summary>
        /// Translates the arc with the given translation
        /// </summary>
        /// <param name="translation">The translation to apply to the Arc</param>
        /// <returns>A new Arc object that has been translated</returns>
        public Arc Translate(Translation translation)
        {
            Point newBasePoint = BasePoint.Translate(translation);
            Point newEndPoint = EndPoint.Translate(translation);
            return new Arc(newBasePoint, newEndPoint, this.InitialDirection);
        }

        /// <summary>
        /// Translates the arc as an IEdge with the given translation
        /// </summary>
        IEdge IEdge.Translate(Point point)
        {
            return this.Translate(point);
        }

        public Arc Reverse()
        {
            return new Arc(EndPoint, BasePoint, CenterPoint, NormalDirection.Reverse());
        }

        IEdge IEdge.Reverse()
        {
            return new Arc(EndPoint, BasePoint, CenterPoint, NormalDirection.Reverse());
        }
        #endregion

    }
}
