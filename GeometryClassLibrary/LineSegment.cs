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
                if(MagnitudeOfDirectionVector == new Dimension())
                {
                    return new Point();
                }

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


        public LineSegment Shift(Shift passedShift)
        {
            return new LineSegment(BasePoint.Shift(passedShift), EndPoint.Shift(passedShift));
        }

        /// <summary>
        /// Projects this LineSegment onto the given Line, which is the projected length of this LineSegment in the direction of the Line projected onto
        /// </summary>
        /// <param name="projectOnto">the Line on which to project the LineSegment</param>
        /// <returns></returns>
        public LineSegment ProjectOntoLine(Line projectOnto)
        {
            //we can do this by making the points into vectors with the basepoint of the line as the basepoint and the endpoint of the 
            //point we want to project then using the dot product to find its relative vector and then position it 
            //relative to the clipping basepoint. We have to make the basepoint to the divisionLine's so that the vectors
            //intersect, allowing us to project the one onto the other easily
            Vector basePointVector = new Vector(projectOnto.BasePoint, BasePoint);

            //now we can project the basepointvector onto the division line's unit vector to find the scalar length of the projection
            //  note: we use the unit vector because it has a length of 1 and will not affect the scaling of the projected vector
            Dimension basePointProjectedLength = basePointVector * projectOnto.DirectionVector.ConvertToUnitVector();

            //now make the projected vector using divisionLine's basePoint (our reference), the divisionLines unitVector (since we
            //  are projecting onto it we know that it is the direction of the resulting vector), and then the length we
            //  found for the projection
            LineSegment basePointProjection = new LineSegment(projectOnto.BasePoint, projectOnto.DirectionVector.ConvertToUnitVector(), basePointProjectedLength);

            //now the basepointProjection starts at the divisionLine's endpoint and ends at the projected point on itself
            Point newBasePoint = basePointProjection.EndPoint;

            //now we just need to do it all again to find the newEndpoint's projection
            Vector endPointVector = new Vector(projectOnto.BasePoint, this.EndPoint);
            Dimension endPointProjectedLength = endPointVector * projectOnto.DirectionVector.ConvertToUnitVector();
            LineSegment endPointProjection = new LineSegment(projectOnto.BasePoint, projectOnto.DirectionVector.ConvertToUnitVector(), endPointProjectedLength);
            Point newEndPoint = endPointProjection.EndPoint;

            //now we have the two projected points and we can make a line segment which represents the porjected line
            return new LineSegment(newBasePoint, newEndPoint);
        }
    }
}
