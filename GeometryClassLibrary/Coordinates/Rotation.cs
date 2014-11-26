using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;
using System.Diagnostics;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Angle to Rotate = {AngleToRotate.Degrees}, Axis of Rotation: BasePoint = {AxisToRotateAround.BasePoint.X.Inches}, {AxisToRotateAround.BasePoint.Y.Inches}, {AxisToRotateAround.BasePoint.Z.Inches}, Direction Vector = {AxisToRotateAround.DirectionVector.XComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.YComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.ZComponentOfDirection.Inches}")]
    public class Rotation
    {
        #region Properties and Fields

        /// <summary>
        /// The angle around the given axis that this rotation reperesents
        /// </summary>
        private Angle _angleToRotate;
        public Angle AngleToRotate
        {
            get { return _angleToRotate; }
        }

        /// <summary>
        /// The axis around which this rotation reperesents
        /// </summary>
        private Line _axisToRotateAround;
        public Line AxisToRotateAround
        {
            get { return _axisToRotateAround; }
        }

        #endregion

        #region Constructors

        //There is no empty constructor because a rotation needs an axis to rotate around at the least
       
        /// <summary>
        /// Creates a rotation about the inputted Axis and with the inputted Angle or 0 if the angle is omitted
        /// </summary>
        /// <param name="axisOfRotation">The Axis to rotate around</param>
        /// <param name="howFarToRotate">The Angle that specifies how far to rotate</param>
        public Rotation(Line axisOfRotation, Angle howFarToRotate = null)
        {
            if ((object)howFarToRotate == null)
            {
                howFarToRotate = new Angle();
            }
            else
            {
                this._angleToRotate = howFarToRotate;
            }
            this._axisToRotateAround = axisOfRotation;
        }

        /// <summary>
        /// Creates a copy of the given rotation
        /// </summary>
        /// <param name="toCopy">the Rotation to copy</param>
        public Rotation(Rotation toCopy)
        {
            _angleToRotate = toCopy.AngleToRotate;
            _axisToRotateAround = new Line(toCopy.AxisToRotateAround);
        }

        #endregion 

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

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
