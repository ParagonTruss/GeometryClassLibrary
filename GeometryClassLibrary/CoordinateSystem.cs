using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// This represents a new coordinated system that is described using the world coordinate system. This class determines 
    /// the translation from the origin to get to the new system as well as the angles about the world's x, y and z axes to 
    /// rotate around in order to get to the desired coordinate system. (executes the rotations: x then y then z)
    /// Note: the "world" coordinate system refers to how we normally percieve the world. The world coordinates are always the same,
    /// but objects can be moved around in it and represent by different sub coordinate systems based on the world ones. This is
    /// how this class works conceptually
    /// Note: because this shift decribes the coordinate system, the effect on objects will be the opposite if those descirbed
    /// for the coordinate system
    /// </summary>
    public class CoordinateSystem
    {
        #region Properties and Fields

        //public static CoordinateSystem CurrentSystem = new CoordinateSystem();

        /// <summary>
        /// The world coordinate System
        /// </summary>
        public readonly static CoordinateSystem WorldCoordinateSystem = new CoordinateSystem();

        /// <summary>
        /// The angle to rotate around the world coordinate system's X axis to get to this coordinate system
        /// </summary>
        public Angle XRotation;

        //public Matrix XMatrix;

        /// <summary>
        /// The angle to rotate around the world coordinate system's Y axis to get to this coordinate system
        /// </summary>
        public Angle YRotation;

        //public Matrix YMatrix;

        /// <summary>
        /// The angle to rotate around the world coordinate system's Z axis to get to this coordinate system
        /// </summary>
        public Angle ZRotation;

        // public Matrix ZMatrix;

        /// <summary>
        /// This coordinate systems origin point relative to the world coordinate system
        /// </summary>
        public Point Origin;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new intance of the world coordinate system
        /// </summary>
        public CoordinateSystem()
        {
            this.Origin = new Point();
            this.XRotation = new Angle();
            this.YRotation = new Angle();
            this.ZRotation = new Angle();
        }

        /// <summary>
        /// Creates a new coordinate system that has the same axis as the world one and is only shifted to the given point
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        public CoordinateSystem(Point passedOrigin)
        {
            Origin = new Point(passedOrigin);
            this.XRotation = new Angle();
            this.YRotation = new Angle();
            this.ZRotation = new Angle();
        }

        /// <summary>
        /// Creates a new coordinate system with the given origin point and with the given rotations, all in repect to the
        /// world coordinate system
        /// </summary>
        /// <param name="passedOrigin">The origin point of this coordinate system in reference to the world coordinate system</param>
        /// <param name="passedXRotation">The rotation around the world coordinate systems X axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedYRotation">The rotation around the world coordinate systems Y axis to rotate around to get to this
        /// coordinate system</param>
        /// <param name="passedZRotation">The rotation around the world coordinate systems Z axis to rotate around to get to this
        /// coordinate system</param>
        public CoordinateSystem(Point passedOrigin, Angle passedZRotation, Angle passedXRotation, Angle passedYRotation)
        {
            Origin = new Point(passedOrigin);
            this.XRotation = new Angle(passedXRotation);
            this.YRotation = new Angle(passedYRotation);
            this.ZRotation = new Angle(passedZRotation);
        }

        /// <summary>
        /// Creates a copy of the given coordinate system
        /// </summary>
        /// <param name="toCopy">the Coordinate System to copy</param>
        public CoordinateSystem(CoordinateSystem toCopy)
        {
            Origin = new Point(toCopy.Origin);
            this.XRotation = new Angle(toCopy.XRotation);
            this.YRotation = new Angle(toCopy.YRotation);
            this.ZRotation = new Angle(toCopy.ZRotation);
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(CoordinateSystem system1, CoordinateSystem system2)
        {
            // covers null reference checks
            if ((object)system1 == null)
            {
                if ((object)system2 == null)
                {
                    return true;
                }
                return false;
            }
            // if the two points' x and y and z are equal, returns true
            return system1.Equals(system2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(CoordinateSystem system1, CoordinateSystem system2)
        {
            // if the two points' x and y are equal, returns false
            if ((object)system1 == null)
            {
                if ((object)system2 == null)
                {
                    return false;
                }
                return true;
            }
            return !system1.Equals(system2);
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

            //try to cast the object to a coordinate System, if it fails then we know the user passed in the wrong type of object
            try
            {
                CoordinateSystem comparableSystem = (CoordinateSystem)obj;

                bool areOriginsEqual = this.Origin == comparableSystem.Origin;
                bool areXAnglesEqual = this.XRotation == comparableSystem.XRotation;
                bool areYAnglesEqual = this.YRotation == comparableSystem.YRotation;
                bool areZAnglesEqual = this.ZRotation == comparableSystem.ZRotation;

                return areOriginsEqual && areXAnglesEqual && areYAnglesEqual && areZAnglesEqual;
            }
            //if they are not the same type than they are not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns this Coordinate System's axes rotations as a single rotation matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetRotationMatrix()
        {
            return Matrix.RotationMatrixAboutZ(this.ZRotation) * Matrix.RotationMatrixAboutX(this.XRotation) * Matrix.RotationMatrixAboutY(this.YRotation);
        }

        /// <summary>
        /// Determines if the two directions represented by the euler triple (x,y, and z angles) are equivalent 
        /// Note: multiple combinations of euler triples can represent the same orientation
        /// </summary>
        /// <param name="toCheckIfEquivalentTo">The Coordinate System to see if this one is equivalent in direction</param>
        /// <returns>Returns a bool of whether or not the two directions are equivalent</returns>
        public bool AreDirectionsEquivalent(CoordinateSystem toCheckIfEquivalentTo)
        {
            //TIM:
            //How can we tell if two rotation matricies represent the same euler triple?
            //this is what I tried but it seemed to not work (see the CoordinateSystem_AreDirectionsEquivalentTests)
            //Is there a way you know of other than just transforming abritrary points and cheking if they are the same
            //with both transformations?
            Matrix thisRotationMatrix = this.GetRotationMatrix();
            Matrix passedRotationMatrix = toCheckIfEquivalentTo.GetRotationMatrix();

            return thisRotationMatrix.Equals(passedRotationMatrix);
        }


        /// <summary>
        /// Returns the shift for this coordinate system to apply to OBJECTS in order to orient them back to the origin
        /// Note: Only works if this is the current shift on the object! if it is already in world coordinates and you 
        /// perform this shift it will move it from the world coordinates to coordinates that are opposite to this one!
        /// </summary>
        /// <returns>Returns the shift to apply to Objects in order to return them from this coordinate system to the world coordinate system</returns>
        public Shift ShiftThatReturnsThisToWorldCoordinateSystem()
        {
            //the simple way is just to create a shift with this coordinate system and then negate it
            return new Shift(this).Negate();
        }

        /// <summary>
        /// Find this coordinate system's (which is currently based on the passed system) shifts relative to the world coordinate 
        /// system instead of the passed coordinate system
        /// </summary>
        /// <param name="passedCoordinateSystem">The coordinate System this coordinate system is currently based on</param>
        /// <returns>Returns a new Coordinate System that reflects this coordinate system based on the world coordinate system 
        /// instead of the passed one</returns>
        public CoordinateSystem FindThisSystemRelativeToWorldSystemCurrentlyRelativeToPassedSystem(CoordinateSystem passedCoordinateSystem)
        {
            CoordinateSystem toReturn = new CoordinateSystem(this);

            //we have to shift our origin point first
            //we need to manually make the shift so that it wont be negated and then translate before rotating
            List<Rotation> rotationsToApplyToOrigin = new List<Rotation>();
            rotationsToApplyToOrigin.Add(new Rotation(Line.ZAxis, passedCoordinateSystem.ZRotation));
            rotationsToApplyToOrigin.Add(new Rotation(Line.XAxis, passedCoordinateSystem.XRotation));
            rotationsToApplyToOrigin.Add(new Rotation(Line.YAxis, passedCoordinateSystem.YRotation));

            //now shift the origin point
            toReturn.Origin = this.Origin.Shift(new Shift(rotationsToApplyToOrigin, passedCoordinateSystem.Origin));

            //we can just add the rotations
            //convert them to matricies
            Matrix[] thisAnglesMatricies = new Matrix[]{
                Matrix.RotationMatrixAboutX(this.XRotation),
                Matrix.RotationMatrixAboutY(this.YRotation),
                Matrix.RotationMatrixAboutZ(this.ZRotation)
            };

            Matrix[] passedAnglesMatricies = new Matrix[]{
                Matrix.RotationMatrixAboutX(this.XRotation),
                Matrix.RotationMatrixAboutY(this.YRotation),
                Matrix.RotationMatrixAboutZ(this.ZRotation)
            };

            //multiply them (order is important!)
            Matrix resultingSystem = thisAnglesMatricies[0] * passedAnglesMatricies[0] * thisAnglesMatricies[1] * passedAnglesMatricies[1] *
                thisAnglesMatricies[2] * passedAnglesMatricies[2];

            //then pull out the data
            Angle[] resultingAngles = resultingSystem.getAnglesOutOfRotationMatrix();

            toReturn.ZRotation = resultingAngles[0];
            toReturn.XRotation = resultingAngles[1];
            toReturn.YRotation = resultingAngles[2];

            return toReturn;
        }

        /*
                /// <summary>
                /// Makes a shift that can be used to move coordinate systems around
                /// </summary>
                /// <returns>Returns a shift to be used on a coordinate systems origin to move it to the corresponding spot</returns>
                public Shift MakeIntoShiftForAnotherCoordinateSystemsOrigin()
                {
                    //we have to make the shift from scratch because
                    Shift systemShift = new Shift();
                    systemShift.RotationsToApply.Add(new Rotation(Line.XAxis, this.XRotation));
                    systemShift.RotationsToApply.Add(new Rotation(Line.YAxis, this.YRotation));
                    systemShift.RotationsToApply.Add(new Rotation(Line.ZAxis, this.ZRotation));

                    systemShift.Displacement = this.Origin;

                    return systemShift;
                }*/

        #endregion
    }
}
