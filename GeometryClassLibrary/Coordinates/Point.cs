using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;
//using VisualGeometryDebugger;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Point class that gracefully handles 2d and 3d points
    /// </summary>
    //[DebuggerVisualizer(typeof(GeometryVisualizer))]   
    public class Point: IEquatable<Point>
    {
        #region Properties and Fields

        public Distance X
        {
            get { return _x; }
        }
        private Distance _x;

        public Distance Y
        {
            get { return _y; }
        }
        private Distance _y;

        public Distance Z
        {
            get { return _z; }
        }
        private Distance _z;

        #endregion

        #region Constructors

        /// <summary>
        /// Zero Constructor
        /// </summary>
        public Point()
        {
            _x = new Distance();
            _y = new Distance();
            _z = new Distance();
        }
        
        /// <summary>
        /// Creates a point with only two Distances. Coordinates are entered assumed XY orientation
        /// </summary>
        /// <param name="Distance1"></param>
        /// <param name="Distance2"></param>
        public Point(Distance Distance1, Distance Distance2)
        {
            _x = Distance1;
            _y = Distance2;
            _z = new Distance(DistanceType.Millimeter, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Distance1"></param>
        /// <param name="Distance2"></param>
        /// <param name="Distance3"></param>
        public Point(Distance Distance1, Distance Distance2, Distance Distance3)
        {
            _x = Distance1;
            _y = Distance2;
            _z = Distance3;
        }

        /// <summary>
        /// Cylindrical
        /// </summary>
        /// <param name="Distance1"></param>
        /// <param name="Distance2"></param>
        /// <param name="angle"></param>
        public Point(Distance Distance1, Distance Distance2, Angle angle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Spherical
        /// </summary>
        /// <param name="Distance"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public Point(Distance Distance, Angle angle1, Angle angle2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new point with the given values with the given Distance type
        /// </summary>
        /// <param name="passedType"></param>
        /// <param name="Distance1"></param>
        /// <param name="Distance2"></param>
        /// <param name="Distance3"></param>
        public Point(DistanceType passedType, double Distance1, double Distance2, double Distance3)
        {
            _x = new Distance(passedType, Distance1);
            _y = new Distance(passedType, Distance2);
            _z = new Distance(passedType, Distance3);
        }

        /// <summary>
        /// copy Constructor
        /// </summary>
        public Point(Point toCopy)
        {
            _x = toCopy._x;
            _y = toCopy._y;
            _z = toCopy._z;
        }

        #endregion

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Point operator +(Point point1, Point point2)
        {
            //first calculate the new x
            Distance newX = point1._x + point2._x;

            //then calcutate the new y
            Distance newY = point1._y + point2._y;

            //then calcutate the new z
            Distance newZ = point1._z + point2._z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        public static Point operator -(Point point1, Point point2)
        {
            //first calculate the new x
            Distance newX = point1._x - point2._x;

            //then calcutate the new y
            Distance newY = point1._y - point2._y;

            //then calcutate the new z
            Distance newZ = point1._z - point2._z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            // covers null reference checks
            if ((object)point1 == null)
            {
                if((object)point2 == null)
                {
                    return true;
                }
                return false;
            }
            // if the two points' x and y and z are equal, returns true
            return point1.Equals(point2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
        {
            // if the two points' x and y are equal, returns false
            if ((object)point1 == null)
            {
                if ((object)point2 == null)
                {
                    return false;
                }
                return true;
            }
            return !point1.Equals(point2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //check for null (wont throw a castexception)
            if (obj == null)
            {
                return false;
            }
            
            //try to cast the object to a Point, if it fails then we know the user passed in the wrong type of object
            try
            {
                Point comparablePoint = (Point)obj;
                return this.Equals(comparablePoint);
            }

            //if they are not the same type than they are not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public bool Equals(Point other)
        {
            // if the two points' x, y, and z are equal, returns true
            return (this._x.Equals(other.X) && this._y.Equals(other.Y) && this._z.Equals(other.Z));
        }

        /// <summary>
        /// Returns the a string converted to a desired unitType
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public string ToString(DistanceType unitType)
        {
            switch (unitType)
            {
                case DistanceType.ArchitecturalString:
                    return "X= " + _x.Architectural.ToString() + ", Y= " + _y.Architectural.ToString() + ", Z=" + _z.Architectural.ToString();
                case DistanceType.Millimeter:
                    return "X= " + _x.Millimeters.ToString() + " Millimeters, Y= " + _y.Millimeters.ToString() + " Millimeters, Z=" + _z.Millimeters.ToString();
                case DistanceType.Centimeter:
                    return "X= " + _x.Centimeters.ToString() + " Centimeters, Y= " + _y.Centimeters.ToString() + " Centimeters, Z=" + _z.Centimeters.ToString();
                case DistanceType.Meter:
                    return "X= " + _x.Meters.ToString() + " Meters, Y= " + _y.Meters.ToString() + " Meters, Z=" + _z.Meters.ToString();
                case DistanceType.Kilometer:
                    return "X= " + _x.Kilometers.ToString() + " Kilometers, Y= " + _y.Kilometers.ToString() + " Kilometers, Z=" + _z.Kilometers.ToString();
                case DistanceType.ThirtySecond:
                    return "X= " + _x.ThirtySeconds.ToString() + " ThirtySeconds, Y= " + _y.ThirtySeconds.ToString() + " ThirtySeconds, Z=" + _z.ThirtySeconds.ToString();
                case DistanceType.Sixteenth:
                    return "X= " + _x.Sixteenths.ToString() + " Sixteenths, Y= " + _y.Sixteenths.ToString() + " Sixteenths, Z=" + _z.Sixteenths.ToString();
                case DistanceType.Inch:
                    return "X= " + _x.Inches.ToString() + " Inches, Y= " + _y.Inches.ToString() + " Inches, Z=" + _z.Inches.ToString();
                case DistanceType.Foot:
                    return "X= " + _x.Feet.ToString() + " Feet, Y= " + _y.Feet.ToString() + " Feet, Z=" + _z.Feet.ToString();
                case DistanceType.Yard:
                    return "X= " + _x.Yards.ToString() + " Yards, Y= " + _y.Yards.ToString() + " Yards, Z=" + _z.Yards.ToString();
                case DistanceType.Mile:
                    return "X= " + _x.Miles.ToString() + " Miles, Y= " + _y.Miles.ToString() + " Miles, Z=" + _z.Miles.ToString();
            }
            //code should never be run
            return "We were unable to identify your desired Unit Type";
        }

        public override string ToString()
        {
            return ToString(DistanceType.Inch);
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="rotateAngle">The rotation angle</param>
        /// <returns>Rotated point</returns>
        public Point Rotate2D(Point centerPoint, Angle rotateAngle)
        {
            ;

            double cosTheta = Math.Cos(rotateAngle.Radians);
            double sinTheta = Math.Sin(rotateAngle.Radians);

            return new Point
            (
                Distance.MakeDistanceWithInches(
                    (cosTheta * (this.X - centerPoint.X).Inches -
                    sinTheta * (this.Y - centerPoint.Y).Inches + centerPoint.X.Inches)),
                Distance.MakeDistanceWithInches(
                    (sinTheta * (this.X - centerPoint.X).Inches +
                    cosTheta * (this.Y - centerPoint.Y).Inches + centerPoint.Y.Inches))
            );
        }

        /// <summary>
        /// Moves the point by the specified amount based on the passed point
        /// </summary>
        public Point Translate(Point passedDisplacementPoint)
        {
            return (this + passedDisplacementPoint);
        }

        /// <summary>
        /// Mirror this point across a line
        /// </summary>
        /// <returns>a new point in the new location</returns>
        public Point MirrorAcross(Line passedAxisLine)
        {
            return this.Rotate3D(new Rotation(passedAxisLine, new Angle(AngleType.Degree, 180)));            
        }

        /// <summary>
        /// uses the distance formula to find a the distance between this point and another
        /// </summary>
        /// <param name="_endPoint"></param>
        /// <returns>new Distance representing the distance</returns>
        public Distance DistanceTo(Point _endPoint)
        {
            if (this == _endPoint)
            {
                return new Distance();
            }

            //distance formula
            double term1 = Math.Pow(( _x - _endPoint._x).Inches, 2);
            double term2 = Math.Pow(( _y - _endPoint._y).Inches, 2);
            double term3 = Math.Pow(( _z - _endPoint._z).Inches, 2);

            double distanceInInches = Math.Sqrt(term1 + term2 + term3);

            return new Distance(DistanceType.Inch, distanceInInches);
        }

        public Distance DistanceTo(Line passedLine)
        {
            Line perpLine = this.MakePerpendicularLineSegment(passedLine);
            double distance = this.DistanceTo(perpLine.Intersection(passedLine)).Inches;
            return new Distance(DistanceType.Inch, distance);
        }

        /// <summary>
        /// Returns a vector that extends from the origin to this point
        /// </summary>
        /// <returns></returns>
        public Vector ConvertToVector()
        {
            return new Vector(this);
        }

        /// <summary>
        /// Rotates a point about an axis with the given angle (returns a new point in that location)
        /// </summary>
        /// <param name="rotationToApply">The Rotation to apply to the point that stores the axis to rotate around and the angle to rotate</param>
        /// <returns></returns>
        public Point Rotate3D(Rotation rotationToApply)
		{
            Point originPoint = PointGenerator.MakePointWithInches(0, 0, 0);

            bool originIsOnPassedAxis = originPoint.IsOnLine(rotationToApply.AxisToRotateAround);

            Point pointForRotating = this;

            Line axisForRotating = rotationToApply.AxisToRotateAround;

            if(!originIsOnPassedAxis)
            {
                //Must translate everything so that the axis line goes through the origin before rotating

                //Move the point negative the basepoint from the origin
                pointForRotating = this.Translate(new Point() - rotationToApply.AxisToRotateAround.BasePoint);

                //Make the axis go through the origin
                axisForRotating = new Line(rotationToApply.AxisToRotateAround.Direction, originPoint);

            }

            Matrix rotationMatrix = Matrix.RotationMatrixAboutAxis(axisForRotating, rotationToApply.AngleToRotate);

            Matrix pointMatrix = pointForRotating.ConvertToMatrixColumn();
                
            Matrix rotatedPointMatrix = rotationMatrix * pointMatrix;
           
            double xOfRotatedPoint = rotatedPointMatrix.GetElement(0, 0);
            double yOfRotatedPoint = rotatedPointMatrix.GetElement(1, 0);
            double zOfRotatedPoint = rotatedPointMatrix.GetElement(2, 0);

            Point pointToReturn = PointGenerator.MakePointWithInches(xOfRotatedPoint, yOfRotatedPoint, zOfRotatedPoint);

            if(originIsOnPassedAxis)
            {
                return pointToReturn;
            }
            else
            {
                //Must shift the point back by the same distance we shifted it before rotating it
                return pointToReturn + rotationToApply.AxisToRotateAround.BasePoint;
            }
        }

        /// <summary>
        /// Returns a line segment that goes through this point, is perpendicular to the destination line, and ends on that line
        /// </summary>
        /// <param name="passedDestinationLine"></param>
        /// <returns></returns>
        public LineSegment MakePerpendicularLineSegment(Line passedDestinationLine)
        {
            //Make line from this point to the base point of the destination line
            Line hypotenuse = new Line(this, passedDestinationLine.BasePoint);

            //Find smallest angle between new line and destination line
            Angle angleBetweenLines = hypotenuse.AngleBetweenIntersectingLine(passedDestinationLine);

            //Make a line segment from the base point of destination line to the point on the destination line that is on a line perpendicular to this point
            double distanceToBasePoint = this.DistanceTo(passedDestinationLine.BasePoint).Inches;
            double distanceToPerpPoint = distanceToBasePoint * (Math.Cos(angleBetweenLines.Radians)); //This is the distance to a point on the destination line that is perpendicular to this point
            LineSegment lineSegmentOnDestinationLine = new LineSegment(passedDestinationLine.BasePoint, passedDestinationLine.Direction, new Distance(DistanceType.Inch, distanceToPerpPoint));

            //Return a new line segment through this point and the end point of the line segment. It is perpendicular to the destination line
            return new LineSegment(this, lineSegmentOnDestinationLine.EndPoint);
        }

        /// <summary>
        /// Returns true if the point is on the passed line, false otherwise
        /// </summary>
        /// <param name="passedPoint"></param>
        /// <returns></returns>
        public bool IsOnLine(Line passedLine)
        {
            //if we are the base point than we are on the line!
            if (passedLine.BasePoint == this)
                return true;

            Vector vectorFromBasePointOfLineToPoint = new Vector(passedLine.BasePoint, this);

            //Take the cross product of the vector from the base point of the line to the point and the line's direction vector
            Vector crossProduct = vectorFromBasePointOfLineToPoint.CrossProduct(passedLine.UnitVector(DistanceType.Inch));

            //if the above cross product is the 0 vector, the point is on the given line
            return crossProduct.Magnitude == new Distance();
        }

        /// <summary>
        /// Returns true if the point is on the passed vector, false otherwise
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public bool IsOnVector(Vector passedVector)
        {
            if (this.IsOnLine(passedVector))
            {
                Vector vectorFromStartPointToPoint = new Vector(passedVector.BasePoint, this);
                Vector vectorFromEndPointToPoint = new Vector(passedVector.EndPoint, this);

                // if the vectors point in opposite directions (towards the middle) or the point is an endpoint, it's true
                return vectorFromStartPointToPoint.PointInOppositeDirections(vectorFromEndPointToPoint) ||
                    this.Equals(passedVector.BasePoint) ||
                    this.Equals(passedVector.EndPoint);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the point is on the passed line segment, false otherwise
        /// </summary>
        /// <param name="passedLineSegment"></param>
        /// <returns></returns>
        public bool IsOnLineSegment(LineSegment passedLineSegment)
        {
            return IsOnVector(passedLineSegment);
        }

        public Vector VectorFromOriginToPoint()
        {
            Point origin = PointGenerator.MakePointWithInches(0,0,0);
            Point thisPoint = PointGenerator.MakePointWithInches(X.Inches, Y.Inches, Z.Inches);

            Vector returnVector = new Vector(origin, thisPoint);

            return returnVector;
        }

        public Matrix ConvertToMatrixColumn()
        {
            return this.ConvertToVector().ConvertToMatrixColumn();
        }

        public Point Shift(Shift passedShift)
        {
            Point pointToReturn = this;

            //we need to untranslate the point first if we are negating a previous shift so that it returns 
            //correcly - for a full explaination look in Shift.cs where isNegatedShift is declared
            if (passedShift.isNegatedShift)
            {
                pointToReturn = pointToReturn.Translate(passedShift.Displacement);
            }

            //we need to apply each rotation in order to the point
            foreach(Rotation rotation in passedShift.RotationsToApply)
            {
                pointToReturn = pointToReturn.Rotate3D(rotation);
            }

            //and then we translate it (unless is a negating shift) so the shift is more intuitive
            if (!passedShift.isNegatedShift)
            {
                pointToReturn = pointToReturn.Translate(passedShift.Displacement);
            }
            return pointToReturn;
        }

        #endregion


    }
}
