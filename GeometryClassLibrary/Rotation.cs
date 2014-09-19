using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Rotation
    {
        private Angle _angleToRotate;
        public Angle angleToRotate
        {
            get { return _angleToRotate; }
        }

        private Line _axisToRotateAround;
        public Line axisToRotateAround
        {
            get { return _axisToRotateAround; }
        }

        #region Constructors
        //There is no empty constructor because a rotation needs an axis to rotate around at the least
        
        /// <summary>
        /// Creates an Rotation about the inputted Axis with a default rotation value of 0
        /// </summary>
        /// <param name="axisOfRotation">The axis to rotate around</param>
        public Rotation(Line axisOfRotation)
        {
            this._axisToRotateAround = axisOfRotation;
            this._angleToRotate = new Angle();
        }

        /// <summary>
        /// Creates a rotation about the inputted Axis and with the inputted Angle
        /// </summary>
        /// <param name="axisOfRotation">The Axis to rotate around</param>
        /// <param name="howFarToRotate">The Angle that specifies how far to rotate</param>
        public Rotation(Line axisOfRotation, Angle howFarToRotate)
        {
            this._axisToRotateAround = axisOfRotation;
            this._angleToRotate = howFarToRotate;
        }

        #endregion 

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        //Adding two rotations together should give you a list of Rotations because they dont straight up add well
        //because they are non comunitive


        public static Rotation operator +(Rotation r1, Angle angleToAdd)
        {
            //add the angle to how far we are already rotating
            return new Rotation(r1._axisToRotateAround, r1._angleToRotate + angleToAdd);
        }

        public static Rotation operator -(Rotation r1, Angle angleToSubtract)
        {
            //subtract the angle to how far we are already rotating
            return new Rotation(r1._axisToRotateAround, r1._angleToRotate - angleToSubtract);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator ==(Rotation r1, Rotation r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator !=(Rotation r1, Rotation r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// does the same thing as == if the passed in object is a Rotation
        /// </summary>
        public override bool Equals(object obj)
        {
            Rotation rotate = (Rotation)obj;

            return this._axisToRotateAround == rotate._axisToRotateAround &&
                this._angleToRotate == rotate._angleToRotate;
        }

        #endregion 
    }
}
