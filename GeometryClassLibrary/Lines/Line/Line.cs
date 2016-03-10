using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnitClassLibrary;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using static GeometryClassLibrary.Point;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.AreaUnit.AreaTypes.Imperial.SquareInchesUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A line in 3d space.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Line : IEquatable<Line>
    {
        #region Properties and Fields

        //Predefined lines to use as references
        public static Line XAxis = new Line(Point.Origin, Direction.Right);
        public static Line YAxis = new Line(Point.Origin, Direction.Up);
        public static Line ZAxis = new Line(Point.Origin, Direction.Out);

        /// <summary>
        /// A point on the line to use as a reference.
        /// </summary>
        [JsonProperty]
        public Point BasePoint { get; protected set; }

        /// <summary>
        /// The direction the line is extends from the base point in one direction
        /// Note: it also extends in the direction opposite
        /// </summary>
        [JsonProperty]
        public Direction Direction { get; protected set; }

        #endregion
     
        #region Constructors

        /// <summary>
        /// Null constructor
        /// </summary>
        protected Line() { }

        /// <summary>
        /// Creates a line through the origin and a point
        /// </summary>
        public Line(Point point)
        {
            this.BasePoint = Origin;
            this.Direction = new Direction(point);
        }

        /// <summary>
        /// Creates a line with the given direction and point if passed, other wise it uses the origin as the base point
        /// </summary>
        [JsonConstructor]
        public Line(Direction direction, Point basePoint = null)
        {
            if (basePoint == null)
            {
                this.BasePoint = Origin;
            }
            else
            {
                this.BasePoint = basePoint;
            }
            this.Direction = direction;
        }

        public Line(Point basePoint, Direction direction)
        {
            this.BasePoint = basePoint;
            this.Direction = direction;
        }
        /// <summary>
        /// Constructs a line through any 2 points
        /// </summary>
        public Line(Point basePoint, Point otherPoint)
        {
            this.BasePoint = basePoint;
            this.Direction = new Direction(basePoint, otherPoint);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="toCopy"></param>
        public Line(Line toCopy)
            : this(toCopy.Direction, toCopy.BasePoint) { }

        /// <summary>
        /// Creates a new line with the same direction but different base 
        /// Useful for turning vectors, and segments back into lines.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="newBasePoint"></param>
        public Line(Point newBasePoint, Line line) : this(line.Direction, newBasePoint) { }

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
            if (obj == null || !(obj is Line))
            {
                return false;
            }

            Line comparableLine = (Line)obj;

            bool linesAreParallel = IsParallelTo(comparableLine);
            bool basePointIsOnLine = BasePoint.IsOnLine(comparableLine);

            return (linesAreParallel && basePointIsOnLine);
           
        }
        public bool Equals(Line other)
        {
            //make sure the passed object is not null
            if (other == null)
            {
                return false;
            }

            bool linesAreParallel = IsParallelTo(other);
            bool basePointIsOnLine = BasePoint.IsOnLine(other);

            return (linesAreParallel && basePointIsOnLine);

        }
        ///// <summary>
        ///// planning on implementing by sorting based on smallest x intercept in 2d plane
        ///// from left to right and if they share then the one that occurs first
        ///// </summary>
        //public int CompareTo(Line other)
        //{
        //    //see if the first line doesnt intersect
        //    try
        //    {
        //        //if it doesnt throw an error it does and we can keep going
        //        Distance nullTest = this.XInterceptIn2D();
        //    }
        //    catch (Exception)
        //    {
        //        //see if the second line also doesnt intersect
        //        try
        //        {
        //            //the second one intersects, so the first is "greater" than the second
        //            Distance nullTest = this.XInterceptIn2D();
        //            return 1;
        //        }
        //        catch (Exception) //if they both dont intersect they are "equal" in this way of sorting
        //        {
        //            return 0;
        //        }
        //    }

        //    //see if only the second one doesnt intersect
        //    try
        //    {
        //        //if it doesnt throw an error it does and we can keep going
        //        Distance nullTest = other.XInterceptIn2D();
        //    }
        //    catch (Exception)
        //    {
        //        //the second doesnt intersect so the first is "smaller" than the second
        //        return -1;
        //    }

        //    //now that we've handled the cases where they dont intersect, we can check the values
        //    if (this.XInterceptIn2D() == other.XInterceptIn2D())
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return this.XInterceptIn2D().CompareTo(other.XInterceptIn2D());
        //    }
        //}

        #endregion

        #region Methods
        /// <summary>
        /// Returns the X intercept of the line if the z Distance is ignored
        /// </summary>
        public Distance XInterceptIn2D()
        {
            if (XZIntercept() == null)
            {
                return null;
            }
            else
            {
                return XZIntercept().X;
            }
        }

        /// <summary>
        /// Returns the Y intecept of the Line if the z Distance is ignored
        /// </summary>
        public Distance YInterceptIn2D()
        {
            if (YZIntercept() == null)
            {
                return null;
            }
            else
            {
                return YZIntercept().Y;
            }
        }

        /// <summary>
        /// Returns the point at which this line intercepts the XY-Plane
        /// </summary>
        public Point XYIntercept()
        {
            return this.IntersectWithPlane(Plane.XY);
        }

        /// <summary>
        /// Returns the point at which this line intercepts the XZ-Plane
        /// </summary>
        public Point XZIntercept()
        {
            return this.IntersectWithPlane(Plane.XZ);
        }

        /// <summary>
        /// Returns the point at which this line intercepts the YZ-Plane
        /// </summary>
        public Point YZIntercept()
        {
            return Plane.YZ.IntersectWithLine(this);
        }

        public Plane PlaneThroughLineInDirectionOf(Enums.Axis passedAxis)
        {
            Line extrusionLine;

            switch (passedAxis)
            {
                case Enums.Axis.X:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(1, 0));
                    break;
                case Enums.Axis.Y:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(0, 1));
                    break;
                case Enums.Axis.Z:
                    extrusionLine = new Line(this.BasePoint,
                        this.BasePoint - Point.MakePointWithInches(0, 0, 1));
                    break;
                default:
                    throw new ArgumentException("You passed in an unknown Axis Enum");
            }
            return new Plane(extrusionLine, this);
        }

        /// <summary>
        /// Returns the smaller of the two angles fromed where the two lines intersect
        /// </summary>
        public Angle SmallestAngleBetween(Line passedIntersectingLine)
        {
            Angle returnAngle = AngleBetween(passedIntersectingLine);

            if (returnAngle.InDegrees > 90)
            {
                return (Angle.StraightAngle - returnAngle);
            }
            else
            {
                return returnAngle;
            }
        }

        /// <summary>
        /// Finds the angle between the two Lines.
        /// </summary>
        public Angle AngleBetween(Line otherLine)
        {
            return this.Direction.AngleBetween(otherLine.Direction);
        }        
	
        public Angle SignedAngleBetween(Line line, Line referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Line.ZAxis;
            }

            return this.Direction.AngleFromThisToThat(line.Direction, referenceNormal.Direction);
	    }

        /// <summary>
        /// Returns the point a given distance along the line.
        /// </summary>
        public Point GetPointAlongLine(Distance distance)
        {
            Distance newX = BasePoint.X + distance * Direction.X;
            Distance newY = BasePoint.Y + distance * Direction.Y;
            Distance newZ = BasePoint.Z + distance * Direction.Z;
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Determines if this line and the passed line are parallel.
        /// </summary>
        public virtual bool IsParallelTo(Line passedLine)
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
            return this.SmallestAngleBetween(passedLine) == 90 * new Angle(1, Degrees);
        }

        /// <summary>
        /// Returns the point at which a line intersects the passed line
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public virtual Point IntersectWithLine(Line passedLine)
        {
            if (this.Equals(passedLine))
            {
                return this.BasePoint;
            }
            if (!this.IsCoplanarWith(passedLine) || this.IsParallelTo(passedLine))
            {
                //The lines do not intersect
                return null;
            }

            //Following a formula from (http://mathworld.wolfram.com/Line-LineIntersection.html)

            Vector directionVectorA = new Vector(this.BasePoint, this.UnitVector(new Inch()));
            Vector directionVectorB = new Vector(passedLine.BasePoint, passedLine.UnitVector(new Inch()));
            Vector basePointDiffVectorC = new Vector(this.BasePoint, passedLine.BasePoint);

            Vector crossProductCB = basePointDiffVectorC.CrossProduct(directionVectorB);
            Vector crossProductAB = directionVectorA.CrossProduct(directionVectorB);

            Measurement crossProductABMagnitudeSquared = crossProductAB.Magnitude.InInches ^ 2;
            Measurement dotProductOfCrossProducts = (crossProductCB.DotProduct(crossProductAB)).MeasurementIn(new SquareInch());

            if (crossProductABMagnitudeSquared == 0)
            {
                //The first if statements should prevent you from ever getting here
                return null;
            }
            Measurement solutionVariable = dotProductOfCrossProducts / crossProductABMagnitudeSquared;
            Distance solutionVariableDistance = new Distance(new Inch(), solutionVariable);

            Point intersectionPoint = this.GetPointAlongLine(solutionVariableDistance);

            return intersectionPoint;
        }

        public virtual Point IntersectWithPlane(Plane plane)
        {
            return plane.IntersectWithLine(this);
        }

        /// <summary>
        /// return the point of intersection, if any, between this line and the passed polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public virtual Point IntersectWithPolygon(Polygon polygon)
        {
            return polygon.IntersectWithLine(this);
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
        public bool IntersectsLine(Line passedLine)
        {
            Point intersect = this.IntersectWithLine(passedLine);

            if (intersect != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not the vector and line intersect
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(LineSegment segment)
        {
            Line newLine = new Line(segment);
            Point intersect = this.IntersectWithLine(newLine);

            if (intersect == null)
            {
                return false;
            }
            return intersect.IsOnLineSegment(segment);
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
            Vector newDirectionVector = this.UnitVector(new Inch()).Rotate(rotationToApply);
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

            Vector newDirectionVector = this.Direction.UnitVector(new Inch());

            foreach (Rotation rotation in rotationsToApply)
            {
                rotated.BasePoint = this.BasePoint.Rotate3D(rotation);
                newDirectionVector = newDirectionVector.UnitVector(new Inch()).Rotate(rotation);
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
            double[] point1Line1 = { this.BasePoint.X.InInches.Value, this.BasePoint.Y.InInches.Value, this.BasePoint.Z.InInches.Value };

            Point anotherPointOnLine1 = this.GetPointAlongLine(new Distance(1, Inches));
            double[] point2Line1 = { anotherPointOnLine1.X.InInches.Value, anotherPointOnLine1.Y.InInches.Value, anotherPointOnLine1.Z.InInches.Value };

            double[] point1Line2 = { passedLine.BasePoint.X.InInches.Value, passedLine.BasePoint.Y.InInches.Value, passedLine.BasePoint.Z.InInches.Value };

            Point anotherPointOnLine2 = passedLine.GetPointAlongLine(new Distance(1, Inches) * 2);
            double[] point2Line2 = { anotherPointOnLine2.X.InInches.Value, anotherPointOnLine2.Y.InInches.Value, anotherPointOnLine2.Z.InInches.Value };

            Matrix pointsMatrix = new Matrix(4, 4);

            pointsMatrix.SetRow(0, point1Line1);
            pointsMatrix.SetRow(1, point2Line1);
            pointsMatrix.SetRow(2, point1Line2);
            pointsMatrix.SetRow(3, point2Line2);

            double[] onesColumn = { 1, 1, 1, 1 };
            pointsMatrix.SetColumn(3, onesColumn);

            // checks if it is equal to 0
            double determinant = Math.Abs(pointsMatrix.Determinant());
            Distance determinateDistance = determinant*new Distance(1, Inches);
            return determinateDistance == Distance.ZeroDistance;
        }

        /// <summary>
        /// Translates the line the given distance in the given direction
        /// </summary>
        /// <returns></returns>
        public Line Translate(Translation translation)
        {
            Point newBasePoint = this.BasePoint.Translate(translation);
            Point newOtherPoint = this.GetPointAlongLine(new Distance(1, Inches) * 2).Translate(translation);

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
            Vector linesVector = this.UnitVector(new Inch());

            Vector shifted = linesVector.Shift(passedShift);

            //then construct a new line with that information
            return new Line(shifted);
        }

        /// <summary>
        /// Creates a vector on this line, of unit length for the passed distance type.
        /// </summary>
        public virtual Vector UnitVector(DistanceType passedType)
        {
            return new Vector(this.BasePoint, this.Direction.UnitVector(passedType));
        }

        /// <summary>
        /// Makes a Perpendicular Line to this line that is in the passed plane
        /// this assumes the line is in the plane
        /// </summary>
        public Line MakePerpendicularLineInGivenPlane(Plane planeToMakePerpendicularLineIn)
        {
            if (planeToMakePerpendicularLineIn.IsParallelTo(this))
            {
                //rotate it 90 degrees in the nornal of the plane and it will be perpendicular to the original
                return this.Rotate(new Rotation(new Vector(this.BasePoint, planeToMakePerpendicularLineIn.NormalVector), RightAngle));
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
            Vector aCrossB = this.Direction.UnitVector(new Inch()).CrossProduct(projectOnto.NormalVector.UnitVector(new Inch()));
            Vector projectionVector = projectOnto.NormalVector.UnitVector(new Inch()).CrossProduct(aCrossB);

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
            Distance distance = point.DistanceTo(this);
            bool equal = distance.Equals(Distance.ZeroDistance);
            return equal;
        }

        public LineSegment GetSegmentAlongLine(Distance distance1, Distance distance2)
        {
            return new LineSegment(GetPointAlongLine(distance1), GetPointAlongLine(distance2));
        }
        #endregion

        
    }
}
