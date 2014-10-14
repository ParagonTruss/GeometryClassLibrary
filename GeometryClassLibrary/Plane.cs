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
        private Point _basePoint; //Could be any point on the plane
        private Vector _normalVector; //Could be any vector that is normal (perpendicular) to the plane

        public Point BasePoint
        {   
            get{ return _basePoint; }
            protected set { _basePoint = value; }
        }

        public Vector NormalVector
        {
            get { return _normalVector; }
            protected set { _normalVector = value; } 
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Zero constructor
        /// </summary>
        public Plane()
        {
            _basePoint = new Point();
            _normalVector = new Vector();
        }

        public Plane(Point passedPoint1, Point passedPoint2, Point passedPoint3 )
        {
            //If all 3 points are noncollinear, they define a plane
            
            Line line1To2 = new Line(passedPoint1,passedPoint2);
            Line line2To3 = new Line(passedPoint2,passedPoint3);
            Line line1To3 = new Line(passedPoint1,passedPoint3);

            if(!passedPoint1.IsOnLine(line2To3) && !passedPoint2.IsOnLine(line1To3) && !passedPoint3.IsOnLine(line1To2))
            {
                _basePoint = passedPoint1;
                _normalVector = line1To2.DirectionVector.CrossProduct(line1To3.DirectionVector);
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
            _basePoint = passedBasePoint;
            _normalVector = passedNormalVector;
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
                _basePoint = passedPoint;
                Vector vectorFromLineToPoint = new Vector(passedLine.BasePoint, passedPoint);
                _normalVector = passedLine.DirectionVector.CrossProduct(vectorFromLineToPoint);
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
            
            if(passedLine1.IsParallelTo(passedLine2))
            {
                _basePoint = passedLine1.BasePoint;
                _normalVector = passedLine1.DirectionVector.CrossProduct(passedLine2.DirectionVector);

                
            }
            if(passedLine1.IsCoplanarWith(passedLine2))
            {
                _basePoint = passedLine1.BasePoint;
                _normalVector = passedLine1.DirectionVector.CrossProduct(passedLine2.DirectionVector);
            }

            else
            {
                String message = "Those 3 points are not on the same plane";
                throw new NotSupportedException(message);
            }
        }

        
        public Plane(IList<Line> passedLineList)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLineList);

            if (passedLineListCasted.AreAllCoplanar())
            {
                _basePoint = passedLineListCasted[0].BasePoint;

                //we have to check against vectors until we find one that is not parralel with the first line we passed in
                //or else the normal vector will be zero (cross product of parralel lines is 0)
                Vector vector1 = passedLineListCasted[0].DirectionVector;
                for (int i = 1; i < passedLineListCasted.Count; i++)
                {
                    _normalVector = vector1.CrossProduct(passedLineListCasted[i].DirectionVector);
                    if (!_normalVector.Equals(new Vector()))
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
            this._basePoint = passedPlane._basePoint;
            this._normalVector = passedPlane.NormalVector;

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
            Dimension dotProduct1 = planeVector * NormalVector;
            Dimension dotProduct2 = passedLine.DirectionVector * NormalVector;

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
            Vector planeVector = new Vector(passedPoint, BasePoint);
            Dimension dotProduct = planeVector * NormalVector.ConvertToUnitVector();

            return dotProduct == new Dimension();
        }

        public Plane Rotate(Line passedAxis, Angle passedAngle)
        {
            Point newBasePoint = _basePoint.Rotate3D(passedAxis, passedAngle);
            Vector newNormalVector = _normalVector.Rotate(passedAxis, passedAngle);
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
            Dimension testDot = new Vector(BasePoint, testPoint) * NormalVector;
            Dimension referenceDot = new Vector(BasePoint, referencePoint) * NormalVector;

            //if they are both either positive or negative than they are both on the same side
            if ((testDot < new Dimension() && referenceDot < new Dimension()) || (testDot > new Dimension() && referenceDot > new Dimension()))
            {
                return true;
            }

            //if they are on opposite sides of the plane or either of the points are on the plan than we will return false
            return false;
        }

        #endregion


    }
}
