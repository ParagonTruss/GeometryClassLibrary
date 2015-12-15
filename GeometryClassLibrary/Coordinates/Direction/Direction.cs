using System;
using Newtonsoft.Json;
using UnitClassLibrary;
using System.Diagnostics;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.AngleUnit;
using UnitClassLibrary.GenericUnit;
using GeometryClassLibrary.Vectors;

namespace GeometryClassLibrary
{
    /// <summary>
    /// St
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Direction : IVector
    {
        #region Properties and Fields

        public static readonly Direction NoDirection = new Direction(Measurement.Zero, Measurement.Zero, Measurement.Zero);
        /// <summary>
        /// A staticly defined Direction that is positive in the X direction, which can be thought of as going "right" in the XY plane.
        /// </summary>
        public static readonly Direction Right = new Direction(Angle.Zero, new Angle(new Degree(), 90));
        /// <summary>
        /// A staticly defined Direction that is positive in the Y direction, which can be thought of as going "up" in the XY plane
        /// </summary>
        public static readonly Direction Up = new Direction(new Angle(new Degree(), 90));
        /// <summary>
        /// A staticly defined Direction that is negative in the X direction, which can be thought of as going "left" in the XY plane
        /// </summary>
        public static readonly Direction Left = new Direction(new Angle(new Degree(), 180));
        /// <summary>
        /// A staticly defined Direction that is negative in the Y direction, which can be thought of as going "down" in the XY plane
        /// </summary>
        public static readonly Direction Down = new Direction(new Angle(new Degree(), 270));
        /// <summary>
        /// A staticly defined Direction that is positive in the Z direction, which can be thought of coming out of the screen towards you when viewing the XY plane
        /// </summary>
        public static readonly Direction Out = new Direction(Angle.Zero, new Angle(new Degree(), 0));
        /// <summary>
        /// A staticly defined Direction that is negative in the Z direction, which can be thought of going back out of the screen away you when viewing the XY plane
        /// </summary>
        public static readonly Direction Back = new Direction(Angle.Zero, new Angle(new Degree(), 180));

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azimuth)
        /// currently, this should be between 0 and 360
        /// </summary>     
        [JsonProperty]
        public Angle Phi { get { throw new NotImplementedException(); } }

