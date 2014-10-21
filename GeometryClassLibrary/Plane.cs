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
    [Serializable]
    public class Plane
    {
        #region Fields and Properties
        public Point BasePoint { get; protected set; }

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
            }
            else
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// A point on a plane and vector that is normal (prependicular) to that plane define the plane
        /// </summary>
        /// <param name="passedBasePoint"></param>
        /// <param name="passedNormalVector"></param>
        public Plane(Point passedBasePoint, Vector passedNormalVector)
        {
            this.BasePoint = passedBasePoint;
            this.NormalVector = new Vector(passedNormalVector);
        }

        /// <summary>
        ///  A point and a line define a plane if the point is not on the line
        /// </summary>
        /// <param name="passedPoint"></param>
        /// <param name="passedLine"></param>
        public Plane(Point passedPoint, Line passedLine)
        {
            if(!passedPoint.IsOnLine(passedLine))
            {
                this.BasePoint = passedPoint;
                Vector vectorFromLineToPoint = new Vector(passedLine.BasePoint, passedPoint);
                this.NormalVector = passedLine.UnitVector(DimensionType.Inch).CrossProduct(vectorFromLineToPoint.UnitVector(DimensionType.Inch));
            }
            else
            {
                String message = "That point is on the line";
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// There are 2 ways that 2 lines can define a plane:
        ///     1. If the 2 lines intersect
        ///     2. If the 2 lines are parallel
        /// </summary>
        /// <param name="passedLine1"></param>
        /// <param name="passedLine2"></param>
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
                if (passedLine1.IsCoplanarWith(passedLine2))
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

        public Plane(Plane passedPlane)
        {
            this.BasePoint = passedPlane.BasePoint;
            this.NormalVector = passedPlane.NormalVector;
        }

        #endregion

        #region Overloaded Operators

        public static bool operator ==(Plane plane1, Plane plane2)
        {
            return plane1.Equals(plane2);
        }

        public static bool operator !=(Plane plane1, Plane plane2)
        {
            return !plane1.Equals(plane2);
        }

        public override bool Equals(object obj)
        {
            Plane comparablePlane = null;
            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparablePlane = (Plane)obj;
                bool checkPoint = this.Contains(comparablePlane.BasePoint);
                bool checkVector = this.NormalVector.PointInSameOrOppositeDirections(comparablePlane.NormalVector);

                return checkPoint && checkVector;
            }
            catch
            {
                return false;
            }
            
            
        }

        #endregion

        #region Methods

        public bool Contains(Polygon passedPlaneRegion)
        {
            // checks to make sure that every line is on the line segment
            foreach (LineSegment segment in passedPlaneRegion.PlaneBoundaries)
            {
                if (!Contains(segment))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Contains(Line passedLine)
        {
            // weird calculus voodoo
            Vector planeVector = new Vector(passedLine.BasePoint, BasePoint);
            Dimension dotProduct1 = planeVector.UnitVector(DimensionType.Inch) * NormalVector.UnitVector(DimensionType.Inch);
            Dimension dotProduct2 = passedLine.UnitVector(DimensionType.Inch) * NormalVector.UnitVector(DimensionType.Inch);

            // if both of the vectors' dotproducts come out to 0, the line is on the plane
            return (dotProduct1.Equals(new Dimension()) && dotProduct2.Equals(new Dimension()));
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
            Dimension dotProduct = planeVector.UnitVector(DimensionType.Inch) * NormalVector.UnitVector(DimensionType.Inch);

            return dotProduct == new Dimension();
        }

        public Plane Rotate(Line passedAxis, Angle passedAngle)
        {
            Point newBasePoint = this.BasePoint.Rotate3D(passedAxis, passedAngle);
            Vector newNormalVector = this.NormalVector.Rotate(passedAxis, passedAngle);
            return new Plane(newBasePoint, newNormalVector);
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
        public Line IntersectionLineWithPlane(Plane otherPlane)
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
                return new Line(intersectLinePoint, intersectionLineDirection);
            }

            return null;
        }

        #endregion
    }
}
