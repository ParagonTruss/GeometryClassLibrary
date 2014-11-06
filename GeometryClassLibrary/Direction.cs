using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [DebuggerDisplay("Azumuth = {Direction.Phi.Degrees}, Inclination{Direction.Theta.Degrees}")]
    [Serializable]
    public class Direction
    {
        #region Properties and Fields

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azumuth)
        /// currently, this should be between 0 and 360
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
        /// Currently, this should be between 0 and 180
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
            this.Phi = new Angle(xyPlaneAngle);
            this.Theta = new Angle(AngleType.Degree, 90);
        }

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        /// <param name="directionPoint">the point to find the angle to relative to the origin</param>
        /// <param name="acceptedDeviationConstant">The value to use for accepted deviation constant for if the distances are small</param>
        public Direction(Point directionPoint, Dimension? acceptedDeviationConstant = null)
            : this()
        {
            //if they didnt pass in a value, use the default
            if (acceptedDeviationConstant == null)
            {
                acceptedDeviationConstant = DeviationConstants.AcceptedEqualityDeviationDimension;
            }

            Dimension distanceToOrigin = directionPoint.DistanceTo(new Point());

            //if it is the origin we just leave it as the base constructor
            if (!distanceToOrigin.EqualsWithinPassedAcceptedDeviation(new Dimension(), acceptedDeviationConstant.Value))
            {
                //if the z is 0 than the angle should be 90 so we can use the xyplane angle
                //Note: we called the base contructor first so it is already 90 unless we change it
                if (!directionPoint.Z.EqualsWithinPassedAcceptedDeviation(new Dimension(), acceptedDeviationConstant.Value))
                {
                    //arcos handles negatives how we want so we dont have to worry about it
                    this.Theta = new Angle(AngleType.Radian, Math.Acos(directionPoint.Z / distanceToOrigin));
                }

                //if the x is zero it is either straight up or down
                if (!directionPoint.X.EqualsWithinPassedAcceptedDeviation(new Dimension(), acceptedDeviationConstant.Value))
                {
                    if (!directionPoint.Y.EqualsWithinPassedAcceptedDeviation(new Dimension(), acceptedDeviationConstant.Value))
                    {
                        //Atan handels negative y fine, but not negative x so use absolute value and worry about fixing for x later
                        this.Phi = new Angle(AngleType.Radian, Math.Atan(directionPoint.Y / directionPoint.X.AbsoluteValue()));

                        //this will handle x being negative since we ignored it earlier so we dont have to worry about y's sign
                        //we can use this because we know at this point x isnt equal to zero and if we used dimension it could be
                        //to close within the default deviation constant but not in the passed one
                        if (directionPoint.X.Inches < 0)
                        {
                            this.Phi = new Angle(AngleType.Degree, 180) - this.Phi;
                        }
                    }
                    else
                    {
                        //we know y is zero and x is non zero so we just need to know which way x is and we use inches
                        //in case the passed deviation is smaller than the default
                        if (directionPoint.X.Inches > 0)
                        {
                            this.Phi = new Angle(AngleType.Degree, 0);
                        }
                        else if (directionPoint.X.Inches < 0)
                        {
                            this.Phi = new Angle(AngleType.Degree, 180);
                        }
                    }
                }
                else
                {
                    //set the angle based on if the y is positive or negative
                    //if y is also zero, just make the xy an angle of 0
                    //Note: we called the base contructor first so it is already 0 unless we change it
                    //Also we already know x is zero so we only care about the Y direction and we use inches
                    //in case the passed deviation is smaller than the default
                    if (directionPoint.Y.Inches > 0)
                    {
                        this.Phi = new Angle(AngleType.Degree, 90);
                    }
                    else if (directionPoint.Y.Inches < 0)
                    {
                        this.Phi = new Angle(AngleType.Degree, 270);
                    }
                }
            }
        }

        /// <summary>
        /// Makes a direction that represents the angle of the second point realtive to the first
        /// </summary>
        /// <param name="basePoint">The first point to find the angle from</param>
        /// <param name="endPoint">The point to use to find the angle of</param>
        public Direction(Point basePoint, Point endPoint, Dimension? acceptedDeviationConstant = null)
            : this(endPoint - basePoint, acceptedDeviationConstant) { }

        /// <summary>
        /// Creates a direction with the given angles, but throws an error if phi is not in range if it is set to 
        /// not allow angles outside its bounds (0 <= theta <= 180)
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the positive x-axis in the xy-plane</param>
        /// <param name="angleToZAxis">The angle from the positive z-axis</param>
        /// <param name="allowAnglesOutOfBounds">If it is true it will adjust the given angles to within the proper bounds, otherwise
        /// if the are outside it will throw an error</param>
        public Direction(Angle xyPlaneAngle, Angle angleToZAxis, Boolean allowAnglesOutOfBounds = false)
        {
            this.Phi = xyPlaneAngle;
            this.Theta = angleToZAxis;

            //if we can have angles outside the phi bound than adjust ours if necessary
            if (!allowAnglesOutOfBounds)
            {
                //if we give it a value outside of what we would expect throw an exception
                if (angleToZAxis < new Angle() && angleToZAxis > new Angle(AngleType.Degree, 180))
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            try
            {
                //try casting to a direction
                Direction comparableDirection = (Direction)obj;

                //now check if the angles are the same
                bool phiEqual = (comparableDirection.Phi == this.Phi);
                bool thetaEqual = (comparableDirection.Theta == this.Theta);

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
        /// Should Use a wrapper class when accessing from a Line or any of ots children!!!
        /// Returns a unit vector with a length of 1 in with the given dimension that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// </summary>
        /// <param name="passedType">Dimesnion Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public Vector UnitVector(DimensionType passedType)
        {
            Dimension magnitude = new Dimension(passedType, 1);
            Direction direction = new Direction(new Point(passedType, XComponentOfDirection, YComponentOfDirection, ZComponentOfDirection), new Dimension(DimensionType.Inch, 0.0001));
            return new Vector(new Point(), direction, magnitude);
        }

        /// <summary>
        /// Creates a new direction that points in the reverse direction of this direction
        /// </summary>
        /// <returns>A new direction that points in the oppposite direction as this one</returns>
        public Direction Reverse()
        {
            return new Direction(this.Phi - new Angle(AngleType.Degree, 180), new Angle(AngleType.Degree, 180) - this.Theta);
        }

        /// <summary>
        /// Dot products the two Directions together
        /// </summary>
        /// <param name="otherDirection"></param>
        /// <returns></returns>
        public Vector DotProduct(Direction otherDirection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
