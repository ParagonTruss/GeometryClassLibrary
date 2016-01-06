using System;
using Newtonsoft.Json;
using UnitClassLibrary;
using System.Diagnostics;
using UnitClassLibrary.DistanceUnit;
using UnitClassLibrary.DistanceUnit.DistanceTypes;
using UnitClassLibrary.AngleUnit;
using GeometryClassLibrary.Vectors;
using static UnitClassLibrary.AngleUnit.Angle;
using static UnitClassLibrary.DistanceUnit.Distance;


namespace GeometryClassLibrary
{
    /// <summary>
    /// St
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Direction
    {
        #region Properties and Fields

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azimuth)
        /// currently, this should be between 0 and 360
        /// </summary>     
        public Angle Phi { get { throw new NotImplementedException(); } }

        /// <summary>
        /// The angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// </summary>
        public Angle Theta { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets for the x-component of this directions unitVector
        /// </summary>
        public Measurement X { get { return Normalized.X; } }

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public Measurement Y { get { return Normalized.Y; } }

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public Measurement Z { get { return Normalized.Z; } }
        [JsonProperty]
        private MeasurementVector _vector;
        private MeasurementVector _normalized;

        private MeasurementVector Normalized
        {
            get
            {
                if (_normalized == null)
                {
                    _normalized = Normalize(_vector);
                }
                return _normalized;
            }
            set
            {
                _normalized = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Null Constructor
        /// </summary>
        private Direction() { }

        public Direction(Measurement x, Measurement y, Measurement z)
        {
            this._vector = new MeasurementVector(x, y, z);
        }
   
        /// <summary>
        /// Create a Direction with no Z component, by giving the angle in the XY plane.
        /// </summary>
        public Direction(Angle xyPlaneAngle)
        {
            var X = Angle.Cosine(xyPlaneAngle);
            var Y = Angle.Sine(xyPlaneAngle);
            this._vector = new MeasurementVector(X, Y);
            this._normalized = _vector;
        }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        public Direction(Point point)
        {
            this._vector = new MeasurementVector(point.X.InInches, point.Y.InInches, point.Z.InInches);
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
            var X = Angle.Cosine(phi) * Angle.Sine(theta);
            var Y = Angle.Sine(phi) * Angle.Sine(theta);
            var Z = Angle.Cosine(theta);
            this._vector = new MeasurementVector(X, Y, Z);
            this._normalized = _vector;
        }
       
        /// <summary>
        /// Creates a copy of the given Direction
        /// </summary>
        /// <param name="toCopy"></param>
        public Direction(Direction toCopy)
        {
            this._vector = toCopy._vector;
            this._normalized = toCopy._normalized;
        }

        [JsonConstructor]
        public Direction(MeasurementVector genericVector)
        {
            this._vector = genericVector;
        }

        public static MeasurementVector Normalize(MeasurementVector vector)
        {
            var x = vector.X;
            var y = vector.Y;
            var z = vector.Z;
            Direction result = new Direction();
            var r = (x * x + y * y + z * z).SquareRoot();
            if (r == Measurement.Zero)
            {
                return MeasurementVector.Zero;
            }
            else
            {
                var d = r.Value;
                return new MeasurementVector(x / d, y / d, z / d);     
            }
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
            if (this._vector.Magnitude() == new Measurement(0, 0) &&
                dir._vector.Magnitude()== new Measurement(0, 0))
            {
                return true;
            }
            Angle angleBetween = this.AngleBetween(dir);
            bool angleIsCloseToZero = (angleBetween == Angle.ZeroAngle);
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
            return new Direction( _vector.Reverse());
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
            var result = this._vector.CrossProduct(otherdirection._vector);
            return new Direction(result);
        }

        public Angle AngleBetween(Direction direction)
        {
            var result = this._vector.AngleBetween(direction._vector);
            return result;
        }


        /// <summary>
        /// finds the signed angle between two directions.
        /// i.e. the order you input the vectors matters: the angle from direction1 to direction2 is negative the angle from direction2 to direction1
        /// The reference normal is what counts as "up" for determining sign.
        /// it defaults to the z direction, because this method will usuallly be used on the XYPLane.
        /// </summary>
        /// <returns></returns>
        public Angle AngleFromThisToThat(Direction direction, Direction referenceNormal = null)
        {
            if (referenceNormal == null)
            {
                referenceNormal = Out;
            }

            Angle testAngle = this.AngleBetween(direction);
            Direction testNormal = this.CrossProduct(direction);
           
            if (testNormal == null || testAngle % Angle.StraightAngle == Angle.ZeroAngle || testNormal == referenceNormal)
            {
                return testAngle;
            }
            if (testNormal.Reverse() == referenceNormal)
            {
                return (Angle.FullCircle - testAngle);
            }
            

            throw new Exception("The reference normal is not perpendicular to these vectors");
        }

        public bool IsPerpendicularTo(Direction d)
        {
            return AngleBetween(d) == Angle.RightAngle;
        }

        public bool IsParallelTo(Direction d)
        {
            var angle = AngleBetween(d);
            return angle == Angle.ZeroAngle ||
                angle == 180 * new Angle(1, Degrees);
        }
        #endregion
    }
}
