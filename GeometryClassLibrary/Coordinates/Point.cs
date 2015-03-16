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
    public class Point : IComparable
    {
        #region Properties and Fields

        public Distance X { get; set; }

        public Distance Y { get; set; }

        public Distance Z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Zero Constructor
        /// </summary>
        public Point()
        {
            X = new Distance();
            Y = new Distance();
            Z = new Distance();
        }

        /// <summary>
        /// Creates a point with only two Distances. Coordinates are entered assumed XY orientation
        /// </summary>
        /// <param name="passedX"></param>
        /// <param name="passedY"></param>
        public Point(Distance passedX, Distance passedY)
        {
            X = passedX;
            Y = passedY;
            Z = new Distance(DistanceType.Inch, 0);
        }

        /// <summary>
        /// Three Dimensional Constructor for a Point
        /// </summary>
        /// <param name="passedX"></param>
        /// <param name="passedY"></param>
        /// <param name="passedZ"></param>
        public Point(Distance passedX, Distance passedY, Distance passedZ)
        {
            X = passedX;
            Y = passedY;
            Z = passedZ;
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
        /// <param name="passedX"></param>
        /// <param name="passedY"></param>
        /// <param name="passedZ"></param>
        public Point(DistanceType passedType, double passedX, double passedY, double passedZ)
        {
            X = new Distance(passedType, passedX);
            Y = new Distance(passedType, passedY);
            Z = new Distance(passedType, passedZ);
        }

        /// <summary>
        /// copy Constructor
        /// </summary>
        public Point(Point toCopy)
        {
            X = toCopy.X;
            Y = toCopy.Y;
            Z = toCopy.Z;
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static Point operator +(Point point1, Point point2)
        {
            //first calculate the new x
            Distance newX = point1.X + point2.X;

            //then calcutate the new y
            Distance newY = point1.Y + point2.Y;

            //then calcutate the new z
            Distance newZ = point1.Z + point2.Z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        public static Point operator -(Point point1, Point point2)
        {
            //first calculate the new x
            Distance newX = point1.X - point2.X;

            //then calcutate the new y
            Distance newY = point1.Y - point2.Y;

            //then calcutate the new z
            Distance newZ = point1.Z - point2.Z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            // covers null reference checks
            if ((object) point1 != null)
            {
                return point1.Equals(point2);
            }
            if ((object) point2 == null)
            {
                return true;
            }
            return false;
            // if the two points' x and y and z are equal, returns true
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


        public bool Equals(Point comparablePoint)
        {
            //check for null (wont throw a castexception)
            if (comparablePoint == null)
            {
                return false;
            }

            // if the two points' x, y, and z are equal, returns true
            return (this.X.Equals(comparablePoint.X) && this.Y.Equals(comparablePoint.Y) && this.Z.Equals(comparablePoint.Z));
        }

        public override string ToString()
        {
            return "X= " + this.X.ToString() + ", Y= " + this.Y.ToString() + ", Z=" + this.Z.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="rotateAngle">The rotation angle</param>
        /// <returns>Rotated point</returns>
        public Point Rotate2D(Point centerPoint, Angle rotateAngle)
        {
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
        /// <param name="passedTranslation"></param>
        public Point Translate(Translation passedTranslation)
        {
            return (this + passedTranslation);
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
        /// <param name="endPoint"></param>
        /// <returns>new Distance representing the distance</returns>
        public Distance DistanceTo(Point endPoint)
        {
            if (this == endPoint)
            {
                return new Distance();
            }

            //distance formula
            double term1 = Math.Pow((X - endPoint.X).Inches, 2);
            double term2 = Math.Pow((Y - endPoint.Y).Inches, 2);
            double term3 = Math.Pow((Z - endPoint.Z).Inches, 2);

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

            if (!originIsOnPassedAxis)
            {
                //Must translate everything so that the axis line goes through the origin before rotating

                //Move the point negative the basepoint from the origin
                pointForRotating = this.Translate(new Translation(new Point() - rotationToApply.AxisToRotateAround.BasePoint));

                //Make the axis go through the origin
                axisForRotating = new Line(rotationToApply.AxisToRotateAround.Direction, originPoint);

            }

            Matrix rotationMatrix = Matrix.RotationMatrixAboutAxis(new Rotation(axisForRotating, rotationToApply.AngleToRotate));

            Matrix pointMatrix = pointForRotating.ConvertToMatrixColumn();

            Matrix rotatedPointMatrix = rotationMatrix * pointMatrix;

            double xOfRotatedPoint = rotatedPointMatrix.GetElement(0, 0);
            double yOfRotatedPoint = rotatedPointMatrix.GetElement(1, 0);
            double zOfRotatedPoint = rotatedPointMatrix.GetElement(2, 0);

            Point pointToReturn = PointGenerator.MakePointWithInches(xOfRotatedPoint, yOfRotatedPoint, zOfRotatedPoint);

            if (originIsOnPassedAxis)
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
            Angle angleBetweenLines = hypotenuse.SmallestAngleBetweenIntersectingLine(passedDestinationLine);

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
        /// <param name="passedLine"></param>
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

        public Vector VectorFromOriginToThisPoint()
        {
            Point origin = PointGenerator.MakePointWithInches(0, 0, 0);
            Point thisPoint = PointGenerator.MakePointWithInches(X.Inches, Y.Inches, Z.Inches);

            Vector returnVector = new Vector(origin, thisPoint);

            return returnVector;
        }

        public Matrix ConvertToMatrixColumn()
        {
            return this.ConvertToVector().ConvertToMatrixColumn();
        }

        /// <summary>
        /// Shifts the Point with the given Shift
        /// </summary>
        /// <param name="passedShift">The Shift to apply to this Point</param>
        /// <returns>returns a new Point that has been shifted</returns>
        public Point Shift(Shift passedShift)
        {
            Point pointToReturn = this;

            //we need to untranslate the point first if we are negating a previous shift so that it returns 
            //correcly - for a full explaination look in Shift.cs where isNegatedShift is declared
            if (passedShift.isNegatedShift)
            {
                pointToReturn = pointToReturn.Translate(new Translation(passedShift.Displacement));
            }

            //we need to apply each rotation in order to the point
            foreach (Rotation rotation in passedShift.RotationsToApply)
            {
                pointToReturn = pointToReturn.Rotate3D(rotation);
            }

            //and then we translate it (unless is a negating shift) so the shift is more intuitive
            if (!passedShift.isNegatedShift)
            {
                pointToReturn = pointToReturn.Translate(new Translation(passedShift.Displacement));
            }
            return pointToReturn;
        }

        /// <summary>
        /// Shifts the Popint with the given coordinateSystem
        /// </summary>
        /// <param name="toShiftWith">the Coordinate System to shift this Point with</param>
        /// <returns>returns a New Point that has been shifted with the CoordinateSystem</returns>
        public Point SystemShift(CoordinateSystem toShiftWith)
        {
            return this.Shift(new Shift(toShiftWith));
        }

        /// <summary>
        /// Shifts this Point from one Coordinate System to the other
        /// </summary>
        /// <param name="to">The Coordinate System to shift this Point to</param>
        /// <param name="from">The coordinate System this Point is currently in. If left out it defaults to the 
        /// World Coordinate Systemm</param>
        /// <returns>Returns a new Point that has been shifted to the given Coordinate System</returns>
        public Point ShiftCoordinateSystemsToFrom(CoordinateSystem to, CoordinateSystem from = null)
        {
            //use our coordinate system if none is passed in
            if (from == null)
            {
                from = CoordinateSystem.WorldCoordinateSystem;
            }
            if(to == null)
            {
                to = CoordinateSystem.WorldCoordinateSystem;
            }

            if(from != CoordinateSystem.WorldCoordinateSystem && to != CoordinateSystem.WorldCoordinateSystem)
            {
                //we have to undo both of the shifts
                Point test = this.Shift(from.ShiftThatReturnsThisToWorldCoordinateSystem());
                return test.Shift(to.ShiftThatReturnsThisToWorldCoordinateSystem()); 
            }
            else if(from != CoordinateSystem.WorldCoordinateSystem)
            {
                //shift it from where it is to the world coordinate system 
                 return this.SystemShift(from);  
            }
            else if(to != CoordinateSystem.WorldCoordinateSystem)
            {
                //then Shift it to the new Cordinate system
                return this.Shift(to.ShiftThatReturnsThisToWorldCoordinateSystem());  
            }
            else
            {
                return this;
            }
        }

        #endregion

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
