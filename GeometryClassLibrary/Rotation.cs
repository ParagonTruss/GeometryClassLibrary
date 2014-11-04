using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    public class Rotation
    {
        #region Properties and Fields

        private Angle _angleToRotate;
        public Angle AngleToRotate
        {
            get { return _angleToRotate; }
        }

        private Line _axisToRotateAround;
        public Line AxisToRotateAround
        {
            get { return _axisToRotateAround; }
        }

        #endregion

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

        /* these dont make sense right now because they assume that the rotations are the same axis
        public static Rotation operator +(Rotation r1, Angle angleToAdd)
        {
            //add the angle to how far we are already rotating
            return new Rotation(r1._axisToRotateAround, r1._angleToRotate + angleToAdd);
        }

        public static Rotation operator -(Rotation r1, Angle angleToSubtract)
        {
            //subtract the angle to how far we are already rotating
            return new Rotation(r1._axisToRotateAround, r1._angleToRotate - angleToSubtract);
        }*/

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator ==(Rotation rotation1, Rotation rotation2)
        {
            if ((object)rotation1 == null)
            {
                if ((object)rotation2 == null)
                {
                    return true;
                }
                return false;
            }
            return rotation1.Equals(rotation2);
        }

        /// <summary>
        /// Not a perfect inequality operator, is only accurate up to Constants.AcceptedEqualityDeviationConstant 
        /// </summary>
        public static bool operator !=(Rotation rotation1, Rotation rotation2)
        {
            if ((object)rotation1 == null)
            {
                if ((object)rotation2 == null)
                {
                    return false;
                }
                return true;
            }
            return !rotation1.Equals(rotation2);
        }

        /// <summary>
        /// does the same thing as == if the passed in object is a Rotation
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure the obj is not null
            if (obj == null)
            {
                return false;
            }

            //try casting it and then comparing it
            try
            {
                Rotation rotate = (Rotation)obj;

                bool axesEqual = this._axisToRotateAround == rotate._axisToRotateAround;
                bool anglesEqual = this._angleToRotate == rotate._angleToRotate;

                return anglesEqual && axesEqual;
            }
            //if it wasnt a rotation than its not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion 
    }
}
