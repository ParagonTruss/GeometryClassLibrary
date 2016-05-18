using System;
using System.Collections.Generic;
using MoreLinq;
using Newtonsoft.Json;
using UnitClassLibrary;
using static UnitClassLibrary.DistanceUnit.Distance;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.AreaUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes.Imperial.InchUnit;
using UnitClassLibrary.AngleUnit;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane is an unbounded flat surface
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Plane : ISurface, IShift<Plane>
    {
        #region Properties and Fields

        public static Plane XY { get { return new Plane(Point.Origin, Direction.Out); } }
        public static Plane XZ { get { return new Plane(Point.Origin, Direction.Up); } }
        public static Plane YZ { get { return new Plane(Point.Origin, Direction.Right); } }

        public virtual bool IsBounded { get { return true; } }
        public Area Area { get { return new Area(Double.PositiveInfinity, Area.SquareInches); } }

        [JsonProperty]
        public virtual Line NormalLine { get; protected set; }
   
        public Point BasePoint { get { return NormalLine.BasePoint; } }

        public Vector NormalVector { get { return new Vector(BasePoint, NormalDirection, new Distance(1, Inches)); } }

        public Direction NormalDirection { get { return NormalLine.Direction; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Empty constuctor. For Initializing derived objects.
        /// </summary>
        protected Plane() { }

        /// <summary>
        /// Creates a Plane that contains the given point and whose normal is in a given direction.
        /// </summary>
        [JsonConstructor]
        public Plane(Direction normalDirection , Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = Point.Origin;
            }
            this.NormalLine = new Line(basePoint, normalDirection);
        }
        public Plane(Point basePoint, Direction normalDirection)
        {
            this.NormalLine = new Line(basePoint, normalDirection);
        }

        public Plane(Line normalLine)
        {
            this.NormalLine = normalLine;
        }

        /// <summary>
        /// Creates a plane that contains the two lines, provided the lines are not the same.
        /// </summary>
        public Plane(Line line1, Line line2)
        {
            //if they arent equivalent lines
            if (line1 != line2)
            {
                //if they are parallel we must find the line between to use to find the normal or else the cross product is 0
                Vector normal;
                if (line1.IsParallelTo(line2))
                {
                    Vector lineBetween = new Vector(line1.BasePoint, line2.BasePoint);
                    normal = new Vector(line1, new Distance(1, Inches)).CrossProduct(lineBetween);
                }
                //if they are coplanar and not parallel we can just cross them
                else if (line1.IsCoplanarWith(line2))
                {
                    normal = new Vector(line1, new Distance(1, Inches)).CrossProduct(new Vector(line2, new Distance(1, Inches)));
                }
                else
                {
                    throw new Exception("The passed lines are not on the same plane.");
                }
                this.NormalLine = new Line(line1.BasePoint, normal.Direction);
            }
            else
            {
                throw new ArgumentException("The passed Lines are the same!");
            }
        }

        /// <summary>
        /// Creates a Plane that contains the passed points, using the first one as the base point
        /// Note: Points should not be all along the same line
        /// </summary>
        public Plane(Point point1, Point point2, Point point3 )
        {
            //If all 3 points are noncollinear, they define a plane

            if (point1 == point2 || point1 == point3 || point2 == point3)
            {
                throw new Exception("The passed points are too close to accurately determine a plane.");
            }

            Vector vector1 = new Vector(point1, point2);
            Vector vector2 = new Vector(point1, point3);

            if (point2.IsOnLine(vector2))
            {
                throw new Exception("The passed points all fall on a common line.");
            } 
            this.NormalLine = new Line(point1, vector1.CrossProduct(vector2).Direction);
        }

        /// <summary>
        /// Copies the given plane
        /// </summary>
        /// <param name="passedPlane">The Plane to copy</param>
        public Plane(Plane toCopy)
        {
            this.NormalLine = toCopy.NormalLine;
        }

        #endregion

        #region Overloaded Operators


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Plane plane1, Plane plane2)
        {
            if ((object)plane1 == null)
            {
                if ((object)plane2 == null)
                {
                    return true;
                }
                return false;
            }
            return plane1.Equals(plane2);
        }

        public static bool operator !=(Plane plane1, Plane plane2)
        {
            if ((object)plane1 == null)
            {
                if ((object)plane2 == null)
                {
                    return false;
                }
                return true;
            }
            return !plane1.Equals(plane2);
        }

        public override bool Equals(object obj)
        {
            //make sure the passed obj isnt null
            if (obj == null || !(obj is Plane))
            {
                return false;
            }
            Plane comparablePlane = (Plane)obj;

            bool checkPoint = this.Contains(comparablePlane.BasePoint);
            bool checkVector = this.NormalVector.IsParallelTo(comparablePlane.NormalVector);

            return checkPoint && checkVector;
         }

        #endregion

        #region Methods

        public bool Contains(Polygon polygon)
        {
            return this == new Plane(polygon);
        }

        public bool Contains(Line line)
        {
            return (this.Contains(line.BasePoint) && NormalVector.IsPerpendicularTo(line));
        }

        public bool Contains(Vector vector)
        {
            return this.Contains(vector.BasePoint) && this.Contains(vector.EndPoint);
        }

        /// <summary>
        /// Returns whether or not the Point passed in is in this Plane
        /// </summary>
        /// <param name="passedPoint">Point to see if the Plane contains</param>
        /// <returns>returns true if the Point is in the Plane and false otherwise</returns>
        public bool Contains(Point passedPoint)
        {
            if (passedPoint == null)
            {
                return false;
            }
            return passedPoint.DistanceTo(this) == Distance.ZeroDistance;
        }

        public Plane Translate(Point translation)
        {
            Vector newNormalVector = this.NormalVector.Translate(translation);
            return new Plane(newNormalVector);
        }

        /// <summary>
        /// Rotates the Plane with the given rotation
        /// </summary>
        public Plane Rotate(Rotation rotationToApply)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(rotationToApply);
            Vector newNormalVector = this.NormalVector.Rotate(rotationToApply);
            return new Plane(newNormalVector.Direction, newBasePoint);
        }

        /// <summary>
        /// Shifts the plane region with the given shift
        /// </summary>
        /// <param name="shiftToApply">The shift to apply to this plane</param>
        /// <returns>Returns a new object of Plane that has been shifted</returns>
        public Plane Shift(Shift shiftToApply)
        {
            Point newBasePoint = this.BasePoint.Shift(shiftToApply);
            Vector newNormalVector = this.NormalVector.Shift(shiftToApply);
            return new Plane(newNormalVector.Direction, newBasePoint);
        }

        /// <summary>
        /// This function returns true if both the points are on the same side of this plane. 
        /// If either point is on the plane it will return false
        /// </summary>
        /// <param name="testPoint">The point to see if it is on the same side of the plane as the reference point</param>
        /// <param name="referencePoint">The point on the side you want to check if the test point is on</param>
        /// <returns>returns true if both points are on the same side of the plane or false if they are not. 
        /// If either point is on the plane it always returns false</returns>
        public bool PointIsOnSameSideAs(Point testPoint, Point referencePoint)
        {
            //as stated by esun at http://forums.anandtech.com/showthread.php?t=162930
            //if the dot product between the point and the normal vector is positive, it is on the side the normal faces
            //if it is negative than the point is on the opposite side of the normal
            //if it is 0 than it is on the plane

            if (testPoint == null || referencePoint == null)
            {
                return false;
            }
            if (this.Contains(testPoint) || this.Contains(referencePoint))
            {
                return false;
            }
            //so find the dot products between the points and the normal of the plane
            Area testDot = new Vector(this.BasePoint, testPoint).DotProduct(this.NormalVector);
            Area referenceDot = new Vector(this.BasePoint, referencePoint).DotProduct(this.NormalVector);

            //if they are both either positive or negative than they are both on the same side
            if ((testDot < Area.Zero && referenceDot < Area.Zero) || (testDot > Area.Zero && referenceDot > Area.Zero))
            {
                return true;
            }

            //if they are on opposite sides of the plane or either of the points are on the plane than we will return false
            return false;
        }

        public bool PointIsOnNormalSide(Point point)
        {
            var dotProduct = this.NormalVector.DotProduct(new Vector(this.BasePoint, point));
            return dotProduct != Area.Zero && dotProduct > Area.Zero;

        }

        /// <summary>
        /// Finds the line where the two planes intersect 
        /// </summary>
        public virtual Line IntersectWithPlane(Plane otherPlane)
        {
            if (this == otherPlane)
            {
                if (BasePoint != otherPlane.BasePoint)
                {
                    return new Line(BasePoint, otherPlane.BasePoint);
                }

                return this.GetLineOnThisPlane();

            }
            if (this.NormalVector.HasSameDirectionAs(otherPlane.NormalVector))
            {
                return null;
            }
            
            Vector intersectionLineDirection = this.NormalVector.CrossProduct(otherPlane.NormalVector);
            
            Line normalInPlane1 = new Line(this.BasePoint, this.NormalVector.CrossProduct(intersectionLineDirection));
            Point basePoint = normalInPlane1.IntersectWithPlane(otherPlane);

            return new Line(basePoint, intersectionLineDirection);
        }

        public Line GetLineOnThisPlane()
        {
            Vector vector1 = this.NormalVector.CrossProduct(Line.XAxis.UnitVector(new Inch()));
            Vector vector2 = this.NormalVector.CrossProduct(Line.YAxis.UnitVector(new Inch()));
            Vector vector3 = this.NormalVector.CrossProduct(Line.ZAxis.UnitVector(new Inch()));
            Vector chosen = new List<Vector>() { vector1, vector2, vector3 }.MaxBy(v => v.Magnitude);
            
            //vectors 1,2,&3 all will be parallel to the plane, but the most precise will be the one with the largest magnitude.
            return new Line(this.BasePoint, chosen);
        }

        /// <summary>
        /// Returns the intersection point between a line and the plane
        /// </summary>
        public virtual Point IntersectWithLine(Line line)
        {
            //Test if the plane contains the line's basePoint
            //This handles the case where the line is inside the plane
            if (this.Contains(line.BasePoint))
            {
                return line.BasePoint;
            }
            if (this.IsParallelTo(line))
            {
                return null;
            }

            var projected = line.BasePoint.ProjectOntoPlane(this);

            Vector toPlane = new Vector(line.BasePoint, projected);
            var cosineOfAngle = toPlane.Direction.DotProduct(line.Direction);
            var distance = toPlane.Magnitude / cosineOfAngle;

            return line.GetPointAlongLine(distance);
        }

        public virtual Point IntersectWithSegment(LineSegment segment)
        {
            Point possibleIntersection = this.IntersectWithLine(segment);
            if (possibleIntersection != null && possibleIntersection.IsOnLineSegment(segment))
            {
                return possibleIntersection;
            }
            return null;
        }

        ///// <summary>
        ///// Returns whether or not the two planes intersect
        ///// </summary>
        ///// <param name="passedPlane"></param>
        ///// <returns></returns>
        //public virtual bool DoesIntersectNotCoplanar(Plane passedPlane)
        //{
        //    return !IsParallelTo(passedPlane);
        //}

        ///// <summary>
        ///// Returns whether or not the polygon and plane intersect
        ///// </summary>
        ///// <param name="passedPolygon"></param>
        ///// <returns></returns>
        //public virtual bool DoesIntersectNotCoplanar(Polygon passedPolygon)
        //{
        //    if (!DoesIntersectNotCoplanar((Plane)passedPolygon))
        //    {
        //        return false;
        //    }

        //    foreach (LineSegment segment in passedPolygon.LineSegments)
        //    {
        //        if (this.DoesIntersectNotCoplanar(segment))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
 
        /// <summary>
        /// Returns whether or not the plane and line intersect, including if the line is on the plane
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public bool Intersects(Line passedLine)
        {
            Point intersection = this.IntersectWithLine(passedLine);
            return (intersection != null);
        }

        /// <summary>
        /// Returns whether or not the given vector intersects the plane
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public virtual bool Intersects(LineSegment passedVector)
        {
            var intersection = new Line(passedVector).IntersectWithPlane(this);
            bool segmentContainsPoint = passedVector.Contains(intersection);
            return segmentContainsPoint;
        }

        /// <summary>
        /// Returns whether or not the two planes intersect
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public bool Intersects(Plane passedPlane)
        {
            return !this.IsParallelTo(passedPlane) || this.IsCoplanarTo(passedPlane);
        }

        /// <summary>
        /// Returns whether or not the polygon and plane intersect
        /// </summary>
        public bool Intersects(Polygon polygon)
        {
            Line slicingLine = polygon.IntersectingSegment(this);
            return (slicingLine != null && polygon.DoesIntersect(slicingLine));
        }

        /// <summary>
        /// Returns whether or not the giving plane is parallel to this plane
        /// </summary>
        public bool IsParallelTo(Plane passedPlane)
        {
            return this.NormalVector.IsParallelTo(passedPlane.NormalVector);
        }

        /// <summary>
        /// Returns whether or not the giving Plane is perpendicular to this plane
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsPerpendicularTo(Plane passedPlane)
        {
            return this.NormalVector.IsPerpendicularTo(passedPlane.NormalVector);
        }

        /// <summary>
        /// Returns whether or not the giving line is parallel to this plane
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsParallelTo(Line passedLine)
        {
            //check to see if it is perpendicular to the normal vector and if it is then it is parallel to the plane because the plane is
            //by definition perpendicular to the normal
            return this.NormalDirection.IsPerpendicularTo(passedLine.Direction);
        }
        
        /// <summary>
        /// Returns whether or not the giving line is perpendicular to this plane
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsPerpendicularTo(Line passedLine)
        {
            //check to see if it is parallel to the normal vector and if it is then it is perpendicular to the plane because the plane is
            //by definition perpendicular to the normal
            return this.NormalVector.IsParallelTo(passedLine);
        }

        /// <summary>
        /// Returns whether or not the two plans are coplanar (parallel and share points)
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public bool IsCoplanarTo(Plane passedPlane)
        {
            return IsParallelTo(passedPlane) && this.Contains(passedPlane.BasePoint);
        }

        /// <summary>
        /// returns the smallest angle between planes.
        /// (if they're parallel returns 0, otherwise the planes intersect and make 4 angles)
        /// (returns the smallest of those);
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public Angle SmallestAngleBetween(Plane passedPlane)
        {
            Vector normal1 = this.NormalVector;
            Vector normal2 = passedPlane.NormalVector;

            Angle angle1 = normal1.AngleBetween(normal2);
            Angle angle2 = normal1.AngleBetween(normal2.Reverse());
            
            if (angle1 < angle2)
            {
                return angle1;
            }
            else
            {
                return angle2;
            }
        }

        public virtual Angle SmallestAngleBetween(Line line)
        {
            var angle =  this.NormalVector.SmallestAngleBetween(line);
            var complement = Angle.RightAngle - angle;
            return complement.ProperAngle;
        }

        public Vector NormalVectorThrough(Point point)
        {
            var basePoint = point.ProjectOntoPlane(this);
            return new Vector(basePoint, point);
        }

        public bool CutsAcross(Polyhedron solid)
        {
            return solid.Slice(this).Count == 2;
        }

        ISurface ISurface.Shift(Shift shift)
        {
            return this.Shift(shift);
        }
        #endregion
    }
}
