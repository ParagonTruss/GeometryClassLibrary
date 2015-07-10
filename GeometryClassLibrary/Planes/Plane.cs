using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane is an unbounded flat surface
    /// </summary>
    public class Plane
    {
        #region Properties and Fields

        public readonly static Plane XY = new Plane(Line.XAxis, Line.YAxis);
        public readonly static Plane XZ = new Plane(Line.XAxis, Line.ZAxis);
        public readonly static Plane YZ = new Plane(Line.YAxis, Line.ZAxis);

        /// <summary>
        /// A point on the plane that is used as a reference point to define it
        /// </summary>
        public Point BasePoint { get; protected set; }

        /// <summary>
        /// The vector that is normal to the plane, with the base point of the vector always
        /// being the same as the basepoint of the plane
        /// </summary>
        private Vector _normalVector;
        public virtual Vector NormalVector
        {
            get { return _normalVector; }
            protected set
            {
                //make sure that the Normal's base point is always the same as the planes base point for convenience
                _normalVector = new Vector(this.BasePoint, value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Plane that contains the given point and has a normalVector of length 1 in inches in the passed in direction
        /// </summary>
        /// <param name="passedBasePoint">A point on the plane to be used as the reference point</param>
        /// <param name="passedNormalDirection">A direction that is normal to the plane</param>
        public Plane(Direction passedNormalDirection = null, Point passedBasePoint = null)
        {
            if (passedBasePoint == null)
            {
                passedBasePoint = new Point();
            }

            if (passedNormalDirection == null)
            {
                passedNormalDirection = Direction.Right;
            }

            this.BasePoint = passedBasePoint;
            this.NormalVector = new Vector(this.BasePoint, passedNormalDirection, new Distance(DistanceType.Inch, 1));
        }

        public Plane(Vector normal) : this(normal.Direction, normal.BasePoint) { }

        /// <summary>
        /// Creates a plane that contains the two lines (they must not be equivalent Lines!)
        /// </summary>
        /// <param name="passedLine1">The first line to define the plane with</param>
        /// <param name="passedLine2">The second line to define the plane with that is not the same as the first</param>
        public Plane(Line passedLine1, Line passedLine2)
        {
            //if they arent equivalent lines
            if (passedLine1 != passedLine2)
            {
                //if they are parallel we must find the line between to use to find the normal or else the cross product is 0
                if (passedLine1.IsParallelTo(passedLine2))
                {
                    this.BasePoint = passedLine1.BasePoint;

                    Vector lineBetween = new Vector(passedLine1.BasePoint, passedLine2.BasePoint);
                    this.NormalVector = passedLine1.UnitVector(DistanceType.Inch).CrossProduct(lineBetween.UnitVector(DistanceType.Inch));
                }
                //if they are coplanar and not parallel we can just cross them
                else if (passedLine1.IsCoplanarWith(passedLine2))
                {
                    this.BasePoint = passedLine1.BasePoint;
                    this.NormalVector = passedLine1.UnitVector(DistanceType.Inch).CrossProduct(passedLine2.UnitVector(DistanceType.Inch));
                }
                else
                {
                    throw new NotSupportedException("Those 3 points are not on the same plane");
                }
            }
            else
            {
                //they are the same line and we cant make a plane
                throw new ArgumentException("The passed Lines are the same!");
            }
        }

        /// <summary>
        /// Creates a Plane that contains the passed points, using the first one as the base point
        /// Note: points should not be all along the same line
        /// </summary>
        /// <param name="passedPoint1">The first point contained on the plane and to be used as the reference point</param>
        /// <param name="passedPoint2">The second point contained on the plane</param>
        /// <param name="passedPoint3">The third point contained on the plane</param>
        public Plane(Point passedPoint1, Point passedPoint2, Point passedPoint3 )
        {
            //If all 3 points are noncollinear, they define a plane
            
            Line line1To2 = new Line(passedPoint1,passedPoint2);
            Line line2To3 = new Line(passedPoint2,passedPoint3);
            Line line1To3 = new Line(passedPoint1,passedPoint3);

            if(!passedPoint1.IsOnLine(line2To3) && !passedPoint2.IsOnLine(line1To3) && !passedPoint3.IsOnLine(line1To2))
            {
                this.BasePoint = passedPoint1;
                this.NormalVector = line1To2.UnitVector(DistanceType.Inch).CrossProduct(line1To3.UnitVector(DistanceType.Inch));

                //make sure it is of size one and not smaller
                this.NormalVector = this.NormalVector.UnitVector(DistanceType.Inch);
            }
            else
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// Copies the given plane
        /// </summary>
        /// <param name="passedPlane">The Plane to copy</param>
        public Plane(Plane toCopy)
        {
            this.BasePoint = new Point(toCopy.BasePoint);
            this.NormalVector = new Vector(toCopy.NormalVector);
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
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Plane, if it fails then we know the user passed in the wrong type of object
            try
            {
                Plane comparablePlane = (Plane)obj;

                bool checkPoint = this.Contains(comparablePlane.BasePoint);
                bool checkVector = this.NormalVector.IsParallelTo(comparablePlane.NormalVector);

                return checkPoint && checkVector;
            }
            //if its not a plane than its not equal
            catch
            {
                return false;
            }
        }

        #endregion

        #region Methods

        public bool Contains(Polygon polygon)
        {
            //// checks to make sure that every linesegment is on the plane
            //foreach (LineSegment segment in polygon.LineSegments)
            //{
            //    if (!this.Contains(segment))
            //    {
            //        return false;
            //    }
            //}

            //return true;
            return this == (Plane)polygon;
        }

        public bool Contains(Line passedLine)
        {
            return (this.Contains(passedLine.BasePoint) && NormalVector.IsPerpendicularTo(passedLine));
        }

        /// <summary>
        /// Returns whether or not the Point passed in is in this Plane
        /// </summary>
        /// <param name="passedPoint">Point to see if the Plane contains</param>
        /// <returns>returns true if the Point is in the Plane and false otherwise</returns>
        public bool Contains(Point passedPoint)
        {
            if (BasePoint == passedPoint)
            {
                return true;
            }
            Vector planeVector = new Vector(passedPoint, BasePoint);
            //Distance dotProduct = planeVector * NormalVector;
            //return dotProduct == new Distance();

            return planeVector.IsPerpendicularTo(NormalVector);
        }

        /// <summary>
        /// Rotates the Plane with the given rotation
        /// </summary>
        /// <param name="rotationToApply">The Rotation(that stores the axis to rotate around and the angle to rotate) to apply to the Plane</param>
        /// <returns></returns>
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

            //so find the dot products between the points and the normal of the plane
            Distance testDot = new Vector(this.BasePoint, testPoint) * this.NormalVector;
            Distance referenceDot = new Vector(this.BasePoint, referencePoint) * this.NormalVector;

            //if they are both either positive or negative than they are both on the same side
            if ((testDot < new Distance() && referenceDot < new Distance()) || (testDot > new Distance() && referenceDot > new Distance()))
            {
                return true;
            }

            //if they are on opposite sides of the plane or either of the points are on the plane than we will return false
            return false;
        }

        /// <summary>
        /// Finds the line where the two planes intersect 
        /// Note: converts the types to inches Internally so some percision might be lost
        /// </summary>
        /// <param name="otherPlane">the other plane to see where it intersects with this plane</param>
        /// <returns>returns the Line of intersection between the planes or null if they do not intersect</returns>
        public virtual Line Intersection(Plane otherPlane)
        {
            //We make them all doubles because we need to ignore the dimesnions for this calculation to work correctly and then restore them
            //at the end, so we choose inches to use as the base to convert to

            if (this == otherPlane)
            {
                if (BasePoint != otherPlane.BasePoint)
                {
                    return new Line(BasePoint, otherPlane.BasePoint);
                }
                Vector lineDirection = Direction.Right.UnitVector(DistanceType.Inch).CrossProduct(this.NormalVector);
                if (lineDirection.Magnitude == new Distance())
                {
                    lineDirection = Direction.Out.UnitVector(DistanceType.Inch).CrossProduct(this.NormalVector);
                }
                return new Line(lineDirection.Direction, this.BasePoint);

            }

            //Explains how to find where planes intersect: http://jacobi.math.wvu.edu/~hjlai/Teaching/Tip-Pdf/Tip3-10.pdf
            //if we have the normals we can cross them to find direction vector of the intersection
            Vector intersectionLineDirection = this.NormalVector.CrossProduct(otherPlane.NormalVector);

            //if it is 0 than thay are parallel and dont intersect
            if (intersectionLineDirection != new Vector())
            {
                //put them in plane notation  adn find what they equal (normal.X * X + normal.Y * Y + normal.Z * Z = planeEqualsValue)
                double thisPlaneEqualsValue = this.NormalVector.XComponent.Inches * this.BasePoint.X.Inches + this.NormalVector.YComponent.Inches * this.BasePoint.Y.Inches +
                    this.NormalVector.ZComponent.Inches * this.BasePoint.Z.Inches;

                double otherPlaneEqualsValue = otherPlane.NormalVector.XComponent.Inches * otherPlane.BasePoint.X.Inches +
                    otherPlane.NormalVector.YComponent.Inches * otherPlane.BasePoint.Y.Inches + otherPlane.NormalVector.ZComponent.Inches * otherPlane.BasePoint.Z.Inches;

                //pull them all out as the same thing because we need to know what Distance we are dealing with
                double thisFirstVariable = this.NormalVector.XComponent.Inches;
                double thisSecondVariable = this.NormalVector.YComponent.Inches;
                double otherFirstVariable = otherPlane.NormalVector.XComponent.Inches;
                double otherSecondVariable = otherPlane.NormalVector.YComponent.Inches;

                //first we play a game to find an axis it will always cross (default is to use z first then y, then x)
                if (intersectionLineDirection.ZComponent == new Distance())
                {
                    //try y
                    if (intersectionLineDirection.YComponent != new Distance())
                    {
                        //make the y the thirdVariable(the one we make 0) and z the second 
                        thisSecondVariable = this.NormalVector.ZComponent.Inches;
                        otherSecondVariable = otherPlane.NormalVector.ZComponent.Inches;
                    }
                    //if not y or z than it must be x because we know it is non zero at this point
                    else
                    {
                        //make x the third variable(the one make 0) and z the first
                        thisFirstVariable = this.NormalVector.ZComponent.Inches;
                        otherFirstVariable = otherPlane.NormalVector.ZComponent.Inches;
                    }
                }

                //make them into a matrix and then solve it
                Matrix equations = new Matrix(2, 2);
                equations.SetElement(0, 0, thisFirstVariable);
                equations.SetElement(0, 1, thisSecondVariable);
                equations.SetElement(1, 0, otherFirstVariable);
                equations.SetElement(1, 1, otherSecondVariable);

                double[] equationEquals = new double[] {thisPlaneEqualsValue, otherPlaneEqualsValue};

                double[] results = equations.SystemSolve(equationEquals);
                

                //now assign them to the point in the way the need to be (assume z again)
                Point intersectLinePoint = PointGenerator.MakePointWithInches(results[0], results[1], 0);
                if (intersectionLineDirection.ZComponent == new Distance())
                {
                    //if y was what we made zero then it was third and z was second
                    if (intersectionLineDirection.YComponent != new Distance())
                    {
                        intersectLinePoint = PointGenerator.MakePointWithInches(results[0], 0, results[1]);
                    }
                    //if x is what we made 0 than it was third and z was first
                    else
                    {
                        intersectLinePoint = PointGenerator.MakePointWithInches(0, results[1], results[0]);
                    }
                }
                return new Line(intersectionLineDirection.Direction, intersectLinePoint);
            }

            return null;
        }

        /// <summary>
        /// Returns the intersection point between a line and the plane
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual Point Intersection(Line passedLine)
        {
            //Test if the plane contains the line's basePoint
            //This handles the case where the line is inside the plane
            if (this.Contains(passedLine.BasePoint))
            {
                return passedLine.BasePoint;
            }

            //Following formula from http://www.netcomuk.co.uk/~jenolive/vect18c.html

            //substitute the x,y and z part of the lines into the plane equation
            //find what the plane is equal to
            Distance thisPlaneEqualsValue = this.NormalVector.DotProduct(new Vector(this.BasePoint));

            //find t's coefficent
            Distance tCoefficient = this.NormalVector.DotProduct(passedLine.Direction.UnitVector(DistanceType.Inch));

            //find the part it equals from the line
            Distance equationEquals = this.NormalVector.DotProduct(new Vector(passedLine.BasePoint));

            //subtract the one from the line from the one from the plane
            Distance equals = thisPlaneEqualsValue - equationEquals;

            //now get the value for t
            double t = equals / tCoefficient;

            if (t == double.NaN)
            {
                return null;
            }
            //now just plug it back into the line equations to find the x,y amd z
            //Distance xComponent = this.BasePoint.X + new Distance(DistanceType.Inch, this.Direction.XComponentOfDirection * t);
            //Distance yComponent = this.BasePoint.Y + new Distance(DistanceType.Inch, this.Direction.YComponentOfDirection * t);
            //Distance zComponent = this.BasePoint.Z + new Distance(DistanceType.Inch, this.Direction.ZComponentOfDirection * t);

            //Point intersectionPoint = new Point(xComponent, yComponent, zComponent);
            Point intersectionPoint = passedLine.GetPointAlongLine(new Distance(DistanceType.Inch, t));

            if ((intersectionPoint.DistanceTo(this.BasePoint)).DistanceInIntrinsicUnitsIsGreaterThan(1000))
            {
                return null;
            }

            return intersectionPoint;
        }

        public Point Intersection(LineSegment segment)
        {
            Point possibleIntersection = this.Intersection((Line)segment);
            if (possibleIntersection != null && possibleIntersection.IsOnLineSegment(segment))
            {
                return possibleIntersection;
            }
            return null;
        }

        /// <summary>
        ///returns true if the line and the plane intersect, exlcuding if the line is on the plane
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual bool DoesIntersectNotCoplanar(Line passedLine)
        {
            return Intersection(passedLine) != null;
        }
        
        /// <summary>
        ///returns true if the vector and the plane intersect, exlcuding if the line is on the plane
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual bool DoesIntersectNotCoplanar(Vector passedVector)
        {
            if (!DoesIntersectNotCoplanar((Line)passedVector))
            {
                return false;
            }

            Point intersect = this.Intersection(passedVector);

            return intersect != null && intersect.IsOnVector(passedVector);
        }

        /// <summary>
        /// Returns whether or not the two planes intersect
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual bool DoesIntersectNotCoplanar(Plane passedPlane)
        {
            return !IsParallelTo(passedPlane);
        }

        /// <summary>
        /// Returns whether or not the polygon and plane intersect
        /// </summary>
        /// <param name="passedPolygon"></param>
        /// <returns></returns>
        public virtual bool DoesIntersectNotCoplanar(Polygon passedPolygon)
        {
            if (!DoesIntersectNotCoplanar((Plane)passedPolygon))
            {
                return false;
            }

            foreach (LineSegment segment in passedPolygon.LineSegments)
            {
                if (this.DoesIntersectNotCoplanar(segment))
                {
                    return true;
                }
            }

            return false;
        }
 
        /// <summary>
        /// Returns whether or not the plane and line intersect, including if the line is on the plane
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Line passedLine)
        {
            Vector lineVector = passedLine.Direction.UnitVector(DistanceType.Inch);
            Vector planeNormal = this.NormalVector;
            return (lineVector.IsPerpendicularTo(planeNormal));
        }

        /// <summary>
        /// Returns whether or not the given vector intersects the plane
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Vector passedVector)
        {
            if (this.Contains(passedVector.BasePoint) || this.Contains(passedVector.EndPoint))
            {
                return true;
            }
            if (this.PointIsOnSameSideAs(passedVector.BasePoint, passedVector.EndPoint))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether or not the two planes intersect
        /// </summary>
        /// <param name="passedPlane"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Plane passedPlane)
        {
            return this.DoesIntersectNotCoplanar(passedPlane) || this.IsCoplanarTo(passedPlane);
        }

        /// <summary>
        /// Returns whether or not the polygon and plane intersect
        /// </summary>
        /// <param name="passedPolygon"></param>
        /// <returns></returns>
        public virtual bool DoesIntersect(Polygon passedPolygon)
        {
            Line slicingLine = passedPolygon.Intersection(this);
            return (slicingLine != null && passedPolygon.DoesIntersect(slicingLine));
        }

        /// <summary>
        /// Returns whether or not the giving plane is parallel to this plane
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsParallelTo(Plane passedPlane)
        {
            return (this.NormalVector.CrossProduct((passedPlane.NormalVector)).Magnitude == new Distance());
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
            return this.NormalVector.IsPerpendicularTo(passedLine);
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

        public Vector NormalVectorThrough(Point point)
        {
            var basePoint = point.ProjectOntoPlane(this);
            return new Vector(basePoint, point);
        }
        #endregion
    }
}
