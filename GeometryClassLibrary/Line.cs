using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Represents an infinite Line
    /// </summary>
    [DebuggerDisplay("UNITS = Inches, BasePoint = {BasePoint.X.Inches}, {BasePoint.Y.Inches} , {BasePoint.Z.Inches}, Vector = {DirectionVector.XComponentOfDirection.Inches}, {DirectionVector.YComponentOfDirection.Inches}, {DirectionVector.ZComponentOfDirection.Inches}")]
    [Serializable]
    public class Line
    {

        public readonly static Line XAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(1, 0, 0));
        public readonly static Line YAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(0, 1, 0));
        public readonly static Line ZAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(0, 0, 1));

        #region Properties
        public Point BasePoint
        {
            get { return _basePoint; }
            set { this._basePoint = value; }
        }
        private Point _basePoint; //this is any point that is on the line

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private Direction _direction;

        public double Slope
        {
            get { throw new NotImplementedException(); }
        }

        //public double XIntercept
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public double YIntercept
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public double ZIntercept
        //{
        //    get { throw new NotImplementedException(); }
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a line using with a direction in the same direction as the given vector
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="direction"></param>
        public Line(Point passedBasePoint, Vector passedDirectionVector)
        {
            _basePoint = passedBasePoint;
            Direction = new Direction(passedDirectionVector);
        }

        /// <summary>
        /// Creates a line with the given direction and point
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="direction"></param>
        public Line(Point passedBasePoint, Direction passedDirection)
        {
            _basePoint = passedBasePoint;
            Direction = passedDirection;
        }

        /// <summary>
        /// Creates a line through the origin and a passed dimension point
        /// </summary>
        /// <param name="passedDirectionReferencePoint"></param>
        public Line(Point passedDirectionReferencePoint)
        {
            _basePoint = PointGenerator.MakePointWithMillimeters(0, 0, 0);
            _direction = new Direction(passedDirectionReferencePoint);
        }

        /// <summary>
        /// Constructs a line through any 2 dimension points
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedOtherPoint"></param>
        public Line(Point passedBasePoint, Point passedOtherPoint)
        {
            //A "zero" line is allowed so that it is possible to construct a zero vector  
            _basePoint = passedBasePoint;
            _direction = new Direction(new Vector(passedBasePoint, passedOtherPoint));

            //else
            //{
            //    var exception = new DivideByZeroException("The two points that were used to define a line are identical");
            //    throw exception;
            //    ErrorHandlerLibrary.ExceptionHandler.ProcessException(exception);
            //}
        }

        /// <summary>
        /// Constructs a line that goes through passed Point and is parallel to the passed Line
        /// </summary>
        /// <param name="passedPoint"></param>
        /// <param name="passedParallelLine"></param>
        public Line(Point passedPoint, Line passedParallelLine)
        {
            _basePoint = new Point(passedPoint);
            this.Direction = new Direction(passedParallelLine.Direction);
        }

        public Line(LineSegment lineSegment1) : this(lineSegment1.BasePoint, lineSegment1.EndPoint) { }

        #endregion

        public Line()
        {
            throw new System.NotImplementedException();
        }

        #region Methods

        public Plane PlaneThroughLineInDirectionOf(Enums.Axis passedAxis)
        {
            Line extrustionLine;

            switch (passedAxis)
            {
                case Enums.Axis.X:
                    extrustionLine = new Line(this.BasePoint,
                        PointGenerator.MakePointWithInches(
                            this.BasePoint.X.Inches - 1,
                            this.BasePoint.Y.Inches,
                            this.BasePoint.Z.Inches));
                    break;
                case Enums.Axis.Y:
                    extrustionLine = new Line(this.BasePoint,
                        PointGenerator.MakePointWithInches(
                            this.BasePoint.X.Inches,
                            this.BasePoint.Y.Inches - 1,
                            this.BasePoint.Z.Inches));
                    break;
                case Enums.Axis.Z:
                    extrustionLine = new Line(this.BasePoint,
                        PointGenerator.MakePointWithInches(
                        this.BasePoint.X.Inches,
                        this.BasePoint.Y.Inches,
                        this.BasePoint.Z.Inches - 1));
                    break;
                default:
                    throw new ArgumentException("You passed in an unknown Axis Enum");
                    break;


            }
            return new Plane(this, extrustionLine);
        }


        public Angle AngleBetweenIntersectingLine(Line passedIntersectingLine)
        {
            if (!DoesIntersect(passedIntersectingLine))
                throw new Exception("No intercept?");

            Dimension dotProduct = DirectionVector * passedIntersectingLine.DirectionVector;
            double productOfMagnitudes = DirectionVector.Magnitude.Millimeters * passedIntersectingLine.DirectionVector.Magnitude.Millimeters;

            double angleBetweenLines = Math.Acos(dotProduct.Millimeters / productOfMagnitudes);

            Angle returnAngle = new Angle(AngleType.Radian, angleBetweenLines);

            if (returnAngle.Degrees > 90)
            {
                return new Angle(AngleType.Radian, Math.PI - angleBetweenLines);
            }
            else
            {
                return returnAngle;
            }

        }

        /// <summary>
        /// Returns a point on the line based on the multiplier entered
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public Point GetPointOnLine(double multiplier)
        {
            Dimension newX = _basePoint.X + _xComponentOfDirection * multiplier;
            Dimension newY = _basePoint.Y + _yComponentOfDirection * multiplier;
            Dimension newZ = _basePoint.Z + _zComponentOfDirection * multiplier;

            //Make sure point is on the line

            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Returns true if the passed line is parallel to (same direction as) this line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsParallelTo(Line passedLine)
        {
            return passedLine.DirectionVector.PointInSameDirection(DirectionVector) ||
                passedLine.DirectionVector.PointInOppositeDirections(DirectionVector);
        }

        /// <summary>
        /// Returns the point at which a line intersects the passed line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual Point Intersection(Line passedLine)
        {
            if (!this.IsCoplanarWith(passedLine))
            {
                //The lines do not intersect
                return null;
            }

            if (this.Equals(passedLine))
            {
                return null;
            }

            //Following a formula from (http://mathworld.wolfram.com/Line-LineIntersection.html)

            Vector directionVectorA = new Vector(this.BasePoint, this.DirectionVector);
            Vector directionVectorB = new Vector(passedLine.BasePoint, passedLine.DirectionVector);
            //Vector directionVectorA = this.DirectionVector;
            //Vector directionVectorB = passedLine.DirectionVector;
            Vector basePointDiffVectorC = new Vector(this.BasePoint, passedLine.BasePoint);

            Vector crossProductCB = basePointDiffVectorC.CrossProduct(directionVectorB);
            Vector crossProductAB = directionVectorA.CrossProduct(directionVectorB);

            double crossProductABMagnitudeSquared = Math.Pow(crossProductAB.Magnitude.Millimeters, 2);
            double dotProductOfCrossProducts = (crossProductCB * crossProductAB).Millimeters;

            if (crossProductABMagnitudeSquared == 0)
            {
                //The first if statements should prevent you from ever getting here
                return null;
            }
            double solutionVariable = dotProductOfCrossProducts / crossProductABMagnitudeSquared;
            Dimension solutionVariableDimension = new Dimension(DimensionType.Millimeter, solutionVariable);

            Point intersectionPoint = this.GetPointOnLine(solutionVariableDimension.Millimeters);

            return intersectionPoint;

        }

        public virtual bool DoesIntersect(Line passedLine)
        {
            return (Equals(passedLine) || !ReferenceEquals(Intersection(passedLine), null));
        }

        public virtual bool DoesIntersect(LineSegment passedSegment)
        {
            Line newLine = new Line(passedSegment);
            Point intersect = this.Intersection(newLine);

            if (!ReferenceEquals(intersect, null))
                return intersect.IsOnLineSegment(passedSegment);
            else
                return false;
        }

        /// <summary>
        /// Rotates a line about the given axis by the amount of the passed angle
        /// </summary>
        /// <param name="passedAxisLine"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public Line Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(passedAxisLine, passedRotationAngle);
            Vector newDirectionVector = this.DirectionVector.Rotate(passedAxisLine, passedRotationAngle);
            return new Line(newBasePoint, newDirectionVector);
        }

        /// <summary>
        /// Returns true if the passed line is in the same plane as this one, AKA if it intersects or is parallel to the other line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsCoplanarWith(Line passedLine)
        {
            double[] point1Line1 = { this.BasePoint.X.Millimeters, this.BasePoint.Y.Millimeters, this.BasePoint.Z.Millimeters };

            Point anotherPointOnLine1 = this.GetPointOnLine(2);
            double[] point2Line1 = { anotherPointOnLine1.X.Millimeters, anotherPointOnLine1.Y.Millimeters, anotherPointOnLine1.Z.Millimeters };

            double[] point1Line2 = { passedLine.BasePoint.X.Millimeters, passedLine.BasePoint.Y.Millimeters, passedLine.BasePoint.Z.Millimeters };

            Point anotherPointOnLine2 = passedLine.GetPointOnLine(2);
            double[] point2Line2 = { anotherPointOnLine2.X.Millimeters, anotherPointOnLine2.Y.Millimeters, anotherPointOnLine2.Z.Millimeters };

            Matrix pointsMatrix = new Matrix(4, 4);

            pointsMatrix.SetRow(0, point1Line1);
            pointsMatrix.SetRow(1, point2Line1);
            pointsMatrix.SetRow(2, point1Line2);
            pointsMatrix.SetRow(3, point2Line2);

            double[] onesColumn = { 1, 1, 1, 1 };
            pointsMatrix.SetColumn(3, onesColumn);

            // checks if it is equal to 0
            double determinate = Math.Abs(pointsMatrix.Determinant());
            Dimension determinateDimension = new Dimension(DimensionType.Millimeter, determinate);
            return determinateDimension == new Dimension();
        }

        public Line Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            Point newBasePoint = this.BasePoint.Translate(passedDirectionVector, passedDisplacement);
            Point newOtherPoint = this.GetPointOnLine(2).Translate(passedDirectionVector, passedDisplacement);

            return new Line(newBasePoint, newOtherPoint);
        }


        #endregion

        #region Overloaded Operators

        public static bool operator ==(Line d1, Line d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Line d1, Line d2)
        {
            return !d1.Equals(d2);
        }

        public override bool Equals(object obj)
        {
            Line passedLine = (Line)obj;
            bool linesAreParallel = IsParallelTo(passedLine);
            bool basePointIsOnLine = BasePoint.IsOnLine(passedLine);

            return (linesAreParallel && basePointIsOnLine);
        }


        #endregion
    }
}
