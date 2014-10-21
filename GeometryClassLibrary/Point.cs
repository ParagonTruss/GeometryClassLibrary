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
    [DebuggerDisplay("{X.Inches}, {Y.Inches}, {Z.Inches}")]
    [Serializable]
    public class Point
    {
        #region fields and Properties

        public Dimension X
        {
            get { return _x; }
        }
        private Dimension _x;

        public Dimension Y
        {
            get { return _y; }
        }
        private Dimension _y;

        public Dimension Z
        {
            get { return _z; }
        }
        private Dimension _z;

        #endregion

        #region Constructors

        /// <summary>
        /// Zero Constructor
        /// </summary>
        public Point()
        {
            _x = new Dimension();
            _y = new Dimension();
            _z = new Dimension();
        }
        
        /// <summary>
        /// copy Constructor
        /// </summary>
        public Point(Point passedPoint)
        {
            _x = passedPoint._x;
            _y = passedPoint._y;
            _z = passedPoint._z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimension1"></param>
        /// <param name="dimension2"></param>
        /// <param name="dimension3"></param>
        public Point(Dimension dimension1, Dimension dimension2, Dimension dimension3)
        {
            _x = dimension1;
            _y = dimension2;
            _z = dimension3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passedType"></param>
        /// <param name="dimension1"></param>
        /// <param name="dimension2"></param>
        /// <param name="dimension3"></param>
        public Point(DimensionType passedType, double dimension1, double dimension2, double dimension3)
        {
            _x = new Dimension(passedType, dimension1);
            _y = new Dimension(passedType, dimension2);
            _z = new Dimension(passedType, dimension3);
        }

        /// <summary>
        /// Creates a point with only two dimensions. Coordinates are entered assumed XY orientation
        /// </summary>
        /// <param name="dimension1"></param>
        /// <param name="dimension2"></param>
        public Point(Dimension dimension1, Dimension dimension2)
        {

            _x = dimension1;
            _y = dimension2;
            _z = new Dimension(DimensionType.Millimeter, 0);
        }

        /// <summary>
        /// Cylindrical
        /// </summary>
        /// <param name="dimension1"></param>
        /// <param name="dimension2"></param>
        /// <param name="angle"></param>
        public Point(Dimension dimension1, Dimension dimension2, Angle angle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Spherical
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public Point(Dimension dimension, Angle angle1, Angle angle2)
        {
            throw new NotImplementedException();
        }

        public Point(Line line1, Line line2)
        {
            Point toCopy = line1.Intersection(line2);
            _x = toCopy.X;
            _y = toCopy.Y;
            _z = toCopy.Z;
        }

        #endregion

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        public static Point operator +(Point point1, Point point2)
        {
            //first calculate the new x
            Dimension newX = point1._x + point2._x;

            //then calcutate the new y
            Dimension newY = point1._y + point2._y;

            //then calcutate the new z
            Dimension newZ = point1._z + point2._z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        public static Point operator -(Point point1, Point point2)
        {
            //first calculate the new x
            Dimension newX = point1._x - point2._x;

            //then calcutate the new y
            Dimension newY = point1._y - point2._y;

            //then calcutate the new z
            Dimension newZ = point1._z - point2._z;

            //create a new Point object with your new values
            return new Point(newX, newY, newZ);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(Point point1, Point point2)
        {
            // covers null reference checks
            if ((Object)point1 == null)
            {
                if((Object)point2 == null)
                {
                    return true;
                }
                return false;
            }
            // if the two points' x and y and z are equal, returns true
            return point1.Equals(point2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
        {
            // if the two points' x and y are equal, returns false
            if ((Object)point1 == null)
            {
                if ((Object)point2 == null)
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

                // if the two points' x and y are equal, returns true
                return (this._x.Equals(comparablePoint.X) && this._y.Equals(comparablePoint.Y) && this._z.Equals(comparablePoint.Z));
            }
            //if they are not the same type than they are not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the a string converted to a desired unitType
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public string ToString(DimensionType unitType)
        {
            switch (unitType)
            {
                case DimensionType.ArchitecturalString:
                    return "X= " + _x.Architectural.ToString() + ", Y= " + _y.Architectural.ToString() + ", Z=" + _z.Architectural.ToString();
                case DimensionType.Millimeter:
                    return "X= " + _x.Millimeters.ToString() + " Millimeters, Y= " + _y.Millimeters.ToString() + " Millimeters, Z=" + _z.Millimeters.ToString();
                case DimensionType.Centimeter:
                    return "X= " + _x.Centimeters.ToString() + " Centimeters, Y= " + _y.Centimeters.ToString() + " Centimeters, Z=" + _z.Centimeters.ToString();
                case DimensionType.Meter:
                    return "X= " + _x.Meters.ToString() + " Meters, Y= " + _y.Meters.ToString() + " Meters, Z=" + _z.Meters.ToString();
                case DimensionType.Kilometer:
                    return "X= " + _x.Kilometers.ToString() + " Kilometers, Y= " + _y.Kilometers.ToString() + " Kilometers, Z=" + _z.Kilometers.ToString();
                case DimensionType.ThirtySecond:
                    return "X= " + _x.ThirtySeconds.ToString() + " ThirtySeconds, Y= " + _y.ThirtySeconds.ToString() + " ThirtySeconds, Z=" + _z.ThirtySeconds.ToString();
                case DimensionType.Sixteenth:
                    return "X= " + _x.Sixteenths.ToString() + " Sixteenths, Y= " + _y.Sixteenths.ToString() + " Sixteenths, Z=" + _z.Sixteenths.ToString();
                case DimensionType.Inch:
                    return "X= " + _x.Inches.ToString() + " Inches, Y= " + _y.Inches.ToString() + " Inches, Z=" + _z.Inches.ToString();
                case DimensionType.Foot:
                    return "X= " + _x.Feet.ToString() + " Feet, Y= " + _y.Feet.ToString() + " Feet, Z=" + _z.Feet.ToString();
                case DimensionType.Yard:
                    return "X= " + _x.Yards.ToString() + " Yards, Y= " + _y.Yards.ToString() + " Yards, Z=" + _z.Yards.ToString();
                case DimensionType.Mile:
                    return "X= " + _x.Miles.ToString() + " Miles, Y= " + _y.Miles.ToString() + " Miles, Z=" + _z.Miles.ToString();
            }
            //code should never be run
            return "We were unable to identify your desired Unit Type";
        }

        public override string ToString()
        {
            return ToString(DimensionType.Inch);
        }
        
        #endregion

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="rotateAngle">The rotation angle</param>
        /// <returns>Rotated point</returns>
        public Point Rotate2D(Point centerPoint, Angle rotateAngle)
        {
            double cosTheta = Math.Cos(rotateAngle.Radians);
            double sinTheta = Math.Sin(rotateAngle.Radians);

            return new Point
            (
                DimensionGenerator.MakeDimensionWithInches(
                    (cosTheta * (this.X - centerPoint.X).Inches -
                    sinTheta * (this.Y - centerPoint.Y).Inches + centerPoint.X.Inches)),
                DimensionGenerator.MakeDimensionWithInches(
                    (sinTheta * (this.X - centerPoint.X).Inches +
                    cosTheta * (this.Y - centerPoint.Y).Inches + centerPoint.Y.Inches))
            );
        }

        /// <summary>
        /// Moves the point the specified distance in the specified direction
        /// </summary>
        /// <returns>a new point in the new location</returns>
        public Point Translate(Direction passedDirection, Dimension passedDisplacement)
        {
            Dimension xDisplacement = passedDisplacement * passedDirection.XComponentOfDirection;
            Dimension yDisplacement = passedDisplacement * passedDirection.YComponentOfDirection;
            Dimension zDisplacement = passedDisplacement * passedDirection.ZComponentOfDirection;

            Point shift = new Point(xDisplacement, yDisplacement, zDisplacement);
            return (this + shift);
        }

        /// <summary>
        /// Moves the point in the specified direction by the magnitude of the direction vector
        /// </summary>
        /// <returns>a new point in the new location</returns>
        public Point Translate(Vector passedDisplacementVector)
        {
            Point shift = new Point(passedDisplacementVector.XComponent, passedDisplacementVector.YComponent, passedDisplacementVector.ZComponent);
            return (this + shift);
        }

        /// <summary>
        /// Mirror this point across a line
        /// </summary>
        /// <returns>a new point in the new location</returns>
        public Point MirrorAcross(Line passedAxisLine)
        {
            return this.Rotate3D(passedAxisLine, new Angle(AngleType.Degree, 180));            
        }

        /// <summary>
        /// uses the distance formula to find a the distance between this point and another
        /// </summary>
        /// <param name="_endPoint"></param>
        /// <returns>new dimension representing the distance</returns>
        public Dimension DistanceTo(Point _endPoint)
        {
            if (this == _endPoint)
            {
                return new Dimension();
            }

            //distance formula
            double term1 = Math.Pow(( _x - _endPoint._x).Inches, 2);
            double term2 = Math.Pow(( _y - _endPoint._y).Inches, 2);
            double term3 = Math.Pow(( _z - _endPoint._z).Inches, 2);

            double distanceInInches = Math.Sqrt(term1 + term2 + term3);

            return new Dimension(DimensionType.Inch, distanceInInches);
        }

        public Dimension DistanceTo(Line passedLine)
        {
            Line perpLine = this.MakePerpendicularLineSegment(passedLine);
            double distance = this.DistanceTo(perpLine.Intersection(passedLine)).Inches;
            return new Dimension(DimensionType.Inch, distance);
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
        /// <param name="passedAxisLine"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public Point Rotate3D(Line passedAxisLine, Angle passedRotationAngle)
		{
            Point originPoint = PointGenerator.MakePointWithInches(0, 0, 0);
            
            bool originIsOnPassedAxis = originPoint.IsOnLine(passedAxisLine);

            Point pointForRotating = this;

            Line axisForRotating = passedAxisLine;

            Direction directionFromOriginToAxis = new Direction();
            Dimension distanceFromOriginToAxis = new Dimension();

            if(!originIsOnPassedAxis)
            {
                //Must translate everything so that the axis line goes through the origin before rotating
                Line directionLine = originPoint.MakePerpendicularLineSegment(passedAxisLine);

                directionFromOriginToAxis = directionLine.Direction;
                distanceFromOriginToAxis = originPoint.DistanceTo(axisForRotating);

                //Move the point
                pointForRotating = this.Translate(directionFromOriginToAxis, distanceFromOriginToAxis * -1);
                
                //Make the axis go through the origin
                axisForRotating = new Line(originPoint, passedAxisLine.Direction);
            }

            Matrix rotationMatrix = Matrix.RotationMatrixAboutAxis(axisForRotating, passedRotationAngle);

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
                return pointToReturn.Translate(directionFromOriginToAxis, distanceFromOriginToAxis);
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
            LineSegment lineSegmentOnDestinationLine = new LineSegment(passedDestinationLine.BasePoint, passedDestinationLine.Direction, new Dimension(DimensionType.Inch, distanceToPerpPoint));

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
            Vector crossProduct = vectorFromBasePointOfLineToPoint.UnitVector(DimensionType.Inch).CrossProduct(passedLine.UnitVector(DimensionType.Inch));

            //if the above cross product is the 0 vector, the point is on the given line
            return crossProduct.Magnitude == new Dimension();
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
            foreach(Rotation rotation in passedShift.rotationsToApply)
            {
                pointToReturn = pointToReturn.Rotate3D(rotation.AxisToRotateAround, rotation.AngleToRotate);
            }

            //and then we translate it (unless is a negating shift) so the shift is more intuitive
            if (!passedShift.isNegatedShift)
            {
                pointToReturn = pointToReturn.Translate(passedShift.Displacement);
            }
            return pointToReturn;
        }    
    }
}
