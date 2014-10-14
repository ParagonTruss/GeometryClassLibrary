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
    /// </summary>
    class Direction
    {
        #region fields and Properties


        /// <summary>
        /// The angle from the positive x-axis in the xy-plane (azumuth)
        /// </summary>
        private Angle _phi;
        public Angle Phi
        {
            get { return _phi; }
        }

        /// <summary>
        /// keeps track of the angle from the positive z-axis (should be a max of 180 degrees) (inclination)
        /// </summary>
        private Angle _theta;
        public Angle Theta
        {
            get { return _theta; }
        }

        /// <summary>
        /// Gets for the x-component of this directions unitVector
        /// </summary>
        public double XComponentOfDirection
        {
            get { return Math.Cos(_phi.Radians) * Math.Sin(_theta.Radians); }
        }

        /// <summary>
        /// Gets for the y-component of this directions unitVector
        /// </summary>
        public double YComponentOfDirection
        {
            get { return Math.Sin(_phi.Radians) * Math.Sin(_theta.Radians); }
        }

        /// <summary>
        /// Gets for the z-component of this directions unitVector
        /// </summary>
        public double ZComponentOfDirection
        {
            get { return Math.Cos(_theta.Radians); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor that makes the angles at zero (note: this will mean it is pointed positive in the z direction since phi is measured form positive z)
        /// </summary>
        public Direction()
        {
            _phi = new Angle();
            _theta = new Angle();
        }

        /// <summary>
        /// Creates a new direction defaulting the angle to the Z axis to 90 degrees, effictivly making it keep track of only the xy plane angle
        /// for 2D manipulations 
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the x-axis in the xy-plane</param>
        public Direction(Angle xyPlaneAngle)
        {
            _phi = xyPlaneAngle;
            _theta = new Angle(AngleType.Degree, 90);
        }

        /// <summary>
        /// Creates a direction with the given angles
        /// Note: throws an error if a value not in the range 0 < theta < 180
        /// </summary>
        /// <param name="xyPlaneAngle">The angle from the positive x-axis in the xy-plane</param>
        /// <param name="angleToZAxis">The angle from the positive z-axis</param>
        public Direction(Angle xyPlaneAngle, Angle angleToZAxis)
        {
            if (angleToZAxis >= new Angle() && angleToZAxis <= new Angle(AngleType.Degree, 180))
            {
                _phi = xyPlaneAngle;
                _theta = angleToZAxis;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        
        /// <summary>
        /// makes the direction based on the vector ( jsut copies its direction)
        /// </summary>
        /// <param name="directionVector">The vector to use the direction of</param>
        public Direction(Vector directionVector)
            : this (directionVector.Direction){}

        /// <summary>
        /// Makes a direction from the origin to the given point
        /// </summary>
        /// <param name="directionPoint"></param>
        public Direction(Point directionPoint)
        {
            _theta = new Angle(AngleType.Radian, Math.Acos(directionPoint.Z /directionPoint.DistanceTo(new Point())));
            _phi = new Angle(AngleType.Radian, Math.Atan(directionPoint.Y / directionPoint.X));
        }

        /// <summary>
        /// Creates a copy of the given Direction
        /// </summary>
        /// <param name="toCopy"></param>
        public Direction(Direction toCopy)
        {
            this._theta = new Angle(toCopy._theta);
            this._phi = new Angle(toCopy._phi);
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

        #endregion
    }
}
