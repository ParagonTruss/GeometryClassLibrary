using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Plane
    {
        #region Fields and Properties
        private Point _basePoint; //Could be any point on the plane
        private Vector _normalVector; //Could be any vector that is normal (perpendicular) to the plane

        public Point BasePoint
        {   
            get{ return _basePoint; }
        }

        public Vector NormalVector
        {
            get { return _normalVector; }
        }

        #endregion

        #region Constructors

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
        Plane(Point passedPoint, Line passedLine)
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
            if(passedLine1.DoesIntersect(passedLine2))
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

        public Plane(IEnumerable<Line> passedLineList)
        {
            List<Line> passedLineListCasted = new List<Line>(passedLineList);

            if (passedLineListCasted.AreAllCoplanar())
            {
                _basePoint = passedLineListCasted[0].BasePoint;
                _normalVector = passedLineListCasted[0].DirectionVector.CrossProduct(passedLineListCasted[1].DirectionVector);
            }
            else
            {
                throw new Exception();
            }
            
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

        public bool Contains(PlaneRegion passedPlaneRegion)
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

        public bool Contains(Point passedPoint)
        {
            Vector planeVector = new Vector(passedPoint, BasePoint);
            Dimension dotProduct = planeVector * NormalVector;

            return (dotProduct.Millimeters == 0);
        }

        public Plane Rotate(Line passedAxis, Angle passedAngle)
        {
            Point newBasePoint = _basePoint.Rotate3D(passedAxis, passedAngle);
            Vector newNormalVector = _normalVector.Rotate(passedAxis, passedAngle);
            return new Plane(newBasePoint, newNormalVector);
        }

        #endregion


    }
}
