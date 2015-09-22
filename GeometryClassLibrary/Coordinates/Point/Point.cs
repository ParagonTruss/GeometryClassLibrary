﻿using System;
using Newtonsoft.Json;
using UnitClassLibrary;
using static UnitClassLibrary.Distance;
//using VisualGeometryDebugger;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Point class that gracefully handles 2d and 3d points
    /// </summary>
    //[DebuggerVisualizer(typeof(GeometryVisualizer))]
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Point
    {
        #region Properties and Fields

        public static readonly Point Origin = new Point(0 * Inch, 0 * Inch, 0 * Inch);

        private Distance _x;
        private Distance _y;
        private Distance _z;

        [JsonProperty]
        public Distance X
        {
            get { return _x; }
            private set { _x = value; }
        }

        [JsonProperty]
        public Distance Y
        {
            get { return _y; }
            private set { _y = value; }
        }

        [JsonProperty]
        public Distance Z
        {
            get { return _z; }
            private set { _z = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        protected Point() { }

        /// <summary>
        ///Create a point that lies in the XY plane.
        /// </summary>
        public Point(Distance passedX, Distance passedY)
        {
            _x = passedX;
            _y = passedY;
            _z = Distance.Zero;
        }

        /// <summary>
        /// Create any point by its coordinates.
        /// </summary>
        [JsonConstructor]
        public Point(Distance x, Distance y, Distance z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Creates the point that is the specified distance from the origin, in the specified direction.
        /// </summary>
        public Point(Direction direction, Distance fromOrigin)
        {
            this._x = direction.XComponent * fromOrigin;
            this._y = direction.YComponent * fromOrigin;
            this._z = direction.ZComponent * fromOrigin;
        }

        /// <summary>
        /// Creates a new point with the given values with the given Distance type
        /// </summary>
        public Point(DistanceType passedType, double passedX, double passedY, double passedZ)
        {
            _x = new Distance(passedType, passedX);
            _y = new Distance(passedType, passedY);
            _z = new Distance(passedType, passedZ);
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public Point(Point toCopy)
        {
            _x = toCopy.X;
            _y = toCopy.Y;
            _z = toCopy.Z;
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
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

        public static Point operator *(double scalar, Point point)
        {
            return new Point(scalar * point.X, scalar * point.Y, scalar * point.Z);
        }

        public static Point operator /(Point point, double divisor)
        {
            return new Point(point.X/ divisor, point.Y/divisor, point.Z/divisor);

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
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
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
            if ((object)comparablePoint == null)
            {
                return false;
            }

            // if the two points are close enough, then they're equal
            return this.DistanceTo(comparablePoint) == Distance.Zero;
            //return (X == comparablePoint.X) && (Y == comparablePoint.Y) && (Z == comparablePoint.Z);
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
        public Point Rotate2D(Point centerPoint, AngularDistance rotateAngle)
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
        public Point Translate(Point passedTranslation)
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
        /// Flips the sign of each coordinate
        /// </summary>
        /// <returns>a new point in the new location</returns>
        public Point Negate()
        {
            return new Point(X * -1, Y * -1, Z * -1);
        }

        /// <summary>
        /// uses the distance formula to find a the distance between this point and another
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns>new Distance representing the distance</returns>
        public Distance DistanceTo(Point endPoint)
        {
            //distance formula
            double term1 = Math.Pow((X - endPoint.X).Inches, 2);
            double term2 = Math.Pow((Y - endPoint.Y).Inches, 2);
            double term3 = Math.Pow((Z - endPoint.Z).Inches, 2);

            double distanceInInches = Math.Sqrt(term1 + term2 + term3);

            return new Distance(DistanceType.Inch, distanceInInches);
        }

        /// <summary>
        /// returns the shortest distance from the line to the point
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public Distance DistanceTo(Line passedLine)
        {
            Point projected = this.ProjectOntoLine(passedLine);
            Distance distance = this.DistanceTo(projected);
            return distance;
        }

        public Distance DistanceTo(Plane passedPlane)
        {
            Vector planeToPointVector = new Vector(passedPlane.BasePoint, this);
            Vector normalVector = planeToPointVector.ProjectOntoLine(passedPlane.NormalVector);
            
            Distance distance = normalVector.Magnitude;
            return distance;
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
            Point originPoint = MakePointWithInches(0, 0, 0);

            bool originIsOnPassedAxis = originPoint.IsOnLine(rotationToApply.AxisOfRotation);

            Point pointForRotating = this;

            Line axisForRotating = rotationToApply.AxisOfRotation;

            if (!originIsOnPassedAxis)
            {
                //Must translate everything so that the axis line goes through the origin before rotating

                //Move the point negative the basepoint from the origin
                pointForRotating = this.Translate(new Translation(Point.Origin - rotationToApply.AxisOfRotation.BasePoint));

                //Make the axis go through the origin
                axisForRotating = new Line(rotationToApply.AxisOfRotation.Direction, originPoint);

            }

            Matrix rotationMatrix = Matrix.RotationMatrixAboutOrigin(new Rotation(axisForRotating, rotationToApply.RotationAngle));

            Matrix pointMatrix = pointForRotating.ConvertToMatrixColumn();

            Matrix rotatedPointMatrix = rotationMatrix * pointMatrix;

            double xOfRotatedPoint = rotatedPointMatrix.GetElement(0, 0);
            double yOfRotatedPoint = rotatedPointMatrix.GetElement(1, 0);
            double zOfRotatedPoint = rotatedPointMatrix.GetElement(2, 0);

            Point pointToReturn = MakePointWithInches(xOfRotatedPoint, yOfRotatedPoint, zOfRotatedPoint);

            if (originIsOnPassedAxis)
            {
                return pointToReturn;
            }
            else
            {
                //Must shift the point back by the same distance we shifted it before rotating it
                return pointToReturn + rotationToApply.AxisOfRotation.BasePoint;
            }
        }

        /// <summary>
        /// Returns a line segment that goes through this point, is perpendicular to the destination line, and ends on that line
        /// </summary>
        /// <param name="passedDestinationLine"></param>
        /// <returns></returns>
        public LineSegment MakePerpendicularLineSegment(Line passedDestinationLine)
        {
            if (!this.IsOnLine(passedDestinationLine))
            {
                //Make line from this point to the base point of the destination line
                Vector hypotenuse = new Vector(passedDestinationLine.BasePoint, this);
                Vector projection = hypotenuse.ProjectOntoLine(passedDestinationLine);
                LineSegment normalSegment = new LineSegment(projection.EndPoint, hypotenuse.EndPoint);
                return normalSegment;
            }
            return null;
        }

        /// <summary>
        /// Projects the point onto the line, by extending the normal direction from the line to the point.
        /// </summary>
        /// <param name="projectOnto"></param>
        /// <returns></returns>
        public Point ProjectOntoLine(Line projectOnto)
        {
            Vector hypotenuse = new Vector(projectOnto.BasePoint, this);
            Direction lineDirection = projectOnto.Direction;
            Vector unitVectorAlongLine = new Vector(projectOnto.BasePoint, lineDirection, Distance.Inch);
            double dotProduct = hypotenuse.DotProduct(unitVectorAlongLine).InchesSquared;
            return (unitVectorAlongLine * dotProduct).EndPoint;
        }

        /// <summary>
        /// projects a point onto a plane from the normal direction.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public Point ProjectOntoPlane(Plane plane)
        {
            Vector toPlane = new Vector(this, plane.NormalVector);
            return ((Line)toPlane).IntersectWithPlane(plane);
        }

        /// <summary>
        /// Returns true if the point is on the passed line, false otherwise
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public bool IsOnLine(Line passedLine)
        {
            return passedLine.Contains(this);
        }

        /// <summary>
        /// Returns true if the point is on the passed vector, false otherwise
        /// </summary>
        /// <param name="passedVector"></param>
        /// <returns></returns>
        public bool IsOnVector(Vector passedVector)
        {
            return passedVector.Contains(this);
        }

        /// <summary>
        /// Returns true if the point is on the passed line segment, false otherwise
        /// </summary>
        /// <param name="passedLineSegment"></param>
        /// <returns></returns>
        public bool IsOnLineSegment(LineSegment passedLineSegment)
        {
            return passedLineSegment.Contains(this);
        }

        public bool IsBaseOrEndPointOf(Vector vector)
        {
            if (this == vector.BasePoint || this == vector.EndPoint)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the point is on the plane.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public bool IsOnPlane(Plane plane)
        {
            return plane.Contains(this);
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
            //correcly - for a full explanation look in Shift.cs where isNegatedShift is declared
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

        #endregion

        #region Static Factory Methods

        public static Point MakePointWithInches(double inputValue1, double inputValue2, double inputValue3 = 0)
        {
            Distance dim1 = new Distance(DistanceType.Inch, inputValue1);
            Distance dim2 = new Distance(DistanceType.Inch, inputValue2);
            Distance dim3 = new Distance(DistanceType.Inch, inputValue3);

            return new Point(dim1, dim2, dim3);
        }

        public static Point MakePointWithMillimeters(double inputValue1, double inputValue2, double inputValue3 = 0)
        {
            Distance dim1 = new Distance(DistanceType.Millimeter, inputValue1);
            Distance dim2 = new Distance(DistanceType.Millimeter, inputValue2);
            Distance dim3 = new Distance(DistanceType.Millimeter, inputValue3);

            return new Point(dim1, dim2, dim3);
        }

        public static Point MakePointWithInches(string inputString1, string inputString2)
        {
            double inputValue1 = double.Parse(inputString1);
            double inputValue2 = double.Parse(inputString2);

            Distance dim1 = new Distance(DistanceType.Inch, inputValue1);
            Distance dim2 = new Distance(DistanceType.Inch, inputValue2);
            return new Point(dim1, dim2);
        }

        public static Point[] Make2DPointArrayWithInches(double[] values)
        {
            Point[] toReturn = null;

            if (values.Length % 2 == 0)
            {
                toReturn = new Point[values.Length / 2];

                for (int i = 0; i < values.Length; i += 2)
                {
                    toReturn[i / 2] = MakePointWithInches(values[i], values[i + 1]);
                }
            }

            return toReturn;
        }

        #endregion
    }
}
