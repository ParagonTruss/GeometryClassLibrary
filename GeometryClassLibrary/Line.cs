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
    public class Line : IComparable<Line>
    {
        #region Properties

        public readonly static Line XAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(1, 0, 0));
        public readonly static Line YAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(0, 1, 0));
        public readonly static Line ZAxis = new Line(PointGenerator.MakePointWithMillimeters(0, 0, 0), PointGenerator.MakePointWithMillimeters(0, 0, 1));

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

        public Dimension XInterceptIn2D
        {
            //if we are ignoring z, we can just take the x component of wher it intersects the xz plane
            get
            {
                if (XZIntercept == null)
                {
                    throw new Exception("This line does not intercept the x axis");
                }
                else
                {
                    return XZIntercept.X;
                }
            }
        }

        public Dimension YInterceptIn2D
        {
            //if we are ignoring z, we can just take the y component of whery it intersects the yz plane
            get
            {
                if (YZIntercept == null)
                {
                    throw new Exception("This line does not intercept the y axis");
                }
                else
                {
                    return YZIntercept.Y;
                }
            }
        }

        public Point XYIntercept
        {
            get { return this.FindXYIntercept(); }
        }

        public Point XZIntercept
        {
            get { return this.FindXZIntercept(); }
        }

        public Point YZIntercept
        {
            get { return this.FindYZIntercept(); }
        }

        public Point FindXYIntercept()
        {
            //make the x axis plane
            Plane xyPlane = new Plane(new Direction(PointGenerator.MakePointWithInches(0,0,1)));

            //then find out where the line and the plane intersect
            return xyPlane.Intersection(this);
        }

        public Point FindXZIntercept()
        {
            //normal vector
            Vector normal = new Vector(PointGenerator.MakePointWithInches(0, 1, 0));

            //make the x axis plane
            Plane xzPlane = new Plane(normal.Direction);

            //then find out where the line and the plane intersect
            return xzPlane.Intersection(this);
        }

        public Point FindYZIntercept()
        {
            //make the x axis plane
            Plane yzPlane = new Plane(new Direction(PointGenerator.MakePointWithInches(1, 0, 0)));

            //then find out where the line and the plane intersect
            return yzPlane.Intersection(this);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public Line()
        {
            _basePoint = new Point();
            _direction = new Direction();
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

        public Line(LineSegment lineSegment1)
            : this(lineSegment1.BasePoint, lineSegment1.Direction) { }

        public Line(Vector passedVector)
            : this(passedVector.BasePoint, passedVector.Direction) { }

        /// <summary>
        /// Constructs a line through any 2 dimension points
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedOtherPoint"></param>
        public Line(Point passedBasePoint, Point passedOtherPoint)
        {
            _basePoint = new Point(passedBasePoint);
            _direction = new Direction(passedBasePoint, passedOtherPoint);
        }

        /// <summary>
        /// Creates a line with the given direction and point
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="direction"></param>
        public Line(Point passedBasePoint, Direction passedDirection)
        {
            _basePoint = new Point(passedBasePoint);
            Direction = new Direction(passedDirection);
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

        /// <summary>
        /// Creates a line using with a direction in the same direction as the given vector
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="direction"></param>
        public Line(Point passedBasePoint, Vector passedDirectionVector)
        {
            _basePoint = new Point(passedBasePoint);
            Direction = new Direction(passedDirectionVector.Direction);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public Line(Line toCopy)
            : this(toCopy.BasePoint, toCopy.Direction) { }

        #endregion

        #region Overloaded Operators

        public static bool operator ==(Line line1, Line line2)
        {
            if ((object)line1 == null)
            {
                if ((object)line2 == null)
                {
                    return true;
                }
                return false;
            }
            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
        {
            if ((object)line1 == null)
            {
                if ((object)line2 == null)
                {
                    return false;
                }
                return true;
            }
            return !line1.Equals(line2);
        }

        public override bool Equals(object line)
        {
            if (line == null)
            {
                return false;
            }

            try
            {
                Line passedLine = (Line)line;
                bool linesAreParallel = IsParallelTo(passedLine);
                bool basePointIsOnLine = BasePoint.IsOnLine(passedLine);

                return (linesAreParallel && basePointIsOnLine);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// planning on implementing by sorting based on smallest x intercept in 2d plane
        /// from left to right and if they share then the one that occurs first
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Line other)
        {
            //see if the firt line doesnt intersect
            try
            {
                //if it doesnt throw an error it does and we can keep going
                Dimension nullTest = this.XInterceptIn2D;
            }
            catch (Exception)
            {
                //see if the second line also doesnt intersect
                try
                {
                    //the second one intersects, so the first is "greater" than the second
                    Dimension nullTest = this.XInterceptIn2D;
                    return 1;
                }
                catch (Exception) //if they both dont intersect they are equal
                {
                    return 0;
                }
            }

            //see if only the second one doesnt intersect
            try
            {
                //if it doesnt throw an error it does and we can keep going
                Dimension nullTest = other.XInterceptIn2D;
            }
            catch (Exception)
            {
                //the second doesnt intersect so the first is "smaller" than the second
                return -1;
            }

            //now that we've handled the cases where they dont intersect, we can check the values
            if (this.XInterceptIn2D == other.XInterceptIn2D)
            {
                return 0;
            }
            else
            {
                return this.XInterceptIn2D.CompareTo(other.XInterceptIn2D);
            }
        }

        #endregion

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

            Dimension dotProduct = this.UnitVector(DimensionType.Inch) * passedIntersectingLine.UnitVector(DimensionType.Inch);

            //since they are unit vectors the magnitudes multiplies together should still be one so the equation simplifies
            // from: A*B = |A||B|cos(theta) to: A*B = cos(theta), which can be rearranged to how we use it here: theta = Acos(A*B)
            double angleBetweenLines = Math.Acos(dotProduct.Inches);

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
            Dimension newX = new Dimension(DimensionType.Inch, _basePoint.X.Inches + Direction.XComponentOfDirection * multiplier);
            Dimension newY = new Dimension(DimensionType.Inch, _basePoint.Y.Inches + Direction.YComponentOfDirection * multiplier);
            Dimension newZ = new Dimension(DimensionType.Inch, _basePoint.Z.Inches + Direction.ZComponentOfDirection * multiplier);

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
            return passedLine.UnitVector(DimensionType.Inch).PointInSameDirection(this.UnitVector(DimensionType.Inch)) ||
                passedLine.UnitVector(DimensionType.Inch).PointInOppositeDirections(this.UnitVector(DimensionType.Inch));
        }

        /// <summary>
        /// Returns whether or not the two lines are perindicular to each other
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsPerpindicularTo(Line passedLine)
        {
            //if they are perpindicular then the dot product should be 0
            Dimension dotted = passedLine.Direction.UnitVector(DimensionType.Inch) * this.Direction.UnitVector(DimensionType.Inch);
            return (dotted == new Dimension());
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

            Vector directionVectorA = new Vector(this.BasePoint, this.UnitVector(DimensionType.Inch));
            Vector directionVectorB = new Vector(passedLine.BasePoint, passedLine.UnitVector(DimensionType.Inch));
            Vector basePointDiffVectorC = new Vector(this.BasePoint, passedLine.BasePoint);

            Vector crossProductCB = basePointDiffVectorC.CrossProduct(directionVectorB);
            Vector crossProductAB = directionVectorA.CrossProduct(directionVectorB);

            double crossProductABMagnitudeSquared = Math.Pow(crossProductAB.Magnitude.Inches, 2);
            double dotProductOfCrossProducts = (crossProductCB * crossProductAB).Inches;

            if (crossProductABMagnitudeSquared == 0)
            {
                //The first if statements should prevent you from ever getting here
                return null;
            }
            double solutionVariable = dotProductOfCrossProducts / crossProductABMagnitudeSquared;
            Dimension solutionVariableDimension = new Dimension(DimensionType.Inch, solutionVariable);

            Point intersectionPoint = this.GetPointOnLine(solutionVariableDimension.Inches);

            return intersectionPoint;
        }

        /// <summary>
        /// Returns whether or not the two lines intersect
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Line passedLine)
        {
            return (Equals(passedLine) || !ReferenceEquals(Intersection(passedLine), null));
        }

        /// <summary>
        /// Determines whether or not the vector and line intersect
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Vector passedVector)
        {
            Line newLine = new Line(passedVector);
            Point intersect = this.Intersection(newLine);

            if (!ReferenceEquals(intersect, null))
                return intersect.IsOnVector(passedVector);
            else
                return false;
        }

        /// <summary>
        /// Determines wheter or not the linesegment and line intersect
        /// </summary>
        /// <param name="passedSegment"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(LineSegment passedSegment)
        {
            return DoesIntersect((Vector)passedSegment);
        }

        /// <summary>
        /// Returns whether or not the Polygon and Line intersect
        /// </summary>
        /// <param name="passedPolygon"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Polygon passedPolygon)
        {
            return (passedPolygon.DoesIntersect(this));
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
            Vector newDirectionVector = this.UnitVector(DimensionType.Inch).Rotate(passedAxisLine, passedRotationAngle);
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

        /// <summary>
        /// Translates the line the given distance in the given direction
        /// </summary>
        /// <param name="passedDirectionVector"></param>
        /// <param name="passedDisplacement"></param>
        /// <returns></returns>
        public Line Translate(Direction passedDirection, Dimension passedDisplacement)
        {
            Point newBasePoint = this.BasePoint.Translate(passedDirection, passedDisplacement);
            Point newOtherPoint = this.GetPointOnLine(2).Translate(passedDirection, passedDisplacement);

            return new Line(newBasePoint, newOtherPoint);
        }

        /// <summary>
        /// Makes the line into a plane perindicular to the xy plane
        /// </summary>
        /// <param name="line">The line to make into a plane</param>
        /// <returns>returns a plane perpindicular to the XY-Plane that contains the given line</returns>
        public Plane MakeIntoPlanePerpindicularToXYPlane()
        {
            //we use the base point and another point on the line so we know that the plane will contain the given line
            //then we use the base point but moved in the z direction so that we know it will also contain that line, which
            //will alway be perpindicular to XY because the only thing changing between the two points is the z.
            Plane plane = new Plane(this.BasePoint, this.GetPointOnLine(2), this.BasePoint + PointGenerator.MakePointWithMillimeters(0, 0, 10));
            return plane;
        }

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given dimension that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// </summary>
        /// <param name="passedType">Dimesnion Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public virtual Vector UnitVector(DimensionType passedType)
        {
            return Direction.UnitVector(passedType);
        }

        /// <summary>
        /// Returns the Direction so that y is always positive, useful for comparing slopes
        /// </summary>
        /// <returns></returns>
        public Direction DirectionAssumingPositiveY()
        {
            //if its greater than 360 we can subtract 180 from it to it is in the opposite direction and we need to flip it
            if (this.Direction.Phi > new Angle(AngleType.Degree, 180))
            {
                return this.Direction.Reverse();
            }

            return new Direction(this.Direction);
        }

        #endregion
    }
}
