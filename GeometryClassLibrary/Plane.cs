﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane is an unbounded flat surface
    /// </summary>
    [DebuggerDisplay("BasePoint = {BasePoint.X.Inches}, {BasePoint.Y.Inches}, {BasePoint.Z.Inches}, Normal Vector: Direction Vector {NormalVector.XComponentOfDirection.Inches}, {NormalVector.YComponentOfDirection.Inches}, {NormalVector.ZComponentOfDirection.Inches}, Magnitude = {Magnitude.Inches}")]
    [Serializable]
    public class Plane
    {
        #region Properties and Fields

        /// <summary>
        /// A point on the plane that is used as a reference point to define it
        /// </summary>
        public Point BasePoint { get; protected set; }

        /// <summary>
        /// The vector that is normal to the plane, with the base point of the vector always
        /// being the same as the basepoint of the plane
        /// </summary>
        private Vector _normalVector;
        public Vector NormalVector
        {
            get { return _normalVector; }
            protected set
            {
                //make sure that the Normal's base point is always the same as the planes base point for convinience
                _normalVector = value;
                _normalVector.BasePoint = this.BasePoint;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Zero constructor
        /// </summary>
        public Plane()
        {
            this.BasePoint = new Point();
            this.NormalVector = new Vector();
        }

        /// <summary>
        /// Creates a Plane base on the list of passed in lines
        /// Note: the lines must be all coplanar
        /// </summary>
        /// <param name="passedLineList">A list of coplanar lines to define the plane with</param>
        public Plane(IList<Line> passedLineList)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLineList);

            if (passedLineListCasted.AreAllCoplanar())
            {
                this.BasePoint = passedLineListCasted[0].BasePoint;

                //we have to check against vectors until we find one that is not parralel with the first line we passed in
                //or else the normal vector will be zero (cross product of parralel lines is 0)
                Vector vector1 = passedLineListCasted[0].UnitVector(DimensionType.Inch);
                for (int i = 1; i < passedLineListCasted.Count; i++)
                {
                    this.NormalVector = vector1.CrossProduct(passedLineListCasted[i].UnitVector(DimensionType.Inch));
                    if (!this.NormalVector.Equals(new Vector()))
                        i = passedLineListCasted.Count;
                }
            }
            else
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// Creates a Plane that contains the given point and has a normalVector of length 1 in inches in the passed in direction
        /// </summary>
        /// <param name="passedBasePoint">A point on the plane to be used as the reference point</param>
        /// <param name="passedDirection">A direction that is normal to the plane</param>
        public Plane(Direction passedDirection, Point passedBasePoint = null)
        {
            if (passedBasePoint == null)
            {
                this.BasePoint = new Point();
            }
            else
            {
                this.BasePoint = passedBasePoint;
            }

            this.NormalVector = new Vector(this.BasePoint, passedDirection, new Dimension(DimensionType.Inch, 1));
        }

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
                    this.NormalVector = passedLine1.UnitVector(DimensionType.Inch).CrossProduct(lineBetween.UnitVector(DimensionType.Inch));
                }
                //if they are coplanar and not parallel we can just cross them
                else if (passedLine1.IsCoplanarWith(passedLine2))
                {
                    this.BasePoint = passedLine1.BasePoint;
                    this.NormalVector = passedLine1.UnitVector(DimensionType.Inch).CrossProduct(passedLine2.UnitVector(DimensionType.Inch));
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
        /// Note: points must not bwe all along the same line
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
                this.NormalVector = line1To2.UnitVector(DimensionType.Inch).CrossProduct(line1To3.UnitVector(DimensionType.Inch));

                //make sure it is of size one and not smaller
                this.NormalVector = this.NormalVector.UnitVector(DimensionType.Inch);
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

            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                Plane comparablePlane = (Plane)obj;

                bool checkPoint = this.Contains(comparablePlane.BasePoint);
                bool checkVector = this.NormalVector.PointInSameOrOppositeDirections(comparablePlane.NormalVector);

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

        /// <summary>
        /// Determines whether or not this plane contains the given polygon
        /// </summary>
        /// <param name="passedPlaneRegion">The polygon to see if it is contained</param>
        /// <returns>Returns true if the polygon is contained and false otherwise</returns>
        public bool Contains(Polygon passedPlaneRegion)
        {
            // checks to make sure that every line is on the line segment
            foreach (LineSegment segment in passedPlaneRegion.PlaneBoundaries)
            {
                if (!this.Contains(segment))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether or not this plane contains the given Line
        /// </summary>
        /// <param name="passedLine">The Line to see if it is contained</param>
        /// <returns>Returns true if the Line is contained and false otherwise</returns>
        public bool Contains(Line passedLine)
        {
            // if both of the vectors' dotproducts come out to 0, the line is on the plane
            return (this.IsParallelTo(passedLine) && this.Contains(passedLine.BasePoint));
        }

        /// <summary>
        /// Returns wether or not the Point passed in is in this Plane
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
            //Dimension dotProduct = planeVector * NormalVector;
            //return dotProduct == new Dimension();

            return planeVector.DotProductIsEqualToZero(NormalVector);
        }

        /// <summary>
        /// Rotates the plane with the given rotation
        /// </summary>
        /// <param name="passedRotation">The rotation object that is to be applied to the plane</param>
        /// <returns>A new plane that has been rotated</returns>
        public Plane Rotate(Rotation passedRotation)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(passedRotation);
            Vector newNormalVector = this.NormalVector.Rotate(passedRotation);
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

            //so find the dot products between the points and the normal of the plane
            Dimension testDot = new Vector(this.BasePoint, testPoint) * this.NormalVector;
            Dimension referenceDot = new Vector(this.BasePoint, referencePoint) * this.NormalVector;

            //if they are both either positive or negative than they are both on the same side
            if ((testDot < new Dimension() && referenceDot < new Dimension()) || (testDot > new Dimension() && referenceDot > new Dimension()))
            {
                return true;
            }

            //if they are on opposite sides of the plane or either of the points are on the plan than we will return false
            return false;
        }

        /// <summary>
        /// Finds the line where the two planes intersect 
        /// Note: covnerts the types to inches Internally so some percision might be lost
        /// </summary>
        /// <param name="otherPlane">the other plane to see where it intersects with this plane</param>
        /// <returns>returns the Line of intersection between the planes or null if they do not intersect</returns>
        public virtual Line Intersection(Plane otherPlane)
        {
            //We make them all doubles because we need to ignore the dimesnions for this calculation to work correctly and then restore them
            //at the end, so we choose inches to use as the base to convert to

            //Explains how to find where planes intersect: http://jacobi.math.wvu.edu/~hjlai/Teaching/Tip-Pdf/Tip3-10.pdf
            //if we have the normals we can cross them to find direction vector of the intersection
            Vector intersectionLineDirection = this.NormalVector.CrossProduct(otherPlane.NormalVector);

            //if it is 0 than thay are parallel and dont intersect
            if (intersectionLineDirection != new Vector())
            {
                //put them in plane notation  adn find what they equal (normal.X * X + normal.Y * Y + normal.Z * Z = planeEqualsValue)
                double thisPlaneEqualsValue = this.NormalVector.XComponent.Inches * this.BasePoint.X.Inches + this.NormalVector.YComponent.Inches * this.BasePoint.Y.Inches +
                    this.NormalVector.ZComponent.Inches * this.BasePoint.Z.Inches;
                //double thisPlaneEqualsAsInches = thisPlaneEqualsValue;

                double otherPlaneEqualsValue = otherPlane.NormalVector.XComponent.Inches * otherPlane.BasePoint.X.Inches +
                    otherPlane.NormalVector.YComponent.Inches * otherPlane.BasePoint.Y.Inches + otherPlane.NormalVector.ZComponent.Inches * otherPlane.BasePoint.Z.Inches;
                //double otherPlaneEqualsAsInches = otherPlaneEqualsValue;

                //pull them all out as the same thing because we need to know what dimension we are dealing with
                double thisFirstVariable = this.NormalVector.XComponent.Inches;
                double thisSecondVariable = this.NormalVector.YComponent.Inches;
                double otherFirstVariable = otherPlane.NormalVector.XComponent.Inches;
                double otherSecondVariable = otherPlane.NormalVector.YComponent.Inches;

                //first we play a game to find an axis it will always cross (default is to use z first then y, then x)
                if (intersectionLineDirection.ZComponent == new Dimension())
                {
                    //try y
                    if (intersectionLineDirection.YComponent != new Dimension())
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
                if (intersectionLineDirection.ZComponent == new Dimension())
                {
                    //if y was what we made zero then it was third and z was second
                    if (intersectionLineDirection.YComponent != new Dimension())
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
        /// <param name="passedLine">The Line to find where it intersect this plane at</param>
        /// <returns>Returns the point at which the line intersects this plane</returns>
        public virtual Point Intersection(Line passedLine)
        {
            //if they are parallel than we know that they do not intersect in a point
            if (this.IsParallelTo(passedLine))
            {
                return null;
            }
            //otherwise they should

            //Following formula from http://www.netcomuk.co.uk/~jenolive/vect18c.html

            //substitute the x,y and z part of the lines into the plane equation
            //find what the plane is equal to
            double thisPlaneEqualsValue = this.NormalVector.XComponent.Inches * this.BasePoint.X.Inches + this.NormalVector.YComponent.Inches * this.BasePoint.Y.Inches +
                this.NormalVector.ZComponent.Inches * this.BasePoint.Z.Inches;

            //find t's coefficent
            Dimension tCoefficient = this.NormalVector.XComponent * passedLine.Direction.XComponentOfDirection +
                this.NormalVector.YComponent * passedLine.Direction.YComponentOfDirection + this.NormalVector.ZComponent * passedLine.Direction.ZComponentOfDirection;

            //find the part it equals ffom the line
            double equationEquals = this.NormalVector.XComponent.Inches * passedLine.BasePoint.X.Inches +
                this.NormalVector.YComponent.Inches * passedLine.BasePoint.Y.Inches + this.NormalVector.ZComponent.Inches * passedLine.BasePoint.Z.Inches;

            //subtract the one from the line from the one from the plane
            double equals = thisPlaneEqualsValue - equationEquals;

            //to keep it from dividing by zero, which means there is not intersection in this case and it should be caught by cheking
            //if they are parallel, but just in case
            if (tCoefficient == new Dimension())
            {
                return null;
            }

            //now get the value for t
            double t = equals / tCoefficient.Inches;

            //now just plug t back into the line equations to find the x,y amd z
            //Dimension xComponent = this.BasePoint.X + new Dimension(DimensionType.Inch, this.Direction.XComponentOfDirection * t);
            //Dimension yComponent = this.BasePoint.Y + new Dimension(DimensionType.Inch, this.Direction.YComponentOfDirection * t);
            //Dimension zComponent = this.BasePoint.Z + new Dimension(DimensionType.Inch, this.Direction.ZComponentOfDirection * t);

            //Point intersectionPoint = new Point(xComponent, yComponent, zComponent);
            Point intersectionPoint = passedLine.GetPointOnLine(t);

            return intersectionPoint;
        }

        /// <summary>
        ///returns true if the line and the plane intersect, exlcuding if the line is on the plane 
        /// also can be thought of as if it has a single distinct intersect point
        /// </summary>
        /// <param name="passedLine">The Line to see if it intersects with this plane, but is not coplanar</param>
        /// <returns>returns whether or not the plane and line intersect and are not coplanar</returns>
        public virtual bool DoesIntersectNotCoplanar(Line passedLine)
        {
            return Intersection(passedLine) != null;
        }
        
        /// <summary>
        ///returns true if the vector and the plane intersect, exlcuding if the line is on the plane
        /// also can be thought of as if it has a single distinct intersect point
        /// </summary>
        /// <param name="passedVector">The Vector to see if it intersects with this plane, but is not coplanar</param>
        /// <returns>returns whether or not the plane and Vector intersect and are not coplanar</returns>
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
        /// Returns whether or not the two planes intersect, exlcuding if they are coplanar
        /// also can be thought of as if it has a single distinct intersect line
        /// </summary>
        /// <param name="passedPlane">The Plane to see if it intersects with this plane, but is not coplanar</param>
        /// <returns>returns whether or not the two planes intersect and are not coplanar</returns>
        public virtual bool DoesIntersectNotCoplanar(Plane passedPlane)
        {
            return !IsParallelTo(passedPlane);
        }

        /// <summary>
        /// Returns whether or not the polygon and plane intersect, exlcuding if the polygon is on the plane
        /// also can be thought of as if it has a single distinct intersect point or line
        /// </summary>
        /// <param name="passedPolygon">The Polygon to see if it intersects with this plane, but is not coplanar</param>
        /// <returns>returns whether or not the Polygon and plane intersect and are not coplanar</returns>
        public virtual bool DoesIntersectNotCoplanar(Polygon passedPolygon)
        {
            if (!DoesIntersectNotCoplanar((Plane)passedPolygon))
            {
                return false;
            }

            foreach (LineSegment segment in passedPolygon.PlaneBoundaries)
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
        /// <param name="passedPlane">The Plane to see if it intersects with this plane</param>
        /// <returns>returns whether or not the line touches the plane in any place</returns>
        public virtual bool DoesIntersect(Line passedLine)
        {
            return this.DoesIntersect(passedLine);
        }

        /// <summary>
        /// Returns whether or not the given vector intersects the plane
        /// </summary>
        /// <param name="passedVector">The Vector to see if it intersects with this plane</param>
        /// <returns>returns whether or not the Vector touches the plane in any place</returns>
        public virtual bool DoesIntersect(Vector passedVector)
        {

            return this.DoesIntersect(passedVector) || this.Contains(passedVector);
        }

        /// <summary>
        /// Returns whether or not the two planes intersect
        /// </summary>
        /// <param name="passedPlane">The Plane to see if it intersects with this plane</param>
        /// <returns>returns whether or not the two planes touch in any place</returns>
        public virtual bool DoesIntersect(Plane passedPlane)
        {
            return this.DoesIntersectNotCoplanar(passedPlane) || this.IsCoplanarTo(passedPlane);
        }

        /// <summary>
        /// Returns whether or not the polygon and plane intersect
        /// </summary>
        /// <param name="passedPolygon">The Plane to see if it intersects with this plane</param>
        /// <returns>returns whether or not the Polygon touches the plane in any place</returns>
        public virtual bool DoesIntersect(Polygon passedPolygon)
        {
            Line slicingLine = passedPolygon.Intersection(this);
            return (slicingLine != null && passedPolygon.DoesIntersect(slicingLine));
        }

        /// <summary>
        /// Returns whether or not the giving plane is parallel to this plane
        /// </summary>
        /// <param name="passedPlane">The plane to check if it is parallel</param>
        /// <returns>Returns whether or not the Plane is parallel to this plane</returns>
        public bool IsParallelTo(Plane passedPlane)
        {
            return this.NormalVector.PointInSameOrOppositeDirections(passedPlane.NormalVector);
        }

        /// <summary>
        /// Returns whether or not the giving Plane is prepindicular to this plane
        /// </summary>
        /// <param name="passedPlane">The plane to check if is perpindicular</param>
        /// <returns>Returns a bool of whether or not the plane is perpindicular to this plane</returns>
        public bool IsPerpindicularTo(Plane passedPlane)
        {
            return this.NormalVector.IsPerpindicularTo(passedPlane.NormalVector);
        }

        /// <summary>
        /// Returns whether or not the giving line is parallel to this plane
        /// </summary>
        /// <param name="passedLine">The line to check if it is parallel</param>
        /// <returns>Returns whether or not the Line is parallel to this plane</returns>
        public bool IsParallelTo(Line passedLine)
        {
            //check to see if it is perpindicular to the normal vector and if it is then it is parallel to the plane because the plane id
            //by definition perpinducluar to the normal
            return this.NormalVector.IsPerpindicularTo(passedLine);
        }
        
        /// <summary>
        /// Returns whether or not the giving line is prepindicular to this plane
        /// </summary>
        /// <param name="passedLine">The line to check if is perpindicular</param>
        /// <returns>Returns a bool of whether or not the line is perpindicular to this plane</returns>
        public bool IsPerpindicularTo(Line passedLine)
        {
            //check to see if it is parallel to the normal vector and if it is then it is perpindicular to the plane because the plane is
            //by definition perpinducluar to the normal
            return this.NormalVector.IsParallelTo(passedLine);
        }

        /// <summary>
        /// Returns whether or not the two plans are coplanar (parallel and share points)
        /// </summary>
        /// <param name="passedPlane">The plane to see if it is coplanar with this one</param>
        /// <returns>Returns a bool of whether or not the planes are coplananr</returns>
        public bool IsCoplanarTo(Plane passedPlane)
        {
            return IsParallelTo(passedPlane) && this.Contains(passedPlane.BasePoint);
        }

        #endregion
    }
}
