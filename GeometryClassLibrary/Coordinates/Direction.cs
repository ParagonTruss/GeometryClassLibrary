using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnitClassLibrary;
using Newtonsoft.Json;

namespace GeometryClassLibrary
{
    /// <summary>
    /// Stores the values of direction using polar coordinates using physic's conventions (theta = angle to z axis, phi = anglin in xy-plane)
    /// Can be thought of either as two angles or as a unit vector
    /// Note: there are singularities (basically a point where the system breaks down) in this system
    /// when theta = 0 or 180 because the phi angle no longer has meaning
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Direction
    {
        #region Properties and Fields
        /// <summary>
        /// A staticly defined Direction that is positive in the X direction, which can be thought of as going "right" in the XY plane.
        /// This is also the "zero" equivalent and is the same as Direction.Right
        /// </summary>
        public static readonly Direction Right = new Direction(new Angle(), new Angle(AngleType.Degree, 90));
        /// <summary>
        /// A staticly defined Direction that is positive in the Y direction, which can be thought of as going "up" in the XY plane
        /// </summary>
        public static readonly Direction Up = new Direction(new Angle(AngleType.Degree, 90));
        /// <summary>
        /// A staticly defined Direction that is negative in the X direction, which can be thought of as going "left" in the XY plane
        /// </summary>
        public static readonly Direction Left = new Direction(new Angle(AngleType.Degree, 180));
        /// <summary>
        /// A staticly defined Direction that is negative in the Y direction, which can be thought of as going "down" in the XY plane
        /// </summary>
        public static readonly Direction Down = new Direction(new Angle(AngleType.Degree, 270));
        /// <summary>
        /// A staticly defined Direction that is positive in the Z direction, which can be thought of comming out of the screen towards you when viewing the XY plane
        /// </summary>
        public static readonly Direction Out = new Direction(new Angle(), new Angle(AngleType.Degree, 0));
        /// <summary>
        /// A staticly defined Direction that is negative in the Z direction, which can be thought of going back out of the screen away you when viewing the XY plane
        /// </summary>
        public static readonly Direction Back = new Direction(new Angle(), new Angle(AngleType.Degree, 180));

        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azumuth)
        /// currently, this should be between 0 and 360
        /// </summary>
        private Angle _phi;
        [JsonProperty]
        public Angle Phi
        {
            get { return _phi; }
            set {_phi = value; }
        }

