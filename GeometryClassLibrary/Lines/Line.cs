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
    public class Line : IComparable<Line>
    {
        #region Properties and Fields

        //Predefined lines to use as references
        public readonly static Line XAxis = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(1, 0, 0));
        public readonly static Line YAxis = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 1, 0));
        public readonly static Line ZAxis = new Line(PointGenerator.MakePointWithInches(0, 0, 0), PointGenerator.MakePointWithInches(0, 0, 1));

        /// <summary>
        /// A point on the line to use as a reference point
        /// </summary>
        public virtual Point BasePoint
        {
            get { return _basePoint; }
            set { this._basePoint = value; }
        }
        private Point _basePoint; //this is any point that is on the line

        /// <summary>
        /// The direction the line is going out of the base point in one direction
        /// Note: it also extends out in the direction opposite of this one
        /// </summary>
        public virtual Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private Direction _direction;


        /// <summary>
        /// Returns the X intercept of the line if the z Distance is ignored
        /// </summary>
        public Distance XInterceptIn2D
        {
            //if we are ignoring z, we can just take the x component of where it intersects the xz plane
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

        /// <summary>
        /// Returns the Y intecept of the Line if the z Distance is ignored
        /// </summary>
        public Distance YInterceptIn2D
        {
            //if we are ignoring z, we can just take the y component of where it intersects the yz plane
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

        /// <summary>
        /// Returns the point at which this line intercepts the XY-Plane
        /// </summary>
        public Point XYIntercept
        {
            get { return this.FindXYIntercept(); }
        }

        /// <summary>
        /// Returns the point at which this line intercepts the XZ-Plane
        /// </summary>
        public Point XZIntercept
        {
            get { return this.FindXZIntercept(); }
        }

        /// <summary>
        /// Returns the point at which this line intercepts the YZ-Plane
        /// </summary>
        public Point YZIntercept
        {
            get { return this.FindYZIntercept(); }
        }

        /// <summary>
        /// Finds the point at which this line intersects the XY-plane
        /// </summary>
        /// <returns>Returns where this line intersects the XY-Plane</returns>
        private Point FindXYIntercept()
        {
            //make the x axis plane
            Plane xyPlane = new Plane(new Direction(PointGenerator.MakePointWithInches(0, 0, 1)));

            //then find out where the line and the plane intersect
            return xyPlane.Intersection(this);
        }

        /// <summary>
        /// Finds the point at which this line intersects the XZ-plane
        /// </summary>
        /// <returns>Returns where this line intersects the XZ-Plane</returns>
        private Point FindXZIntercept()
        {
            //normal vector
            Vector normal = new Vector(PointGenerator.MakePointWithInches(0, 1, 0));

            //make the x axis plane
            Plane xzPlane = new Plane(normal.Direction);

            //then find out where the line and the plane intersect
            return xzPlane.Intersection(this);
        }

        /// <summary>
        /// Finds the point at which this line intersects the YZ-plane
        /// </summary>
        /// <returns>Returns where this line intersects the YZ-Plane</returns>
        private Point FindYZIntercept()
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
        /// Creates a line through the origin and a passed Distance point
        /// </summary>
        /// <param name="passedDirectionReferencePoint"></param>
        public Line(Point passedDirectionReferencePoint)
        {
            _basePoint = PointGenerator.MakePointWithMillimeters(0, 0, 0);
            _direction = new Direction(passedDirectionReferencePoint);
        }

        /// <summary>
        /// Creates a line with the given direction and point if passed, other wise it uses the origin as the base point
        /// </summary>
        /// <param name="direction">The direction the line goes in one direction from its reference point
        /// Note: it also goes out in the opposite direction as this</param>
        /// <param name="basePoint">A point on the line to use as a reference point</param>
        public Line(Direction passedDirection, Point passedBasePoint = null)
        {
            if (passedBasePoint == null)
            {
                this.BasePoint = new Point();
            }
            else
            {
                this.BasePoint = new Point(passedBasePoint);
            }
            this.Direction = new Direction(passedDirection);
        }


        /// <summary>
        /// Constructs a line through any 2 points
        /// </summary>
        /// <param name="passedBasePoint">The first point on the line and the point to use as the reference point</param>
        /// <param name="passedOtherPoint">The other point which the line goes through</param>
        public Line(Point passedBasePoint, Point passedOtherPoint)
        {
            _basePoint = new Point(passedBasePoint);
            _direction = new Direction(passedBasePoint, passedOtherPoint);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public Line(Line toCopy)
            : this(toCopy.Direction, toCopy.BasePoint) { }

        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


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

        public override bool Equals(object obj)
        {
            //make sure the passed object is not null
            if (obj == null)
            {
                return false;
            }

            //try casting and comparing it
            try
            {
                Line comparableLine = (Line)obj;

                bool linesAreParallel = IsParallelTo(comparableLine);
                bool basePointIsOnLine = BasePoint.IsOnLine(comparableLine);

                return (linesAreParallel && basePointIsOnLine);
            }
            //if it wasnt a Line than it wasnt equal
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
            //see if the first line doesnt intersect
            try
            {
                //if it doesnt throw an error it does and we can keep going
                Distance nullTest = this.XInterceptIn2D;
            }
            catch (Exception)
            {
                //see if the second line also doesnt intersect
                try
                {
                    //the second one intersects, so the first is "greater" than the second
                    Distance nullTest = this.XInterceptIn2D;
                    return 1;
                }
                catch (Exception) //if they both dont intersect they are "equal" in this way of sorting
                {
                    return 0;
                }
            }

            //see if only the second one doesnt intersect
            try
            {
                //if it doesnt throw an error it does and we can keep going
                Distance nullTest = other.XInterceptIn2D;
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
            }
            return new Plane(this, extrustionLine);
        }

        //Mkae this in linesegment and make it direction dependent
        /// <summary>
        /// Returns the smaller of the two angles fromed where the two lines itnersect
        /// </summary>
        /// <param name="passedIntersectingLine">The line to find the angle between</param>
        /// <returns>The smaller of the two angles formed by this line pair</returns>
        public Angle SmallestAngleBetweenIntersectingLine(Line passedIntersectingLine)
        {
            Angle returnAngle = AngleBetweenIntersectingLine(passedIntersectingLine);

            if (returnAngle.Degrees > 90)
            {
                return new Angle(AngleType.Degree, 180) - returnAngle;
            }
            else
            {
                return returnAngle;
            }
        }

        /// <summary>
        /// Finds the angle between the two intersecting Lines. This operation is order dependent!
        /// </summary>
        /// <param name="passedIntersectingLine">The Line to find the angle with</param>
        /// <returns>Returns the angle betweent the two Lines </returns>
        public Angle AngleBetweenIntersectingLine(Line passedIntersectingLine)
        {
            if (!DoesIntersect(passedIntersectingLine))
            {
                //if it is the same line or opposite it will go here and we need to check for that and then make it 0 or 180
                Line thisAsLine = new Line(this);
                if (thisAsLine.Equals(passedIntersectingLine))
                {
                    if (this.Direction == passedIntersectingLine.Direction)
                    {
                        return new Angle();
                    }
                    else
                    {
                        return new Angle(AngleType.Degree, 180);
                    }
                }
                throw new Exception("No intercept?");
            }

            Distance dotProduct = this.UnitVector(DistanceType.Inch) * passedIntersectingLine.UnitVector(DistanceType.Inch);

            //since they are unit vectors the magnitudes multiplies together should still be one so the equation simplifies
            // from: |CP| = |A||B|sin(theta) to: |CP| = sin(theta), which can be rearranged to how we use it here: theta = Asin(|CP|)
            double angleBetweenVector = Math.Acos(dotProduct.Inches);

            //now find out if it should be negative or positive by looking at the plane they form and its normal

            Angle returnAngle = new Angle(AngleType.Radian, angleBetweenVector);

            return returnAngle;
        }

        /// <summary>
        /// Returns a point on the line based on the multiplier entered
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public Point GetPointOnLine(double multiplier)
        {
            Distance newX = new Distance(DistanceType.Inch, _basePoint.X.Inches + Direction.XComponentOfDirection * multiplier);
            Distance newY = new Distance(DistanceType.Inch, _basePoint.Y.Inches + Direction.YComponentOfDirection * multiplier);
            Distance newZ = new Distance(DistanceType.Inch, _basePoint.Z.Inches + Direction.ZComponentOfDirection * multiplier);

            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// returns the point a given distance along the line.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Point GetPointAlongLine(Distance distance)
        {
            Distance newX = _basePoint.X + distance * Direction.XComponentOfDirection;
            Distance newY = _basePoint.Y + distance * Direction.YComponentOfDirection;
            Distance newZ = _basePoint.Z + distance * Direction.ZComponentOfDirection;
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Returns true if the passed line is parallel to (same direction as) this line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsParallelTo(Line passedLine)
        {
            return (passedLine.Direction == this.Direction || passedLine.Direction == this.Direction.Reverse());
        }

        /// <summary>
        /// Returns whether or not the two lines are perindicular to each other
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual bool IsPerpendicularTo(Line passedLine)
        {
            //if they are perpendicular then the dot product should be 0
            //Distance dotted = passedLine.Direction.UnitVector(DistanceType.Inch) * this.Direction.UnitVector(DistanceType.Inch);
            //return (dotted == new Distance());
            return passedLine.UnitVector(DistanceType.Inch).DotProductIsEqualToZero(this.UnitVector(DistanceType.Inch));
        }

        /// <summary>
        /// Returns the point at which a line intersects the passed line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual Point Intersection(Line passedLine)
        {
            if (this.Equals(passedLine))
            {
                return this.BasePoint;
            }
            if (!this.IsCoplanarWith(passedLine))
            {
                //The lines do not intersect
                return null;
            }

            //Following a formula from (http://mathworld.wolfram.com/Line-LineIntersection.html)

            Vector directionVectorA = new Vector(this.BasePoint, this.UnitVector(DistanceType.Inch));
            Vector directionVectorB = new Vector(passedLine.BasePoint, passedLine.UnitVector(DistanceType.Inch));
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
            Distance solutionVariableDistance = new Distance(DistanceType.Inch, solutionVariable);

            Point intersectionPoint = this.GetPointAlongLine(solutionVariableDistance);

            return intersectionPoint;
        }

        public virtual Point Intersection(Plane plane)
        {
            return plane.Intersection(this);
        }

        /// <summary>
        /// return the point of intersection, if any, between this line and the passed polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public virtual Point Intersection(Polygon polygon)
        {
            return polygon.Intersection(this);
        }

        public virtual List<Point> IntersectionCoplanarPoints(Polygon polygon)
        {
            return polygon.IntersectionCoplanarPoints(this);
        }

        /// <summary>
        /// Returns whether or not the two lines intersect
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Line passedLine)
        {
            Point intersect = this.Intersection(passedLine);

            if (intersect != null)
            {
                return true;
            }
            return false;
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

            if (intersect == null)
            {
                return false;
            }
            return intersect.IsOnVector(passedVector);
        }

        /// <summary>
        /// Determines whether or not the line and linesegment intersect
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
        //ToDo: Needs unit Test
        public virtual bool DoesIntersect(Polygon passedPolygon)
        {
            return (passedPolygon.DoesIntersect(this));
        }

        /// <summary>
        /// Returns whether ot not this line itnersects the Polyhedron
        /// </summary>
        /// <param name="passedPolyhedron"></param>
        /// <returns>Returns true if the Line intersected the Polyhedron or false if it did not</returns>
        public virtual bool DoesIntersect(Polyhedron passedPolyhedron)
        {
            foreach (Polygon polygon in passedPolyhedron.Polygons)
            {
                if (this.DoesIntersect(polygon))
                {
                    return true;
                }
            }
            return false;
        }
       
        /// <summary>
        /// Rotates a line about the given axis by the amount of the passed angle
        /// </summary>
        /// <param name="rotationToApply">The Rotation to apply to the point that stores the axis to rotate around and the angle to rotate</param>
        /// <returns></returns>
        public Line Rotate(Rotation rotationToApply)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Vector newDirectionVector = this.UnitVector(DistanceType.Inch).Rotate(rotationToApply);
            return new Line(newDirectionVector.Direction, newBasePoint);
        }

        /// <summary>
        /// Rotates a line with the given lists of rotations
        /// </summary>
        /// <param name="rotationsToApply">The list of Rotations(that stores the axis to rotate around and the angle to rotate) to apply to the Line</param>
        /// <returns></returns>
        public Line Rotate(List<Rotation> rotationsToApply)
        {
            Line rotated = new Line(this);

            Vector newDirectionVector = this.Direction.UnitVector(DistanceType.Inch);

            foreach (Rotation rotation in rotationsToApply)
            {
                rotated.BasePoint = this.BasePoint.Rotate3D(rotation);
                newDirectionVector = newDirectionVector.UnitVector(DistanceType.Inch).Rotate(rotation);
            }

            rotated.Direction = newDirectionVector.Direction;

            return rotated;
        }

        /// <summary>
        /// Returns true if the passed line is in the same plane as this one, AKA if it intersects or is parallel to the other line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsCoplanarWith(Line passedLine)
        {
            double[] point1Line1 = { this.BasePoint.X.Inches, this.BasePoint.Y.Inches, this.BasePoint.Z.Inches };

            Point anotherPointOnLine1 = this.GetPointAlongLine(Distance.Inch);
            double[] point2Line1 = { anotherPointOnLine1.X.Inches, anotherPointOnLine1.Y.Inches, anotherPointOnLine1.Z.Inches };

            double[] point1Line2 = { passedLine.BasePoint.X.Inches, passedLine.BasePoint.Y.Inches, passedLine.BasePoint.Z.Inches };

            Point anotherPointOnLine2 = passedLine.GetPointAlongLine(Distance.Inch * 2);
            double[] point2Line2 = { anotherPointOnLine2.X.Inches, anotherPointOnLine2.Y.Inches, anotherPointOnLine2.Z.Inches };

            Matrix pointsMatrix = new Matrix(4, 4);

            pointsMatrix.SetRow(0, point1Line1);
            pointsMatrix.SetRow(1, point2Line1);
            pointsMatrix.SetRow(2, point1Line2);
            pointsMatrix.SetRow(3, point2Line2);

            double[] onesColumn = { 1, 1, 1, 1 };
            pointsMatrix.SetColumn(3, onesColumn);

            // checks if it is equal to 0
            double determinate = Math.Abs(pointsMatrix.Determinant());
            Distance determinateDistance = new Distance(DistanceType.Inch, determinate);
            return determinateDistance == new Distance();
        }

        /// <summary>
        /// Translates the line the given distance in the given direction
        /// </summary>
        /// <returns></returns>
        public Line Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newOtherPoint = this.GetPointAlongLine(Distance.Inch * 2).Translate(translation);

            return new Line(newBasePoint, newOtherPoint);
        }

        /// <summary>
        /// Shifts the Line with the given shift
        /// </summary>
        /// <param name="passedShift">The shift to apply to the Line</param>
        /// <returns>Returns a new Line that has been shifted with the given Shift</returns>
        public Line Shift(Shift passedShift)
        {
            //shift it as a vector since we currently don't shift directions
            Vector linesVector = this.UnitVector(DistanceType.Inch);

            Vector shifted = linesVector.Shift(passedShift);

            //then construct a new line with that information
            return new Line(shifted);
        }

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// </summary>
        /// <param name="passedType">Dimension Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public virtual Vector UnitVector(DistanceType passedType)
        {
            return new Vector(this.BasePoint, this.Direction.UnitVector(passedType));
        }

        /// <summary>
        /// Makes a Perpendicular Line to this line that is in the passed plane
        /// this assumes the line is in the plane
        /// </summary>
        /// <param name="planeToMakePerpindicularLineIn">The plane in which this line and the perpindicular line should both contain</param>
        /// <returns>A new Line that is in the passed plane and perpindicular to this line</returns>
        public Line MakePerpendicularLineInGivenPlane(Plane planeToMakePerpendicularLineIn)        {
            if (planeToMakePerpendicularLineIn.IsParallelTo(this))
            {
                //rotate it 90 degrees in the nornal of the plane and it will be perpendicular to the original
                return this.Rotate(new Rotation(planeToMakePerpendicularLineIn.NormalVector, new Angle(AngleType.Degree, 90)));
            }
            else
            {
                throw new ArgumentOutOfRangeException("The given line is not in the given plane");
            }
        }

        /// <summary>
        /// Projects the given line onto the Plane
        /// </summary>
        /// <param name="projectOnto">The Plane to Project this Line onto</param>
        /// <returns>Returns a new Line that is this Line projected onto the Plane</returns>
        public Line ProjectOntoPlane(Plane projectOnto)
        {
            //http://www.euclideanspace.com/maths/geometry/elements/plane/lineOnPlane/index.htm
            //When using unit vectors, the project on the plane is simply planeNormal X (lineDirection X planeNormal)
            Vector aCrossB = this.Direction.UnitVector(DistanceType.Inch).CrossProduct(projectOnto.NormalVector.UnitVector(DistanceType.Inch));
            Vector projectionVector = projectOnto.NormalVector.UnitVector(DistanceType.Inch).CrossProduct(aCrossB);

            return new Line(projectionVector.Direction, this.BasePoint);
        }

        /// <summary>
        /// Determines if the line contains the point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Point point)
        {
            if (point == null)
            {
                return false;
            }
            if (point == BasePoint)
            {
                return true;
            }

            Vector checkVector = new Vector(BasePoint, point);
            return checkVector.IsParallelTo(this);
        }

        #endregion
    }
}
