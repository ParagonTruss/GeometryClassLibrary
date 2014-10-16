using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Stores the values of direction using polar coordinates using physic's conventions (theta = angle to z axis, phi = anglin in xy-plane)
    /// Can be thought of either as two angles or as a unit vector
    /// </summary>
    public class Direction
    {
        #region fields and Properties

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azumuth)
        /// </summary>
        private Angle _phi;
        public Angle Phi 
        {
            get { return _phi; }
            set
            {
                _phi = value;

                //now make sure that it is > 0 and less than 360
                while (_phi >= new Angle(AngleType.Degree, 360))
                {
                    _phi -= new Angle(AngleType.Degree, 360);
                }
                while (_phi < new Angle())
                {
                    _phi += new Angle(AngleType.Degree, 360);
                }
            }
        }

        /// <summary>
        /// keeps track of the angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// </summary>
        private Angle _theta;
        public Angle Theta
        {
            get { return _theta; }
            set
            {
                _theta = value;

                //make sure that theta (angle to z-axis) is between 0 and 180
                //Note: if it is between 180 and 360, we need to flip the directon of phi as well

                //first make sure it is between 0 and 360
                while (_theta >= new Angle(AngleType.Degree, 360))
                {
                    _theta -= new Angle(AngleType.Degree, 360);
                }
                while (_theta < new Angle())
                {
                    _theta += new Angle(AngleType.Degree, 360);
                }

                //now if it is greater than 180, we need to flip the _phi and the _theta by subtracting 180 degrees
                if (_theta > new Angle(AngleType.Degree, 180))
                {
                    _theta -= new Angle(AngleType.Degree, 180);

                    //we throw if it is null because then the Phi will not be reversed in direction to reflect the change we made in theta
                    if (this.Phi != null)
                    {
                        this.Phi -= new Angle(AngleType.Degree, 180);
                    }
                    else
                    {
                        throw new InvalidOperationException("Phi must be initialized before Theta in a Direction");
                    }
                }
            }
        }

        /// <summary>
        /// Gets for the x-component of this directions unitVector
        /// </summary>
        public double XComponentOfDirection
        {
            get { return Math.Cos(this.Phi.Radians) * Math.Sin(this.Theta.Radians); }
        }

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public double YComponentOfDirection
        {
            get { return Math.Sin(this.Phi.Radians) * Math.Sin(this.Theta.Radians); }
        }

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public double ZComponentOfDirection
        {
            get { return Math.Cos(this.Theta.Radians); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor that makes a angle equivalent to 0 direction in the xy-plane (This means theta is 90)
        /// </summary>
        public Direction()
        {
            this.Phi = new Angle();
            this.Theta = new Angle(AngleType.Degree, 90);
        }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        /// <param name="directionPoint"></param>
        public Direction(Point directionPoint)
            : this()
        {
            Dimension distanceToOrigin = directionPoint.DistanceTo(new Point());

            //if it is the origin we just leave it as the base constructor
            if (distanceToOrigin != new Dimension())
            {
                //if the z is 0 than the angle should be 90 so we can use the xyplane angle
                //Note: we called the base contructor first so it is already 90 unless we change it
                if (directionPoint.Z != new Dimension())
                {
                    //arcos handles negatives how we want so we dont have to worry about it
                    this.Theta = new Angle(AngleType.Radian, Math.Acos(directionPoint.Z / distanceToOrigin));
                }

                //if the x is zero it is either straight up or down
                if (directionPoint.X != new Dimension())
                {
                    //Atan handels negative y fine, but not negative x so use absolute value and worry about fixing for x later
                    this.Phi = new Angle(AngleType.Radian, Math.Atan(directionPoint.Y / directionPoint.X.AbsoluteValue()));

                    //this will handle x being negative since we ignored it earlier so we dont have to worry about y's sign
                    if (directionPoint.X < new Dimension())
                    {
                        this.Phi = new Angle(AngleType.Degree, 180) - this.Phi;
                    }
                }
                else
                {
                    //set the angle based on if the y is positive or negative
                    //if y is also zero, just make the xy an angle of 0
                    //Note: we called the base contructor first so it is already 0 unless we change it
                    if (directionPoint.Y > new Dimension())
                    {
                        this.Phi = new Angle(AngleType.Degree, 90);
                    }
                    else if (directionPoint.Y < new Dimension())
                    {
                        this.Phi = new Angle(AngleType.Degree, 270);
                    }
                }
            }
        }

        /// <summary>
        /// makes the direction based on the vector ( jsut copies its direction)
        /// </summary>
        /// <param name="directionVector">The vector to use the direction of</param>
        public Direction(Vector directionVector)
            : this(directionVector.Direction) { }

        /// <summary>
        /// Creates a new direction defaulting the angle to the Z axis to 90 degrees, effictivly making it keep track of only the xy plane angle
        /// for 2D manipulations 
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the x-axis in the xy-plane</param>
        public Direction(Angle xyPlaneAngle)
        {
            this.Phi = xyPlaneAngle;
            this.Theta = new Angle(AngleType.Degree, 90);
        }

        /// <summary>
        /// Creates a direction with the given angles
        /// Note: throws an error if a value not in the range 0 < theta < 180
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the positive x-axis in the xy-plane</param>
        /// <param name="angleToZAxis">The angle from the positive z-axis</param>
        public Direction(Angle xyPlaneAngle, Angle angleToZAxis)
            : this (xyPlaneAngle, angleToZAxis, false) { }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        /// <param name="directionPoint"></param>
        public Direction(Point basePoint, Point endPoint)
            : this(endPoint - basePoint) { }
       
        /// <summary>
        /// Creates a direction with the given angles, but throws an error if phi is not in range if it is set to 
        /// not allow angles outside its bounds (0 <= theta <= 180)
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the positive x-axis in the xy-plane</param>
        /// <param name="angleToZAxis">The angle from the positive z-axis</param>
        /// <param name="allowAnglesOutOfBounds">If it is true it will adjust the given angles to within the proper bounds, otherwise
        /// if the are outside it will throw an error</param>
        private Direction(Angle phi, Angle theta, Boolean allowAnglesOutOfBounds)
        {
            this.Phi = phi;
            this.Theta = theta;

            //if we can have angles outside the phi bound than adjust ours if necessary
            if (!allowAnglesOutOfBounds)
            {
                //if we give it a value outside of what we would expect throw an exception
                if (theta < new Angle() && theta > new Angle(AngleType.Degree, 180))
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Creates a copy of the given Direction
        /// </summary>
        /// <param name="toCopy"></param>
        public Direction(Direction toCopy)
        {
            this.Theta = new Angle(toCopy.Theta);
            this.Phi = new Angle(toCopy.Phi);
        }


        #endregion

        #region Overloaded Operators

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
            if ((object)direction1 == null)
            {
                if ((object)direction2 == null)
                {
                    return false;
                }
                return true;
            }
            return !direction1.Equals(direction2);
        }

        public override bool Equals(object direction)
        {
            if (direction == null)
            {
                return false;
            }            

            try
            {
                //try casting to a direction
                Direction vec = (Direction)direction;

                //now check if the angles are the same
                Boolean phiEqual = (vec.Phi == this.Phi);
                Boolean thetaEqual = (vec.Theta == this.Theta);

                //returns true if all were true of false if at least one was not
                return (phiEqual && thetaEqual);
            }
            //wasnt even a direction so it must not be equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a unit vector with a length of 1 in with the given dimension that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// </summary>
        /// <param name="passedType">Dimesnion Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public Vector UnitVector(DimensionType passedType)
        {
            return new Vector(new Point(passedType, XComponentOfDirection, YComponentOfDirection, ZComponentOfDirection));
        }

        public Direction Reverse()
        {
            return new Direction(this.Phi - new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 180) - this.Theta);
        }

        public Vector DotProduct(Direction otherDirection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
