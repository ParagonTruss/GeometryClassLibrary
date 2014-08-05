using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("UNITS = Millimeters, Base Point = {BasePoint.X.Millimeters}, {BasePoint.Y.Millimeters}, {BasePoint.Z.Millimeters}, End Point = {EndPoint.X.Millimeters}, {EndPoint.Y.Millimeters}, {EndPoint.Z.Millimeters}, Length = {Length.Millimeters},  Direction Vector = {XComponentOfDirection.Millimeters}, {YComponentOfDirection.Millimeters}, {ZComponentOfDirection.Millimeters}")]
    public class LineSegment : Line, IComparable<LineSegment>
    {
        #region Properties

        public Point EndPoint
        {
            get 
            {
                Dimension xOfEndPoint = DimensionGenerator.MakeDimensionWithMillimeters(Math.Round(BasePoint.X.Millimeters + (_length.Millimeters * XComponentOfDirection.Millimeters / MagnitudeOfDirectionVector.Millimeters), 6));
                Dimension yOfEndPoint = DimensionGenerator.MakeDimensionWithMillimeters(Math.Round(BasePoint.Y.Millimeters + (_length.Millimeters * YComponentOfDirection.Millimeters / MagnitudeOfDirectionVector.Millimeters), 6));
                Dimension zOfEndPoint = DimensionGenerator.MakeDimensionWithMillimeters(Math.Round(BasePoint.Z.Millimeters + (_length.Millimeters * ZComponentOfDirection.Millimeters / MagnitudeOfDirectionVector.Millimeters), 6));

                return new Point(xOfEndPoint,yOfEndPoint,zOfEndPoint);
            }
            set
            {
                _length = this.BasePoint.DistanceTo(value);
            }
        }

        public Dimension Length
        {
            get { return _length; }
            set { _length = value; }
            
        }
        private Dimension _length;

        public Point MidPoint
        {
            get 
            {
                //Find midpoint for each component
                Dimension xMid = new Dimension(DimensionType.Millimeter, (base.BasePoint.X.Millimeters + EndPoint.X.Millimeters) / 2);
                Dimension yMid = new Dimension(DimensionType.Millimeter, (base.BasePoint.Y.Millimeters + EndPoint.Y.Millimeters) / 2);
                Dimension zMid = new Dimension(DimensionType.Millimeter, (base.BasePoint.Z.Millimeters + EndPoint.Z.Millimeters) / 2);

                return new Point(xMid, yMid, zMid);
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Line Segment that extends from the origin to the given end point
        /// </summary>
        /// <param name="passedEndPoint"></param>
        public LineSegment(Point passedEndPoint)
            : base(passedEndPoint)
        {
            Length = passedEndPoint.DistanceTo(PointGenerator.MakePointWithInches(0,0,0));
        }


        /// <summary>
        /// Constructs a line segment from a given start point to a given end point
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedEndPoint"></param>
        public LineSegment(Point passedBasePoint, Point passedEndPoint)
            : base(passedBasePoint, passedEndPoint)
        {
            Length = passedBasePoint.DistanceTo(passedEndPoint);
        }

        /// <summary>
        /// Constructs a line segment of length 1 that extends from the given start point in the given direction
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedDirection"></param>
        public LineSegment(Point passedBasePoint, Vector passedDirection)
            : base(passedBasePoint, passedDirection)
        {
            Length = new Dimension(DimensionType.Millimeter, 1);
        }

        public LineSegment(Point passedBasePoint, Vector passedDirection, Dimension passedLength)
            : base(passedBasePoint, passedDirection)
        {
            Length = passedLength;
        }

        #endregion

        #region Overloaded Operators
        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(LineSegment segment1, LineSegment segment2)
        {
            return segment1.Equals(segment2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(LineSegment segment1, LineSegment segment2)
        {
            return !segment1.Equals(segment2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            LineSegment comparableSegment = null;

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableSegment = (LineSegment)obj;

                // if the two points' x and y are equal, returns true
                return (comparableSegment.BasePoint.Equals(this.BasePoint) && comparableSegment.EndPoint.Equals(this.EndPoint))
                    || (comparableSegment.BasePoint.Equals(this.EndPoint) && comparableSegment.EndPoint.Equals(this.BasePoint));
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sometimes we want to check if the linesegment would intersect with line if it were extended towards the line
        /// </summary>
        /// <param name="passedLineSegment"></param>
        /// <returns></returns>
        public Point HypotheticalIntersection(Line passedLine)
        {
            return new Line(this).Intersection(passedLine);
        }

        public override Point Intersection(Line passedLine)
        {
            Point intersect = this.HypotheticalIntersection(passedLine);

            if (!ReferenceEquals(intersect, null) && intersect.IsOnLineSegment(this))
                return intersect;
            else
                return null;
        }

        /// <summary>
        /// Checks to see if a line segment contains another line segment.  Useful for checking if members touch
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool Contains(LineSegment passedLine)
        {
            if (this.Length >= passedLine.Length)
            {
                if (this.IsCoplanarWith(passedLine) && this.IsParallelTo(passedLine))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DoesSharesABaseOrEndPointWith(LineSegment passedLineSegment)
        {
            return (this.BasePoint == passedLineSegment.EndPoint
                || this.BasePoint == passedLineSegment.BasePoint
                || this.EndPoint == passedLineSegment.EndPoint
                || this.EndPoint == passedLineSegment.BasePoint);
        }
		
        public LineSegment Rotate(Line passedRotaionAxis, Angle passedAngle)
        {
            Point basePoint = this.BasePoint.Rotate3D(passedRotaionAxis, passedAngle);
            Point endPoint = this.EndPoint.Rotate3D(passedRotaionAxis, passedAngle);

            return new LineSegment(basePoint, endPoint);
        }

        public LineSegment Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            Point newBasePoint = this.BasePoint.Translate(passedDirectionVector, passedDisplacement);
            Point newEndPoint = this.EndPoint.Translate(passedDirectionVector, passedDisplacement);

            return new LineSegment(newBasePoint, newEndPoint);
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other segment
        /// NOTE: BASED SOLELY ON LENGTH.  MAY WANT TO CHANGE LATER
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(LineSegment other)
        {
            if (this.Length.Equals(other.Length))
                return 0;
            else
                return this.Length.CompareTo(other.Length);
        }

        #endregion

        
    }
}