        /// <summary>
        /// The angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// </summary>
        [JsonProperty]
        public Angle Theta { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets for the x-component of this directions unitVector
        /// </summary>
        public Measurement X { get; private set; }

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public Measurement Y { get; private set; }

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public Measurement Z { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        private Direction() { }

        public Direction(Measurement x, Measurement y, Measurement z) : this(DetermineDirection(x,y,z)) { }

   
        /// <summary>
        /// Create a Direction with no Z component, by giving the angle in the XY plane.
        /// </summary>
        public Direction(Angle xyPlaneAngle)
        {
            this.X = Angle.Cosine(xyPlaneAngle);
            this.Y = Angle.Sine(xyPlaneAngle);
            this.Z = new Measurement(0.0);
        }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        public Direction(Point directionPoint) 
            : this (DetermineDirection(directionPoint.X.Measurement,directionPoint.Y.Measurement,directionPoint.Z.Measurement))
        {
        }

        /// <summary>
        /// Makes a direction that represents the angle of the second point realtive to the first
        /// </summary>
        public Direction(Point basePoint, Point endPoint)
            : this(endPoint - basePoint) { }

        /// <summary>
        /// Creates a direction with the given angles. 
        /// </summary>
        public Direction(Angle phi, Angle theta)
        {
            this.X = Angle.Cosine(phi) * Angle.Sine(theta);
            this.Y = Angle.Sine(phi) * Angle.Sine(theta);
            this.Z = Angle.Cosine(theta);
        }
       
        /// <summary>
        /// Creates a copy of the given Direction
        /// </summary>
        /// <param name="toCopy"></param>
        public Direction(Direction toCopy)
        {
            this.X = toCopy.X;
            this.Y = toCopy.Y;
            this.Z = toCopy.Z;
        }

        public static Direction DetermineDirection(Measurement x, Measurement y, Measurement z)
        {
            Direction result = new Direction();
            var r = (x * x + y * y + z * z).SquareRoot();
            if (r == Measurement.Zero)
            {
                result.X = Measurement.Zero;
                result.Y = Measurement.Zero;
                result.Z = Measurement.Zero;
            }
            else
            {             
                result.X = x / r;
                result.Y = y / r;
                result.Z = z / r;
            }
            return result;
        }
        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector operator *(Direction d, Distance m)
        {
            return new Vector(d, m);
        }
        public static Vector operator *(Distance m, Direction d)
        {
            return new Vector(d, m);
        }

        public static bool operator ==(Direction direction1, Direction direction2)
        {
            if ((object)direction1 == null)
            {
                if ((object)direction2 == null)
                {
                    return true;
                }
                return false;
            }
            return direction1.Equals(direction2);
        }

        public static bool operator !=(Direction direction1, Direction direction2)
        {
            return !(direction1 == direction2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            try
            {
                return this.Equals(obj as Direction);   
            }
            //wasnt even a direction so it must not be equal
            catch (InvalidCastException)
            {
                return false;
            }
        }
        public bool Equals(Direction dir)
        {
            if (ReferenceEquals(dir, null))
            {
                return false;
            }
            if (new UnitLessVector(this).Magnitude.IntrinsicValue == Measurement.Zero &&
                new UnitLessVector(dir).Magnitude.IntrinsicValue == Measurement.Zero)
            {
                return true;
            }
            Angle angleBetween = this.AngleBetween(dir);
            bool angleIsCloseToZero = (angleBetween == Angle.Zero);
            return angleIsCloseToZero;
        }

        public override string ToString()
        {
            return String.Format("X = {0}, Y = {1}, Z = {2}", Math.Round(X.Value, 2), Math.Round(Y.Value, 2), Math.Round(Z.Value, 2));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates a vector with this direction and unit length in the passed distance type.
        /// </summary>
        public Vector UnitVector(DistanceType passedType)
        {
            return this * new Distance(passedType, 1);
        }

        /// <summary>
        /// Creates a new direction in the reverse direction of this one.
        /// </summary>
        public Direction Reverse()
        {
            var direction = new Direction();
            direction.X = this.X.Negate();
            direction.Y = this.Y.Negate();
            direction.Z = this.Z.Negate();
            return direction;
        }

        /// <summary>
        /// 
        /// </summary>
        public Measurement DotProduct(Direction otherDirection)
        {
            var xTerm = this.X * otherDirection.X;
            var yTerm = this.Y * otherDirection.Y;
            var zTerm = this.Z * otherDirection.Z;

            var sum = xTerm + yTerm + zTerm;
            return sum;
        }

        public Direction CrossProduct(Direction otherdirection)
        {
            var d1 = this;
            var d2 = otherdirection;

            double xTerm = (d1.Y * d2.Z - d1.Z * d2.Y).Value;
            double yTerm = (d1.Z * d2.X - d1.X * d2.Z).Value;
            double zTerm = (d1.X * d2.Y - d1.Y * d2.X).Value;

            var point = Point.MakePointWithInches(xTerm, yTerm, zTerm);
            if (point != Point.Origin)
            {
                return new Direction(point);
            }
            return null; //The directions are parallel
        }

        public Angle AngleBetween(Direction direction)
        {
            var dotProduct = this.DotProduct(direction);
            Angle angle = Angle.ArcCos(dotProduct);
            return angle;
        }


        /// <summary>
        /// finds the signed angle between two directions.
        /// i.e. the order you input the vectors matters: the angle from direction1 to direction2 is negative the angle from direction2 to direction1
        /// The reference normal is what counts as "up" for determining sign.
        /// it defaults to the z direction, because this method will usuallly be used on the XYPLane.
        /// </summary>
        /// <returns></returns>
        public Angle SignedAngleBetween(Direction direction, Direction referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Out;
            }

            Angle testAngle = this.AngleBetween(direction).ModOutTwoPi;
            Direction testNormal = this.CrossProduct(direction);
           
            if (testNormal == null || testAngle % new Angle(new Degree(), 180) == Angle.Zero || testNormal == referenceNormal)
            {
                return testAngle;
            }
            if (testNormal.Reverse() == referenceNormal)
            {
                return (testAngle.Negate());
            }
            

            throw new Exception("The reference normal is not perpendicular to these vectors");
        }

        public bool IsPerpendicularTo(Direction d)
        {
            return AngleBetween(d) == 90 * Angle.Degree;
        }

        public bool IsParallelTo(Direction d)
        {
            var angle = AngleBetween(d);
            return angle == Angle.Zero ||
                angle == 180 * Angle.Degree;
        }
        #endregion
    }
}