        /// <summary>
        /// keeps track of the angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// Currently, this should be between 0 and 180
        /// </summary>
        private Angle _theta;
        [JsonProperty]
        public Angle Theta
        {
            get { return _theta; }
            set
            {
                _theta = value;

                //make sure that theta (angle to z-axis) is between 0 and 180
                //Note: if it is between 180 and 360, we need to flip the directon of phi as well

                //if it is greater than 180, we need to flip the _phi and the _theta by subtracting 180 degrees
                if (_theta > new Angle(AngleType.Degree, 180))
                {
                    _theta = _theta.Reverse();

                    //we throw if it is null because then the Phi will not be reversed in direction to reflect the change we made in theta
                    if (this.Phi != null)
                    {
                        this.Phi = this.Phi.Reverse();
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
        public double XComponent
        {
            get { return Math.Cos(this.Phi.Radians) * Math.Sin(this.Theta.Radians); }
        }

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public double YComponent
        {
            get { return Math.Sin(this.Phi.Radians) * Math.Sin(this.Theta.Radians); }
        }

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public double ZComponent
        {
            get { return Math.Cos(this.Theta.Radians); }
        }

        #endregion

        #region Constructors

        ///// <summary>
        ///// Empty Constructor that makes a angle equivalent to 0 direction in the xy-plane (This means theta is 90)
        ///// </summary>
        public Direction()
        {
            this.Phi = new Angle();
            this.Theta = new Angle(AngleType.Degree, 90);
        }

        public Direction(double x, double y, double z)
        {
            var rho = Math.Sqrt(x*x + y*y + z*z);
            var r = Math.Sqrt(x*x + y*y);
            this.Theta = Angle.ArcSin(r / rho);
            this.Phi = Angle.ArcCos(x / r);
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
        public Direction(Point directionPoint, Distance acceptedDeviationConstant = null)
        {
            this.Phi = new Angle();
            this.Theta = new Angle(AngleType.Degree, 90);
            //if they didnt pass in a value, use the default
            if (acceptedDeviationConstant == null)
            {
                acceptedDeviationConstant = DeviationDefaults.AcceptedEqualityDeviationDistance;
            }

            Distance distanceToOrigin = directionPoint.DistanceTo(new Point());

            //if it is the origin we just leave it as the base constructor
            if (!_lessThanButNotEqualToAcceptanceConstantFrom0(distanceToOrigin, acceptedDeviationConstant))
            {
                //if the z is 0 than the angle should be 90 so we can use the xyplane angle
                //Note: we called the base contructor first so it is already 90 unless we change it
                if (!_lessThanButNotEqualToAcceptanceConstantFrom0(directionPoint.Z, acceptedDeviationConstant))
                {
                    //arcos handles negatives how we want so we dont have to worry about it
                    this.Theta = new Angle(AngleType.Radian, Math.Acos(directionPoint.Z / distanceToOrigin));
                }

                //if the x is not zero then we use it for the phi - other wise check y and if y is 0 also then we want it either up or down
                if (!_lessThanButNotEqualToAcceptanceConstantFrom0(directionPoint.X, acceptedDeviationConstant))
                {
                    //if y is 0 then we are either left or right
                    if (!_lessThanButNotEqualToAcceptanceConstantFrom0(directionPoint.Y, acceptedDeviationConstant))
                    {
                        //Atan handels negative y fine, but not negative x so use absolute value and worry about fixing for x later
                        this.Phi = new Angle(AngleType.Radian, Math.Atan(directionPoint.Y / directionPoint.X.AbsoluteValue()));

                        //this will handle x being negative since we ignored it earlier so we dont have to worry about y's sign
                        //we can use this because we know at this point x isnt equal to zero and if we used Distance it could be
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
                    //check the y still because we could just have really small x and ys in which case we probaly want it up or down
                    //and we know x is zero already so we dont have to check it
                    if (!_lessThanButNotEqualToAcceptanceConstantFrom0(directionPoint.Y, acceptedDeviationConstant))
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
                    else
                    {
                        //if we were close enough in x and y then we really should be either up or down so if its closer to
                        //up then make it up
                        if(this.Theta < new Angle(AngleType.Degree, 90))
                        {
                            this.Theta = new Angle();
                        }
                        else
                        {
                            this.Theta = new Angle(AngleType.Degree, 180);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compares the two distances to see if the first is less than the accepted deviance constant from 0
        /// </summary>
        /// <param name="distance">The distance to check if it is closer to zero that then deviation constant</param>
        /// <param name="acceptedDeviationConstant">The Distance value of which we care about and distances within this we consider equal to each other</param>
        /// <returns>Returns whether or not the distance is closer to 0 than the deviation constant</returns>
        private static bool _lessThanButNotEqualToAcceptanceConstantFrom0(Distance distance, Distance acceptedDeviationConstant)
        {
            return (!(distance.Inches == acceptedDeviationConstant.Inches)) && distance.EqualsWithinDeviationConstant(new Distance(), acceptedDeviationConstant);
        }

        /// <summary>
        /// Makes a direction that represents the angle of the second point realtive to the first
        /// </summary>
        /// <param name="basePoint">The first point to find the angle from</param>
        /// <param name="endPoint">The point to use to find the angle of</param>
        public Direction(Point basePoint, Point endPoint, Distance acceptedDeviationConstant = null)
            : this(endPoint - basePoint, acceptedDeviationConstant) { }

        /// <summary>
        /// Creates a direction with the given angles, but throws an error if phi is not in range if it is set to 
        /// not allow angles outside its bounds (0 <= theta <= 180)
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the positive x-axis in the xy-plane</param>
        /// <param name="angleToZAxis">The angle from the positive z-axis</param>
        /// <param name="allowAnglesOutOfBounds">If it is true it will adjust the given angles to within the proper bounds, otherwise
        /// if the are outside it will throw an error</param>
        public Direction(Angle phi, Angle theta, Boolean allowAnglesOutOfBounds = false)
        {
            this.Phi = phi;
            this.Theta = theta;

            //if we can have angles outside the phi bound than adjust ours if necessary
            if (!allowAnglesOutOfBounds)
            {
                //if we give it a value outside of what we would expect throw an exception
                if (theta < new Angle() || theta > new Angle(AngleType.Degree, 180))
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

                Angle angleBetween = this.AngleBetween(comparableDirection);
                bool angleIsCloseToZero = (angleBetween == new Angle());
                return angleIsCloseToZero;
                ////now check if the angles are the same
                //bool phiEqual = (comparableDirection.Phi == this.Phi);
                //bool thetaEqual = (comparableDirection.Theta == this.Theta);

                ////we need to check if the angles are both the equivalent singularities (basically a point where the system breaks down - in this
                ////  case when theta = 0 or 180 because the phi no longer has meaning) and in this case we only care about the theta angle since
                ////  the phi has no meaning
                //bool areBothSameSingularity = thetaEqual && (this.Theta == new Angle() || this.Theta == new Angle(AngleType.Degree, 180));

                ////returns true if all were true of false if at least one was not
                //return areBothSameSingularity || (phiEqual && thetaEqual);

                
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
        /// Should Use a wrapper class when accessing from a Line or any of its children!!!
        /// Returns a unit vector with a length of 1 in with the given Distance that is equivalent to this direction
        /// Note: if you want a generic unitvector, you must call each of the components individually and keep track of them
        /// </summary>
        /// <param name="passedType">Dimension Type that will be used. The vector will have a length of 1 in this unit type</param>
        /// <returns></returns>
        public Vector UnitVector(DistanceType passedType)
        {
            Distance magnitude = new Distance(passedType, 1);
            Direction direction = new Direction(new Point(passedType, XComponent, YComponent, ZComponent), new Distance(DistanceType.Inch, 0.0001));
            return new Vector(new Point(), direction, magnitude);
        }

        /// <summary>
        /// Creates a new direction that points in the reverse direction of this direction
        /// </summary>
        /// <returns>A new direction that points in the oppposite direction as this one</returns>
        public Direction Reverse()
        {
            //reverse phi(xy) and then complement theta (from z) around 90 since its only 180 range
            return new Direction(this.Phi.Reverse(), new Angle(AngleType.Degree, 180) - this.Theta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherDirection"></param>
        /// <returns></returns>
        public double DotProduct(Direction otherDirection)
        {

            var xTerm = this.XComponent * otherDirection.XComponent;
            var yTerm = this.YComponent * otherDirection.YComponent;
            var zTerm = this.ZComponent * otherDirection.ZComponent;
            
            var sum = xTerm + yTerm + zTerm;
            return sum;
        }

        public Direction CrossProduct(Direction otherDirection)
        {
            Vector vector1 = this.UnitVector(DistanceType.Inch);
            Vector vector2 = otherDirection.UnitVector(DistanceType.Inch);
            Vector product = vector1.CrossProduct(vector2);
            return product.Direction;
        }

        public Angle AngleBetween(Direction direction)
        {
            var dotProduct = this.DotProduct(direction);
            Angle angle = Angle.ArcCos(dotProduct);
            return angle;
        }

        #endregion
    }
}
