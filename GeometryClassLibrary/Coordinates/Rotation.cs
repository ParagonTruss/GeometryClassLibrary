using System;
using System.Diagnostics;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    [DebuggerDisplay("Angle to Rotate = {AngleToRotate.Degrees}, Axis of Rotation: BasePoint = {AxisToRotateAround.BasePoint.X.Inches}, {AxisToRotateAround.BasePoint.Y.Inches}, {AxisToRotateAround.BasePoint.Z.Inches}, Direction Vector = {AxisToRotateAround.DirectionVector.XComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.YComponentOfDirection.Inches}, {AxisToRotateAround.DirectionVector.ZComponentOfDirection.Inches}")]
    public class Rotation
    {
        #region Properties and Fields
        private readonly AngularDistance _rotationAngle;
        private readonly Line _axisOfRotation;

        public AngularDistance RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
        }

        public Line AxisOfRotation
        {
            get
            {
                return _axisOfRotation;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the identity rotation
        /// </summary>
        public Rotation()
        {
            this._rotationAngle = Angle.Zero;
            this._axisOfRotation = Line.XAxis;
        }
       
        /// <summary>
        /// Creates a rotation about the input Axis and with the input Angle of 0 if the angle is omitted
        /// </summary>
        /// <param name="axisOfRotation">The Axis to rotate around</param>
        /// <param name="rotationAngle">The Angle that specifies how far to rotate</param>
        public Rotation(Line axisOfRotation, AngularDistance rotationAngle)
        {
            this._rotationAngle = rotationAngle;
            this._axisOfRotation = axisOfRotation;
        }

        public Rotation(AngularDistance rotationAngle, Line axisOfRotation = null)
        {
            if (axisOfRotation == null)
            {
                axisOfRotation = Line.ZAxis;
            }
            this._axisOfRotation = axisOfRotation;
            this._rotationAngle = rotationAngle;
        }

        /// <summary>
        /// Creates a copy of the given rotation
        /// </summary>
        /// <param name="toCopy">the Rotation to copy</param>
        public Rotation(Rotation toCopy)
        {
            this._rotationAngle = toCopy._rotationAngle;
            this._axisOfRotation = toCopy._axisOfRotation;
        }

        #endregion 

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


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
        public override bool Equals(object other)
        {
            //make sure the obj is not null
            if (other == null)
            {
                return false;
            }

            //try casting it and then comparing it
            try
            {
                Rotation rotation = (Rotation)other;
                if (this.AxisOfRotation != rotation.AxisOfRotation)
                {
                    return false;
                }
                return (this.AxisOfRotation.Direction == rotation.AxisOfRotation.Direction &&
                    this.RotationAngle == rotation.RotationAngle) ||
                    (this.AxisOfRotation.Direction == rotation.AxisOfRotation.Direction.Reverse() &&
                    this.RotationAngle == rotation.RotationAngle.Negate());

            }
            //if it wasn't a rotation than it's not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Returns the inverse rotation.
        /// </summary>
        public Rotation Inverse()
        {
            Angle inverseAngle = new Angle(AngleType.Radian, this.RotationAngle.Radians * -1);
            return new Rotation(this.AxisOfRotation, inverseAngle);
        }

        #endregion
    }
}
